//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CottonDBMS.DataModels;
using Microsoft.Azure.Documents;
using CottonDBMS.Cloud;
using Newtonsoft;
using CottonDBMS.GinApp.Helpers;
using ZXing;
using PdfSharp;
using MigraDoc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using CottonDBMS.Helpers;

namespace CottonDBMS.GinApp.Dialogs
{
    public partial class QRCodeDialog : Form
    {
        public QRCodeDialog()
        {
            InitializeComponent();
        }

        public async Task LoadDialogAsync()
        {
            bool hasError = false;
          
            //get read only keys
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                var readOnlyEndpointSetting = uow.SettingsRepository.GetSettingWithDefault(GinAppSettingKeys.AZURE_DOCUMENTDB_READONLY_ENDPOINT, "");
                var readOnlyKeySetting = uow.SettingsRepository.GetSettingWithDefault(GinAppSettingKeys.AZURE_DOCUMENTDB_READONLY_KEY, "");
                var ginName = uow.SettingsRepository.GetSettingWithDefault(GinAppSettingKeys.GIN_NAME, "");
                var email = uow.SettingsRepository.GetSettingWithDefault(GinAppSettingKeys.IMAP_USERNAME, "");                               

                if (string.IsNullOrWhiteSpace(readOnlyEndpointSetting) || string.IsNullOrWhiteSpace(readOnlyKeySetting))
                {
                    hasError = true;
                    lblError.Text = "Unable to create QR code.  Azure Cosmos DB READ ONLY Settings have not been saved.";
                }
                else if (string.IsNullOrWhiteSpace(email))
                {
                    hasError = true;
                    lblError.Text = "Unable to create QR code.  IMAP username/email missing or not yet saved.";
                }              

                if (!hasError)
                {
                    try
                    {
                        bool writeAble = await CottonDBMS.Cloud.DocumentDBContext.CanWrite(readOnlyEndpointSetting, readOnlyKeySetting);
                        bool readAble = await CottonDBMS.Cloud.DocumentDBContext.CanRead(readOnlyEndpointSetting, readOnlyKeySetting);

                        if (writeAble)
                        {
                            hasError = true;
                            lblError.Text = "Unable to create QR code.  Azure Cosmos Read Only settings are allowing write access.  Please ensure you have not entered the read/write key for the READ only key.";
                        }
                        else if (!readAble)
                        {
                            hasError = true;
                            lblError.Text = "Unable to create QR code.  Unable to read from Azure Cosmos DB.  Please verify your READ ONLY Uri and Key is correct.";
                        }
                    }
                    catch (Exception exc)
                    {
                        hasError = true;
                        lblError.Text = exc.Message;
                    }
                }

                btnDownloadQRCode.Enabled = !hasError;
                lblInfoText.Visible = !hasError;
                lblError.Visible = hasError;
                pictureBoxQRCode.Visible = !hasError;

                if (!hasError)
                {
                    //no errors were found so configuration is good - we need to generate a json document with the endpoint information
                    var info = new CottonDBMS.DataModels.MobileConnectionInfo();
                    info.Email = email;
                    info.Gin = ginName;
                    info.Url = readOnlyEndpointSetting;
                    info.Key = readOnlyKeySetting;

                    //we encrypt to mask the data in the QR code - RFID Module Scan will decrypt
                    var dataString = EncryptionHelper.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(info));

                    var writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };
                    writer.Options.Height = 300;
                    writer.Options.Width = 300;
                    var result = writer.Write(dataString);
                    pictureBoxQRCode.Image = result;

                    var decrypted = EncryptionHelper.Decrypt(dataString);
                }
            }
        }

        private async void QRCodeDialog_Load(object sender, EventArgs e)
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDownloadQRCode_Click(object sender, EventArgs e)
        {

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {


                var document = new MigraDoc.DocumentObjectModel.Document();

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

                // Each MigraDoc document needs at least one section.
                Section section = document.AddSection();

                // Create header
                Paragraph paragraph1 = section.AddParagraph();
                paragraph1.AddText("Read Only Connection Code");
                paragraph1.Format.Font.Size = 24;
                paragraph1.Format.Font.Bold = true;
                paragraph1.Format.Alignment = ParagraphAlignment.Left;
                paragraph1.Format.SpaceAfter = new Unit(16.0);
                paragraph1.Format.SpaceBefore = new Unit(0.0);

                

                Paragraph paragraph2 = section.AddParagraph();
                paragraph2.AddText("Use this code to share client, farm, and field lists with RFID Module Scan.  Open the RFID Module Scan app, goto \"Settings\", tap \"Connect to Gin\", and then scan this code with the device camera.");
                paragraph2.Format.Font.Size = 8;
                paragraph2.Format.Font.Bold = true;
                paragraph2.Format.Alignment = ParagraphAlignment.Left;
                paragraph2.Format.SpaceAfter = new Unit(16.0);
                paragraph2.Format.SpaceBefore = new Unit(0.0);

                // Create footer
                /*Paragraph paragraph = section.Footers.Primary.AddParagraph();
                paragraph.AddText(string.Format("- Generated {0} {1} -", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));
                paragraph.Format.Font.Size = 8;
                paragraph.Format.Alignment = ParagraphAlignment.Center;*/

                pictureBoxQRCode.Image.Save("connection_code.bmp");
                var image = section.AddImage("connection_code.bmp");
                image.Width = new Unit(2.0, UnitType.Inch);
                image.Height = new Unit(2.0, UnitType.Inch);

                var pdfRenderer = new MigraDoc.Rendering.PdfDocumentRenderer(false);
                pdfRenderer.Document = document;
                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save(fileDialog.FileName);
                System.Diagnostics.Process.Start(fileDialog.FileName);

                this.Close();
            }
        }
    }
}
