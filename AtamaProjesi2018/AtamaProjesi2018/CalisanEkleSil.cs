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
    public partial class CalisanEkleSil : Form
    {
        public CalisanEkleSil()
        {
            InitializeComponent();
            var connectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            con = new SqlConnection(connectionString);
        }

        SqlConnection con;
        string ad="";
        string soyad="";
        string isBasiTarihi="";
        string kase="";
        string not="";
        int isJoker =0;
        string bantlamaP="";              // 1
        string klipTestP="";              // 2
        string elektrikselTestP="";       // 3
        string gozKontrolP="";            // 4
        string paketlemeP="";             // 5
        string malzemeTasimaP="";         // 6
        string modulHazirlamaP = "";      // 7
        string sicilNo="";
        string delSicilNo="";
        
        string kidem = "";

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // not bölümü
            not = textBox5.Text;

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // iş başı tarihi bölümü
            isBasiTarihi = textBox3.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // adı bölümü
            ad = textBox1.Text;

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // soyadı bölümü
            soyad = textBox2.Text;

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // kaşe bölümü
            kase = textBox4.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            // sicil numarası bölümü
            sicilNo = textBox6.Text;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // joker çalışan onay bölümü
            if (checkBox1.Checked == true)
                isJoker = 1;
            else
                isJoker = 0;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            // bantlama prosesi bölümü
            bantlamaP = textBox7.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            // klip test prosesi bölümü
            klipTestP = textBox8.Text;
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            // elektriksel test prosesi bölümü
            elektrikselTestP = textBox9.Text;
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            // göz kontrol prosesi bölümü
            gozKontrolP = textBox10.Text;
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            // paketleme prosesi bölümü
            paketlemeP = textBox11.Text;
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            // malzeme taşıma prosesi bölümü
            malzemeTasimaP = textBox12.Text;
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            // silinecek çalışanın sicil numarası bölümü
            delSicilNo = textBox13.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // çalışan ekle butonu
            if((ad=="" || ad==null) && (soyad=="" || soyad == null) && (isBasiTarihi=="" || isBasiTarihi==null) 
                && (sicilNo=="" || sicilNo==null))
                MessageBox.Show("Ad, Soyad, İş Başı Tarihi ve Sicil No bölümleri boş bırakılamaz!!");
            else
            {
                try
                {
                    Boolean sicilNoExist = false;
                    string query = "select*from Çalışan where SicilNo=@sicilNo";
                    SqlCommand cmdExist = new SqlCommand(query,con);                    
                    cmdExist.Parameters.AddWithValue("@sicilNo",sicilNo);
                    con.Open();
                    SqlDataReader dReader = cmdExist.ExecuteReader();

                    while (dReader.Read())
                    {
                        if (dReader.HasRows == true)
                        {
                            sicilNoExist = true;
                            break;
                        }
                    }
                    dReader.Close();
                    if (sicilNoExist == true)
                    {
                        MessageBox.Show(string.Format("{0} :Sicil numaralı çalışan zaten mevcut!",sicilNo));
                    }
                    else
                    {
                        calisanEkle(ad,soyad,isBasiTarihi,kase,not,sicilNo,bantlamaP,klipTestP,elektrikselTestP,gozKontrolP,paketlemeP,malzemeTasimaP,isJoker,kidem,modulHazirlamaP);
                    }
                    //dReader.Close();
                    con.Close();
                }
                catch(Exception ex) { MessageBox.Show(ex.Message); }

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // çalışanı sil butonu
            if (delSicilNo != null)
                deleteCalisan(delSicilNo);
            else
                MessageBox.Show("Silinecek Sicil Numarasını Giriniz!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // tüm çalışanları listele butonu

            
            try
            {
                string query = "select * from Çalışan";
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
            // joker çalışanları listele butonu
            try
            {
                string query = "select * from Çalışan where isjoker=@joker";
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                com.Parameters.AddWithValue("@joker", 1);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // çalışanların listelendiği bölüm
        }

        public void calisanEkle(string ad, string soyad, string isBasi, string kase, string not, string sicilNo,
            string bantlama, string kliptest, string elektrikseltest, string gozkontrol, string paketleme, string malzemetasima,
            int isjoker, string kidem , string modulhazirlama) {
            try
            {
                string query = "select count(ID) from Çalışan";
                SqlCommand cmd = new SqlCommand(query, con);
                //con.Open();
                int id = (int)cmd.ExecuteScalar();
                cmd.ExecuteNonQuery();
                con.Close();
                int currentid = 0;
                if (id == 0)
                {
                    currentid = 1;
                }
                else
                {
                    string queryLast = "select max(ID) from Çalışan";
                    SqlCommand findLast = new SqlCommand(queryLast, con);
                    con.Open();
                    int id2 = (int)findLast.ExecuteScalar();
                    findLast.ExecuteNonQuery();
                    currentid = id2 + 1;
                    con.Close();

                }
                string queryAdd = "insert into Çalışan (ID,Adı,Soyadı,SicilNo,İşBaşıTarihi,Kaşe,BilgiNotu,isJoker,BantlamaProses" +
                    ",KlipTestProses,ElektrikselTestProses,GözKontrolProses,PaketlemeProses,MalzemeTaşımaProses,ModulHazirlamaProses,Kıdem)" +
                    " values(@id,@adı,@soyadı,@sicilNo,@işbaşı,@kaşe,@bilginotu,@isjoker,@bantlama,@kliptest,@elektrikseltest" +
                    ",@gözkontrol,@paketleme,@malzemetaşıma,@modulhazirlama,@kıdem)";
                SqlCommand cmdAdd = new SqlCommand(queryAdd,con);
                con.Open();

                cmdAdd.Parameters.AddWithValue("@id",currentid);
                cmdAdd.Parameters.AddWithValue("@adı",ad);
                cmdAdd.Parameters.AddWithValue("@soyadı",soyad);
                cmdAdd.Parameters.AddWithValue("@işbaşı",isBasi);
                cmdAdd.Parameters.AddWithValue("@kaşe",kase);
                cmdAdd.Parameters.AddWithValue("@bilginotu",not);
                cmdAdd.Parameters.AddWithValue("@isjoker",isjoker);
                cmdAdd.Parameters.AddWithValue("@bantlama",bantlama);
                cmdAdd.Parameters.AddWithValue("@kliptest",kliptest);
                cmdAdd.Parameters.AddWithValue("@elektrikseltest",elektrikseltest);
                cmdAdd.Parameters.AddWithValue("@gözkontrol",gozkontrol);
                cmdAdd.Parameters.AddWithValue("@paketleme",paketleme);
                cmdAdd.Parameters.AddWithValue("@malzemetaşıma",malzemetasima);
                cmdAdd.Parameters.AddWithValue("@kıdem",kidem);
                cmdAdd.Parameters.AddWithValue("@sicilNo",sicilNo);
                cmdAdd.Parameters.AddWithValue("@modulhazirlama",modulhazirlama);
                cmdAdd.ExecuteNonQuery();
                MessageBox.Show(string.Format("( {0} ) Sicil Numaralı  ( {1} {2} )  Çalışan sisteme Eklendi",sicilNo,ad.ToUpper(),soyad.ToUpper()));
                clearBoxes();
                con.Close(); // hata kontrol 30.07.18
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void deleteCalisan(string sicilNo) {
            try {
                string findID = "select ID from Çalışan Where SicilNo=@sicilno";
                SqlCommand findcmd = new SqlCommand(findID,con);
                con.Open();
                findcmd.Parameters.AddWithValue("@sicilno",sicilNo);
                int findid = (int)findcmd.ExecuteScalar();
                findcmd.ExecuteNonQuery();
                con.Close();

                string queryDelCalisan = "delete from Devamsızlık where ÇalışanID=@delid";
                SqlCommand cmdDelCalisan = new SqlCommand(queryDelCalisan,con);
                con.Open();
                cmdDelCalisan.Parameters.AddWithValue("@delid",findid);
                cmdDelCalisan.ExecuteNonQuery();
                con.Close();

                string query = "delete from Çalışan where SicilNo=@sicilNo";
                SqlCommand delCom = new SqlCommand(query, con);
                con.Open();
                delCom.Parameters.AddWithValue("@sicilNo",sicilNo);
                delCom.ExecuteNonQuery();
                MessageBox.Show(string.Format("{0}  Sicil Numaralı çalışan silindi.",sicilNo));
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public void clearBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox17.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Normal çalışanları listele butonu
            try
            {
                string query = "select * from Çalışan where isjoker=@joker";
                SqlCommand com = new SqlCommand(query,con);
                con.Open();
                com.Parameters.AddWithValue("@joker", 0);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();

            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        string araAd="";
        string araSoyisim="";
        string araSicilno="";
        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            // Aranan isim
            araAd = textBox14.Text;
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            // Aranan soyisim
            araSoyisim = textBox15.Text;
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            // Aranan sicil No
            araSicilno = textBox16.Text;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Arama butonu
            try
            {
                string query = "select * from Çalışan where Adı=@adı or Soyadı=@soyadı or SicilNo=@sicilNo";
                SqlCommand cmd = new SqlCommand(query,con);
                con.Open();
                cmd.Parameters.AddWithValue("@adı",araAd);
                cmd.Parameters.AddWithValue("@soyadı",araSoyisim);
                cmd.Parameters.AddWithValue("@sicilNo", araSicilno);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            // modül hazırlama proses
            modulHazirlamaP = textBox17.Text;
        }
    }
}
