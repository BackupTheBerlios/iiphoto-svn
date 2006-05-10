using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace ConsoleApplication1
{
    class MySqlite {
        private string DBfile;
        private SQLiteConnection conn;

        public MySqlite(string path) {
            DBfile = path;
        }

        public void createDB() {
            if (System.IO.File.Exists(DBfile) == false)
            {
                SQLiteConnection.CreateFile(DBfile);
            }
        }

        public void connect() {
            this.createDB();
            this.conn = new SQLiteConnection("Data Source="+DBfile+";Version=3;");
            conn.Open();
        }

        public void close() {
            conn.Close();
        }

        public void fastInserts(string tableName, string parameters, string[,] fields)
        {
            using (SQLiteTransaction dbTrans = conn.BeginTransaction())
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "insert into " + tableName + " values(" + parameters + ");";

                    SQLiteParameter[] sqlparams = new SQLiteParameter[fields.GetLength(1)];

                    for (int i = 0; i < fields.GetLength(1); i++) {
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

        public SQLiteDataAdapter getAdapter(string query, bool forEdit) {
            SQLiteDataAdapter sqladapt = new SQLiteDataAdapter(query, conn);
            DataSet dataSet = new DataSet("Dane");
            if (forEdit)
            {
                new SQLiteCommandBuilder(sqladapt);
            }
            return sqladapt;
        }

        public DataSet getData(SQLiteDataAdapter ad)
        {
            DataSet dataSet = new DataSet("Dane");
            ad.Fill(dataSet);
            return dataSet;
        }

        public DataSet select(string select)
        {
            SQLiteDataAdapter sqladapt = new SQLiteDataAdapter(select, conn);
            DataSet dataSet = new DataSet("Dane");
            sqladapt.Fill(dataSet);
            return dataSet;
        }

        public void executeQuery(string query)
        {
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            MySqlite db = new MySqlite("mydb.db3");
            db.connect();

            try
            {
                db.executeQuery("create table users(" +
                                "id integer primary key autoincrement," +
                                "username varchar(32)," +
                                "password varchar(128))");
            } catch (SQLiteException e) {
                Console.WriteLine("{0}", e.Message);
            }

            string[,] users = new string[50, 2];
            for (int n = 0; n < 50; n++) {
                users[n, 0] = "user_" + n.ToString();
                users[n, 1] = "pass_" + n.ToString();
            }

            db.fastInserts("users", "NULL, ?,?", users);

            DataSet dataSet = db.select("select * from users;");
            foreach (DataTable t in dataSet.Tables)
            {
                Console.WriteLine("Tabela {0} zawiera {1} wiersze", t.TableName, t.Rows.Count);
                foreach (DataRow r in t.Rows)
                {
                    Console.Write("-> ");
                    foreach (DataColumn c in t.Columns)
                        Console.Write("{0}={1}, ", c.ColumnName, r[c.ColumnName]);
                    Console.WriteLine();
                }
            }

            dataSet.Dispose();

            SQLiteDataAdapter da = db.getAdapter("select * from users;", true);
            DataSet ds = db.getData(da);

            DataRow[] drows = ds.Tables[0].Select("username = 'user_30'");
            for (int i = 0; i < drows.Length; i++)
            {
                drows[i].BeginEdit();
                drows[i]["password"] = "NOWE HASLO!";
                drows[i].EndEdit();
            }
            Console.WriteLine("zmodyfikowano: {0} wierszy", da.Update(ds));
            ds.Dispose();
            da.Dispose();

            dataSet = db.select("select * from users;");
            foreach (DataTable t in dataSet.Tables)
            {
                Console.WriteLine("Tabela {0} zawiera {1} wiersze", t.TableName, t.Rows.Count);
                foreach (DataRow r in t.Rows)
                {
                    Console.Write("-> ");
                    foreach (DataColumn c in t.Columns)
                        Console.Write("{0}={1}, ", c.ColumnName, r[c.ColumnName]);
                    Console.WriteLine();
                }
            }

            db.close();
            Console.ReadLine();
        }
    }
}
