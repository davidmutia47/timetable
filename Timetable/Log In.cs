using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timetable
{
    public partial class Log_In : Form
    {
        private static String conString = "datasource=localhost;port=3306;username=root;password=dave;database=timetable2";
        MySqlConnection con = new MySqlConnection(conString);
        public Log_In()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, EventArgs e)
        {
            try
            {
                if(!tbLoginId.Text.Equals("") && !tbLoginPass.Text.Equals(""))
                using(MySqlConnection connection = con)
                {
                        if (connection.State.Equals(ConnectionState.Closed))
                            connection.Open();
                        using(MySqlCommand cmd= connection.CreateCommand())
                        {
                            cmd.CommandText = "Select * from users where id=@id and pass= md5(@pass)";
                            cmd.Parameters.AddWithValue("@id", tbLoginId.Text);
                            cmd.Parameters.AddWithValue("@pass", tbLoginPass.Text);
                            MySqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                this.Hide();
                                new Form1().Show();
                            }
                            else
                            {
                                MessageBox.Show("Log in failed, Check your log in credentials!!");
                            }
                        }
                }
                else
                {
                    MessageBox.Show("Fill in all the fields");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Log_In_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
