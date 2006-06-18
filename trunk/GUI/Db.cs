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
    /// <summary>
    /// Klasa do korzystania z bazy danych SQLite. Zawiera wszystkie metody potrzebne do obs³ugi bazy danych
    /// </summary>
    class Db
    {
        private string DBfile;
        private SQLiteConnection conn;

        /// <summary>
        /// Konstrunktor. Je¿eli baza jeszcze nie jest stworzona to jest tworzona
        /// </summary>
        public Db()
        {
            DBfile = Config.katalogAplikacji + "\\" + Config.plikBazy;

            if (System.IO.File.Exists(DBfile) == false)
            {
                SQLiteConnection.CreateFile(DBfile);
                this.Polacz();
                this.UtworzTabele();
                this.Rozlacz();
            }
        }

        /// <summary>
        /// Metoda tworz¹ca wszystkie tabele bazy potrzebne w aplikacji
        /// </summary>
        private void UtworzTabele()
        {
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
                                      "nazwa varchar(100)," +
                                      "album integer," +
                                      "unique(nazwa,album)" +
                                 ");");

                    WykonajQuery("create table TagZdjecia" +
                                 "(" +
                                      "id_zdjecia integer references Zdjecie(id_zdjecia)," +
                                      "id_tagu integer references Tag(id_tagu)" +
                                 ");");

                    //*/
                    //MessageBox.Show("utworzono baze");

                    dbTrans.Commit();

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        /// <summary>
        /// Metoda która zestawia po³aczenie z plikiem bazy
        /// </summary>
        public void Polacz()
        {
            this.conn = new SQLiteConnection("Data Source=" + DBfile + ";Version=3;");    
            conn.Open();
        }

        /// <summary>
        /// Metoda koñcz¹ca po³¹czenie z plikiem bazy
        /// </summary>
        public void Rozlacz()
        {
            conn.Close();
        }

        /// <summary>
        /// Metoda do szybkiego wstawiania du¿ej iloœci danych
        /// </summary>
        /// <param name="fields">tablica dwu-wymiarowa z polami tworz¹cymi kolejne rekordy</param>
        /// <param name="parameters">string z parametrami jakie wpisujemy do tabeli</param>
        /// <param name="tableName">nazwa tabeli</param>
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

        /// <summary>
        /// Metoda która wykonuje zapytanie sql i zwraca adapter z którego u¿ytkownik bêdzie móg³ wy³uskaæ dane jakie potrzebuje
        /// </summary>
        /// <param name="forEdit">pole okreœlaj¹ce czy ma byæ edytowalne czy nie</param>
        /// <param name="query">zapytanie sql</param>
        /// <returns>zwraca SQLiteDataAdapter</returns>
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

        /// <summary>
        /// Metoda zwracaj¹ca DataSet. Pobiera dane z SQLiteDataAdapter-a
        /// </summary>
        /// <param name="ad">SQLiteDataAdapter</param>
        /// <returns>zwraca DataSet</returns>
        public DataSet PobierzDane(SQLiteDataAdapter ad)
        {
            DataSet dataSet = new DataSet("Dane");
            ad.Fill(dataSet);
            return dataSet;
        }

        /// <summary>
        /// Metoda wykonuj¹ca zapytanie typu 'select' i zwracaj¹ca dane w obiekcie DataSet
        /// </summary>
        /// <param name="select">zapytanie sql typu 'select'</param>
        /// <returns>zwraca wynik w DataSet</returns>
        public DataSet Select(string select)
        {
            SQLiteDataAdapter sqladapt = new SQLiteDataAdapter(select, conn);
            DataSet dataSet = new DataSet("Dane");
            //MessageBox.Show(select);
            sqladapt.Fill(dataSet);
            return dataSet;
        }

        /// <summary>
        /// Metoda wykonuj¹ca zapytanie sql
        /// </summary>
        /// <param name="query">zapytanie sql</param>
        public int WykonajQuery(string query)
        {
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Metoda wstawiaj¹ca rekord do tabeli
        /// </summary>
        /// <param name="parameters">string z parametrami</param>
        /// <param name="tableName">nazwa tabeli</param>
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

        /// <summary>
        /// Metoda wstawiaj¹ca rekord ale nie trzeba podawaæ wszystkich parametrów
        /// </summary>
        /// <param name="parameters">string z parametrami</param>
        /// <param name="pola">string z polami</param>
        /// <param name="tableName">nazwa tabeli</param>
        public void Insert_czesci(string tableName, string pola, string parameters)
        {
            using (SQLiteTransaction dbTrans = conn.BeginTransaction())
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "insert into " + tableName + "(" + pola + ") values(" + parameters + ");";

                    //MessageBox.Show("insert into " + tableName + "(" + pola + ") values(" + parameters + ");");

                    cmd.ExecuteNonQuery();

                    dbTrans.Commit();
                }
            }
        }

        /// <summary>
        /// Metoda usuwaj¹ca zawartoœæ tabeli
        /// </summary>
        /// <param name="tableName">nazwa tabeli</param>
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

        /// <summary>
        /// Metoda usuwaj¹ca rekordy o spe³niej¹cych warunkach
        /// </summary>
        /// <param name="po_where">warunki jakie musz¹ byæ spe³nione aby rekord zosta³ usuniêty</param>
        /// <param name="tableName">nazwa tabeli</param>
        public void Delete(string tableName, string po_where)
        {
            using (SQLiteTransaction dbTrans = conn.BeginTransaction())
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "delete from " + tableName + " where " + po_where + ";";

                    //MessageBox.Show("delete from " + tableName + " where " + po_where + ";");

                    cmd.ExecuteNonQuery();

                    dbTrans.Commit();
                }
            }
        }

        /// <summary>
        /// Metoda uaktualniaj¹ca rekordy 
        /// </summary>
        /// <param name="parameters">parametry jakie wystêpuja po 'set'</param>
        /// <param name="tableName">nazwa tabeli</param>
        public void Update(string tableName, string parameters)
        {
            using (SQLiteTransaction dbTrans = conn.BeginTransaction())
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "update " + tableName + " set " + parameters + " ;";
                    //Console.WriteLine(cmd.CommandText);
                    cmd.ExecuteNonQuery();

                    dbTrans.Commit();
                }
            }
        }
    }
}
