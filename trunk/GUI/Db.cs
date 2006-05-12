using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;

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

        public void StworzBD()
        {
            if (System.IO.File.Exists(DBfile) == false)
            {
                SQLiteConnection.CreateFile(DBfile);

                try
                {                                       
                    //SQLiteTransaction dd = conn.BeginTransaction()
                    //MessageBox.Show("przed utworzono cd");

                    using (SQLiteTransaction dbTrans = conn.BeginTransaction())
                    {                     
                        WykonajQuery("create table CD" +
                                     "(" +
                                          "serial varchar(50) primary key," +
                                          "nazwa varchar(100)" +
                                     ");");

                        WykonajQuery("create table Zdjecie" +
                                     "(" +
                                          "id_zdjecia integer primary key autoincrement," +
                                          "sciezka varchar(200)," +
                                          "data_dodania date," +
                                          "data_wykonania date," +
                                          "komentarz varchar(250)," +
                                          "autor varchar(30)," +
                                          "nazwa_pliku varchar(50)," +
                                          "orientacja integer," +
                                          "cd varchar(50) references CD(serial) on delete cascade on update cascade" +
                                     ");");


                        WykonajQuery("create table Tag" +
                                     "(" +
                                          "id_tagu integer primary key autoincrement," +
                                          "nazwa varchar(100) ," +
                                          "album integer" +
                                     ");");

                        WykonajQuery("create table TagZdjecia" +
                                     "(" +
                                          "id_zdjecia integer references Zdjecie(id_zdjecia)," +
                                          "id_tagu integer references Tag(id_tagu)" +
                                     ");");

                        //*/
                        MessageBox.Show("utworzono baze");

                        dbTrans.Commit();

                    }                    
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);                    
                }
            }
        }

        public void Polacz()
        {
            this.StworzBD();
            this.conn = new SQLiteConnection("Data Source=" + DBfile + ";Version=3;");
            conn.Open();
        }

        public void Rozlacz()
        {
            conn.Close();
        }

        public void SzybkieInserty(string tableName, string parameters, string[,] fields)
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

        public SQLiteDataAdapter PobierzAdapter(string query, bool forEdit)
        {
            SQLiteDataAdapter sqladapt = new SQLiteDataAdapter(query, conn);
            DataSet dataSet = new DataSet("Dane");
            if (forEdit)
            {
                new SQLiteCommandBuilder(sqladapt);
            }
            return sqladapt;
        }

        public DataSet PobierzDane(SQLiteDataAdapter ad)
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

        public void WykonajQuery(string query)
        {
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
        }
        public void Insert(string tableName, string parameters)
        {
            using (SQLiteTransaction dbTrans = conn.BeginTransaction())
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "insert into " + tableName + " values(" + parameters + ");";

                    cmd.ExecuteNonQuery();

                    dbTrans.Commit();
                }
            }
        }

        public void Insert_czesci(string tableName, string pola, string parameters)
        {
            using (SQLiteTransaction dbTrans = conn.BeginTransaction())
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "insert into " + tableName + "(" + pola + ") values(" + parameters + ");";

                    MessageBox.Show("insert into " + tableName + "(" + pola + ") values(" + parameters + ");");

                    cmd.ExecuteNonQuery();

                    dbTrans.Commit();
                }
            }
        }

        public void Delete_zawartosci_tabeli(string tableName)
        {
            using (SQLiteTransaction dbTrans = conn.BeginTransaction())
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "delete from " + tableName + ";";

                    cmd.ExecuteNonQuery();

                    dbTrans.Commit();
                }
            }


        }

        public void Delete(string tableName, string po_where)
        {
            using (SQLiteTransaction dbTrans = conn.BeginTransaction())
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "delete from " + tableName + " where " + po_where + ";";

                    cmd.ExecuteNonQuery();

                    dbTrans.Commit();
                }
            }


        }

        public void Update(string tableName, string parameters)
        {
            using (SQLiteTransaction dbTrans = conn.BeginTransaction())
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "update " + tableName + " set " + parameters + " ;";

                    cmd.ExecuteNonQuery();

                    dbTrans.Commit();
                }
            }

        }
    }
}
