using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Daily_Report.Iteams
{
    [Table("Client_DealerDetail")]
    public class Client_DealerDetail
    {
        [PrimaryKey, AutoIncrement]
        public int Client_DealerDetail_Id { get; set; }
        public int NewClient_J_id { get; set; }
        [MaxLength(50)]
        public string Client_DealerDetail_cement { get; set; }
        [MaxLength(100)]
        public string Client_DealerDetail_dealer { get; set; }
        [MaxLength(10)]
        public string Client_DealerDetail_quanity { get; set; }
        public string Client_DealerDetail_createdDate { get; set; }
    }
}
