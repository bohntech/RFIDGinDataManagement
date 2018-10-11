//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using CottonDBMS.DataModels;

namespace CottonDBMS.Cloud {

    /// <summary>
    /// This class is a singleton static class adapted from the Azure Cosmos DB starter samples
    /// It is used to hold a single client session open for the application to share.  This
    /// enables session consistency so a single client will always see it's own updates.
    /// </summary>
    public static class DocumentDBContext
    {
        public static string DatabaseId = "CottonModuleDB";
        public static string CollectionId = "CottonDocs";

        private static DocumentClient client;

        private static async Task<long> BulkOperation<T>(string procName, List<T> documents, int chunkSize) where T : BaseEntity
        {
            List<T> chunk = new List<T>();
            long added = 0;
            while (documents.Count() > 0)
            {
                chunk.Clear();
                if (documents.Count() <= chunkSize)
                {
                    added += await BulkOp<T>(procName, documents);
                    documents.Clear();
                }
                else
                {
                    added += await BulkOp<T>(procName, documents.GetRange(0, chunkSize));
                    documents.RemoveRange(0, chunkSize);
                }

            }

            return added;
        }

        private static async Task<long> BulkOp<T>(string procName, List<T> documents) where T : BaseEntity
        {
            int total = 0;
            long added = 0;
            try
            {
                if (documents != null && documents.Count() > 0)
                {
                    foreach (var d in documents)
                    {
                        d.SelfLink = UriFactory.CreateDocumentUri(DatabaseId, CollectionId, d.Id).ToString();
                    }

                    total = documents.Count();
                    while (added < total)
                    {
                        documents.RemoveRange(0, (int)added);
                        long count = await ExecuteProcedure<long>(procName, documents);
                        if (count == 0) throw new Exception("No documents imported " + procName);
                        else added += count;
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
            return added;
        }

        public static bool Initialized
        {
            get
            {
                return client != null;
            }
        }

        public static async Task<T> GetItemAsync<T>(string id)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return default(T);
                }
                else
                {
                    throw;
                }
            }
        }

        public static async Task<StoredProcedureResponse<TReturnVal>> ExecuteProcedure<TReturnVal>(string procedureName, params dynamic[] parms)
        {            
            var response = await client.ExecuteStoredProcedureAsync<TReturnVal>(UriFactory.CreateStoredProcedureUri(DatabaseId, CollectionId, procedureName), new RequestOptions { EnableScriptLogging=false }, parms);
            return response;
        }
        
        public static async Task<long> BulkUpdate<T>( List<T> documents, int chunkSize) where T : BaseEntity
        {
            return await BulkOperation<T>("bulkUpdate", documents, chunkSize);
        }

        public static async Task<long> BulkUpsert<T>(List<T> documents, int chunkSize) where T : BaseEntity
        {
            return await BulkOperation<T>("bulkUpsert", documents, chunkSize);
        }

        public static async Task<long> BulkCreate<T>(List<T> documents, int chunkSize) where T : BaseEntity
        {
            return await BulkOperation<T>("bulkCreate", documents, chunkSize);
        }

        public static async Task<long> BulkDelete<T>(List<T> documents, int chunkSize) where T : BaseEntity
        {   
            return await BulkOperation<T>("bulkDelete", documents, chunkSize);
        }        
        
