using Daily_Report.Services;
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
        
        private void generate_csv(object sender, EventArgs e)
        {
            try
            {

                Excel_client_collection.ItemsSource = SQLhelper.SQL_Database.Get_Client_excelwithdate(Fromdate, Todate);

            }
            catch (Exception ex)
            {
                DisplayAlert("Contact Developer", ex.Message, "OK");
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
    }
}