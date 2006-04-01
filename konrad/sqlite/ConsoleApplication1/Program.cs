using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace ConsoleApplication1
{
    class Program
    {
        static void CreateTable(SQLiteConnection cnn)
        {
            SQLiteCommand cmd = cnn.CreateCommand();

            cmd.CommandText = "create table users(" +
                              "id integer primary key autoincrement," +
                              "username varchar(32)," +
                              "password varchar(128))";

            cmd.ExecuteNonQuery();
        }
        static void FastInsert(SQLiteConnection cnn)
        {
            using (SQLiteTransaction dbTrans = cnn.BeginTransaction())
            {
                using (SQLiteCommand cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = "insert into users VALUES(NULL,?,?)";
                    SQLiteParameter Field1 = cmd.CreateParameter();
                    SQLiteParameter Field2 = cmd.CreateParameter();
                    cmd.Parameters.Add(Field1);
                    cmd.Parameters.Add(Field2);
                    for (int n = 0; n < 50; n++)
                    {
                        Field1.Value = "user_" + n.ToString();
                        Field2.Value = "password_" + n.ToString();
                        cmd.ExecuteNonQuery();
                    }
                    dbTrans.Commit();
                }
            }
        }
        static private SQLiteCommand prepareUpdate(SQLiteConnection cnn)
        {
            SQLiteCommand cmd = cnn.CreateCommand();

            cmd.CommandText = "UPDATE users SET username = @userName, password = @pass WHERE id = @id";

            cmd.Parameters.Add(new SQLiteParameter("@userName"));
            cmd.Parameters.Add(new SQLiteParameter("@pass"));
            cmd.Parameters.Add(new SQLiteParameter("@id"));

            return cmd;
        }

        static public void UpdateUser(SQLiteCommand cmd, string username, string pass, string id)
        {
            cmd.Parameters["@userName"].Value = username;
            cmd.Parameters["@pass"].Value = pass;
            cmd.Parameters["@id"].Value = id;

            cmd.ExecuteNonQuery();
        }

        static public void SelectUsers(SQLiteConnection cnn)
        {
            SQLiteDataAdapter sqladapt = new SQLiteDataAdapter("SELECT * FROM users;", cnn);
            DataSet dataSet = new DataSet("Dane");

            sqladapt.Fill(dataSet);

            Console.WriteLine("DataSet {0} zawiera {1} tabele", dataSet.DataSetName, dataSet.Tables.Count);

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
        }

        static void Main(string[] args)
        {
            if (System.IO.File.Exists("mydb.db3") == false)
            {
                SQLiteConnection.CreateFile("mydb.db3");
            }
            SQLiteConnection conn = new SQLiteConnection("Data Source=mydb.db3;Version=3;");
            conn.Open();

            try
            {
                Program.CreateTable(conn);
            } catch (SQLiteException e) {
                Console.WriteLine("{0}", e.Message);
            }

            Program.FastInsert(conn);

            SQLiteCommand cmd = Program.prepareUpdate(conn);
            for (int i = 5; i < 10; i++)
            {
                Program.UpdateUser(cmd, "updatedName_" + i.ToString(), "updatedPass_" + i.ToString(), i.ToString());
            }

            Program.SelectUsers(conn);

            conn.Close();
            Console.ReadLine();
        }
    }
}
