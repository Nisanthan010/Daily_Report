using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Daily_Report.views;
using System.IO;

namespace Daily_Report.Services
{
    public class SQLhelper
    {
        public SQLiteConnection _connection;

        private static SqlServices _CONNECTION;
        public static SqlServices SQL_Database
        {
            get
            {
                if (_CONNECTION == null)
                {
                    string Path_database = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string file_name = "database1.db3";
                    string Data_table_path = Path.Combine(Path_database, file_name);
                    _CONNECTION = new SqlServices(Data_table_path);
                }
                return _CONNECTION;
            }
        }


    }
}
