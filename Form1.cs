using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Feladat
{
    public partial class Form1 : Form
    {
        private MySqlConnection con;

        public Form1()
        {
            InitializeComponent();
            con = new MySqlConnection("Server=localhost;Port=3306;Database=todolist;User ID=luca;");


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BindData();
        }



        void BindData()
        {
            con.Open();
            using (MySqlCommand mySqlCommand = new MySqlCommand("Select * from listtable", con))
            {
                using (MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand))
                {
                    DataTable table = new DataTable();
                    mySqlDataAdapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Minden mező kitöltése kötelező.");
                return; 
            }

            con.Open();
            SqlCommand cnn = new SqlCommand("Insert into listtable(title, description) values(@title, @description)");
            cnn.Parameters.AddWithValue("@title", textBox1.Text);
            cnn.Parameters.AddWithValue("@description", textBox2.Text);

            try
            {
                cnn.ExecuteNonQuery();
                MessageBox.Show("Adatok hozzá lettek adva");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba az adatok hozzáadása során: " + ex.Message);
            }
            finally
            {
                con.Close();
                BindData(); 
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("A kereséshez adja meg a címet.");
                return;
            }

            con.Open();
            SqlCommand cnn = new SqlCommand("Select * from listtable where title = @title");
            cnn.Parameters.AddWithValue("@title", textBox1.Text);

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cnn);
                DataTable table = new DataTable();
                da.Fill(table);
                dataGridView1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba a keresés során: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            BindData();
            con.Open();
            using (MySqlCommand mySqlCommand = new MySqlCommand("Select * from listtable where title = @title", con))
            {
                mySqlCommand.Parameters.AddWithValue("@title", textBox1.Text);
                using (MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand))
                {
                    DataTable table = new DataTable();
                    mySqlDataAdapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
            con.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("A törléshez adja meg a címet.");
                return;
            }

            con.Open();
            SqlCommand cnn = new SqlCommand("Delete from listtable where title = @title");
            cnn.Parameters.AddWithValue("@title", textBox1.Text);

            try
            {
                cnn.ExecuteNonQuery();
                MessageBox.Show("Törlésre került");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba a törlés során: " + ex.Message);
            }
            finally
            {
                con.Close();
                BindData();
            }
            con.Open();
            using (MySqlCommand mySqlCommand = new MySqlCommand("Delete from listtable where title = @title", con))
            {
                mySqlCommand.Parameters.AddWithValue("@title", textBox1.Text);
                mySqlCommand.ExecuteNonQuery();
            }
            con.Close();
            BindData();
            MessageBox.Show("Törlésre került");
        }
    }
}
