using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace AtamaProjesi2018
{
    public partial class HatEkle : Form
    {
        public HatEkle()
        {
            InitializeComponent();
            var connectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            con = new SqlConnection(connectionString);
        }

        SqlConnection con;

        string hatAdı="";
        string formenAdı="";
        string formenSoyadı="";
        string malzemeci="";
        string delHatAdı="";

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Hat adı bölümü
            hatAdı = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Formen adı bölümü
            formenAdı = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Formen soyadı bölümü
            formenSoyadı = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // Malzemeci bölümü
            malzemeci = textBox4.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // Silinecek hat adı bölümü
            delHatAdı = textBox5.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Ekle butonu
            if (hatAdı == null || hatAdı == "")
                MessageBox.Show("Hat Adı boş bırakılamaz");
            else
            {
                try
                {
                    Boolean bolumExist = false;
                    string query = "select*from Hat where HatAdı=@hatadı";
                    SqlCommand cmdExist = new SqlCommand(query, con);
                    cmdExist.Parameters.AddWithValue("@hatadı", hatAdı);
                    con.Open();
                    SqlDataReader dReader = cmdExist.ExecuteReader();

                    while (dReader.Read())
                    {
                        if (dReader.HasRows == true)
                        {
                            bolumExist = true;
                            break;
                        }
                    }dReader.Close();
                    con.Close();
                    if (bolumExist == true)
                    {
                        MessageBox.Show(string.Format("{0} :Adlı hat zaten mevcut!", hatAdı));
                    }
                    else
                    {
                        hatEkle(hatAdı,formenAdı,formenSoyadı,malzemeci);
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Hat listesi bölümü
            try
            {
                string query = "select * from Hat";
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Sil bölümü
            try
            {
                string query = "delete from Hat where HatAdı=@hatAdı";
                SqlCommand delCom = new SqlCommand(query, con);
                con.Open();
                delCom.Parameters.AddWithValue("@hatAdı", delHatAdı);
                delCom.ExecuteNonQuery();
                MessageBox.Show(string.Format("{0}  hat silindi.", delHatAdı));
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public void hatEkle(string hatadı,string formenadı,string formensoyadı,string malzemeci)
        {
            try
            {
                string querymax = "select count(ID) from Hat";
                SqlCommand cmdmax = new SqlCommand(querymax, con);
                con.Open();
                int id = (int)cmdmax.ExecuteScalar();
                cmdmax.ExecuteNonQuery();
                con.Close();
                int currentid = 0;
                if (id == 0)
                {
                    currentid = 1;
                }
                else
                {
                    string queryLast = "select max(ID) from Hat";
                    SqlCommand findLast = new SqlCommand(queryLast, con);
                    con.Open();
                    int id2 = (int)findLast.ExecuteScalar();
                    findLast.ExecuteNonQuery();
                    currentid = id2 + 1;
                    con.Close();

                }
                /*string queryLast2 = "select max(ID) from Bölümler";
                SqlCommand findLast2 = new SqlCommand(queryLast2, con);
                con.Open();
                int id = (int)findLast2.ExecuteScalar();
                int currentid = 0;
                findLast2.ExecuteNonQuery();
                con.Close();
                currentid = id + 1;*/

                string query = "insert into Hat(ID,HatAdı,FormenAdı,FormenSoyadı,Malzemeci) values(@id,@hatadı,@formenadı,@formensoyadı,@malzemeci)";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", currentid);
                cmd.Parameters.AddWithValue("@hatadı",hatadı);
                cmd.Parameters.AddWithValue("@formenadı",formenadı);
                cmd.Parameters.AddWithValue("@formensoyadı", formensoyadı);
                cmd.Parameters.AddWithValue("@malzemeci", malzemeci);
                cmd.ExecuteNonQuery();
                MessageBox.Show(string.Format("( {0} ) adlı hat eklendi", hatadı));
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
