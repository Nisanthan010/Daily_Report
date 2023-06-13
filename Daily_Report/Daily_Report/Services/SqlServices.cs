using SQLite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using Daily_Report.Iteams;
using System.Threading.Tasks;
using System.Linq;

namespace Daily_Report.Services
{
    public class SqlServices
    {
        public SQLiteConnection Task_DB;
        public SqlServices(string DatabasePath)
        {
            Task_DB = new SQLiteConnection(DatabasePath);
          
            Db_table();
        }
        public void Db_table()
        {
            Task_DB.CreateTable<SMTP_EmailSetup>();
            Task_DB.CreateTable<New_client>();
            Task_DB.CreateTable<Client_DealerDetail>();
        }
        public void Delete_Db()
        {
            string query1 = "drop table Client_DealerDetail";

            string query2 = "drop table New_client";
            Task_DB.Query<New_client>(query1);
            Task_DB.Query<New_client>(query2);
            // Task_DB.Close();
            // GC.Collect();
            //GC.WaitForPendingFinalizers();
            //File.Delete(DatabasePath);

        }

        #region SMTP_SETUP
        public int Get_smtpRecordCount()
        {
            string query = "SELECT COUNT(*) FROM SMTP_EmailSetup";
            // Execute the query and retrieve the count
            int count = Task_DB.ExecuteScalar<int>(query);
            return count;
        }
        public int Get_smtpRecordMaxID()
        {
            string query = "SELECT MAX(Id) FROM SMTP_EmailSetup";
            // Execute the query and retrieve the count
            int count = Task_DB.ExecuteScalar<int>(query);
            return count;
        }
        public void Insert_SMTPRecord()
        {
            int count = Get_smtpRecordCount();
            if (count < 1)
            {
                SMTP_EmailSetup smtp_email = new SMTP_EmailSetup
                {
                    SenderEmail = "nisanthantrail@gmail.com",
                    SmtpEmailHost = "smtp.gmail.com",
                    SmtpPassword = "aajyzddkgthwxapg",
                    SmtpEmailPort = "587",
                    ReceivedEmail = "nikinisanthan@gmail.com",
                    IsSent = true
                };
                // Insert the record into the table
                Task_DB.Insert(smtp_email);
               
            }
            else
            {
                if (count > 1)
                {
                    Task_DB.Query<SMTP_EmailSetup>("DELETE FROM SMTP_EmailSetup");

                    SMTP_EmailSetup smtp_email = new SMTP_EmailSetup
                    {
                        SenderEmail = "nisanthantrail@gmail.com",
                        SmtpEmailHost = "smtp.gmail.com",
                        SmtpPassword = "aajyzddkgthwxapg",
                        SmtpEmailPort = "587",
                        ReceivedEmail = "nikinisanthan@gmail.com",
                        IsSent = true
                    };
                    // Insert the record into the table
                    Task_DB.Insert(smtp_email);
                }
            }
        }
        public int Update_SMTP_EmailSetup(SMTP_EmailSetup Email_Set)
        {
            int count = Get_smtpRecordCount();
            if (count > 1)
            {
                Task_DB.Query<SMTP_EmailSetup>("DELETE FROM SMTP_EmailSetup");
                Insert_SMTPRecord();
            }
            
            Email_Set = Email_sendDetail();
            return  Task_DB.Update(Email_Set);
        }
        public SMTP_EmailSetup Email_sendDetail()
        {
            var count = Get_smtpRecordCount();
            var smtp_email = Task_DB.Table<SMTP_EmailSetup>().FirstOrDefault(x => x.Id == count);
            return smtp_email;
        }
        #endregion
        #region NEW_CLIENT
        public List<New_client> Get_New_client()
        {
           return  Task_DB.Query<New_client>("SELECT * FROM New_client");

        }
        public List<New_client> Get_New_clientDate(DateTime Get_date )
        {
            //return Task_DB.Query<New_client>("SELECT * FROM New_client");
            DateTime now = Get_date;
            string formattedDate = now.ToString("yyyy-MM-dd");

            string query = $"SELECT * FROM New_client WHERE Client_createddate like '%{formattedDate}%'";
            //List<ResultType> results = connection.Query<ResultType>(query);
              return Task_DB.Query<New_client>(query);
        }

        public int Add_New_client(New_client New_client)
        {
            return  Task_DB.Insert(New_client);
        }
        public int Update_New_client(New_client New_client)
        {
            return  Task_DB.Update(New_client);
        }
        public int Delete_New_client(New_client New_client)
        {
            return Task_DB.Delete(New_client);
        }
        public List<New_client> DeleteAll_New_client()
        {
            return Task_DB.Query<New_client>("DELETE  FROM New_client");
        }
        #endregion
        #region Client_DealerDetail
        public List<Client_DealerDetail> Get_Client_DealerDetail()
        {
            return Task_DB.Query<Client_DealerDetail>("SELECT * FROM Client_DealerDetail");
        }
        
