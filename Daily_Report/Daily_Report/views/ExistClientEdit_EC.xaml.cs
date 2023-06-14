using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Daily_Report.Iteams;
using Daily_Report.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Daily_Report.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExistClientEdit_EC : ContentPage
    {
        public ExistClientEdit_EC()
        {
            InitializeComponent();
        }
        New_client com_client = new New_client();
        string Createddate;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Appearing_detail();

        }
        public void Appearing_detail()
        {
            New_client selectedItem = Application.Current.Properties["Last_selected"] as New_client;
            com_client = selectedItem;
            if (selectedItem != null)
            {
                client_name.Text = com_client.Client_name;
                client_number.Text = com_client.Client_number;
            }
        }
        private async void update_client_Clicked(object sender, EventArgs e)
        {
            try
            {
                Client_DealerDetail DealerDetail = new Client_DealerDetail();
                DealerDetail.NewClient_J_id = com_client.New_client_Id;
                DealerDetail.Client_DealerDetail_cement = Cement_client.Text;
                DealerDetail.Client_DealerDetail_dealer = Dealer_client.Text;
                DealerDetail.Client_DealerDetail_quanity = Quantity_client.Text;
                DateTime now = DateTime.Now;
                Createddate = now.ToString("yyyy-MM-dd HH:mm:ss");
                DealerDetail.Client_DealerDetail_createdDate = Createddate;
                SQLhelper.SQL_Database.Add_Client_DealerDetail(DealerDetail);
                //need to add stmp configuration
                await Send_email_smtp(Get_email_setup());
                await DisplayAlert("Mail", "send successfully", "OK");
               await  Navigation.PopAsync();
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
                 maildetail.Body = $"<html><body><p>Hello,</p><p>Name:{com_client.Client_name}<br />Phone Number:{com_client.Client_number}<br />Location:{com_client.Client_location}<br />cement:{Cement_client.Text}<br />Dealer:{Dealer_client.Text}<br />Quantity:{Quantity_client.Text}<br />Remark:<br />{com_client.Client_Remark}<br /></p><p>Regards,<br />{Createddate}</p></body></html>";
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