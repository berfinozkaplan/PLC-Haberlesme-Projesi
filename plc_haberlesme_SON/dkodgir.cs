using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing; // Added for Color

namespace plc_haberlesme_SON
{
    public partial class dkodgir : Form
    {
        SqlConnection Connection = Form1.Connection;
        private string dogrulamaKodu;
        private string iletisim;
        public dkodgir(string kod, string iletisimBilgisi)
        {
            InitializeComponent();
            dogrulamaKodu = kod;
            iletisim = iletisimBilgisi;
            button1.Click += button1_Click;

            // Placeholder davranışı
            textBox1.Text = "Doğrulama kodu giriniz";
            textBox1.ForeColor = Color.Gray;
            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;

            // Form Load eventini ekle
            this.Load += dkodgir_Load;
            this.Shown += dkodgir_Shown;
        }

        public dkodgir() // Designer için parametresiz constructor
        {
            InitializeComponent();

            // Placeholder davranışı
            textBox1.Text = "Doğrulama kodu giriniz";
            textBox1.ForeColor = Color.Gray;
            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;

            // Form Load eventini ekle
            this.Load += dkodgir_Load;
            this.Shown += dkodgir_Shown;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Doğrulama kodu giriniz")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Doğrulama kodu giriniz";
                textBox1.ForeColor = Color.Gray;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string girilenKod = textBox1.Text.Trim();
            if (girilenKod == dogrulamaKodu)
            {
                // Kod doğruysa yeni şifre olarak kodu kaydet
                try
                {
                    Connection.Open();
                    // yenisifre tablosunda güncelle
                    SqlCommand cmd1 = new SqlCommand("UPDATE yenisifre SET pass = @pass, re_pass = @repass WHERE username = @username OR e_mail = @email", Connection);
                    cmd1.Parameters.AddWithValue("@pass", Cryptology.Encryption(girilenKod, 2));
                    cmd1.Parameters.AddWithValue("@repass", Cryptology.Encryption(girilenKod, 2));
                    cmd1.Parameters.AddWithValue("@username", iletisim);
                    cmd1.Parameters.AddWithValue("@email", iletisim);
                    cmd1.ExecuteNonQuery();
                    // yenikayit tablosunda güncelle
                    SqlCommand cmd2 = new SqlCommand("UPDATE yenikayit SET pass = @pass WHERE e_mail = @email", Connection);
                    cmd2.Parameters.AddWithValue("@pass", Cryptology.Encryption(girilenKod, 2));
                    cmd2.Parameters.AddWithValue("@email", iletisim);
                    cmd2.ExecuteNonQuery();
                    Connection.Close();
                    MessageBox.Show("Kod Doğrulandı. Yeni şifrenizi girebilirsiniz.");
                    this.Hide();
                    var frm = System.Windows.Forms.Application.OpenForms["Form1"];
                    if (frm != null)
                    {
                        frm.Show();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                    if (Connection.State == System.Data.ConnectionState.Open)
                        Connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Tekrar deneyiniz.");
                textBox1.Clear();
                textBox1.Focus();
            }
        }

        public void TextBoxlariSifirla()
        {
            textBox1.Clear();
        }

        private void dkodgir_Load(object sender, EventArgs e)
        {
            // Form ilk açıldığında placeholder'ları ayarla
            TextBoxlariSifirla();
        }

        private void dkodgir_Shown(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }


    }
}
