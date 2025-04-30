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
using static Guna.UI2.Native.WinApi;

namespace disKlinigiOtomasyonu
{
    public partial class Randevu: Form
    {
        public Randevu()
        {
            InitializeComponent();
        }

        ConnectionString MyCon = new ConnectionString();
        private void fillHasta()
        {
            SqlConnection baglanti = MyCon.GetCon();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select HAd from HastaTbl", baglanti);
            SqlDataReader rdr;
            rdr = komut.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("HAd", typeof(string));
            dt.Load(rdr);
            RandevuAdCb.ValueMember="HAd";
            RandevuAdCb.DataSource = dt;
            baglanti.Close();
        }

        private void fillTedavi()
        {
            SqlConnection baglanti = MyCon.GetCon();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select TAd from TedaviTbl", baglanti);
            SqlDataReader rdr;
            rdr = komut.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("TAd", typeof(string));
            dt.Load(rdr);
            RandevuTedaviCb.ValueMember="TAd";
            RandevuTedaviCb.DataSource = dt;
            baglanti.Close();
        }


        private void Randevu_Load(object sender, EventArgs e)
        {
            fillHasta();
            fillTedavi();
            uyeler();
            Reset();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void uyeler()
        {
            Hastalar Hs = new Hastalar();
            string query = "select * from RandevuTbl";
            DataSet ds = Hs.ShowHasta(query);
            RandevuDGV.DataSource=ds.Tables[0];
        }

        void Reset()
        {
            RandevuAdCb.SelectedIndex = -1;
            RandevuTedaviCb.SelectedIndex = -1;
            RandevuTarih.Text = "";
            RandevuSaatCb.SelectedIndex = -1;
        }

        void filter()
        {
            Hastalar Hs = new Hastalar();
            string query = "select * from RandevuTbl where Hasta like '%"+AraTb.Text+"%'";
            DataSet ds = Hs.ShowHasta(query);
            RandevuDGV.DataSource=ds.Tables[0];
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            string query = "insert into RandevuTbl values ('"+RandevuAdCb.SelectedValue.ToString()+"' , '"+RandevuTedaviCb.SelectedValue.ToString()+"' , '"+RandevuTarih.Value.Date+"' , '"+RandevuSaatCb.Text+"')";
            Hastalar Hs = new Hastalar();
            try
            {
                Hs.HastaEkle(query);
                MessageBox.Show("Randevu Başarıyla Eklendi");
                uyeler();
                Reset();

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        int key = 0;

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            Hastalar Hs = new Hastalar();
            if (key==0)
            {
                MessageBox.Show("Güncellenecek Randevuyu Seçiniz!");
            }
            else
            {
                try
                {
                    string query = "Update RandevuTbl set Hasta='"+RandevuAdCb.SelectedValue.ToString()+"', Tedavi='"+RandevuTedaviCb.SelectedIndex.ToString()+"', RTarih='"+RandevuTarih.Text+"' , RSaati ='"+RandevuSaatCb.SelectedItem.ToString()+"' where RId="+key+";";
                    Hs.HastaSil(query);
                    MessageBox.Show("Randevu Başarıyla Güncellendi !");
                    uyeler();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void RandevuDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RandevuAdCb.SelectedValue=RandevuDGV.SelectedRows[0].Cells[1].Value.ToString();
            RandevuTedaviCb.SelectedValue=RandevuDGV.SelectedRows[0].Cells[2].Value.ToString();
            RandevuTarih.Text=RandevuDGV.SelectedRows[0].Cells[3].Value.ToString();
            RandevuSaatCb.Text=RandevuDGV.SelectedRows[0].Cells[4].Value.ToString();


            if (RandevuAdCb.SelectedIndex== -1)
            {
                key = 0;

            }
            else
            {
                key=Convert.ToInt32(RandevuDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            Hastalar Hs = new Hastalar();
            if (key==0)
            {
                MessageBox.Show("Silinecek Randevuyu Seçiniz!");
            }
            else
            {
                try
                {
                    string query = "delete from RandevuTbl where RId="+key+"";
                    Hs.HastaSil(query);
                    MessageBox.Show("Randevu Başarıyla Silindi !");
                    uyeler();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void guna2GradientButton9_Click(object sender, EventArgs e)
        {
            Hasta hst = new Hasta();
            hst.Show();
            this.Hide();
        }

        private void guna2GradientButton8_Click(object sender, EventArgs e)
        {
            Randevu rnd = new Randevu();
            rnd.Show();
            this.Hide();
        }

        private void guna2GradientButton7_Click(object sender, EventArgs e)
        {
            Tedavi tdv = new Tedavi();
            tdv.Show();
            this.Hide();
        }

        private void guna2GradientButton6_Click(object sender, EventArgs e)
        {
            Receteler rct = new Receteler();
            rct.Show();
            this.Hide();
        }

        private void AraTb_TextChanged(object sender, EventArgs e)
        {
            filter();
        }
    }
}
