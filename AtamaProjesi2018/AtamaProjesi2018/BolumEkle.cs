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
    public partial class BolumEkle : Form
    {
        public BolumEkle()
        {
            InitializeComponent();
            var connectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            con = new SqlConnection(connectionString);
        }

        string bolumAdı="";
        string bolumid = "";

        SqlConnection con;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Bölüm adı
            bolumAdı = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Ekle butonu
            if (bolumAdı == null || bolumAdı == "")
                MessageBox.Show("Bölüm Adı boş bırakılamaz");
            else
            {
                try
                {
                    Boolean bolumExist = false;
                    string query = "select*from Bölümler where BölümAdı=@bölümadı";
                    SqlCommand cmdExist = new SqlCommand(query, con);
                    cmdExist.Parameters.AddWithValue("@bölümadı", bolumAdı);
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
                        MessageBox.Show(string.Format("{0} :Adlı bölüm zaten mevcut!", bolumAdı));
                    }
                    else
                    {
                        bolumEkle(bolumAdı);
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Bölümleri listele butonu

            try
            {
                string query = "select * from Bölümler";
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
            // Bölüm sil butonu
            if (bolumid == null || bolumid == "")
                MessageBox.Show("Silinecek bölümümn id si boş bırakılamaz");
            else
            {
                try
                {

                    string queryDelBol = "update Çalışan set BolumID=null where BolumID=@bolumid";
                    SqlCommand cmdDelBol = new SqlCommand(queryDelBol, con);
                    con.Open();
                    cmdDelBol.Parameters.AddWithValue("@bolumid", bolumid);
                    cmdDelBol.ExecuteNonQuery();
                    con.Close();
                    // düzeltmeler 11 kasım
                    string queryDelBolIstasyon = "delete from Istasyon where BolumID=@delBolum";
                    SqlCommand cmdDelBolIstasyon = new SqlCommand(queryDelBolIstasyon,con);
                    con.Open();
                    cmdDelBolIstasyon.Parameters.AddWithValue("@delBolum",bolumid);
                    cmdDelBolIstasyon.ExecuteNonQuery();
                    con.Close();
                    //düzeltme sonu

                    string query = "delete from Bölümler where ID=@bolumid";
                    SqlCommand delCom = new SqlCommand(query, con);
                    con.Open();
                    delCom.Parameters.AddWithValue("@bolumid", bolumid);
                    delCom.ExecuteNonQuery();
                    MessageBox.Show(string.Format("{0}  ID li bölüm silindi.", bolumid));
                    con.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        public void bolumEkle(string bAdı) {
            try
            {
                string querymax = "select count(ID) from Bölümler";
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
                    string queryLast = "select max(ID) from Bölümler";
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

                string query = "insert into Bölümler(ID,BölümAdı) values(@id,@bölümadı)";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", currentid);
                cmd.Parameters.AddWithValue("@bölümadı", bAdı);
                cmd.ExecuteNonQuery();
                MessageBox.Show(string.Format("( {0} ) adlı bölüm eklendi", bAdı));
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Silinecek bölüm id
            bolumid = textBox2.Text;
        }

        string istasyonSayisi="";
        string istastonBolumID = "";
        string delIstasyonID = "";
        string id;
        string standartZaman = "";

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // istasyon sayısı
            istasyonSayisi = textBox3.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // istasyon eklenecek bölüm ID
            istastonBolumID = textBox5.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // istasyon ekle butonu
            if ((istasyonSayisi == "" || istasyonSayisi == null) && (istastonBolumID == "" || istastonBolumID == null)
                && (standartZaman == "" || standartZaman == null))
                MessageBox.Show("Eklenecek istasyon sayısı, bölüm id ve standart zaman boş bırakılamaz");
            else
            {
                try
                {
                    string querymax = "select count(ID) from Istasyon";
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
                        string queryLast = "select max(ID) from Istasyon";
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
                    if (Convert.ToInt32(istasyonSayisi) > 20)
                        MessageBox.Show("Tek seferde 20 den fazla istasyon eklenemez");
                    else
                    {
                        for (int i = 0; i < Convert.ToInt32(istasyonSayisi); i++)
                        {
                            string query = "insert into Istasyon(ID,BolumID,StandartZaman) values(@id,@bölümid,@zaman)";
                            SqlCommand cmd = new SqlCommand(query, con);
                            con.Open();
                            cmd.Parameters.AddWithValue("@id", currentid);
                            cmd.Parameters.AddWithValue("@bölümid", istastonBolumID);
                            cmd.Parameters.AddWithValue("@zaman", standartZaman);
                            cmd.ExecuteNonQuery();
                            con.Close();
                            currentid++;
                        }
                        MessageBox.Show(string.Format("( {0} ) adet istasyon eklendi", istasyonSayisi));
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // silinecek istasyon ID
            delIstasyonID = textBox4.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // istasyon sil butonu

            if (delIstasyonID == null || delIstasyonID == "")
                MessageBox.Show("Silinecek istasyon id si boş bırakılamaz");
            else
            {
                try
                {

                    string queryDelBol = "update Çalışan set IstasyonID=null where IstasyonID=@istasyonid";
                    SqlCommand cmdDelBol = new SqlCommand(queryDelBol, con);
                    con.Open();
                    cmdDelBol.Parameters.AddWithValue("@istasyonid", delIstasyonID);
                    cmdDelBol.ExecuteNonQuery();
                    con.Close();
                    

                    string query = "delete from Istasyon where ID=@istasyonid";
                    SqlCommand delCom = new SqlCommand(query, con);
                    con.Open();
                    delCom.Parameters.AddWithValue("@istasyonid", delIstasyonID);
                    delCom.ExecuteNonQuery();
                    MessageBox.Show(string.Format("{0}  ID li istasyon silindi.", delIstasyonID));
                    con.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // istasyonların listelendiği datagridview 
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // bölüm listesi combobox
            id = comboBox1.SelectedValue.ToString();
            listeyukle(id);
        }

        public void listeyukle(string key)
        {

            try
            {
                string query = "select i.ID,i.StandartZaman,i.BolumID,i.Vekil1ID,i.Vekil2ID from Istasyon i inner join Bölümler b on i.BolumID=b.ID where i.BolumID=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", key);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                con.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BolumEkle_Load(object sender, EventArgs e)
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

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            // istasyon standart zaman
            standartZaman = textBox6.Text;
        }
    }
}
