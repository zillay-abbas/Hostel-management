using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
namespace sza_Databaselink_app
{
    public partial class home : Form
    {
        private OleDbConnection connection = new OleDbConnection();
        public home()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Files-Data\MS Access\management_system.accdb;
Persist Security Info=False;";
        }

        private void home_Load(object sender, EventArgs e)
        {
            connection.Open();
            MessageBox.Show("Connect successfully");
            connection.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            connection.Open();
            var comm = new OleDbCommand();
            comm.Connection = connection;
            comm.CommandText = "select * from [admin-data] where Username='" + textBox1.Text + "' and Password='" + textBox2.Text + "'";
            OleDbDataReader rd = comm.ExecuteReader();
            int count = 0;
            while (rd.Read())
            {
                count++;
            }
            if (count == 0)
            {
                MessageBox.Show("Invalid Password");
            }
            else if (count == 1)
            {
                MessageBox.Show("Login Successfully");
                this.Hide();
                var f = new Form2();
                f.Show();
            }
            connection.Close();
        }
    }
}
