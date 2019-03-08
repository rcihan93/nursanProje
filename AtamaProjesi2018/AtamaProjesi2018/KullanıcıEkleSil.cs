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
    public partial class KullanıcıEkleSil : Form
    {

        string username="";
        string userpw="";
        string department="";
        string obligation="";
        string temppw1, temppw2;
        
        public KullanıcıEkleSil()
        {
            InitializeComponent();
            var connectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            con = new SqlConnection(connectionString);
        }
        SqlConnection con;

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //kullanıcı adının bulunduğu bölüm
            username = textBox1.Text;
            
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //şifre ilk giriş
            temppw1 = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //şifre onay
            temppw2 = textBox3.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //onayla butonu

            if (username == "" || username==null) {
                MessageBox.Show("Kullanıcı adı boş bırakılamaz"); }
            else
            {

                //SqlConnection con = new SqlConnection("Data Source=DESKTOP-FFJ67NJ;Initial Catalog=NURSAN-DP2018;Integrated Security=True");

                try
                {
                    Boolean userExist = false;
                    string str = "select*from Kullanıcılar where KullanıcıAdı=@username";
                    SqlCommand cmd = new SqlCommand(str, con);
                    cmd.Parameters.AddWithValue("@username", username);
                    con.Open();
                    SqlDataReader dReader = cmd.ExecuteReader();

                    while (dReader.Read())
                    {
                        if (dReader.HasRows == true)
                        {
                            userExist = true;
                            break;
                        }
                    }dReader.Close();
                    con.Close();
                    if (userExist == true)
                    {
                        MessageBox.Show("Kullanıcı adı zaten mevcut. Lütfen başka bir isim giriniz!!");
                    }
                    else
                    {
                        //MessageBox.Show("kullanıcı adı kullanılabilir");
                        Boolean equPw = temppw1.Equals(temppw2);
                        if (equPw == true)
                        {
                            userpw = temppw2;
                            addNewUser(username, userpw, department, obligation);
                            MessageBox.Show(string.Format("--> {0} <-- Kullanıcısı başarıyla oluşturuldu", username));
                            textBox1.Clear();
                            textBox2.Clear();
                            textBox3.Clear();
                            textBox4.Clear();
                            textBox5.Clear();
                            username = null;
                        }
                        else
                        {
                            MessageBox.Show("Şifreler eşleşmiyor. Tekrar giriniz!!");
                            textBox2.Clear();
                            textBox3.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //departman bölümü
            department = textBox4.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            //görev bölümü
            obligation = textBox5.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //iptal bölümü
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //kullanıcı listesini getir butonu
           // SqlConnection con = new SqlConnection("Data Source=DESKTOP-FFJ67NJ;Initial Catalog=NURSAN-DP2018;Integrated Security=True");
            try {

                string query = "select * from Kullanıcılar";
                SqlCommand parseValues = new SqlCommand(query,con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(parseValues);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();
            }
            catch(Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //kullanıcıyı sil butonu
            if (username != null)
                deleteUser(username);
            else
                MessageBox.Show("Lütfen Silinecek Kullanıcı Adını Giriniz!!");
        }

        public void addNewUser(string username,string password,string departman,string gorev)
        {
           // SqlConnection con = new SqlConnection("Data Source=DESKTOP-FFJ67NJ;Initial Catalog=NURSAN-DP2018;Integrated Security=True");
            try
            {
                string query1 = "select max(ID) from Kullanıcılar";
                SqlCommand findlast = new SqlCommand(query1, con);
                con.Open();
                int id = (int)findlast.ExecuteScalar();
                findlast.ExecuteNonQuery();
                con.Close();
                int currentid = id + 1;
                string query2= "insert into Kullanıcılar (ID,KullanıcıAdı,Şifre,Departman,Görev) values (@id,@username,@password,@departman,@görev)";
                SqlCommand addNewUser = new SqlCommand(query2,con);
                con.Open();
                addNewUser.Parameters.AddWithValue("@id",currentid);
                addNewUser.Parameters.AddWithValue("@username", username);
                addNewUser.Parameters.AddWithValue("@password", password);
                addNewUser.Parameters.AddWithValue("@departman", departman);
                addNewUser.Parameters.AddWithValue("@görev", gorev);
                addNewUser.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //çıkış butonu
            this.Visible = false;
        }

        private void KullanıcıEkleSil_Load(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void deleteUser(string userName)
        {
            
            try {
                if (userName != "nursan")
                {
                    string query = "delete from Kullanıcılar where KullanıcıAdı=@deleteUser";
                    SqlCommand deleteUser = new SqlCommand(query, con);
                    con.Open();
                    deleteUser.Parameters.AddWithValue("@deleteUser", userName);
                    deleteUser.ExecuteNonQuery();
                    MessageBox.Show(string.Format("{0} -> Kullanıcısı silindi", username));
                    con.Close();
                }
                else MessageBox.Show("Süper Admin Silinemez!!");
            }
            catch(Exception ex)
            { MessageBox.Show(ex.Message); }
        }

    }
}
