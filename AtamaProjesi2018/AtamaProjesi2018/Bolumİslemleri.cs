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
    public partial class Bolumİslemleri : Form
    {
        public Bolumİslemleri()
        {
            InitializeComponent();
            var connectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            con = new SqlConnection(connectionString);
        }
        string calisanid = "";
        string bolumid = "";
        string silinecekid = "";

        SqlConnection con;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // normal çalışan listesi
            try
            {
                string query = "select ID,Adı,Soyadı,SicilNo,BolumID,IstasyonID from Çalışan where isjoker=@joker";
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                com.Parameters.AddWithValue("@joker", 0);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // atanacak çalışan id
            calisanid = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // bölüm id
            bolumid = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // bölümü silinecek çalışan id
            silinecekid = textBox3.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // atama butonu

            if ((calisanid == null || calisanid == "") && (bolumid == null || bolumid == ""))
                MessageBox.Show("Çalışan ID ve Bölüm ID boş bırakılamaz.");
            else
            {

                Boolean bolumNoExist = false;
                string query = "select*from Bölümler where ID=@id";
                SqlCommand cmdExist = new SqlCommand(query, con);
                cmdExist.Parameters.AddWithValue("@id", bolumid);
                con.Open();
                SqlDataReader dReader = cmdExist.ExecuteReader();

                while (dReader.Read())
                {
                    if (dReader.HasRows == true)
                    {
                        bolumNoExist = true;
                        break;
                    }

                    dReader.Close();
                }
                con.Close();

                Boolean userCheck = false;
                string query2 = "select*from Çalışan where ID=@id";
                SqlCommand cmd2 = new SqlCommand(query2,con);
                cmd2.Parameters.AddWithValue("@id",calisanid);
                con.Open();
                SqlDataReader userReader = cmd2.ExecuteReader();
                while (userReader.Read())
                {
                    if (userReader.HasRows == true)
                    {
                        userCheck = true;
                        break;
                    }
                    userReader.Close();
                }con.Close();

                if (bolumNoExist == true && userCheck==true) {
                    try
                    {
                        string query1 = "update Çalışan set BolumID=@bolumid where ID=@id";
                        SqlCommand cmd = new SqlCommand(query1, con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@id", calisanid);
                        cmd.Parameters.AddWithValue("@bolumid", bolumid);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Bölüm Ataması Tamamlandı");
                        con.Close();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                else
                {
                    MessageBox.Show("Geçersiz Bölüm ID veya Çalışan ID");
                }
                
                /*try
                {
                    string query = "update Çalışan set BolumID=@bolumid where ID=@id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@id", calisanid);
                    cmd.Parameters.AddWithValue("@bolumid", bolumid);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bölüm Ataması Tamamlandı");

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }*/
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // sil butonu
            if (silinecekid == null || silinecekid == "")
                MessageBox.Show("Silinecek Çalışanın İd si boş bırakılamaz");
            else
            {

                Boolean delcont= false;
                string delCheck = "select*from Çalışan where ID=@id";
                SqlCommand cmdCheck = new SqlCommand(delCheck,con);
                con.Open();
                cmdCheck.Parameters.AddWithValue("@id",silinecekid);
                SqlDataReader delReader = cmdCheck.ExecuteReader();
                while (delReader.Read())
                {
                    if (delReader.HasRows == true)
                    {
                        delcont = true;
                        break;
                    }
                    delReader.Close();
                }con.Close();
                if (delcont == true)
                {
                    try
                    {
                        string query = "update Çalışan set BolumID=@bolumid where ID=@id";
                        SqlCommand cmd = new SqlCommand(query, con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@id", silinecekid);
                        cmd.Parameters.AddWithValue("@bolumid", DBNull.Value);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Çalışanın bölümü silindi");
                        con.Close();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                else
                    MessageBox.Show("Böyle bir ID bulunmamaktadır");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // bölümlerin listesi
            try
            {
                string query = "select * from Bölümler";
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

        string istasyonCalID = "";
        string istasyonID = "";
        string delCalİstasyonID = "";
        string id;

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // istasyona atanacak çalışan id
            istasyonCalID = textBox4.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // istasyonun id si
            istasyonID = textBox5.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // istasyona atama butonu

            if ((istasyonCalID == null || istasyonCalID == "") && (istasyonID == null || istasyonID == ""))
                MessageBox.Show("Çalışan ID ve İstasyon ID boş bırakılamaz.");
            else
            {

                Boolean istasyonNoExist = false;
                string query = "select*from Istasyon where ID=@id";
                SqlCommand cmdExist = new SqlCommand(query, con);
                cmdExist.Parameters.AddWithValue("@id", istasyonID);
                con.Open();
                SqlDataReader dReader = cmdExist.ExecuteReader();

                while (dReader.Read())
                {
                    if (dReader.HasRows == true)
                    {
                        istasyonNoExist = true;
                        break;
                    }

                    dReader.Close();
                }
                con.Close();

                Boolean userCheck = false;
                string query2 = "select*from Çalışan where ID=@id";
                SqlCommand cmd2 = new SqlCommand(query2, con);
                cmd2.Parameters.AddWithValue("@id", istasyonCalID);
                con.Open();
                SqlDataReader userReader = cmd2.ExecuteReader();
                while (userReader.Read())
                {
                    if (userReader.HasRows == true)
                    {
                        userCheck = true;
                        break;
                    }
                    userReader.Close();
                }
                con.Close();

                Boolean alreadyUsed = false;
                string query3 = "select*from Çalışan where IstasyonID=@istasyonid";
                SqlCommand cmd3 = new SqlCommand(query3, con);
                cmd3.Parameters.AddWithValue("@istasyonid",istasyonID);
                con.Open();
                SqlDataReader usedReader = cmd3.ExecuteReader();
                while (usedReader.Read())
                {
                    if(usedReader.HasRows==true)
                    {
                        alreadyUsed = true;
                        break;
                    }
                    usedReader.Close();
                }
                con.Close();

                if (istasyonNoExist == true && userCheck == true && alreadyUsed == false)
                {
                    try
                    {
                        string query1 = "update Çalışan set IstasyonID=@istasyonid where ID=@id";
                        SqlCommand cmd = new SqlCommand(query1, con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@id", istasyonCalID);
                        cmd.Parameters.AddWithValue("@istasyonid", istasyonID);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("İstasyon Ataması Tamamlandı");
                        con.Close();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                else
                {
                    MessageBox.Show("Geçersiz İstasyon ID veya Çalışan ID veya İstasyon zaten kullanımda");
                }

                /*try
                {
                    string query = "update Çalışan set BolumID=@bolumid where ID=@id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@id", calisanid);
                    cmd.Parameters.AddWithValue("@bolumid", bolumid);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bölüm Ataması Tamamlandı");

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }*/
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // istasyonu sıralanacak bölüm comboboxı

            id = comboBox1.SelectedValue.ToString();
            listeyukle(id);
            bolumCalisanlari(id);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // çalışanın istasyonunu sil butonu

            if (delCalİstasyonID == null || delCalİstasyonID == "")
                MessageBox.Show("İstasyonu silinecek Çalışanın İd si boş bırakılamaz");
            else
            {

                Boolean delcont = false;
                string delCheck = "select*from Çalışan where ID=@id";
                SqlCommand cmdCheck = new SqlCommand(delCheck, con);
                con.Open();
                cmdCheck.Parameters.AddWithValue("@id", delCalİstasyonID);
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
                        string query = "update Çalışan set IstasyonID=@istasyonid where ID=@id";
                        SqlCommand cmd = new SqlCommand(query, con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@id",delCalİstasyonID);
                        cmd.Parameters.AddWithValue("@istasyonid", DBNull.Value);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Çalışanın istasyonu silindi");
                        con.Close();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                else
                    MessageBox.Show("Böyle bir ID bulunmamaktadır");
            }

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            // istasyonu silinecek çalışanın idsi
            delCalİstasyonID = textBox6.Text;
        }

        public void listeyukle(string key)
        {

            try
            {
                //select i.ID,i.StandartZaman,b.BölümAdı from Istasyon i inner join Bölümler b on i.BolumID=b.ID where i.BolumID=@id
                //select c.ID,c.Adı,c.Soyadı,i.ID,i.StandartZaman,b.BölümAdı from ((Istasyon i inner join Bölümler b on i.BolumID=b.ID) inner join Çalışan c on c.IstasyonID=i.ID) where i.BolumID=@id
                // 2. si istasyon çalışanlarını listeler
                string query = "select i.ID,i.StandartZaman,b.BölümAdı from Istasyon i inner join Bölümler b on i.BolumID=b.ID where i.BolumID=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", key);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView3.DataSource = dt;
                con.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Bolumİslemleri_Load(object sender, EventArgs e)
        {
            try
            {
                string query = "select ID,BölümAdı from Bölümler";
                SqlCommand cmd = new SqlCommand(query, con);
                if (con.State != ConnectionState.Open)
                    con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dtable = new DataTable();
                dtable.Columns.Add("ID", typeof(string));
                dtable.Columns.Add("BölümAdı", typeof(string));
                dtable.Load(reader);

                comboBox1.ValueMember = "ID";
                comboBox1.DisplayMember = "BölümAdı";
                con.Close();
                comboBox1.DataSource = dtable;
                //reader.Close();
                //con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // seçilen bölümün çalışanları listelenir
        }

        public void bolumCalisanlari(string key)
        {
            try
            {
                string query = "select c.ID,c.Adı,c.Soyadı,c.IstasyonID,c.BolumID from Çalışan c inner join Bölümler b on c.BolumID=b.ID where c.BolumID=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", key);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView4.DataSource = dt;
                con.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
