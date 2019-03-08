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
    public partial class DevamsizlikGirisi : Form
    {
        public DevamsizlikGirisi()
        {
            InitializeComponent();
            var connectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            con = new SqlConnection(connectionString);
        }

        SqlConnection con;

        string adi="";
        string soyadi="";
        string sicilNo="";
        string suresi="";
        string sebebi="";
        string aramaAdi="";
        string aramaSoyadi="";
        string aramaSicilno="";
        string devamsizlikId="";

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // adı bölümü
            adi = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // soyadı bölümü
            soyadi = textBox2.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // sicil no bölümü
            sicilNo = textBox5.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // süresi bölümü
            suresi = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // sebebi bölümü
            sebebi = textBox4.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            // arama adı
            aramaAdi = textBox6.Text;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            // arama soyadı
            aramaSoyadi = textBox7.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            // arama sicil no
            aramaSicilno = textBox8.Text;
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            // devamsızlık id bölümü
            devamsizlikId = textBox9.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // onayla buton
            if (sicilNo == null || sicilNo == "")
                MessageBox.Show("Sicil No boş bırakılamaz");
            else
            {
                devamsizligGirisi(sebebi, suresi, sicilNo);
                
                try
                {
                    string query = "select EkipID from Çalışan where SicilNo=@sicilno";
                    SqlCommand cmdBul = new SqlCommand(query, con);
                    con.Open();
                    cmdBul.Parameters.AddWithValue("@sicilno", sicilNo);
                    int ekipid = (int)cmdBul.ExecuteScalar();
                    //cmdBul.ExecuteNonQuery();
                    con.Close();

                if (ekipid != 0)
                {
                   DialogResult result= MessageBox.Show("Devamsızlık bilgisi girilen çalışan bir ekipte bulunmaktadır.",
                       " Joker çalışan tablosuna yönlendiriliyorsunuz!!",MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        YerineAtamaYap f1 = new YerineAtamaYap(sicilNo);
                        f1.Show();
                    }
                    else if (result == DialogResult.No)
                    {

                    }
                }

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // iptal butonu
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // ara butonu
            try
            {
                string query = "select * from Çalışan where Adı=@adı or Soyadı=@soyadı or SicilNo=@sicilNo";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.Parameters.AddWithValue("@adı", aramaAdi);
                cmd.Parameters.AddWithValue("@soyadı", aramaSoyadi);
                cmd.Parameters.AddWithValue("@sicilNo", aramaSicilno);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // sil butonu
            try
            {
                string query = "delete from Devamsızlık where ID=@id";
                SqlCommand delCom = new SqlCommand(query, con);
                con.Open();
                delCom.Parameters.AddWithValue("@id", devamsizlikId);
                delCom.ExecuteNonQuery();
                con.Close();
                MessageBox.Show(string.Format("{0} id'li devamsızlık kaydı silindi.", devamsizlikId));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // devamsızlık listesini getir
            try
            {
                string query = "select * from Devamsızlık d inner join Çalışan c on d.ÇalışanID=c.ID";
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
        public void devamsizligGirisi(string sebebi,string süresi,string sicilno)
        {
            string today = DateTime.Now.ToString("yyyy/MM/dd");
            try
            {
                string querymax = "select count(ID) from Devamsızlık";
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
                    string queryLast = "select max(ID) from Devamsızlık";
                    SqlCommand findLast = new SqlCommand(queryLast, con);
                    con.Open();
                    int id2 = (int)findLast.ExecuteScalar();
                    findLast.ExecuteNonQuery();
                    currentid = id2 + 1;
                    con.Close();

                }

                string queryFind = "select * from Çalışan where SicilNo=@sicilno";
                SqlCommand cmdFind = new SqlCommand(queryFind, con);
                cmdFind.Parameters.AddWithValue("@sicilno",sicilno);
                con.Open();
                int findId = (int)cmdFind.ExecuteScalar();
                cmdFind.ExecuteNonQuery();
                con.Close();

                string query = "insert into Devamsızlık(ID,Sebebi,Süresi,Tarih,ÇalışanID) values(@id,@sebebi,@süresi,@tarih,@çalışanid)";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", currentid);
                cmd.Parameters.AddWithValue("@sebebi", sebebi);
                cmd.Parameters.AddWithValue("@süresi", süresi);
                cmd.Parameters.AddWithValue("@tarih", today);
                cmd.Parameters.AddWithValue("çalışanid",findId);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show(string.Format("( {0} {1} ) adlı çalışanın devamsızlık bilgisi eklendi",adi,soyadi));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }    
    }
}
