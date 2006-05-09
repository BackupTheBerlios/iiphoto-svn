using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace Photo
{
    class Db
    {
        private string DBfile;
        private SQLiteConnection conn;

        public Db(string path)
        {
            DBfile = path;
        }

        public void CreateDB()
        {
            if (System.IO.File.Exists(DBfile) == false)
            {
                SQLiteConnection.CreateFile(DBfile);
            }
        }

        public void Connect()
        {
            this.CreateDB();
            this.conn = new SQLiteConnection("Data Source=" + DBfile + ";Version=3;");
            conn.Open();
        }

        public void close()
        {
            conn.Close();
        }
    }
}
