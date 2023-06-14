using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Daily_Report.Iteams;
using Daily_Report.Services;
using System.Net.Mail;
using System.Net;
using System.Net.Http;
using Xamarin.Essentials;

namespace Daily_Report.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewClient : ContentPage
    {
        public NewClient()
        {
            InitializeComponent();
        }

        string checkname;
        string checkphoneNumber;
        private string CreatedDate;
        New_client client = new New_client();
        public void Manadatoryfield()
        {
            if (string.IsNullOrWhiteSpace(Client_name.Text))
            {
                // Display an error message or handle the validation failure
                DisplayAlert("Error", "Name is required", "OK");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Phone_number.Text))
                {
                    // Display an error message or handle the validation failure
                    DisplayAlert("Error", "Phone Number is required", "OK");
                }
                else
                {
                    checkphoneNumber = "Y";
                }
                checkname = "Y";
            }
           
        }

        private async void NewClient_save_clicked(object sender, EventArgs e)
        {
            try
            {
                Manadatoryfield();
                if (checkname == "Y" && checkphoneNumber=="Y")
                {
                   
                    client.Client_name = Client_name.Text;
                    client.Client_number = Phone_number.Text;
                    client.Client_location = Location.Text;
                    client.Client_Remark = Remark_newclient.Text;
                    DateTime now = DateTime.Now;
                     CreatedDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                    client.Client_createddate = CreatedDate;
                    SQLhelper.SQL_Database.Add_New_client(client);
                   
                    //need to add stmp configuration
                   await Send_email_smtp(Get_email_setup());
                   await DisplayAlert("Mail", "send successfully", "OK");
                    Client_name.Text = string.Empty;
                    Phone_number.Text = string.Empty;
                    Location.Text = string.Empty;
                    Remark_newclient.Text = string.Empty;

                   await Navigation.PopAsync();
                }       
            }
            catch (Exception ex) 
            {
               await DisplayAlert("Contact Developer", ex.Message, "OK");
            }                
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
                maildetail.Subject = "Daily Meet Client Detail";
                maildetail.Body = $"<html><body><p>Hello,</p><p>Name:{client.Client_name}<br />Phone Number:{Phone_number.Text}<br />Location    :{Location.Text}<br />Remark:<br />{Remark_newclient.Text}<br /></p><p>Regards,<br />{CreatedDate}</p></body></html>";
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
    }
}