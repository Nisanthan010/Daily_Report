using Daily_Report.Iteams;
using Daily_Report.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LINQtoCSV;
using System.Net.Mail;
using System.Net;

namespace Daily_Report.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExcelGenerateReport : ContentPage
    {
        public ExcelGenerateReport()
        {
            InitializeComponent();
        }
        private DateTime Fromdate { get; set; }
        private DateTime Todate { get; set; }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Excel_client_collection.ItemsSource = SQLhelper.SQL_Database.Get_Client_excel();
            Fromdate=DateTime.Now;
            Todate= DateTime.Now;
            FromDatePicker.MaximumDate=Fromdate;
        }
        
        private async void generate_csv(object sender, EventArgs e)
        {
            try
            {

                Excel_client_collection.ItemsSource = Get_Client_Excels();
                add_email_attachment();
               await Send_email_smtp(Get_email_setup());


            }
            catch (Exception ex)
            {
               await DisplayAlert("Contact Developer", ex.Message, "OK");
            }
        }

        private void Excel_client_collection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
        private void ToDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            Todate = e.NewDate;
            FromDatePicker.Date = Todate;
            FromDatePicker.MaximumDate = Todate;
            Fromdate=e.NewDate;
        }

        private void FromDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            Fromdate = e.NewDate;
            ToDatePicker.MinimumDate=Fromdate;
        }
        public string Path_root()
        {
            var path = string.Empty;
            path = Path.Combine(FileSystem.CacheDirectory, "attached.csv");
            return path;
        }
        public void add_email_attachment()
        {
            
            var path = string.Empty;
            
            path = Path_root();
            List<Get_client_excel> tableData = Get_Client_Excels();
            CsvFileDescription CsvFileDescription = new CsvFileDescription();
            CsvFileDescription.FirstLineHasColumnNames = true;
            CsvFileDescription.SeparatorChar = ',';
            var csvcontext = new CsvContext();
            csvcontext.Write(tableData, path, CsvFileDescription);
           
        }
        public SMTP_EmailSetup Get_email_setup()
        {
            return SQLhelper.SQL_Database.Email_sendDetail();
        }
        public async Task Send_email_smtp(SMTP_EmailSetup EmailDetail)
        {
            SmtpClient clientdetail = new SmtpClient();
            MailMessage maildetail = new MailMessage();
            try
            {
                clientdetail.Port = Convert.ToInt32(EmailDetail.SmtpEmailPort.Trim());
                clientdetail.Host = EmailDetail.SmtpEmailHost.Trim();
                clientdetail.EnableSsl = EmailDetail.IsSent;
                clientdetail.DeliveryMethod = SmtpDeliveryMethod.Network;
                clientdetail.UseDefaultCredentials = false;
                clientdetail.Credentials = new NetworkCredential(EmailDetail.SenderEmail, EmailDetail.SmtpPassword);
                maildetail.From = new MailAddress(EmailDetail.SenderEmail);
                maildetail.To.Add(EmailDetail.ReceivedEmail);
                maildetail.IsBodyHtml = true;
                maildetail.Subject = "Excel Client Detail";
                 maildetail.Body = $"<html><body><p>Hello,</p><p>We generate the excel file data range <br />{Fromdate}<br />to<br />{Todate}<br /></p><p>Regards,<br />{DateTime.Now}</p></body></html>";
                var attachment = new Attachment(Path_root());

                // Add the attachment to the message
                maildetail.Attachments.Add(attachment);

                clientdetail.Send(maildetail);


            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }
            finally
            {
                maildetail.Dispose();
                clientdetail.Dispose();
            }
        }
        public List<Get_client_excel> Get_Client_Excels()
        {
           return SQLhelper.SQL_Database.Get_Client_excelwithdate(Fromdate, Todate);
        }

    }
}