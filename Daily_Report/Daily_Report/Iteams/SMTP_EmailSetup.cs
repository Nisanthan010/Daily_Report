using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Daily_Report.Iteams
{
    [Table("SMTP_EmailSetup")]
    public class SMTP_EmailSetup
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string SenderEmail { get; set; }

        [MaxLength(50)]
        public string SmtpEmailHost { get; set; }

        [MaxLength(50)]
        public string SmtpPassword { get; set; }

        [MaxLength(10)]
        public string SmtpEmailPort { get; set; }

        [MaxLength(100)]
        public string ReceivedEmail { get; set; }

        public bool IsSent { get; set; }

    }
}
