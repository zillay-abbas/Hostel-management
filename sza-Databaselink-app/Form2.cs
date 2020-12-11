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
    public partial class Form2 : Form
    {
        private OleDbConnection connection = new OleDbConnection();
        public Form2()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Files-Data\MS Access\management_system.accdb;
Persist Security Info=False;";
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            connection.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connection;
            string query = "select ID,Name from student_det";
            string query2 = "select Room_No,No_of_Seats from room_det";
            cmd.CommandText = query;
            var da = new OleDbDataAdapter(cmd);
            DataTable ta = new DataTable();
            da.Fill(ta);
            dataGridView2.DataSource = ta;
            cmd.CommandText = query2;
            var da2 = new OleDbDataAdapter(cmd);
            var ta2 = new DataTable();
            da2.Fill(ta2);
            dataGridView1.DataSource = ta2;
            connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            home h = new home();
            h.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connection.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connection;
            cmd.CommandText = "select * from room_det where Room_No=" + textBox10.Text;
            OleDbDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string totalseats = reader["No_of_Seats"].ToString();
                string resSeats = reader["Reserved_Seats"].ToString();
                int tSeats = Convert.ToInt32(totalseats);
                int reseats = Convert.ToInt32(resSeats);
                if (tSeats == reseats)
                {
                    MessageBox.Show("Seats of this room is full check another room");
                }
                else
                {
                    var command = new OleDbCommand();
                    command.Connection = connection; 
                    command.CommandText = "insert into student_det (Name,Room_No,Submitted_Fee,Remaining_Fee) values('" + textBox9.Text + "','" + textBox10.Text + "','" + textBox11.Text + "','" + textBox12.Text + "')";
                    command.ExecuteNonQuery();
                    MessageBox.Show("Data Saved");
                    OleDbCommand comd = new OleDbCommand();   // student id showing
                    comd.Connection = connection;
                    comd.CommandText = "";
                    string seatinc=reader["Reserved_Seats"].ToString();
                    string seatdec=reader["Empty_Seats"].ToString();
                    int empDec=Convert.ToInt32(seatdec);
                    int resInc = Convert.ToInt32(seatinc);
                    resInc += 1;
                    empDec -= 1;
                    command.CommandText="update room_det set Reserved_Seats="+resInc+",Empty_Seats="+empDec+" where Room_No="+textBox10.Text;
                    command.ExecuteNonQuery();
                    textBox13.Text = textBox9.Text = textBox10.Text = textBox11.Text = textBox12.Text = null;
                }
            }
            else
            {
                MessageBox.Show("There is no room fist create room for students");
            }
            reader.Close(); 
            OleDbCommand cmnd = new OleDbCommand();
            cmnd.Connection = connection;
            string qury = "select ID,Name from student_det";
            cmnd.CommandText = qury;
            var da = new OleDbDataAdapter(cmnd);
            DataTable ta = new DataTable();
            da.Fill(ta);
            dataGridView2.DataSource = ta;
            connection.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool vis = false;
            connection.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connection;
            string qurey = "Select * from student_det where Name='"+ textBox6.Text+"'";
            cmd.CommandText = qurey;
            OleDbDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                textBox5.Text = reader["Name"].ToString();
                textBox14.Text = reader["ID"].ToString();
                textBox4.Text = reader["Room_no"].ToString();
                textBox7.Text = reader["Submitted_Fee"].ToString();
                textBox8.Text = reader["Remaining_Fee"].ToString();
            }
            else
            {
                vis = true;
                MessageBox.Show("Record Not Found");
                textBox6.Text = null;
            }
            connection.Close();
            if (vis == false)
            {
                button5.Visible = true;
            }
            else if (vis == true)
            {
                button5.Visible = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            del_box db = new del_box();
            if (db.ShowDialog() == DialogResult.Yes)
            {
                connection.Open();
                var cmd = new OleDbCommand();
                var comd = new OleDbCommand();
                cmd.Connection = connection;
                comd.Connection = connection;
                comd.CommandText = "select * from room_det where Room_no=" + textBox4.Text;
                OleDbDataReader reader = comd.ExecuteReader();
                reader.Read();
                string seatdec = reader["Reserved_Seats"].ToString();
                string seatinc = reader["Empty_Seats"].ToString();
                int empinc = Convert.ToInt32(seatinc);
                int resdec = Convert.ToInt32(seatdec);
                resdec -= 1;
                empinc += 1;
                string query = "delete from student_det where Name='" + textBox6.Text + "'";
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student is removed.");
                cmd.CommandText = "update room_det set Reserved_Seats=" + resdec + ",Empty_Seats=" + empinc + " where Room_No=" + textBox4.Text;
                cmd.ExecuteNonQuery();
                textBox4.Text = textBox5.Text = textBox6.Text = textBox7.Text = textBox14.Text = textBox8.Text = null;
                connection.Close();
            }
            connection.Open();
            OleDbCommand cmnd = new OleDbCommand();
            cmnd.Connection = connection;
            string qury = "select ID,Name from student_det";
            cmnd.CommandText = qury;
            var da = new OleDbDataAdapter(cmnd);
            DataTable ta = new DataTable();
            da.Fill(ta);
            dataGridView2.DataSource = ta;
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connection;
            if (textBox1.Text == null || textBox4.Text == null || textBox7.Text == null || textBox8.Text == null || textBox14.Text == null)
            {
                MessageBox.Show("Fill Empty Space's");
            }
            else
            {
                string query = "update student_det set Name='" + textBox5.Text + "' ,Room_No=" + textBox4.Text + " ,Submitted_Fee=" + textBox7.Text + " ,Remaining_Fee=" + textBox8.Text + " where ID=" + textBox14.Text;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Updated");
                OleDbCommand cmnd = new OleDbCommand();
                cmnd.Connection = connection;
                string qury = "select ID,Name from student_det";
                cmnd.CommandText = qury;
                var da = new OleDbDataAdapter(cmnd);
                DataTable ta = new DataTable();
                da.Fill(ta);
                dataGridView2.DataSource = ta;
                connection.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox15.Text == "" || textBox16.Text == "")
            {
                MessageBox.Show("Invalid Values.");
            }
            else
            {
            connection.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connection; 
            cmd.CommandText="Select * from Room_det where Room_No="+textBox15.Text;
            OleDbDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                MessageBox.Show("This Room is already exists if you want to update press update button");
            }
            else
            {
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "Insert into Room_det (Room_No,No_of_Seats,Empty_Seats,Reserved_Seats) values(" + textBox15.Text + "," + textBox16.Text + "," + textBox16.Text + ","+'0'+")";
                command.ExecuteNonQuery();
                MessageBox.Show("Room Created");
                textBox15.Text = textBox16.Text = null;
            }
            connection.Close();
        }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox15.Text == "" || textBox16.Text == "")
            {
                MessageBox.Show("First fill the above.");
            }
            else
            {
            connection.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connection; 
            cmd.CommandText="select * from room_det where Room_no="+textBox15.Text;
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string res_comp=reader["Reserved_Seats"].ToString();
                int res_s=Convert.ToInt32(res_comp);
                int text_comp=Convert.ToInt32(textBox16.Text);
                if (text_comp >= res_s)
                {//update
                    var command = new OleDbCommand();
                    command.Connection = connection;
                    int emptySeat=text_comp-res_s;
                    command.CommandText="update room_det set No_of_Seats="+textBox16.Text+",Empty_Seats="+ emptySeat+" where Room_no="+textBox15.Text;
                    command.ExecuteNonQuery();
                    MessageBox.Show("Room Seats are updated");
                    textBox15.Text = textBox16.Text = "";
                }
                else
                {
                    MessageBox.Show("Entered seats of this room is less than reserved seats first remove students form the room to update");
                }
            }
            else
            {
                MessageBox.Show("This Room is not created first create this room");
            }
            connection.Close();
        }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox17.Text == "")
            {
                MessageBox.Show("Enter Room number to search");
            }
            else
            {
                connection.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;
                cmd.CommandText = "Select * from Room_det where Room_no=" + textBox17.Text;
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    textBox2.Text = reader["No_of_Seats"].ToString();
                    textBox1.Text = reader["Empty_Seats"].ToString();
                    textBox3.Text = reader["Reserved_Seats"].ToString();
                }
                else
                {
                    MessageBox.Show("Room with this Number does not exists try another");
                    textBox2.Text = textBox3.Text = textBox1.Text = textBox17.Text = null;
                }
                connection.Close();
            }
        } 
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView1.CurrentRow.Selected = true;
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["No_of_Seats"].Value.ToString();
                textBox17.Text = dataGridView1.Rows[e.RowIndex].Cells["Room_no"].Value.ToString();
                string str = e.RowIndex.ToString();
                MessageBox.Show(str);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox17.Text == "")
            {
                MessageBox.Show("Fisrt Enter the room number to delete that room.");
            }
            else
            {
            connection.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = connection;
            cmd.CommandText = "Select * from room_det where Room_no=" + textBox17.Text;
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string resSeats = reader["Reserved_Seats"].ToString();
                int rS = Convert.ToInt32(resSeats);
                if (rS != 0)
                {
                    MessageBox.Show("Before removing room first remove the reserved seats");
                }
                else
                {
                    var com = new OleDbCommand();
                    com.Connection = connection;
                    com.CommandText = "delete * from room_det where Room_no=" + textBox17.Text;
                    com.ExecuteNonQuery();
                    MessageBox.Show("Room Number " + textBox17.Text + " is removed from Hostel");
                    textBox17.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Room not found. First create the room");
            }
            connection.Close();
        }
        }
    }
}
