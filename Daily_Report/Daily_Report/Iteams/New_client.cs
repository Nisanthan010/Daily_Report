using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Daily_Report.Iteams
{
    [Table("New_client")]
    public class New_client
    {
        [PrimaryKey, AutoIncrement]
        public int New_client_Id { get; set; }

        [MaxLength(100)]
        public string Client_name { get; set; }

        [MaxLength(20)]
        public string Client_number { get; set; }
        [MaxLength(100)]
        public string Client_location { get; set; }
        [MaxLength(100)]
        public string Client_Remark { get; set; }    
        public string Client_createddate { get; set;}
    }
}
