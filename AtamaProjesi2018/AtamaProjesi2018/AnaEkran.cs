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
    public partial class AnaEkran : Form
    {
        public string curuser;
        public AnaEkran()
        {
            InitializeComponent();
            var connectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            con = new SqlConnection(connectionString);
        }

        /*public AnaEkran(string curuser) : this()
        {
            this.curuser = curuser;
        }*/

        SqlConnection con;

        private void AnaEkran_Load(object sender, EventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            //Kullanıcıların eklenip silindiği kısma yönlendiren buton
            string user;
            
            if (curuser != "nursan")
            {
                MessageBox.Show("Sadece Süper Admin Giriş Yapabilir !!!");
            }
            else
            {
                KullanıcıEkleSil f1 = new KullanıcıEkleSil();
                f1.Show();
            }
            
        }

        public void curUser(string text) {
            curuser = text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //çalışan ekle - sil bölümü butonu
            CalisanEkleSil f2 = new CalisanEkleSil();
            f2.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Bölüm Ekle butonu
            BolumEkle f1 = new BolumEkle();
            f1.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Hat Ekle Butonu
            HatEkle f1 = new HatEkle();
            f1.Show();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Takım Oluştur butonu
            TakimOlustur f1 = new TakimOlustur();
            f1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Devamsızlık Girişi butonu
            DevamsizlikGirisi f1 = new DevamsizlikGirisi();
            f1.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Bölümlere Ata Çıkar Butonu
            Bolumİslemleri f1 = new Bolumİslemleri();
            f1.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // ekiplere atama butonu
            EkiplereAtaCikar f1 = new EkiplereAtaCikar();
            f1.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // ekipleri görüntüle butonu
            EkipleriGoruntulecs f1 = new EkipleriGoruntulecs();
            f1.Show();
        }

        private void AnaEkran_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // ekipleri güncelle butonu

            DialogResult result = MessageBox.Show("Ekip bilgileri güncellenip, ekiplere atanmış bulunan joker çalışanlar pasif" +
                " duruma getirilecektir.","Onaylıyor musunuz?",MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                string query = "update Çalışan set EkipID=@ekipid,IstasyonID=@istasyonid where isJoker=1";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.Parameters.AddWithValue("@ekipid",DBNull.Value);
                cmd.Parameters.AddWithValue("@istasyonid",DBNull.Value);
                cmd.ExecuteNonQuery();

                string queryIstasyon = "update Istasyon set Vekil1ID=@vekil1,Vekil2ID=@vekil2";
                SqlCommand cmdIstasyon = new SqlCommand(queryIstasyon, con);
                cmdIstasyon.Parameters.AddWithValue("@vekil1", DBNull.Value);
                cmdIstasyon.Parameters.AddWithValue("@vekil2", DBNull.Value);
                cmdIstasyon.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Bilgiler güncellendi");
            }
            else if(result==DialogResult.No)
            {
                MessageBox.Show("Bir değişiklik yapılmadı.");
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            JokerGuncelle f1 = new JokerGuncelle();
            f1.Show();
        }
    }
}