        public int Add_Client_DealerDetail(Client_DealerDetail Client_DealerDetail)
        {
            return Task_DB.Insert(Client_DealerDetail);
        }
        public int Update_Client_DealerDetail(Client_DealerDetail Client_DealerDetail)
        {
            return Task_DB.Update(Client_DealerDetail);
        }
        public int Delete_Client_DealerDetail(Client_DealerDetail Client_DealerDetail)
        {
            return Task_DB.Delete(Client_DealerDetail);
        }
        public List<Client_DealerDetail> DeleteAll_Client_DealerDetail()
        {
            return Task_DB.Query<Client_DealerDetail>("DELETE  FROM Client_DealerDetail");
        }
        #endregion
        #region cvs_EXCEL_generate
        public List<Get_client_excel> Get_Client_excel()
        {
            string query = "SELECT * FROM New_client LEFT JOIN Client_DealerDetail ON New_client.New_client_Id = Client_DealerDetail.NewClient_J_id";
            List<Get_client_excel> results = Task_DB.Query<Get_client_excel>(query);
            return results;
        }
        public List<Get_client_excel> Get_Client_excelwithdate(DateTime min_date, DateTime max_date)
        {
            List<Get_client_excel> results = new List<Get_client_excel> ();
            int maxQuery=0;
            int minQuery=0;
            int count=0, tcount=0;
            string minu="";
            string maxu="";
            string min = min_date.ToString("yyyy-MM-dd");
            string max = max_date.ToString("yyyy-MM-dd");

            for (int i=0; tcount <= 0 ; i++)
            {
                if (min != max)
                {
                    DateTime originalDate = min_date;
                    DateTime increaseDate = originalDate.AddDays(+i);

                    minu = increaseDate.ToString("yyyy-MM-dd");

                    string querymin = $"SELECT COUNT(*) FROM New_client where Client_createddate like '%{minu}%' ";
                    Console.WriteLine($"{i} {minu}");
                    tcount = Task_DB.ExecuteScalar<int>(querymin);
                    count=tcount;
                    Console.WriteLine(tcount);
                }
                else
                {
                    minu = min_date.ToString("yyyy-MM-dd");

                    string querymin = $"SELECT COUNT(*) FROM New_client where Client_createddate like '%{minu}%' ";
                    Console.WriteLine($"{i} {minu}");
                    count = Task_DB.ExecuteScalar<int>(querymin);
                    Console.WriteLine(tcount);
                    tcount = 1;
                }
            }        
            // Execute the query and retrieve the count
            if (count > 0)
            {
                 minQuery = Task_DB.Table<New_client>()
                 .Where(x => x.Client_createddate.Contains(minu))
                 .Min(x => x.New_client_Id);
            }

            tcount = 0;
            for (int i = 0; tcount <= 0; i++)
            {
                if (min != max)
                {
                    DateTime originalDate = max_date;
                    DateTime reducedDate = originalDate.AddDays(-i);
                    maxu = reducedDate.ToString("yyyy-MM-dd");
                    Console.WriteLine($"{i} {maxu}");
                    string querymax = $"SELECT COUNT(*) FROM New_client where Client_createddate like '%{maxu}%' ";
                    // Execute the query and retrieve the count
                    tcount = Task_DB.ExecuteScalar<int>(querymax);
                    count = tcount;
                    Console.WriteLine(tcount);
                }
                else
                {
                    maxu = max_date.ToString("yyyy-MM-dd");

                    string querymin = $"SELECT COUNT(*) FROM New_client where Client_createddate like '%{maxu}%' ";
                    Console.WriteLine($"{i} {maxu}");
                    count = Task_DB.ExecuteScalar<int>(querymin);
                    tcount=1;
                }
            }
            if (count > 0)
            {
                 maxQuery = Task_DB.Table<New_client>()
               .Where(x => x.Client_createddate.Contains(maxu))
               .Max(x => x.New_client_Id);
            }
            
            if (minQuery <= maxQuery )
            {
                string query = $"SELECT * FROM New_client LEFT JOIN Client_DealerDetail ON New_client.New_client_Id = Client_DealerDetail.NewClient_J_id WHERE New_client.New_client_Id BETWEEN {minQuery} AND {maxQuery} ";
                 results = Task_DB.Query<Get_client_excel>(query);
            }
            else 
            {
                maxQuery = minQuery;
                string query = $"SELECT * FROM New_client LEFT JOIN Client_DealerDetail ON New_client.New_client_Id = Client_DealerDetail.NewClient_J_id WHERE New_client.New_client_Id BETWEEN {minQuery} AND {maxQuery} ";
                results = Task_DB.Query<Get_client_excel>(query);
            }
            return results;
        }
        #endregion
    }
}