        public static async Task<IEnumerable<T>> GetAllItemsAsync<T>(Expression<Func<T, bool>> predicate) where T: class
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static async Task<IEnumerable<T>> GetAllItemsAsync<T>(Expression<Func<T, bool>> predicate, int MaxCount) where T : class
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = MaxCount })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static IOrderedQueryable<T> GetQueryable<T>(int maxCount = 100)
        {
            return client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = maxCount, PopulateQueryMetrics=true });
        }

        public static IOrderedQueryable<T> GetContinueableQueryable<T>(string continuationToken, int maxCount = 50)
        {
            return client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = maxCount, RequestContinuation = continuationToken });
        }

        public static async Task<bool> DocumentExists<T>(Expression<Func<T, bool>> predicate) where T: BaseEntity
        {
            var query = client.CreateDocumentQuery<T>(
               UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
               new FeedOptions { MaxItemCount = 1 })
               .Where(predicate)
               .Select(t => t.Id)
               .AsDocumentQuery();

            List<string> items = new List<string>();

            if (query.HasMoreResults)
                items.AddRange(await query.ExecuteNextAsync<string>());

            return items.Count() > 0;
        }

        public static async Task<Document> CreateItemAsync<T>(T item)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item, new RequestOptions { ConsistencyLevel = ConsistencyLevel.Eventual });
        }

        public static async Task<Document> UpdateItemAsync<T>(string id, T item)
        {
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item, new RequestOptions { ConsistencyLevel = ConsistencyLevel.Eventual });
        }

        public static async Task<Document> UpsertItemAsync<T>(T item)
        {
            return await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item, new RequestOptions { ConsistencyLevel = ConsistencyLevel.Eventual }, true);
        }

        public static async Task<Document> DeleteItemAsync<T>(string id)
        {           
           return await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), new RequestOptions { ConsistencyLevel = ConsistencyLevel.Eventual });           
        }

        public static void Initialize(string endpoint, string authKey)
        {
            client = new DocumentClient(new Uri(endpoint), authKey, 
                new ConnectionPolicy {EnableEndpointDiscovery = false }, ConsistencyLevel.Eventual);            
            object o = client.Session;

            if (endpoint.ToLower().IndexOf("azure.com") > 0)
            {
                //CreateDatabaseIfNotExistsAsync().Wait();
                //CreateCollectionIfNotExistsAsync().Wait();
            }
        }

        public static async Task<bool> CanWrite(string endpoint, string authKey)
        {
            try
            {
                var client = new DocumentClient(new Uri(endpoint), authKey,
                   new ConnectionPolicy { EnableEndpointDiscovery = false }, ConsistencyLevel.Eventual);

                var writeTestDoc = new BaseEntity { Id = "WRITE_TEST", EntityType = EntityType.WRITE_TEST, Name = "WRITE_TEST", SyncedToCloud = true, Source = InputSource.GIN };
                await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), writeTestDoc);

                return true;
            }
            catch(DocumentClientException exc)
            {
                string s = exc.Message;
                return false;
            }
        }

        public static async Task<bool> CanRead(string endpoint, string authKey)
        {
            try
            {
                var client = new DocumentClient(new Uri(endpoint), authKey,
                   new ConnectionPolicy { EnableEndpointDiscovery = false }, ConsistencyLevel.Eventual);
                
                var doc = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, "WRITE_TEST"));
                return true;
            }
            catch (DocumentClientException exc)
            {
                string s = exc.Message;
                return false;
            }
        }

        public static string GetDocLink(string id)
        {
            return UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id).ToString();
        }
               
        public static async Task<bool> DatabaseExistsAsync()
        {
            try
            {
                
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));

                return true;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }
                else
                {
                    throw new Exception("Error checking for existing Azure Document DB", e);
                }
            }
           
        }

        public static async Task CreateDatabaseAsync()
        {          
            await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
        }

        private static async Task CreateStoredProcedureIfNotExistsAsync(string procedureName, string body)
        {
            DocumentCollection collection = await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));

            string storedProceduresLink = collection.StoredProceduresLink;
            string StoredProcedureName = procedureName;

            StoredProcedure storedProcedure = client.CreateStoredProcedureQuery(storedProceduresLink)
                                    .Where(sp => sp.Id == StoredProcedureName)
                                    .AsEnumerable()
                                    .FirstOrDefault();

            if (storedProcedure == null)
            {
                // Register a stored procedure
                storedProcedure = new StoredProcedure
                {
                    Id = StoredProcedureName,
                    Body = body
                };
                storedProcedure = await client.CreateStoredProcedureAsync(storedProceduresLink,
            storedProcedure);
            }
        }

        public static async Task CreateStoredProceduresAsync()
        {
            string bulkCreate = @"

function bulkCreate(docs) {  
       
    var created = 0;
    var collection = getContext().getCollection();
    var collectionLink = collection.getSelfLink();

    for(var i=0; i < docs.length; i++) {
        var isAccepted = __.createDocument(collectionLink, docs[i], function(err) {
                if (err) throw err;
                else {
                    created++;
                    getContext().getResponse().setBody(created); 
                }
        });

        if (!isAccepted) {            
            getContext().getResponse().setBody(created); 
            return;
        }
    }
    
}
";

            string bulkUpdate = @"

function bulkUpdate(docs) {  
    var context = getContext();
    var collection = context.getCollection();
    var created = 0;

    var collectionLink = collection.getSelfLink();        
            for (var i = 0; i < docs.length; i++)
            {
                var isAccepted = __.replaceDocument(docs[i]._self, docs[i], function(err) {
                    if (err) throw err;
                    else
                    {
                        created++;
                        getContext().getResponse().setBody(created);
                    }
                });

            if (!isAccepted)
            {
                getContext().getResponse().setBody(created);
                return;
            }
        }

    }
";

    string bulkDelete = @"

function bulkDelete(documents) {  
    var context = getContext();
    var collection = context.getCollection();
    var deleted = 0;
    var collectionLink = collection.getSelfLink();        
    for(var i = 0; i<documents.length; i++) {
        var isAccepted = collection.deleteDocument(documents[i]._self, {}, function(err, responseOptions)
        {
            if (err) throw err;
            else
            {
                deleted++;
                getContext().getResponse().setBody(deleted);
            }
        });

        if (!isAccepted) {            
            getContext().getResponse().setBody(deleted); 
            return;
        }
    }    
}
";

            string bulkUpsert = @"
function bulkUpsert(docs) {  
    var context = getContext();
    var collection = context.getCollection();
    var created = 0;

    var collectionLink = collection.getSelfLink();        
            for (var i = 0; i < docs.length; i++)
            {
                var isAccepted = __.upsertDocument(collectionLink, docs[i], function(err) {
                    if (err) throw err;
                    else
                    {
                        created++;
                        getContext().getResponse().setBody(created);
                    }
                });

            if (!isAccepted)
            {
                getContext().getResponse().setBody(created);
                return;
            }
        }

    }
";

            //await CreateStoredProcedureIfNotExistsAsync("updateLinkedDataByClientId", updateLinkedDataByClientId);
            ///await CreateStoredProcedureIfNotExistsAsync("updateLinkedDataByFarmId", updateLinkedDataByFarmId);
            //await CreateStoredProcedureIfNotExistsAsync("updateLinkedDataByFieldId", updateLinkedDataByFieldId);

            await CreateStoredProcedureIfNotExistsAsync("bulkCreate", bulkCreate);
            await CreateStoredProcedureIfNotExistsAsync("bulkUpdate", bulkUpdate);
            await CreateStoredProcedureIfNotExistsAsync("bulkDelete", bulkDelete);
            await CreateStoredProcedureIfNotExistsAsync("bulkUpsert", bulkUpsert);
}

        public static async Task<bool> CollectionExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
                return true;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }
                else
                {
                    throw new Exception("Error checking for collection." , e);
                }
            }
        }

        public static async Task DeleteCollectionAsync()
        {
            await client.DeleteDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
        }

        public static async Task CreateCollectionAsync()
        {
            IndexingPolicy policy = new IndexingPolicy(new RangeIndex(DataType.String, -1), new RangeIndex(DataType.Number));
            policy.IndexingMode = IndexingMode.Consistent;
                
            policy.ExcludedPaths.Add(new ExcludedPath { Path = "/modulehistory/*" });        

            IncludedPath documentTypePath = new IncludedPath();
            documentTypePath.Path = "/entitytype/?";
            documentTypePath.Indexes.Add(new HashIndex(DataType.String, -1));
            policy.IncludedPaths.Add(documentTypePath);           

            IncludedPath clientIdPath = new IncludedPath();
            clientIdPath.Path = "/clientid/?";
            clientIdPath.Indexes.Add(new HashIndex(DataType.String, -1));
            policy.IncludedPaths.Add(clientIdPath);

            IncludedPath farmIdPath = new IncludedPath();
            farmIdPath.Path = "/farmid/?";
            farmIdPath.Indexes.Add(new HashIndex(DataType.String, -1));
            policy.IncludedPaths.Add(farmIdPath);

            IncludedPath fieldIdPath = new IncludedPath();
            fieldIdPath.Path = "/fieldid/?";
            fieldIdPath.Indexes.Add(new HashIndex(DataType.String, -1));
            policy.IncludedPaths.Add(fieldIdPath);
            
            var result = await client.CreateDocumentCollectionAsync(
                       UriFactory.CreateDatabaseUri(DatabaseId),
                       new DocumentCollection { Id = CollectionId, IndexingPolicy = policy },
                       new RequestOptions { OfferThroughput = 800,  ConsistencyLevel = ConsistencyLevel.Session });
            
        }        
    }
}
