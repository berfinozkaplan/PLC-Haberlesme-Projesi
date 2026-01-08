using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks;


namespace plc_haberlesme_SON
{
    public partial class Form1 : Form
    {
        public static Form1 Instance;

        public static SqlConnection Connection = new SqlConnection("Data Source=berfin\\SQLEXPRESS; Initial Catalog=plc_haberlesme; Integrated Security=TRUE");
         private static string connectionString = "Data Source=berfin\\SQLEXPRESS; Initial Catalog=plc_haberlesme; Integrated Security=TRUE";

        public static string SonGonderilenKod = "";
        public static string SonKullaniciIletisim = "";

        private Button btnSifreCoz;
        private TextBox txtSifreCoz;

        private static bool girisUyariGosterildi = false;
        private static bool hataliUyariGosterildi = false;

        private Timer noFocusTimer;

        public Form1()
        {
            InitializeComponent();
            Instance = this;
            this.AcceptButton = this.button1;
            this.Load += Form1_Load;
            // this.button1.Click += new System.EventHandler(this.button1_Click); // 

            // Şifre çözme aracı ekle
            btnSifreCoz = new Button();
            btnSifreCoz.Text = "Şifreyi Çöz";
            btnSifreCoz.Size = new System.Drawing.Size(100, 25);
            btnSifreCoz.Location = new System.Drawing.Point(350, 260);
            btnSifreCoz.Click += BtnSifreCoz_Click;
            this.Controls.Add(btnSifreCoz);

            txtSifreCoz = new TextBox();
            txtSifreCoz.Size = new System.Drawing.Size(120, 25);
            txtSifreCoz.Location = new System.Drawing.Point(230, 260);
            this.Controls.Add(txtSifreCoz);

            

            // Placeholder davranışı
            textBox1.Text = "Kullanıcı adı giriniz";
            textBox1.ForeColor = Color.Gray;
            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;

            textBox2.Text = "Şifre giriniz";
            textBox2.ForeColor = Color.Gray;
            textBox2.Enter += textBox2_Enter;
            textBox2.Leave += textBox2_Leave;
            this.Shown += Form1_Shown;

            // Timer ayarı
            noFocusTimer = new Timer();
            noFocusTimer.Interval = 10; // 10 ms yeterli
            noFocusTimer.Tick += NoFocusTimer_Tick;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            noFocusTimer.Start();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Kullanıcı adı giriniz")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Kullanıcı adı giriniz";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Şifre giriniz")
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
                textBox2.Text = "Şifre giriniz";
                textBox2.ForeColor = Color.Gray;
                textBox2.PasswordChar = '\0';
            }
        }


