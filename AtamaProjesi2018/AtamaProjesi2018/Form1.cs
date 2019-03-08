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

    public partial class Form1 : Form
    {
        //const string admin = "nursan";
        //const string password = "123456";
        public string userid;
        public string userpw;
        public Form1()
        {
            InitializeComponent();
            var connectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            con = new SqlConnection(connectionString);
        }
        SqlConnection con;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Kullanıcı adının girildiği kısım
            userid = textBox1.Text;
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //şifre girilen kısım
            userpw = textBox2.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Giriş butonu 
            
            try
            {
                con.Open();
                string sql = "select*from Kullanıcılar where KullanıcıAdı=@KullanıcıAdı and Şifre=@Şifre";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@KullanıcıAdı", userid);
                cmd.Parameters.AddWithValue("@Şifre", userpw);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();

                bool loginSuccessfull = ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0));
                if (loginSuccessfull)
                {
                    this.Visible = false;
                    AnaEkran fAnaekran = new AnaEkran();
           
                    fAnaekran.curUser(textBox1.Text);
                    fAnaekran.Show();
                    //MessageBox.Show("Giriş Başarılı");
                }
                else
                    MessageBox.Show("Giriş Başarısız!!");

            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
            finally
            {
                con.Close();
            }
            /*if (userid == admin && userpw == password)
           {
               MessageBox.Show("Giriş Başarılı");
               Form f2 = new AnaEkran();
               f2.Show();
               this.Visible = false;

           }
           else
               MessageBox.Show("Kullanıcı Adı veya Şifre Hatalı!!!");
               */

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox2.Focus();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
                        
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    con.Open();
                    string sql = "select*from Kullanıcılar where KullanıcıAdı=@KullanıcıAdı and Şifre=@Şifre";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@KullanıcıAdı",userid);
                    cmd.Parameters.AddWithValue("@Şifre",userpw);
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();

                    bool loginSuccessfull = ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0));
                    if (loginSuccessfull)
                    {
                        this.Visible = false;
                        AnaEkran fAnaekran = new AnaEkran();

                        fAnaekran.curUser(textBox1.Text);
                        fAnaekran.Show();
                        //MessageBox.Show("Giriş Başarılı");
                    }
                    else
                        MessageBox.Show("Giriş Başarısız!!");
               
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
                finally
                {
                    con.Close();
                }
            }
    }

    private void button2_Click(object sender, EventArgs e)
    {
        textBox1.Clear();
        textBox2.Clear();
    }
        private void Form1_Load(object sender, EventArgs e) { }
     
    }  
}
