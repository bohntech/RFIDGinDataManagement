//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CottonDBMS.GinApp.Dialogs;
using CottonDBMS.GinApp.Classes;
using CottonDBMS.DataModels;
using System.IO;
using PdfSharp;
using MigraDoc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace CottonDBMS.GinApp.UserControls
{
    public partial class ReportPage : UserControl
    {
        bool initialized = false;
        bool altRow = false;

        public ReportPage()
        {
            InitializeComponent();         
        }

        public void LoadData()
        {
            try
            {
                filterBar.ShowApplyButton = false;
                filterBar.ShowLocationOptions = true;
                filterBar.ShowSort1 = true;
                filterBar.ShowSort2 = true;
                filterBar.ShowSort3 = true;

                if (!initialized)
                {
                    initialized = true;
                    filterBar.Enabled = false;
                    BusyMessage.Show("Loading...", this.FindForm());                    
                    filterBar.Initialize();
                    BusyMessage.Close();
                    filterBar.Enabled = true;                    
                    try
                    {
                        //load export templates
                        string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        currentDir = currentDir.TrimEnd('\\') + "\\ExportTemplates";

                        cboOutputType.Items.Clear();
                        foreach (var file in System.IO.Directory.EnumerateFiles(currentDir))
                        {
                            var info = new System.IO.FileInfo(file);
                            cboOutputType.Items.Add(info.Name.Replace(info.Extension, ""));
                        }
                        cboOutputType.SelectedIndex = 0;
                    }
                    catch (Exception exc)
                    {
                        Logging.Logger.Log(exc);
                        MessageBox.Show("Unable to load export templates.");
                    }

                    cboExportType.SelectedIndex = 0;
                }
                else
                {
                    filterBar.RefreshAutocompletes();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error occurred trying to load filters.");
                Logging.Logger.Log(exc);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {

        }

        private void cboExportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblOutputTemplate.Visible = cboOutputType.Visible = (cboExportType.SelectedItem.ToString().ToLower().IndexOf("csv") >= 0);
            lblPdfTitle.Visible = tbReportTitle.Visible = (cboExportType.SelectedItem.ToString().ToLower().IndexOf("pdf") >= 0);
        }

        private List<ModuleEntity> getRecentModuleDocsAsync()
        {
            List<ModuleEntity> outputDocs = new List<ModuleEntity>();
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                BusyMessage.UpdateMessage("Retrieving results");
                var results = uow.ModuleRepository.GetAllMatchingModules(filterBar.Filter);                
                return results.ToList();
            }

        }

        private List<ModuleHistoryEntity> getAllLocationRecordsAsync()
        {
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                BusyMessage.UpdateMessage("Retrieving results");
                var results = uow.ModuleRepository.GetAllMatchingLocations(filterBar.Filter);
                return results.ToList();
            }
        }

        private string[] GetTemplateFileLines()
        {
            try
            {
                string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                currentDir = currentDir.TrimEnd('\\') + "\\ExportTemplates";

                foreach (var f in Directory.GetFiles(currentDir))
                {
                    if (f.IndexOf(cboOutputType.SelectedItem.ToString()) >= 0)
                    {
                        return System.IO.File.ReadAllLines(f);
                    }
                }             
            }
            catch (Exception exc)
            {
                MessageBox.Show("Could not locate template file");
                Logging.Logger.Log(exc);
            }
            return null;
        }

        private string GetTemplateFileExt()
        {
            try
            {
                string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                currentDir = currentDir.TrimEnd('\\') + "\\ExportTemplates";

                foreach (var f  in Directory.GetFiles(currentDir))
                {

                    if (f.IndexOf(cboOutputType.SelectedItem.ToString()) >= 0)
                    {
                        var info = new System.IO.FileInfo(f);
                        return info.Extension;
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
            return ".csv";
        }

        private string getOutputLine(string template, ModuleEntity doc)
        {
            string outputLine = template.Replace("{Farm}", doc.FarmName)
                           .Replace("{Field}", doc.Field.Name)
                           .Replace("{Client}", doc.ClientName)
                           .Replace("{ModuleId}", doc.ModuleId)
                           .Replace("{SerialNumber}", doc.Name)
                           .Replace("{LoadNumber}", doc.LoadNumberString)
                           .Replace("{ImportedLoadNumber}", doc.ImportedLoadNumber)
                           .Replace("{Latitude}", doc.Latitude.ToString())
                           .Replace("{Longitude}", doc.Longitude.ToString())
                           .Replace("{TruckID}", doc.TruckID)
                           .Replace("{Driver}", doc.Driver)
                           .Replace("{Status}", doc.StatusName)
                           .Replace("{Notes}", doc.Notes)
                           .Replace("{EventType}", "")
                           .Replace("{GMTDate}", doc.Created.ToString("MM/dd/yyyy"))
                           .Replace("{GMTTime}", doc.Created.ToString("HH:mm:ss"))
                           .Replace("{DateAdded}", doc.LocaleCreatedTimestamp)
                           .Replace("{DateAddedDate}", doc.Created.ToLocalTime().ToString("MM/dd/yyyy"))
                           .Replace("{DateAddedTime}", doc.Created.ToLocalTime().ToString("HH:mm:ss"));

            return outputLine;
        }

        private string getHistoryOutputLine(string template, ModuleHistoryEntity doc)
        {
            string outputLine = template.Replace("{Farm}", doc.Module.Field.Farm.Name)
                           .Replace("{Field}", doc.Module.Field.Name)
                           .Replace("{Client}", doc.Module.Field.Farm.Client.Name)
                           .Replace("{ModuleId}", doc.ModuleId)
                           .Replace("{SerialNumber}", doc.Module.Name)
                           .Replace("{LoadNumber}", doc.Module.LoadNumberString)
                           .Replace("{ImportedLoadNumber}", doc.Module.ImportedLoadNumber)
                           .Replace("{Latitude}", doc.Latitude.ToString())
                           .Replace("{Longitude}", doc.Longitude.ToString())
                           .Replace("{TruckID}", doc.TruckID)
                           .Replace("{Driver}", doc.Driver)
                           .Replace("{Status}", doc.StatusName)
                           .Replace("{Notes}", doc.Module.Notes)
                           .Replace("{EventType}", doc.EventName)
                           .Replace("{GMTDate}", doc.Created.ToString("MM/dd/yyyy"))
                           .Replace("{GMTTime}", doc.Created.ToString("HH:mm:ss"))
                           .Replace("{DateAdded}", doc.LocalCreatedTimestamp)
                           .Replace("{DateAddedDate}", doc.Created.ToLocalTime().ToString("MM/dd/yyyy"))
                           .Replace("{DateAddedTime}", doc.Created.ToLocalTime().ToString("HH:mm:ss"));

            return outputLine;
        }

        private void setupStyles(Document document)
        {           
            Style style = document.Styles["Normal"];          
            style.Font.Name = "Verdana";          
            style = document.Styles[StyleNames.Header];  
                     
            style.ParagraphFormat.AddTabStop("16cm", MigraDoc.DocumentObjectModel.TabAlignment.Right);          
            style = document.Styles[StyleNames.Footer];          
            style.ParagraphFormat.AddTabStop("8cm", MigraDoc.DocumentObjectModel.TabAlignment.Center);
         
            // Create a new style called Table based on style Normal
            style = document.Styles.AddStyle("Table", "Normal");          
            style.Font.Name = "Verdana";          
            style.Font.Name = "Arial";          
            style.Font.Size = 8;
            
            document.DefaultPageSetup.BottomMargin = Unit.FromInch(0.5);
            document.DefaultPageSetup.TopMargin = Unit.FromInch(0.5);
            document.DefaultPageSetup.RightMargin = Unit.FromInch(0.5);
            document.DefaultPageSetup.LeftMargin = Unit.FromInch(0.5);
            document.DefaultPageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;
        }        

        private void setupPdf(Document document, string[] headerTitles, ref Table table, string reportTitle)
        {
            setupStyles(document);

            // Each MigraDoc document needs at least one section.
            Section section = document.AddSection();

            // Create header
            Paragraph paragraph1 = section.AddParagraph();
            paragraph1.AddText(reportTitle);
            paragraph1.Format.Font.Size = 14;
            paragraph1.Format.Font.Bold = true;
            paragraph1.Format.Alignment = ParagraphAlignment.Left;
            paragraph1.Format.SpaceAfter = new Unit(12.0);

            // Create footer
            Paragraph paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText(string.Format("- Generated {0} {1} -", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));
            paragraph.Format.Font.Size = 8;
            paragraph.Format.Alignment = ParagraphAlignment.Center; 
                      

            // Create the item table
            table = section.AddTable();
            table.Style = "Table";
            table.Borders.Color = new MigraDoc.DocumentObjectModel.Color(0, 0, 0);
            table.Borders.Width =  new Unit(0.7);
            table.Borders.Left.Width = new Unit(0.7);
            table.Borders.Right.Width = new Unit(0.7);
            table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            foreach (var title in headerTitles)
            {
                string[] parts = title.Split(':');
                Column column = table.AddColumn(parts[1]);
               // column.Format.LineSpacingRule = LineSpacingRule.Single;                
                column.Format.Alignment = ParagraphAlignment.Center;                
            }       

            // Create the header of the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Left;
            row.Format.Font.Bold = true;
            row.Format.Font.Name = "Verdana";
            row.Shading.Color =  new MigraDoc.DocumentObjectModel.Color(200,200,200);
            row.Borders.Visible = true;
            //row.HeightRule = RowHeightRule.Auto;
            row.TopPadding = new Unit(4.0);
            row.BottomPadding = new Unit(4.0);            

            for (int i = 0; i < headerTitles.Length;i++)
            {
                string[] parts = headerTitles[i].Split(':');
                var p = row.Cells[i].AddParagraph(parts[0]);                
                row.Cells[i].Format.Font.Bold = false;
                row.Cells[i].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[i].VerticalAlignment = VerticalAlignment.Center;
                //row.Cells[i].Format.LineSpacingRule = LineSpacingRule.Single;                
            }
           
        }

        private void generateMapPdf(string reportTitle, string imageFilename, string outputFilename)
        {
            Document document = new Document();
            setupStyles(document);

            // Each MigraDoc document needs at least one section.
            Section section = document.AddSection();

            // Create header
            Paragraph paragraph1 = section.AddParagraph();
            paragraph1.AddText(reportTitle);
            paragraph1.Format.Font.Size = 14;
            paragraph1.Format.Font.Bold = true;
            paragraph1.Format.Alignment = ParagraphAlignment.Left;
            paragraph1.Format.SpaceAfter = new Unit(6.0);
            paragraph1.Format.SpaceBefore = new Unit(0.0);

            // Create footer
            /*Paragraph paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText(string.Format("- Generated {0} {1} -", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));
            paragraph.Format.Font.Size = 8;
            paragraph.Format.Alignment = ParagraphAlignment.Center;*/

            var image = section.AddImage(imageFilename);
            image.Width = new Unit(10.7, UnitType.Inch);
            image.Height = new Unit(6.5, UnitType.Inch);

            var pdfRenderer = new MigraDoc.Rendering.PdfDocumentRenderer(false);
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.Save(outputFilename);
            System.Diagnostics.Process.Start(outputFilename);
        }

        private void addPdfRow(Document document, string[] rowValues, Table table)
        {
            Row row = table.AddRow();            
            row.Borders.Visible = true;
            row.TopPadding = new Unit(4.0);
            row.BottomPadding = new Unit(4.0);
            
            row.Format.Font.Name = "Verdana"; 
            if (altRow)
            {
                row.Shading.Color = new MigraDoc.DocumentObjectModel.Color(230, 230, 230);
            }
            altRow = !altRow;
                   
            for (int i=0; i < rowValues.Length;i++)
            {           
                row.Cells[i].AddParagraph(rowValues[i]);             
                row.Cells[i].Format.Font.Bold = false;              
                row.Cells[i].Format.Alignment = ParagraphAlignment.Left;                
            }
        }

        public void StoreBounds(object x1, object y1, object x2, object y2, object xCenter, object yCenter)
        {

        }

        

        private void generatePDFAsync(string outputFilename)
        {
            //apply filters to get all records using paging - streaming records into a temporary database table            
            BusyMessage.Show("Downloading data...", this.FindForm());            
            try
            {
                Document mDoc = new Document();
                Table table = new Table();
                int record = 1;
                int total = 0;
                StringBuilder sb = new StringBuilder(32000);
                if (filterBar.Filter.RecentOnly)
                {
                    using (StreamWriter sr = new StreamWriter(outputFilename, false))
                    {
                        var outputDocs = getRecentModuleDocsAsync();
                        string[] headers = { "Client:3cm", "Farm:2.8cm", "Field:3cm", "Serial:2.2cm", "Load:2.55cm", "Latitude:3.15cm", "Longitude:3.15cm", "TruckID:2.0cm", "Driver:1.75cm", "Status:1.4cm", "Timestamp:2.0cm" };
                        string[] columns = { "{Client}", "{Farm}", "{Field}", "{SerialNumber}", "{LoadNumber}", "{Latitude}", "{Longitude}", "{TruckID}", "{Driver}", "{Status}", "{DateAdded}" };
                                              
                        setupPdf(mDoc, headers, ref table, tbReportTitle.Text);
                        total = outputDocs.Count();
                        
                        foreach (var d in outputDocs)
                        {
                            BusyMessage.UpdateMessage(string.Format("Building Row {0} of {1}", record, total));

                            List<string> values = new List<string>();
                            foreach(var v in columns)
                            {
                                values.Add(getOutputLine(v, d));
                            }                            
                            addPdfRow(mDoc, values.ToArray(), table);
                            record++;
                        }
                    }
                }
                else
                {
                    using (StreamWriter sr = new StreamWriter(outputFilename, false))
                    {
                        var historyDocs = getAllLocationRecordsAsync();                        
                        string[] headers = { "Client:2.5cm", "Farm:2.0cm", "Field:3cm", "Serial:2.2cm", "Load:2.55cm", "Latitude:3.15cm", "Longitude:3.15cm", "TruckID:2.0cm", "Driver:1.75cm", "Status:1.2cm", "Event:1.75cm", "Timestamp:2.0cm" };
                        string[] columns = { "{Client}", "{Farm}", "{Field}", "{SerialNumber}", "{LoadNumber}", "{Latitude}", "{Longitude}", "{TruckID}", "{Driver}", "{Status}", "{EventType}", "{DateAdded}" };

                        setupPdf(mDoc, headers, ref table, tbReportTitle.Text);
                        total = historyDocs.Count();
                        foreach (var d in historyDocs)
                        {
                            BusyMessage.UpdateMessage(string.Format("Building Row {0} of {1}", record, total));
                            List<string> values = new List<string>();
                            foreach (var v in columns)
                            {
                                values.Add(getHistoryOutputLine(v, d));
                            }
                            addPdfRow(mDoc, values.ToArray(), table);
                            record++;
                        }
                    }
                }

                if (total > 10000)
                {
                    BusyMessage.Close();
                    MessageBox.Show("Too many records to render to a single PDF.  Please add additional filter criteria to reduce the number of included records.");
                    return;
                }
                else
                {
                    BusyMessage.UpdateMessage("Rendering PDF");
                    var pdfRenderer = new MigraDoc.Rendering.PdfDocumentRenderer(false);
                    pdfRenderer.Document = mDoc;
                    pdfRenderer.RenderDocument();
                    pdfRenderer.PdfDocument.Save(outputFilename);
                    System.Diagnostics.Process.Start(outputFilename);
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);                
                MessageBox.Show(exc.Message);
            }
            BusyMessage.Close();
        }

        private void generateCSVAsync(string outputFilename)
        {
            //apply filters to get all records using paging - streaming records into a temporary database table            
            BusyMessage.Show("Downloading data...", this.FindForm());
            string[] templateLines = GetTemplateFileLines();            
            bool hasError = false;
            try
            {
                if (filterBar.Filter.RecentOnly)
                {
                    using (StreamWriter sr = new StreamWriter(outputFilename, false))
                    {
                        var outputDocs = getRecentModuleDocsAsync();
                        sr.WriteLine(templateLines[0]);
                        string outputLine = "";
                        foreach (var doc in outputDocs)
                        {
                            outputLine = getOutputLine(templateLines[1], doc);
                            sr.WriteLine(outputLine);
                        }
                    }
                }
                else
                {                    
                    using (StreamWriter sr = new StreamWriter(outputFilename, false))
                    {
                        var historyDocs = getAllLocationRecordsAsync();
                        sr.WriteLine(templateLines[0]);
                        string outputLine = "";
                        foreach (var doc in historyDocs)
                        {
                            outputLine = getHistoryOutputLine(templateLines[1], doc);
                            sr.WriteLine(outputLine);
                        }
                    }
                }
            }            
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                hasError = true;
                MessageBox.Show(exc.Message);
            }
            BusyMessage.Close();
            if (!hasError) MessageBox.Show("File saved successfully.");
        }

        private void btnExport_Click_1(object sender, EventArgs e)
        {
            if (cboExportType.SelectedIndex == 0)
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        generateCSVAsync(saveFileDialog.FileName);
                    }
                    catch(IOException ioExc)
                    {
                        MessageBox.Show(ioExc.Message);
                    }
                }
            }
            else if (cboExportType.SelectedIndex == 1)
            {                
                if (string.IsNullOrWhiteSpace(tbReportTitle.Text))
                {
                    MessageBox.Show("Please enter a title for the report.");
                }
                else if (savePdfDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        generatePDFAsync(savePdfDialog.FileName);
                    }
                    catch (IOException ioExc)
                    {
                        MessageBox.Show(ioExc.Message);
                    }
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(tbReportTitle.Text))
                {
                    MessageBox.Show("Please enter a title for the report.");
                }
                else
                {
                    MapReportDialog reportDialog = new MapReportDialog();
                    string appDataGinDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).TrimEnd('\\') + "\\" + FolderConstants.ROOT_APP_DATA_FOLDER + "\\"
                    + FolderConstants.GIN_APP_DATA_FOLDER;

                    reportDialog.ImageFilename = appDataGinDir + "\\" + Guid.NewGuid().ToString() + ".jpg";
                    reportDialog.SetFilter(filterBar.Filter);
                    if (reportDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            if (savePdfDialog.ShowDialog() == DialogResult.OK)
                            {
                                generateMapPdf(tbReportTitle.Text, reportDialog.ImageFilename, savePdfDialog.FileName);
                            }
                        }
                        catch (IOException ioExc)
                        {
                            MessageBox.Show(ioExc.Message);
                        }
                        finally
                        {
                            System.IO.File.Delete(reportDialog.ImageFilename);
                        }
                    }
                }
            }
        }
    }
}
