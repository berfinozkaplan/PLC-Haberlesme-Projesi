using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace plc_haberlesme_SON
{
    public partial class dkodgönder : Form
    {
        SqlConnection Connection = Form1.Connection;
        public dkodgönder()
        {
            InitializeComponent();
            this.Text = "Yeni Şifre Belirle";
            Label label1 = new Label() { Text = "Kullanıcı Adı:", Top = 20, Left = 20, Width = 120 };
            TextBox txtKullaniciAdi = new TextBox() { Name = "txtKullaniciAdi", Top = 45, Left = 20, Width = 200 };
            Label label2 = new Label() { Text = "Yeni Şifre:", Top = 80, Left = 20, Width = 120 };
            TextBox txtYeniSifre = new TextBox() { Name = "txtYeniSifre", Top = 105, Left = 20, Width = 200, PasswordChar = '*' };
            Label label3 = new Label() { Text = "Yeni Şifre (Tekrar):", Top = 140, Left = 20, Width = 120 };
            TextBox txtYeniSifreTekrar = new TextBox() { Name = "txtYeniSifreTekrar", Top = 165, Left = 20, Width = 200, PasswordChar = '*' };
            Button btnGuncelle = new Button() { Name = "btnGuncelle", Text = "Şifreyi Güncelle", Top = 210, Left = 20, Width = 150 };
            this.Controls.Add(label1);
            this.Controls.Add(txtKullaniciAdi);
            this.Controls.Add(label2);
            this.Controls.Add(txtYeniSifre);
            this.Controls.Add(label3);
            this.Controls.Add(txtYeniSifreTekrar);
            this.Controls.Add(btnGuncelle);
            btnGuncelle.Click += (s, e) => {
                string kullaniciAdi = txtKullaniciAdi.Text.Trim();
                string yeniSifre = txtYeniSifre.Text;
                string yeniSifreTekrar = txtYeniSifreTekrar.Text;
                if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(yeniSifre) || string.IsNullOrEmpty(yeniSifreTekrar))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun.");
                    return;
                }
                if (yeniSifre != yeniSifreTekrar)
                {
                    MessageBox.Show("Şifreler aynı değil!");
                    return;
                }
                try
                {
                    Connection.Open();
                    SqlCommand command = new SqlCommand("UPDATE yenisifre SET pass = @pass, re_pass = @repass WHERE username = @username", Connection);
                    command.Parameters.AddWithValue("@pass", Cryptology.Encryption(yeniSifre, 2));
                    command.Parameters.AddWithValue("@repass", Cryptology.Encryption(yeniSifreTekrar, 2));
                    command.Parameters.AddWithValue("@username", Cryptology.Encryption(kullaniciAdi, 2));
                    int result = command.ExecuteNonQuery();
                    Connection.Close();
                    if (result > 0)
                    {
                        MessageBox.Show("Şifre başarıyla güncellendi.");
                        this.Hide();
                        var frm = System.Windows.Forms.Application.OpenForms["Form1"];
                        if (frm != null)
                        {
                            frm.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı adı bulunamadı!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                    if (Connection.State == System.Data.ConnectionState.Open)
                        Connection.Close();
                }
            };

            // Form Load eventini ekle
            this.Load += dkodgönder_Load;
        }

        private void dkodgönder_Load(object sender, EventArgs e)
        {
            // Form ilk açıldığında placeholder'ları ayarla
            TextBoxlariSifirla();
        }




        public void TextBoxlariSifirla()
        {
            // Dinamik olarak oluşturulan kontrolleri sıfırla
            foreach (Control control in this.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Clear();
                }
            }
        }
    }
} 