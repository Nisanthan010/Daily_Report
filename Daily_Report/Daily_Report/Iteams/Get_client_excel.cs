using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Daily_Report.Iteams
{
    [Table("Get_client_excel")]
    public class Get_client_excel
    {
        //public int New_client_Id { get; set; }
        public string Client_name { get; set; }
        public string Client_number { get; set; }
        public string Client_location { get; set; }
       // public DateTime Client_createddate { get; set; }
       //  public int Client_DealerDetail_Id { get; set; }
       // public int NewClient_J_id { get; set; }
        public string Client_DealerDetail_cement { get; set; }
        public string Client_DealerDetail_dealer { get; set; }
        public string Client_DealerDetail_quanity { get; set; }
        //public DateTime Client_DealerDetail_createdDate { get; set; }

       //Need details for excel
        public string Client_Remark { get; set; }
        public DateTime Client_createddate { get; set; }
       
    }
}
