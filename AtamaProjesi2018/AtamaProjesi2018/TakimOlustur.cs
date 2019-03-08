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
    public partial class TakimOlustur : Form
    {
        public TakimOlustur()
        {
            InitializeComponent();
            var connectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            con = new SqlConnection(connectionString);
        }

        SqlConnection con;

        string ekipadi="";
        string operatoradi="";
        string ekipno="";
        string gorevi="";
        int isjoker = 0;
        string silinecekid = "";
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // ekip adı
            ekipadi = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // operatör adı
            operatoradi = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // ekip no
            ekipno = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // görevi
            gorevi = textBox4.Text;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // joker ekip onay checkbox
            if (checkBox1.Checked == true)
                isjoker = 1;
            else
                isjoker = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // ekle
            if (ekipadi=="" || ekipno=="")
                MessageBox.Show("Ekip Adı ve Ekip No boş bırakılamaz");
            else
            {
                
                try
                {
                    Boolean ekipNoExist = false;
                    string query = "select*from Ekip where EkipNO=@ekipNo";
                    SqlCommand cmdExist = new SqlCommand(query, con);
                    cmdExist.Parameters.AddWithValue("@ekipNo", ekipno);
                    con.Open();
                    SqlDataReader dReader = cmdExist.ExecuteReader();

                    while (dReader.Read())
                    {
                        if (dReader.HasRows == true)
                        {
                            ekipNoExist = true;
                            break;
                        }//düzeltmeler 11 kasım
                        dReader.Close();
                    }//düzeltmeler 11 kasım
                    con.Close();
                    if (ekipNoExist == true)
                    {
                        MessageBox.Show(string.Format("{0} Numaralı ekip zaten mevcut!", ekipno));
                    }
                    else
                    {
                        ekipOlustur(ekipadi,operatoradi,ekipno,gorevi,isjoker);
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // iptal
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // ekipleri görüntüle
            try
            {
                string query = "select * from Ekip";
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
        public void ekipOlustur(string eadi,string oadi,string eno,string gorev,int isjoker)
        {
            try
            {
                string querymax = "select count(ID) from Ekip";
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
                    string queryLast = "select max(ID) from Ekip";
                    SqlCommand findLast = new SqlCommand(queryLast, con);
                    con.Open();
                    int id2 = (int)findLast.ExecuteScalar();
                    findLast.ExecuteNonQuery();
                    currentid = id2 + 1;
                    con.Close();

                }

                string query = "insert into Ekip(ID,EkipAdı,OperatörAdı,EkipNO,Görevi,isJoker) values(@id,@ekadi,@oadi,@eno,@gorev,@isjoker)";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", currentid);
                cmd.Parameters.AddWithValue("@ekadi", eadi);
                cmd.Parameters.AddWithValue("@oadi", oadi);
                cmd.Parameters.AddWithValue("@eno", eno);
                cmd.Parameters.AddWithValue("@gorev", gorev);
                cmd.Parameters.AddWithValue("@isjoker",isjoker);
                cmd.ExecuteNonQuery();
                MessageBox.Show(string.Format("( {0} ) adlı ekip oluşturuldu", eadi));
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // silinecek ekip id
            silinecekid = textBox5.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // sil butonu
            if (silinecekid == null || silinecekid == "")
                MessageBox.Show("Silinecek ekibin id si boş bırakılamaz");
            else
            {
                try
                {
                    string queryDelEkip = "update Çalışan set EkipID=null where EkipID=@ekipid";
                    SqlCommand cmdDelEkip = new SqlCommand(queryDelEkip,con);
                    con.Open();
                    cmdDelEkip.Parameters.AddWithValue("@ekipid",silinecekid);
                    cmdDelEkip.ExecuteNonQuery();
                    con.Close();
                    string query = "delete from Ekip where ID=@ekipid";
                    SqlCommand delCom = new SqlCommand(query, con);
                    con.Open();
                    delCom.Parameters.AddWithValue("@ekipid", silinecekid);
                    delCom.ExecuteNonQuery();
                    MessageBox.Show(string.Format("{0}  ID li ekip silindi.", silinecekid));
                    con.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        string projeID = "";
        string ekipID = "";
        string delEkipID = "";
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            // proje İD
            projeID = textBox6.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // ata butonu
            if (ekipID == "" || projeID == "")
                MessageBox.Show("Ekip ID ve Proje ID boş bırakılamaz.");
            else
            {
                try
                {
                    Boolean ekipCheck = false;
                    string queryEkip = "select*from Ekip where ID=@id";
                    SqlCommand cmdEkip = new SqlCommand(queryEkip, con);
                    con.Open();
                    cmdEkip.Parameters.AddWithValue("@id", ekipID);
                    SqlDataReader rEkip = cmdEkip.ExecuteReader();
                    while (rEkip.Read())
                    {
                        if (rEkip.HasRows == true)
                        {
                            ekipCheck = true;
                            break;
                        }
                        rEkip.Close();
                    }
                    con.Close();

                    Boolean projeCheck = false;
                    string queryProje = "select*from Hat where ID=@projeid";
                    SqlCommand cmdProje = new SqlCommand(queryProje, con);
                    con.Open();
                    cmdProje.Parameters.AddWithValue("@projeid", projeID);
                    SqlDataReader rProje = cmdProje.ExecuteReader();
                    while (rProje.Read())
                    {
                        if (rProje.HasRows)
                        {
                            projeCheck = true;
                            break;
                        }
                        rProje.Close();
                    }
                    con.Close();

                    if (ekipCheck == true && projeCheck == true)
                    {
                        try
                        {
                            string query = "update Ekip set HatID=@projeid where ID=@ekipid";
                            SqlCommand cmd = new SqlCommand(query, con);
                            con.Open();
                            cmd.Parameters.AddWithValue("@projeid", projeID);
                            cmd.Parameters.AddWithValue("@ekipid", ekipID);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Ekip projeye atandı.");
                            con.Close();

                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                    else
                        MessageBox.Show("Girilen bilgiler hatalıdır! Kontrol ediniz.");


                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            // proje listesini getir butonu
            try
            {
                string query = "select * from Hat";
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

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            // ekip ıd
            ekipID = textBox7.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            // çıkarılacak ekip ıd
            delEkipID = textBox8.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // çıkar butonu

            if (delEkipID == "")
                MessageBox.Show("Projeden çıkarılacak ekip ID boş bırakılamaz");
            else
            {
                Boolean ekipCheck = false;
                string queryEkip = "select*from Ekip where ID=@id";
                SqlCommand cmdEkip = new SqlCommand(queryEkip, con);
                con.Open();
                cmdEkip.Parameters.AddWithValue("@id", delEkipID);
                SqlDataReader rEkip = cmdEkip.ExecuteReader();
                while (rEkip.Read())
                {
                    if (rEkip.HasRows == true)
                    {
                        ekipCheck = true;
                        break;
                    }
                    rEkip.Close();
                }
                con.Close();

                if (ekipCheck == true)
                {
                    try
                    {
                        string queryDel = "update Ekip set HatID=@projeid where ID=@silinecekid";
                        SqlCommand cmdDel = new SqlCommand(queryDel,con);
                        con.Open();
                        cmdDel.Parameters.AddWithValue("@silinecekid",delEkipID);
                        cmdDel.Parameters.AddWithValue("@projeid",DBNull.Value);
                        cmdDel.ExecuteNonQuery();
                        MessageBox.Show("Ekip projeden çıkarıldı.");
                        con.Close();                             
                    }
                    catch(Exception ex) { MessageBox.Show(ex.Message); }
                }
                else
                    MessageBox.Show("Böyle bir ID bulunmamaktadır.");
            }
        }
    }
}
