using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Guna.UI2.Native.WinApi;

namespace disKlinigiOtomasyonu
{
    public partial class Receteler: Form
    {
        public Receteler()
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
            HastaAdSoyadCb.ValueMember="HAd";
            HastaAdSoyadCb.DataSource = dt;
            baglanti.Close();
        }

        private void fillTedavi()
        {
            SqlConnection baglanti = MyCon.GetCon();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select * from RandevuTbl where Hasta='"+HastaAdSoyadCb.SelectedValue.ToString()+"'", baglanti);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(komut);
            sda.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                TedaviTb.Text=dr["Tedavi"].ToString();
            }
            baglanti.Close();
        }

        private void fillPrice()
        {
            SqlConnection baglanti = MyCon.GetCon();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select * from TedaviTbl where TAd='"+TedaviTb.Text+"'", baglanti);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(komut);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
               TutarTb.Text=dr["TUcret"].ToString();
            }
            baglanti.Close();
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

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Receteler_Load(object sender, EventArgs e)
        {
            fillHasta();
            uyeler();
            Reset();
        }

        private void HastaAdSoyadCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            fillTedavi();
        }

        private void TutarTb_TextChanged(object sender, EventArgs e)
        {
            fillPrice();
        }

        private void TedaviTb_TextChanged(object sender, EventArgs e)
        {
            fillPrice();
        }

        void uyeler()
        {
            Hastalar Hs = new Hastalar();
            string query = "select * from ReceteTbl";
            DataSet ds = Hs.ShowHasta(query);
            ReceteDGV.DataSource=ds.Tables[0];
        }

        void Reset()
        {
            HastaAdSoyadCb.SelectedItem = "";
            TedaviTb.Text = "";
            TutarTb.Text = "";
            IlaclarTb.Text = "";
            IlacMiktartb.Text = "";
        }

        void filter()
        {
            Hastalar Hs = new Hastalar();
            string query = "select * from ReceteTbl where HasAd like '%"+AraTb.Text+"%'";
            DataSet ds = Hs.ShowHasta(query);
            ReceteDGV.DataSource=ds.Tables[0];
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            string query = "insert into ReceteTbl values ('"+HastaAdSoyadCb.SelectedValue.ToString()+"' , '"+TedaviTb.Text+"' , '"+TutarTb.Text+"', '"+IlaclarTb.Text+"' , '"+IlacMiktartb.Text+"')";
            Hastalar Hs = new Hastalar();
            try
            {
                Hs.HastaEkle(query);
                MessageBox.Show("Reçete Başarıyla Eklendi");
                uyeler();
                Reset();

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        int key = 0;

        private void ReceteDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            HastaAdSoyadCb.Text=ReceteDGV.SelectedRows[0].Cells[1].Value.ToString();
            TedaviTb.Text=ReceteDGV.SelectedRows[0].Cells[2].Value.ToString();
            TutarTb.Text=ReceteDGV.SelectedRows[0].Cells[3].Value.ToString();
            IlaclarTb.Text=ReceteDGV.SelectedRows[0].Cells[4].Value.ToString();
            IlacMiktartb.Text=ReceteDGV.SelectedRows[0].Cells[3].Value.ToString();

            if (HastaAdSoyadCb.Text=="")
            {
                key = 0;

            }
            else
            {
                key=Convert.ToInt32(ReceteDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            Hastalar Hs = new Hastalar();
            if (key==0)
            {
                MessageBox.Show("Silinecek Reçeteyi Seçiniz!");
            }
            else
            {
                try
                {
                    string query = "delete from ReceteTbl where ReceteId="+key+"";
                    Hs.HastaSil(query);
                    MessageBox.Show("Reçete Başarıyla Silindi !");
                    uyeler();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void AraTb_TextChanged(object sender, EventArgs e)
        {
            filter();
        }

        Bitmap bitmap;

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            int height = ReceteDGV.Height;
            ReceteDGV.Height=ReceteDGV.RowCount * ReceteDGV.RowTemplate.Height * 2;
            bitmap =new Bitmap(ReceteDGV.Width, ReceteDGV.Height);
            ReceteDGV.DrawToBitmap(bitmap, new Rectangle(0, 10, ReceteDGV.Width, ReceteDGV.Height));
            ReceteDGV.Height=height;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
        }
    }
}
