using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daily_Report.Iteams;
using Daily_Report.Services;
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
        private void update_client_Clicked(object sender, EventArgs e)
        {
            try
            {
                Client_DealerDetail DealerDetail = new Client_DealerDetail();
                DealerDetail.NewClient_J_id = com_client.New_client_Id;
                DealerDetail.Client_DealerDetail_cement = Cement_client.Text;
                DealerDetail.Client_DealerDetail_dealer = Dealer_client.Text;
                DealerDetail.Client_DealerDetail_quanity = Quantity_client.Text;
                DateTime now = DateTime.Now;
                string formattedDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                DealerDetail.Client_DealerDetail_createdDate = formattedDate;
                SQLhelper.SQL_Database.Add_Client_DealerDetail(DealerDetail);
                //need to add stmp configuration
                DisplayAlert("Mail", "send successfully", "OK");
                Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                DisplayAlert("Contact Developer", ex.Message, "OK");
            }
        }
    }
}