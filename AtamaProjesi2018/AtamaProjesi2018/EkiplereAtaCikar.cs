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
    public partial class EkiplereAtaCikar : Form
    {
        public EkiplereAtaCikar()
        {
            InitializeComponent();
            var connectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            con = new SqlConnection(connectionString);
        }

        SqlConnection con;

        string calisanid = "";
        string ekipid = "";
        string silinecekid = "";

        private void button1_Click(object sender, EventArgs e)
        {
            // uygun çalışanların listesi
            string today = DateTime.Now.ToString("yyyy/MM/dd");
            try
            {
                string query = "select c.ID,Adı,Soyadı,BolumID from Çalışan c left join Devamsızlık d on c.ID=d.ÇalışanID where tarih>@today and tarih!=@today or tarih is null and c.isJoker=0 and c.EkipID is null";
                SqlCommand cmd = new SqlCommand(query,con);
                con.Open();
                cmd.Parameters.AddWithValue("@today",today);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();

            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // ekip listesi
            
            try
            {
                string query = "select * from Ekip";
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                con.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // çalışan id
            calisanid = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // ekip id
            ekipid = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // ekiplerden çıkarılacak çalışan id
            silinecekid = textBox3.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // ekibe ata butonu
            

            if ((calisanid == null || calisanid == "") && (ekipid == null || ekipid == ""))
                MessageBox.Show("Çalışan ID ve Ekip ID boş bırakılamaz");
            else
            {

                Boolean calisanCheck = false;
                string queryCheck = "select*from Çalışan where ID=@id";
                SqlCommand cmdCheck = new SqlCommand(queryCheck, con);
                con.Open();
                cmdCheck.Parameters.AddWithValue("@id", calisanid);
                SqlDataReader checkReader = cmdCheck.ExecuteReader();
                while (checkReader.Read())
                {
                    if (checkReader.HasRows == true)
                    {
                        calisanCheck = true;
                        break;
                    }
                    checkReader.Close();
                }
                con.Close();

                Boolean ekipCheck = false;
                string queryCheck2 = "select*from Ekip where ID=@ekipid";
                SqlCommand cmdCheck2 = new SqlCommand(queryCheck2, con);
                cmdCheck2.Parameters.AddWithValue("@ekipid", ekipid);
                con.Open();
                SqlDataReader checkReader2 = cmdCheck2.ExecuteReader();
                while (checkReader2.Read())
                {
                    if (checkReader2.HasRows == true)
                    {
                        ekipCheck = true;
                        break;
                    }
                    checkReader2.Close();
                }
                con.Close();

                if (calisanCheck == true && ekipCheck == true)
                {
                    try
                    {
                        string query1 = "update Çalışan set EkipID=@ekipid where ID=@id";
                        SqlCommand cmd = new SqlCommand(query1, con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@id", calisanid);
                        cmd.Parameters.AddWithValue("@ekipid", ekipid);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Ekip Ataması Tamamlandı");
                        con.Close();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                else
                    MessageBox.Show("Geçersiz Çalışan ID veya Ekip ID");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // çıkar butonu
            if (silinecekid == null || silinecekid == "")
                MessageBox.Show("Silinecek Çalışanın ID si boş bırakılamaz");
            else
            {
                

                Boolean delcont = false;
                string delCheck = "select*from Çalışan where ID=@id";
                SqlCommand cmdCheck = new SqlCommand(delCheck, con);
                con.Open();
                cmdCheck.Parameters.AddWithValue("@id", silinecekid);
                SqlDataReader delReader = cmdCheck.ExecuteReader();
                while (delReader.Read())
                {
                    if (delReader.HasRows == true)
                    {
                        delcont = true;
                        break;
                    }
                    delReader.Close();
                }
                con.Close();
                if (delcont == true)
                {
                    try
                    {
                        string query = "update Çalışan set EkipID=@ekipid where ID=@id";
                        SqlCommand cmd = new SqlCommand(query, con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@id", silinecekid);
                        cmd.Parameters.AddWithValue("@ekipid", DBNull.Value);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Çalışanın ekibi silindi");
                        con.Close();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                else
                    MessageBox.Show("Böyle bir ID bulunmamaktadır");
            }
        
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // joker çalışan listesi

            string today = DateTime.Now.ToString("yyyy/MM/dd");
            try
            {
                string query = "select*from Çalışan c left join Devamsızlık d on c.ID=d.ÇalışanID where tarih>@today and tarih!=@today or tarih is null and c.isJoker=1 and c.EkipID is null";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.Parameters.AddWithValue("@today", today);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // tüm normal çalışanlar
            try
            {
                string query = "select ID,Adı,Soyadı,SicilNo,isJoker,EkipID from Çalışan where isJoker=0";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            // tüm joker çalışanlar
            
            try
            {
                string query = "select ID,Adı,Soyadı,SicilNo,isJoker,EkipID from Çalışan where isJoker=1";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
    }
}
