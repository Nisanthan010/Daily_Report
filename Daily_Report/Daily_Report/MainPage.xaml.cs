using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Daily_Report.views;
using Daily_Report.Services;

namespace Daily_Report
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void New_client_button(object sender, EventArgs e)
        {
          await Navigation.PushAsync(new NewClient());
        }

        private async void Exist_client_clicked(object sender, EventArgs e)
        {
            //SQLhelper.SQL_Database.Delete_Db();
            await Navigation.PushAsync(new ExistClient());
        }

        private async void excel_generate_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExcelGenerateReport());
        }

        private async void Email_setup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EmailSetup());
        }
    }
}
