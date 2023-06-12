using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Daily_Report.Iteams;
using Daily_Report.Services;

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

        private void NewClient_save_clicked(object sender, EventArgs e)
        {
            try
            {
                Manadatoryfield();
                if (checkname == "Y" && checkphoneNumber=="Y")
                {
                    New_client client = new New_client();
                    client.Client_name = Client_name.Text;
                    client.Client_number = Phone_number.Text;
                    client.Client_location = Location.Text;
                    client.Client_Remark = Remark_newclient.Text;
                    DateTime now = DateTime.Now;
                     string formattedDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                    client.Client_createddate = formattedDate;
                    SQLhelper.SQL_Database.Add_New_client(client);
                    Client_name.Text = string.Empty;
                    Phone_number.Text = string.Empty;
                    Location.Text = string.Empty;
                    Remark_newclient.Text = string.Empty;
                    //need to add stmp configuration
                    DisplayAlert("Mail", "send successfully", "OK");
                    Navigation.PopAsync();
                }       
            }
            catch (Exception ex) 
            {
                DisplayAlert("Contact Developer", ex.Message, "OK");
            }                
        }
    }
}