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
    public partial class JokerGuncelle : Form
    {
        public JokerGuncelle()
        {
            InitializeComponent();

            var connectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            con = new SqlConnection(connectionString);
        }

        SqlConnection con;

        string id="";
        string bantlama = "";
        string kliptest = "";
        string elektrikselkontrol = "";
        string gozkontrol = "";
        string paketleme = "";
        string malzemetasima = "";
        string modulhazirlama = "";


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // çalışan id
            id = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // bantlama proses
            bantlama = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // klip test proses
            kliptest = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // elektriksel test proses
            elektrikselkontrol = textBox4.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // gös kontrol proses
            gozkontrol = textBox5.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            // paketleme proses
            paketleme = textBox6.Text;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            // malzeme taşıma proses
            malzemetasima = textBox7.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            // modul hazırlama proses
            modulhazirlama = textBox8.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // jokerleri listele 

            try
            {
                string query = "select*from Çalışan where isJoker=1";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // joker çalışan bilgilerini güncelle
            try
            {
                string query = "update Çalışan set BantlamaProses=@bantlama,KlipTestProses=@kliptest,ElektrikselTestProses=@elektriksel," +
                    "GözKontrolProses=@gözkontrol,PaketlemeProses=@paketleme,MalzemeTaşımaProses=@malzemetasıma," +
                    "ModulHazirlamaProses=@modulhazırlama where ID=@id";
                SqlCommand cmd = new SqlCommand(query,con);
                con.Open();

                cmd.Parameters.AddWithValue("@id",id);
                cmd.Parameters.AddWithValue("@bantlama",bantlama);
                cmd.Parameters.AddWithValue("@kliptest",kliptest);
                cmd.Parameters.AddWithValue("@elektriksel",elektrikselkontrol);
                cmd.Parameters.AddWithValue("@gözkontrol",gozkontrol);
                cmd.Parameters.AddWithValue("@paketleme",paketleme);
                cmd.Parameters.AddWithValue("@malzemetasıma",malzemetasima);
                cmd.Parameters.AddWithValue("@modulhazırlama",modulhazirlama);
                cmd.ExecuteNonQuery();
                con.Close();
                clearBoxex();

                MessageBox.Show("Çalışan Bilgisi Güncellenmiştir");

            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void clearBoxex()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();

        }
    }
}
