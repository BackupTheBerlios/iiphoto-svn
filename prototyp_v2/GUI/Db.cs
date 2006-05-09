using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

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

        public void Close()
        {
            conn.Close();
        }

        public void FastInserts(string tableName, string parameters, string[,] fields)
        {
            using (SQLiteTransaction dbTrans = conn.BeginTransaction())
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "insert into " + tableName + " values(" + parameters + ");";

                    SQLiteParameter[] sqlparams = new SQLiteParameter[fields.GetLength(1)];

                    for (int i = 0; i < fields.GetLength(1); i++)
                    {
                        sqlparams[i] = cmd.CreateParameter();
                        cmd.Parameters.Add(sqlparams[i]);
                    }

                    for (int i = 0; i < fields.GetLength(0); i++)
                    {
                        for (int j = 0; j < fields.GetLength(1); j++)
                        {
                            sqlparams[j].Value = fields[i, j];
                        }
                        cmd.ExecuteNonQuery();
                    }
                    dbTrans.Commit();
                }
            }
        }

        public SQLiteDataAdapter GetAdapter(string query, bool forEdit)
        {
            SQLiteDataAdapter sqladapt = new SQLiteDataAdapter(query, conn);
            DataSet dataSet = new DataSet("Dane");
            if (forEdit)
            {
                new SQLiteCommandBuilder(sqladapt);
            }
            return sqladapt;
        }

        public DataSet GetData(SQLiteDataAdapter ad)
        {
            DataSet dataSet = new DataSet("Dane");
            ad.Fill(dataSet);
            return dataSet;
        }

        public DataSet Select(string select)
        {
            SQLiteDataAdapter sqladapt = new SQLiteDataAdapter(select, conn);
            DataSet dataSet = new DataSet("Dane");
            sqladapt.Fill(dataSet);
            return dataSet;
        }

        public void ExecuteQuery(string query)
        {
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
        }
    }
}