        bool isthere;
        private void button1_Click(object sender, EventArgs e)
        {
            // DEBUG: BURAYA BREAKPOINT KOYUN! (F9 ile veya sol kenara tıklayarak)
            if (!girisUyariGosterildi && (string.IsNullOrWhiteSpace(textBox1.Text) || textBox1.Text == "Kullanıcı adı giriniz" ||
                string.IsNullOrWhiteSpace(textBox2.Text) || textBox2.Text == "Şifre giriniz"))
            {
                girisUyariGosterildi = true;
                MessageBox.Show("Lütfen giriş yapınız.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            girisUyariGosterildi = false;

            string username = textBox1.Text.Trim();
            string pass = textBox2.Text.Trim();

            // 1. Boşluk ve placeholder kontrolü
            if (string.IsNullOrWhiteSpace(username) || username == "Kullanıcı Adı Giriniz" ||
                string.IsNullOrWhiteSpace(pass) || pass == "Şifreyi Giriniz")
            {
                MessageBox.Show("Lütfen boş bırakılan alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Kod ile giriş kontrolü
            if (!string.IsNullOrEmpty(SonGonderilenKod) && pass == SonGonderilenKod)
            {
                bool basarili = false;
                try
                {
                    Connection.Open();
                    // yenikayit tablosunda güncelle (sadece username ve e_mail ile arama yapılabilir)
                    SqlCommand cmd1 = new SqlCommand("UPDATE yenikayit SET pass = @pass WHERE username = @username OR e_mail = @e_mail", Connection);
                    cmd1.Parameters.AddWithValue("@pass", Cryptology.Encryption(pass, 2));
                    cmd1.Parameters.AddWithValue("@username", username);
                    cmd1.Parameters.AddWithValue("@e_mail", SonKullaniciIletisim);
                    int result1 = cmd1.ExecuteNonQuery();

                    // yenisifre tablosunda sadece username ile güncelle (re_pass de güncellenecek)
                    SqlCommand cmd2 = new SqlCommand("UPDATE yenisifre SET pass = @pass, re_pass = @repass WHERE username = @username", Connection);
                    cmd2.Parameters.AddWithValue("@pass", Cryptology.Encryption(pass, 2));
                    cmd2.Parameters.AddWithValue("@repass", Cryptology.Encryption(pass, 2));
                    cmd2.Parameters.AddWithValue("@username", username);
                    int result2 = cmd2.ExecuteNonQuery();

                    Connection.Close();
                    if (result1 > 0 || result2 > 0)
                    {
                        basarili = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                    if (Connection.State == System.Data.ConnectionState.Open)
                        Connection.Close();
                }
                if (basarili)
                {
                    SonGonderilenKod = "";
                    SonKullaniciIletisim = "";
                    this.Hide();
                    ipadres frm = new ipadres();
                    frm.Show();
                    return;
                }
                else
                {
                    MessageBox.Show("Kodu tekrar giriniz.");
                    return;
                }
            }

            // 3. Normal giriş kontrolü
            bool isthere = false;

            Connection.Open();
            SqlCommand command = new SqlCommand("select * from yenikayit", Connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string dbUser = reader["username"].ToString().Trim();
                string dbPass = Cryptology.Decryption(reader["pass"].ToString().Trim(), 2);

                if (username == dbUser && pass == dbPass)
                {
                    isthere = true;
                    break;
                }
            }

            Connection.Close();

            if (isthere)
            {
                this.Hide();
                ipadres frm = new ipadres();
                frm.Show();
            }
            else
            {
                if (!hataliUyariGosterildi)
                {
                    hataliUyariGosterildi = true;
                    MessageBox.Show("Kullanıcı adı veya parola hatalı. Lütfen tekrar deneyiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                hataliUyariGosterildi = false;
            }
        }




        private void button2_Click(object sender, EventArgs e)
        {
            // Programdan çıkış
            DialogResult onay = MessageBox.Show("Çıkmak istediğinizden emin misiniz?", "Program", MessageBoxButtons.YesNo);
            if (onay == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        bool move;
        int mouse_x;
        int mouse_y;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);

            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.Show();

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dkodgonder sifreForm = new dkodgonder();
            sifreForm.Show(); // Show() metodu otomatik olarak sıfırlama yapacak
            this.Hide();
        }

        public void DogrulamaKodunuGuncelle()
        {
            if (!string.IsNullOrEmpty(SonGonderilenKod))
            {
                label3.Text = $"Doğrulama Kodunuz: {SonGonderilenKod}";
                label3.Visible = true;
                
                
            }
            else
            {
                label3.Text = "";
                label3.Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DogrulamaKodunuGuncelle();
            // Form ilk açıldığında placeholder'ları ayarla
            TextBoxlariSifirla();
        }

        private void BtnSifreCoz_Click(object sender, EventArgs e)
        {
            string sifre = txtSifreCoz.Text.Trim();
            if (string.IsNullOrEmpty(sifre))
            {
                MessageBox.Show("Lütfen veritabanındaki şifreyi girin.");
                return;
            }
            try
            {
                string cozulmus = Cryptology.Decryption(sifre, 2);
                MessageBox.Show($"Orijinal kod: {cozulmus}", "Şifre Çözüm Sonucu");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Çözüm sırasında hata oluştu: " + ex.Message);
            }
        }

        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            ComboBox cmb = sender as ComboBox;
            e.DrawBackground();
            Color textColor = (e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit ? Color.Gray : Color.Black;
            using (SolidBrush brush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(cmb.Items[e.Index].ToString(), e.Font, brush, e.Bounds);
            }
            e.DrawFocusRectangle();
        }

        public void TextBoxlariSifirla()
        {
            textBox1.Text = "Kullanıcı adı giriniz";
            textBox1.ForeColor = Color.Gray;
            textBox2.Text = "Şifre giriniz";
            textBox2.ForeColor = Color.Gray;
            textBox2.PasswordChar = '\0';
            txtSifreCoz.Clear();
        }

        private void NoFocusTimer_Tick(object sender, EventArgs e)
        {
            this.ActiveControl = null; // Hiçbir kontrol seçili olmasın
            noFocusTimer.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Çıkmak istediğinizden emin misiniz?", "Program", MessageBoxButtons.YesNo);
            if (onay == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }

}

