using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtamaProjesi2018
{
    public partial class EkipleriGoruntulecs : Form
    {
        public EkipleriGoruntulecs()
        {
            InitializeComponent();
            var connectionString = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
            con = new SqlConnection(connectionString);


        }

        SqlConnection con;
        string id;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // combobox
            id = comboBox1.SelectedValue.ToString();
            listeyukle(id);
        }

        private void EkipleriGoruntulecs_Load(object sender, EventArgs e)
        {
           
            try
            {
                string query = "select ID,EkipAdı from Ekip";
                SqlCommand cmd = new SqlCommand(query, con);
                if(con.State != ConnectionState.Open)
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dtable = new DataTable();
                dtable.Columns.Add("ID", typeof(string));
                dtable.Columns.Add("EkipAdı", typeof(string));
                dtable.Load(reader);

                comboBox1.ValueMember = "ID";
                comboBox1.DisplayMember = "EkipAdı";
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

        public void listeyukle(string key)
        {

            try
            {
                string query = "select*from Çalışan c inner join Ekip e on c.EkipID=e.ID where c.EkipID=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", key);
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
