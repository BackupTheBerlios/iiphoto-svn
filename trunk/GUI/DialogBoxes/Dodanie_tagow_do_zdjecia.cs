using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace Photo.DialogBoxes
{
    public partial class Dodanie_tagow_do_zdjecia : Form
    {
        public Dodanie_tagow_do_zdjecia()
        {
            InitializeComponent();
        }


        private void Wypelnij()
        {
            Db baza = new Db();
            baza.Polacz();
            try
            {

            }
            catch (SqlException ex)
            {
                MessageBox.Show("blad bazy");
            }

            baza.Rozlacz();
        }
    }
}