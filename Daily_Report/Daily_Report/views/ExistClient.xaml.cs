using Daily_Report.Interfaces;
using Daily_Report.Services;
using Daily_Report.Iteams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Daily_Report.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExistClient : ContentPage
    {
        public ExistClient()
        {
            InitializeComponent();
        }
        private DateTime myDatePicker;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            myDatePicker = DateTime.Now;
            DatePicker_client.Date = myDatePicker;
           New_client_collection.ItemsSource = SQLhelper.SQL_Database.Get_New_clientDate(myDatePicker);
        }
       public New_client Last_selected;
        private async void New_client_collection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Last_selected = e.CurrentSelection.FirstOrDefault() as New_client;
                if (Last_selected != null)
                {
                    Application.Current.Properties["Last_selected"] = Last_selected;
                    // Store the selected item in the session
                    
                    //Selected_iteam.Client = Last_selected;
                }
                    await Navigation.PushAsync(new ExistClientEdit_EC());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Contact Developer", ex.Message, "OK");
            }
           
        }

        private void DatePicker_client_DateSelected(object sender, DateChangedEventArgs e)
        {
            myDatePicker = e.NewDate;
           // myDatePicker.ToString("yyyy-MM-dd HH:mm:ss");
            New_client_collection.ItemsSource = SQLhelper.SQL_Database.Get_New_clientDate(myDatePicker);
        }
    }
}