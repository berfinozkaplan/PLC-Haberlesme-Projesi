using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing;
using plc_haberlesme_SON;

namespace plc_haberlesme_SON
{
    public partial class dkodgonder : Form
    {
        SqlConnection Connection = Form1.Connection;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private string dogrulamaKodu;
        private System.Windows.Forms.Button button2;
        private string iletisim;
        
        public dkodgonder()
        {
            InitializeComponent();
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.AcceptButton = this.button1;
            this.CancelButton = this.button3;

            // Placeholder davranışı
            textBox1.Text = "E posta giriniz";
            textBox1.ForeColor = Color.Gray;
            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;

            // Form Load eventini ekle
            this.Load += dkodgonder_Load;
            this.Shown += dkodgonder_Shown;
        }

        private void dkodgonder_Shown(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dkodgonder));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.textBox1.ForeColor = System.Drawing.Color.Gray;
            this.textBox1.Location = new System.Drawing.Point(90, 149);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(257, 36);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = "E posta giriniz";
            this.textBox1.Enter += new System.EventHandler(this.textBox1_Enter);
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Firebrick;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button1.Location = new System.Drawing.Point(90, 191);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(257, 39);
            this.button1.TabIndex = 12;
            this.button1.Text = "Kod Gönder";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button3.Location = new System.Drawing.Point(12, 263);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(84, 30);
            this.button3.TabIndex = 18;
            this.button3.Text = "Geri";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.BackgroundImage = global::plc_haberlesme_SON.Properties.Resources.xxx_removebg_preview;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button2.Location = new System.Drawing.Point(364, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(62, 59);
            this.button2.TabIndex = 19;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // dkodgonder
            // 
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.BackgroundImage = global::plc_haberlesme_SON.Properties.Resources.şifremi_unuttumBUYUK;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(438, 305);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "dkodgonder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Şifremi Unuttum";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dkodgonder_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dkodgonder_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dkodgonder_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string iletisim = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(iletisim) || iletisim == "E posta giriniz")
            {
                MessageBox.Show("Lütfen e posta giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bool kayitVar = false;
            try
            {
                Connection.Open();
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM yenikayit WHERE e_mail = @e_mail", Connection);
                command.Parameters.AddWithValue("@e_mail", iletisim);
                int count = (int)command.ExecuteScalar();
                kayitVar = count > 0;
                Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                if (Connection.State == System.Data.ConnectionState.Open)
                    Connection.Close();
                return;
            }
            if (!kayitVar)
            {
                MessageBox.Show("Hatalı veya eksik e posta girişi. Lütfen tekrar deneyiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Kod üret ve göster
            Random rnd = new Random();
            dogrulamaKodu = rnd.Next(100000, 999999).ToString();
            MessageBox.Show($"Doğrulama kodunuz: {dogrulamaKodu}", "Doğrulama Kodu");
            Form1.SonGonderilenKod = dogrulamaKodu;
            Form1.SonKullaniciIletisim = iletisim;

            DialogResult result = MessageBox.Show(
                "Doğrulama kodunu yeni şifre olarak girmek istiyor musunuz?",
                "Program",
                MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is Form1 f1)
                    {
                        f1.DogrulamaKodunuGuncelle();
                    }
                    if (frm is yenisifre ys)
                    {
                        ys.TextBoxlariSifirla();
                        ys.Show();
                        ys.BringToFront();
                        this.Close();
                        return;
                    }
                }
                yenisifre yeniSifreForm = new yenisifre();
                yeniSifreForm.Show();
                this.Close();
            }
            else
            {
                this.Hide();
                var frm = System.Windows.Forms.Application.OpenForms["Form1"] as plc_haberlesme_SON.Form1;
                if (frm != null)
                {
                    frm.DogrulamaKodunuGuncelle();
                    frm.Show();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Geriye gitmek istediğinizden emin misiniz?", "Program", MessageBoxButtons.YesNo);

            if (onay == DialogResult.Yes)
            {
                Form1.SonGonderilenKod = "";
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is Form1 f1)
                    {
                        f1.TextBoxlariSifirla(); // <-- Bunu ekle!
                        f1.DogrulamaKodunuGuncelle();
                        f1.Show();
                        f1.BringToFront();
                        break;
                    }
                }
                this.Hide();
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "E posta giriniz")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "E posta giriniz";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void dkodgonder_Load(object sender, EventArgs e)
        {
            // Form ilk açıldığında placeholder'ları ayarla
            TextBoxlariSifirla();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Çıkmak istediğinizden emin misiniz?", "Program", MessageBoxButtons.YesNo);
            if (onay == DialogResult.Yes)
            {
                Application.Exit();
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

        bool move;
        int mouse_x;
        int mouse_y;
        private void dkodgonder_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;
        }

        private void dkodgonder_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void dkodgonder_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);
            }
        }

        public void TextBoxlariSifirla()
        {
            // TextBox'ı tamamen sıfırla
            textBox1.Text = "E posta giriniz";
            textBox1.ForeColor = Color.Gray;
            textBox1.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular);
        }
    }
}
