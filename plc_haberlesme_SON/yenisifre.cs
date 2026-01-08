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
using System.Security.Policy;

namespace plc_haberlesme_SON
{
    public partial class yenisifre : Form
    {
        public yenisifre()
        {
            InitializeComponent();
            this.AcceptButton = this.button1;
            this.CancelButton = this.button2;
            this.Load += new System.EventHandler(this.yenisifre_Load);
            this.button1.Click += new System.EventHandler(this.button1_Click);
            
            this.AutoScaleMode = AutoScaleMode.None;

            // Placeholder davranışı
            textBox1.Text = "Doğrulama kodu giriniz";
            textBox1.ForeColor = Color.Gray;
            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;

            textBox2.Text = "Yeni şifre giriniz";
            textBox2.ForeColor = Color.Gray;
            textBox2.Enter += textBox2_Enter;
            textBox2.Leave += textBox2_Leave;

            // Form Load eventini ekle
            this.Load += yenisifre_Load;
            this.Shown += yenisifre_Shown;
        }

        private void yenisifre_Shown(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }


        private void yenisifre_Load(object sender, EventArgs e)
        {
            TextBoxlariSifirla();

            if (!string.IsNullOrEmpty(Form1.SonGonderilenKod))
            {
                label4.Text = $"Doğrulama Kodunuz: {Form1.SonGonderilenKod}";
                label4.Visible = true;
            }
            else
            {
                label4.Text = "";
                label4.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string kod = textBox1.Text.Trim();
            string yeniSifre = textBox2.Text.Trim();

            // Placeholder veya boşluk kontrolü
            if (string.IsNullOrEmpty(kod) || kod == "Doğrulama kodu giriniz" ||
                string.IsNullOrEmpty(yeniSifre) || yeniSifre == "Yeni şifre giriniz")
            {
                MessageBox.Show("Lütfen yeni şifrenizi oluşturun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kod yanlışsa
            if (kod != Form1.SonGonderilenKod)
            {
                MessageBox.Show("Lütfen doğrulama kodunu tekrar giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (Form1.Connection.State != System.Data.ConnectionState.Open)
                    Form1.Connection.Open();

                SqlCommand cmd = new SqlCommand("UPDATE yenikayit SET pass = @pass WHERE e_mail = @e_mail", Form1.Connection);
                cmd.Parameters.AddWithValue("@pass", Cryptology.Encryption(yeniSifre, 2));
                cmd.Parameters.AddWithValue("@e_mail", Form1.SonKullaniciIletisim);
                int result = cmd.ExecuteNonQuery();
                Form1.Connection.Close();

                if (result > 0)
                {
                    MessageBox.Show("Yeni şifre başarıyla oluşturuldu.", "Program");
                    Form1.SonGonderilenKod = "";
                    Form1.SonKullaniciIletisim = "";
                    this.Hide();
                    var frm = System.Windows.Forms.Application.OpenForms["Form1"] as plc_haberlesme_SON.Form1;
                    if (frm != null)
                    {
                        frm.DogrulamaKodunuGuncelle();
                        frm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya parola hatalı. Lütfen tekrar deneyiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                if (Form1.Connection.State == System.Data.ConnectionState.Open)
                    Form1.Connection.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Geriye gitmek istediğinizden emin misiniz?", "Program", MessageBoxButtons.YesNo);

            if (onay == DialogResult.Yes)
            {
                this.Hide();
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is dkodgonder dkgFrm)
                    {
                        dkgFrm.TextBoxlariSifirla();
                        dkgFrm.Show();
                        dkgFrm.BringToFront();
                        return;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            

            
            {
                Form1.SonGonderilenKod = "";
                var frm = System.Windows.Forms.Application.OpenForms["Form1"] as plc_haberlesme_SON.Form1;
                if (frm != null)
                {
                    frm.DogrulamaKodunuGuncelle();
                    frm.Show();
                }
                this.Hide();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Çıkmak istediğinizden emin misiniz?", "Program", MessageBoxButtons.YesNo);
            if (onay == DialogResult.Yes)
            {
                Application.Exit();
            }
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
        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Yeni şifre giriniz")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
                textBox2.PasswordChar = '*';
            }
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = "Yeni şifre giriniz";
                textBox2.ForeColor = Color.Gray;
                textBox2.PasswordChar = '\0';
            }
        }

       


        bool move;
        int mouse_x;
        int mouse_y;
        private void yenisifre_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;
        }

        private void yenisifre_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        

        private void yenisifre_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);

            }
        }

        public void TextBoxlariSifirla()
        {
            textBox1.Text = "Doğrulama kodu giriniz";
            textBox1.ForeColor = Color.Gray;
            textBox2.Text = "Yeni şifre giriniz";
            textBox2.ForeColor = Color.Gray;
            textBox2.PasswordChar = '\0';
        }


    }
}
