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
    public partial class YerineAtamaYap : Form
    {
        string sicilno = "";
        string selectedIndex = "";

        public YerineAtamaYap()
        {
            InitializeComponent();
            var connectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            con = new SqlConnection(connectionString);
        }

        public YerineAtamaYap(string str)
        {
            InitializeComponent();

            var connectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            con = new SqlConnection(connectionString);

            sicilno = str;
        }


        SqlConnection con;

        string ekipid = "";
        string jokerid = "";
        int ekipidOto;
        int selectedJoker,tempjoker;
        int istasyonID;

        private void YerineAtamaYap_Load(object sender, EventArgs e)
        {
            string queryDevamsiz = "select c.Adı,c.Soyadı,c.SicilNo,c.EkipID,c.IstasyonID,b.BölümAdı from Çalışan c inner join Bölümler b on c.BolumID=b.ID where c.SicilNo=@sicilno";
            SqlCommand cmdDevamsiz = new SqlCommand(queryDevamsiz,con);
            con.Open();
            cmdDevamsiz.Parameters.AddWithValue("@sicilno",sicilno);
            SqlDataAdapter da = new SqlDataAdapter(cmdDevamsiz);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

            comboBox1.Items.Add("MontajProses");
            comboBox1.Items.Add("KlipTestProses");
            comboBox1.Items.Add("ElektrikselTestProses");
            comboBox1.Items.Add("GözKontrolProses");
            comboBox1.Items.Add("PaketlemeProses");
            comboBox1.Items.Add("MalzemeTaşımaProses");
            comboBox1.Items.Add("ModülHazırlamaProses");


        }

        private void button1_Click(object sender, EventArgs e)
        {
            // joker çalışan listesi butonu
            /*
             string query = "select*from Çalışan c left join Devamsızlık d on c.ID=d.ÇalışanID where tarih>@today and tarih!=@today or tarih is null and c.isJoker=0";
                SqlCommand cmd = new SqlCommand(query,con);
                con.Open();
                cmd.Parameters.AddWithValue("@today",today);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();
             */

            try
            {
                string today = DateTime.Now.ToString("yyyy/MM/dd");
                string queryJoker = "select c.ID,c.Adı,c.Soyadı,c.isJoker,c.BantlamaProses," +
                    "c.KlipTestProses,c.ElektrikselTestProses,c.GözKontrolProses," +
                    "c.PaketlemeProses,c.MalzemeTaşımaProses,c.ModulHazirlamaProses from Çalışan c left join Devamsızlık d on c.ID=d.ÇalışanID where (tarih>@today or tarih!=@today or tarih is null) and c.isJoker=1 and c.EkipID is null and c.IstasyonID is null";
                SqlCommand cmdJoker = new SqlCommand(queryJoker, con);
                con.Open();
                cmdJoker.Parameters.AddWithValue("@today", today);
                SqlDataAdapter dajoker = new SqlDataAdapter(cmdJoker);
                DataTable dtjoker = new DataTable();
                dajoker.Fill(dtjoker);
                dataGridView2.DataSource = dtjoker;
                con.Close();
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // ekip id
            ekipid = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // ata
            if (ekipid == null || ekipid == "")
                MessageBox.Show("Ekip ID boş bırakılamaz");
            else
            {
                try
                {
                    Boolean calisanCheck = false;
                    string queryCheck = "select*from Çalışan where ID=@id";
                    SqlCommand cmdCheck = new SqlCommand(queryCheck, con);
                    con.Open();
                    cmdCheck.Parameters.AddWithValue("@id", jokerid);
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

                    try
                    {
                        string queryFindIstasyon = "select i.ID from Istasyon i inner join Çalışan c on i.ID=c.IstasyonID where c.SicilNo=@sicilno";
                        SqlCommand cmdIstasyon = new SqlCommand(queryFindIstasyon, con);
                        con.Open();
                        cmdIstasyon.Parameters.AddWithValue("@sicilno", sicilno);
                        istasyonID = (Int32)cmdIstasyon.ExecuteScalar();
                        cmdIstasyon.ExecuteNonQuery();
                        con.Close();
                    }catch(Exception ex) { MessageBox.Show(ex.Message); }
                    if (calisanCheck == true && ekipCheck == true)
                {
                    string queryAta = "update Çalışan set EkipID=@ekipid,IstasyonID=@istasyonid where ID=@id";
                    SqlCommand cmdAta = new SqlCommand(queryAta, con);
                    con.Open();
                    cmdAta.Parameters.AddWithValue("@ekipid", ekipid);
                    cmdAta.Parameters.AddWithValue("@id", jokerid);
                        cmdAta.Parameters.AddWithValue("@istasyonid",istasyonID);
                    cmdAta.ExecuteNonQuery();
                        /*
                        string queryAtEkip = "update Çalışan set EkipID=@ekipid where SicilNo=@sicilno";
                        SqlCommand cmdAt = new SqlCommand(queryAtEkip, con);
                        con.Open();
                        cmdAt.Parameters.AddWithValue("@sicilno", sicilno);
                        cmdAt.Parameters.AddWithValue("@ekipid", DBNull.Value);
                        cmdAt.ExecuteNonQuery();  */
                        con.Close();
                        MessageBox.Show("Atama tamamlandı");
                    }
                else
                    MessageBox.Show("Geçersiz joker id veya ekip id girdiniz!");

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // joker id
            jokerid = textBox2.Text;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // atama yapılacak bölüm
            selectedIndex = comboBox1.GetItemText(this.comboBox1.SelectedItem);
            if (selectedIndex == "MontajProses")
            {
                selectedIndex = "BantlamaProses";
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // otomatik ata butonu

            try
            {
                string queryBul = "select EkipID from Çalışan where SicilNo=@sicilno";
                SqlCommand cmdBul = new SqlCommand(queryBul, con);
                con.Open();
                cmdBul.Parameters.AddWithValue("@sicilno", sicilno);
                ekipidOto = (Int32)cmdBul.ExecuteScalar();
                cmdBul.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            try
            {
                string today = DateTime.Now.ToString("yyyy/MM/dd");
                string queryZero = "select isnull(min(c.ID),0) from Çalışan c left join Devamsızlık d on c.ID=d.ÇalışanID where isJoker=1 and EkipID is null and (tarih>@today or tarih!=@today or tarih is null)";
                string queryJoker = "select top 1 c.ID," + selectedIndex + " from Çalışan c left join Devamsızlık d on c.ID=d.ÇalışanID where isJoker=1 and c.IstasyonID is null and EkipID is null and (tarih>@today or tarih!=@today or tarih is null) ORDER BY cast(" + selectedIndex + " as int) DESC";
                SqlCommand cmdJoker = new SqlCommand(queryJoker, con);
                SqlCommand cmdZero = new SqlCommand(queryZero,con);
                con.Open();
                //cmdJoker.Parameters.AddWithValue("@column",selectedIndex);
                cmdJoker.Parameters.AddWithValue("@today", today);
                cmdZero.Parameters.AddWithValue("@today",today);
                tempjoker = (Int32)cmdZero.ExecuteScalar();
                if (tempjoker == 0)
                    selectedJoker = 0;
                else
                selectedJoker = (Int32)cmdJoker.ExecuteScalar();

                cmdZero.ExecuteNonQuery();
                cmdJoker.ExecuteNonQuery();
                con.Close();


            } catch (Exception ex) { MessageBox.Show(ex.Message); }
            try
            {
                string queryFindIstasyon = "select i.ID from Istasyon i inner join Çalışan c on i.ID=c.IstasyonID where c.SicilNo=@sicilno";
                SqlCommand cmdIstasyon = new SqlCommand(queryFindIstasyon, con);
                con.Open();
                cmdIstasyon.Parameters.AddWithValue("@sicilno", sicilno);
                istasyonID = (Int32)cmdIstasyon.ExecuteScalar();
                cmdIstasyon.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            if (selectedJoker == 0)
            {
                MessageBox.Show("Yeterli joker bulunmamaktadır. İş yakın istasyonlara paylaştırılacaktır.");
                istasyonOto(istasyonID);
            }

            else
            {
                try
                {
                    string queryOto = "update Çalışan set EkipID=@ekipid,IstasyonID=@istasyonid where ID=@id";
                    SqlCommand cmdOto = new SqlCommand(queryOto, con);
                    con.Open();

                    cmdOto.Parameters.AddWithValue("@id", selectedJoker);
                    cmdOto.Parameters.AddWithValue("@ekipid", ekipidOto);
                    cmdOto.Parameters.AddWithValue("@istasyonid", istasyonID);
                    cmdOto.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }

            MessageBox.Show("Atama Tamamlandı.");
        }
        
        public void istasyonOto(int emptyStation) {
            int emptyBolumId = checkBolum(emptyStation);
            int maxIstasyon = findMaxID(emptyBolumId);
            if(emptyStation==1)
            {
                kontrolFirst(emptyStation);
            }
            else if (emptyStation == maxIstasyon)
            {
                kontrolLast(emptyStation);
            }
            else
            {
                kontrol(emptyStation);
            }
        }
        public int checkBolum(int id) {
            int result = 0;
            try
            {
                string query = "select b.ID from Bölümler b inner join Istasyon i on b.ID=i.BolumID where i.ID=@id";
                SqlCommand cmdEmpty = new SqlCommand(query,con);
                cmdEmpty.Parameters.AddWithValue("@id",id);
                con.Open();
                result = (Int32)cmdEmpty.ExecuteScalar();
                cmdEmpty.ExecuteNonQuery();
                con.Close();

            }catch(Exception ex) { MessageBox.Show(ex.Message); }

            return result;
        }
        public int findMaxID(int id) {
            int result = 0;
            try
            {   //select isnull(max(i.ID),0) from Istasyon i inner join Bölümler b on i.BolumID=b.ID where b.ID=@id
                string query = "select isnull(max(i.ID),0) from Istasyon i inner join Bölümler b on i.BolumID=b.ID where b.ID=@id";
                SqlCommand cmdMax = new SqlCommand(query,con);
                cmdMax.Parameters.AddWithValue("@id",id);
                con.Open();
                result = (Int32)cmdMax.ExecuteScalar();
                cmdMax.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return result;
        }
        public void kontrol(int id)
        {
            int hedefArtı = id + 1;
            int hedefEksi = id - 1;
            int artı=0, eksi=0;
                try
                {
                    string today = DateTime.Now.ToString("yyyy/MM/dd");
                    string query = "select isnull(min(d.ID),0) from Çalışan c inner join Devamsızlık d on c.ID=d.ÇalışanID where c.IstasyonID=@id and tarih=@today";
                    SqlCommand cmdKontrolArtı = new SqlCommand(query,con);
                    cmdKontrolArtı.Parameters.AddWithValue("@id",hedefArtı);
                    cmdKontrolArtı.Parameters.AddWithValue("@today",today);
                    con.Open();
                    artı = (Int32)cmdKontrolArtı.ExecuteScalar();
                    cmdKontrolArtı.ExecuteNonQuery();
                    con.Close();
                }catch(Exception ex) { MessageBox.Show(ex.Message); }
                if (artı == 0)
                    ataIstasyon(hedefArtı,id);
                else                 
                    MessageBox.Show("Çalışanın sağındaki istasyon uygun değil!");
                
            
                try
                {
                    string today = DateTime.Now.ToString("yyyy/MM/dd");
                    string query = "select isnull(min(d.ID),0) from Çalışan c inner join Devamsızlık d on c.ID=d.ÇalışanID where c.IstasyonID=@id and tarih=@today";
                    SqlCommand cmdKontrolEksi = new SqlCommand(query, con);
                    cmdKontrolEksi.Parameters.AddWithValue("@id", hedefEksi);
                    cmdKontrolEksi.Parameters.AddWithValue("@today",today);
                    con.Open();
                    eksi = (Int32)cmdKontrolEksi.ExecuteScalar();
                    cmdKontrolEksi.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                if (eksi == 0)
                    ataIstasyon(hedefEksi,id);
                else 
                    MessageBox.Show("Çalışanın solundaki istasyon uygun değil!");
                 
            
        }
        public void kontrolFirst(int id) {
            int hedefArtı = id + 1;
            int hedefArtıPlus = id + 2;
            int artı = 0, artıPlus = 0;
            try
            {
                string today = DateTime.Now.ToString("yyyy/MM/dd");
                string query = "select isnull(min(d.ID),0) from Çalışan c inner join Devamsızlık d on c.ID=d.ÇalışanID where c.IstasyonID=@id and tarih=@today";
                SqlCommand cmdKontrolArtı = new SqlCommand(query, con);
                cmdKontrolArtı.Parameters.AddWithValue("@id", hedefArtı);
                cmdKontrolArtı.Parameters.AddWithValue("@today", today);
                con.Open();
                artı = (Int32)cmdKontrolArtı.ExecuteScalar();
                cmdKontrolArtı.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            if (artı == 0)
                ataIstasyon(hedefArtı, id);
            else
                MessageBox.Show("Çalışanın sağındaki istasyon uygun değil!");


            try
            {
                string today = DateTime.Now.ToString("yyyy/MM/dd");
                string query = "select isnull(min(d.ID),0) from Çalışan c inner join Devamsızlık d on c.ID=d.ÇalışanID where c.IstasyonID=@id and tarih=@today";
                SqlCommand cmdKontrolEksi = new SqlCommand(query, con);
                cmdKontrolEksi.Parameters.AddWithValue("@id", hedefArtıPlus);
                cmdKontrolEksi.Parameters.AddWithValue("@today", today);
                con.Open();
                artıPlus = (Int32)cmdKontrolEksi.ExecuteScalar();
                cmdKontrolEksi.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            if (artıPlus == 0)
                ataIstasyon(hedefArtıPlus, id);
            else
                MessageBox.Show("Çalışanın 2 sağındaki istasyon uygun değil!");
        }

        public void kontrolLast(int id)
        {
            int hedefEksiPlus = id - 2;
            int hedefEksi = id - 1;
            int eksiPlus = 0, eksi = 0;


            try
            {
                string today = DateTime.Now.ToString("yyyy/MM/dd");
                string query = "select isnull(min(d.ID),0) from Çalışan c inner join Devamsızlık d on c.ID=d.ÇalışanID where c.IstasyonID=@id and tarih=@today";
                SqlCommand cmdKontrolEksi = new SqlCommand(query, con);
                cmdKontrolEksi.Parameters.AddWithValue("@id", hedefEksi);
                cmdKontrolEksi.Parameters.AddWithValue("@today", today);
                con.Open();
                eksi = (Int32)cmdKontrolEksi.ExecuteScalar();
                cmdKontrolEksi.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            if (eksi == 0)
                ataIstasyon(hedefEksi, id);
            else
                MessageBox.Show("Çalışanın solundaki istasyon uygun değil!");

            try
            {
                string today = DateTime.Now.ToString("yyyy/MM/dd");
                string query = "select isnull(min(d.ID),0) from Çalışan c inner join Devamsızlık d on c.ID=d.ÇalışanID where c.IstasyonID=@id and tarih=@today";
                SqlCommand cmdKontrolArtı = new SqlCommand(query, con);
                cmdKontrolArtı.Parameters.AddWithValue("@id", hedefEksiPlus);
                cmdKontrolArtı.Parameters.AddWithValue("@today", today);
                con.Open();
                eksiPlus = (Int32)cmdKontrolArtı.ExecuteScalar();
                cmdKontrolArtı.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            if (eksiPlus == 0)
                ataIstasyon(hedefEksiPlus, id);
            else
                MessageBox.Show("Çalışanın 2 solundaki istasyon uygun değil!");


        }

        public void ataIstasyon(int value,int istasyonid) {
            int getCalisanID=0;
            int check=0;
            try
            {
                string query = "select ID from Çalışan where IstasyonID=@istasyonid";
                SqlCommand cmdBul = new SqlCommand(query,con);
                cmdBul.Parameters.AddWithValue("@istasyonid",value);
                con.Open();
                getCalisanID = (Int32)cmdBul.ExecuteScalar();
                cmdBul.ExecuteNonQuery();
                con.Close();
            }catch(Exception ex) { MessageBox.Show(ex.Message); }

            try
            {
                string query = "select isnull(Vekil1ID,0) from Istasyon where ID=@id";
                SqlCommand cmdCheck = new SqlCommand(query,con);
                cmdCheck.Parameters.AddWithValue("@id",istasyonid);
                con.Open();
                check = (Int32)cmdCheck.ExecuteScalar();
                cmdCheck.ExecuteNonQuery();
                con.Close();
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
            if (check == 0)
            {
                try
                {
                    string queryata = "update Istasyon set Vekil1ID=@calisanid where ID=@id";
                    SqlCommand cmdata = new SqlCommand(queryata,con);
                    cmdata.Parameters.AddWithValue("@calisanid",getCalisanID);
                    cmdata.Parameters.AddWithValue("@id",istasyonid);
                    con.Open();
                    cmdata.ExecuteNonQuery();
                    con.Close();
                    //MessageBox.Show("Atama tamamlandı");
                }catch(Exception ex) { MessageBox.Show(ex.Message); }
            }
            else
            {

                try
                {
                    string queryata = "update Istasyon set Vekil2ID=@calisanid where ID=@id";
                    SqlCommand cmdata = new SqlCommand(queryata, con);
                    cmdata.Parameters.AddWithValue("@calisanid", getCalisanID);
                    cmdata.Parameters.AddWithValue("@id", istasyonid);
                    con.Open();
                    cmdata.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Atama tamamlandı");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
        
    }
}
