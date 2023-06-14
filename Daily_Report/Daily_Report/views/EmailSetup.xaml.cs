using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Daily_Report.Services;
using Daily_Report.Iteams;

namespace Daily_Report.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailSetup : ContentPage
    {
        public EmailSetup()
        {
            InitializeComponent();
        }

        SMTP_EmailSetup smtp_mail = new SMTP_EmailSetup();
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (SQLhelper.SQL_Database.Get_smtpRecordCount() == 0)
            {
                SQLhelper.SQL_Database.Insert_SMTPRecord();
            }
            smtp_mail = SQLhelper.SQL_Database.Email_sendDetail();
            Reciever_mail.Text = smtp_mail.ReceivedEmail;
            Sender_mail.Text = smtp_mail.SenderEmail;
            Smtp_password.Text = smtp_mail.SmtpPassword;
        }

        private void smtp_saved_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(Smtp_password.Text)) 
                {
                    SQLhelper.SQL_Database.Insert_SMTPRecord();
                }
                else
                {
                    smtp_mail = SQLhelper.SQL_Database.Email_sendDetail();
                    if(smtp_mail != null)
                    {
                         smtp_mail.ReceivedEmail= Reciever_mail.Text;
                         smtp_mail.SenderEmail= Sender_mail.Text;
                         smtp_mail.SmtpPassword = Smtp_password.Text;
                        SQLhelper.SQL_Database.Update_SMTP_EmailSetup(smtp_mail);
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                DisplayAlert("Contact Developer", ex.Message, "OK");
            }

        }
    }
}