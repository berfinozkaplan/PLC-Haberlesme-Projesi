using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;

namespace plc_haberlesme_SON
{
    public partial class ipadres : Form
    {
        public ipadres()
        {
            InitializeComponent();
            this.AcceptButton = this.button1;
            this.CancelButton = this.button3;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            comboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox1.DrawItem += comboBox1_DrawItem;
            // Sadece bir kez event ekle
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            comboBox2.DrawMode = DrawMode.Normal;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);

            // Placeholder label davranışı
            labelPlaceholder.Visible = string.IsNullOrEmpty(comboBox1.Text);
            comboBox1.TextChanged += (s, e) =>
            {
                labelPlaceholder.Visible = string.IsNullOrEmpty(comboBox1.Text);
            };
            comboBox1.Leave += (s, e) =>
            {
                labelPlaceholder.Visible = string.IsNullOrEmpty(comboBox1.Text);
            };

        }

        // --- EKLENDİ: Bağlantı stringini kontrol eden yardımcı fonksiyon ---
        private void EnsureConnectionString()
        {
            if (Form1.Connection == null)
            {
                Form1.Connection = new SqlConnection();
            }
            if (string.IsNullOrEmpty(Form1.Connection.ConnectionString))
            {
                Form1.Connection.ConnectionString = "Server=SERVER_ADI;Database=VERITABANI_ADI;User Id=KULLANICI_ADI;Password=ŞİFRE;";
                // Yukarıdaki satırı kendi bağlantı bilgine göre doldur!
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

        private void ipadres_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureConnectionString();
                if (Form1.Connection.State != ConnectionState.Open)
                    Form1.Connection.Open();

                SqlCommand cmd = new SqlCommand("SELECT DISTINCT plc_ip FROM plcip", Form1.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                comboBox1.Items.Clear();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["plc_ip"].ToString());
                }
                reader.Close();
                Form1.Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                if (Form1.Connection.State == ConnectionState.Open)
                    Form1.Connection.Close();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string ipAddress = comboBox1.SelectedItem?.ToString();
            string port = comboBox2.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(ipAddress) || string.IsNullOrEmpty(port))
            {
                MessageBox.Show("Lütfen boş bırakılan yerleri tamamlayınız.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bool ipPortVar = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM plcip WHERE plc_ip = @ip AND plc_port = @port", conn);
                    cmd.Parameters.AddWithValue("@ip", ipAddress);
                    cmd.Parameters.AddWithValue("@port", port);
                    int count = (int)await cmd.ExecuteScalarAsync();
                    ipPortVar = count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                return;
            }
            if (!ipPortVar)
            {
                MessageBox.Show("Lütfen geçerli bir IP ve port seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Bağlantı başarılı, anasayfacs'a geç
            this.Hide();
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is anasayfacs anasayfaFrm)
                {
                    anasayfaFrm.Show();
                    anasayfaFrm.BringToFront();
                    return;
                }
            }
            anasayfacs yeniAnasayfa = new anasayfacs();
            yeniAnasayfa.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is Form1 f1)
                {
                    f1.TextBoxlariSifirla();
                    f1.Show();
                    f1.BringToFront();
                    return;
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
                        f1.TextBoxlariSifirla();
                        f1.DogrulamaKodunuGuncelle();
                        f1.Show();
                        f1.BringToFront();
                        break;
                    }
                }
                this.Hide(); // <-- Bu satır kesinlikle if bloğunun İÇİNDE ve foreach'ten SONRA olmalı!
            }
        }

        private void button4_Click(object sender, EventArgs e)
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
        private void ipadres_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;
        }

        private void ipadres_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void ipadres_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);

            }
        }

        public void ComboBoxSifirla()
        {
            comboBox1.SelectedIndex = -1;
            comboBox1.Text = "";
            comboBox2.Items.Clear(); // Port listesini temizle
            comboBox2.SelectedIndex = -1;
            comboBox2.Text = "";
            label1.Visible = true; // Portu giriniz yazısını göster
        }

        private string connectionString = "Server=BERFIN\\SQLEXPRESS;Database=plc_haberlesme;Trusted_Connection=True;";
        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            string selectedIP = comboBox1.SelectedItem?.ToString().Trim();
            if (!string.IsNullOrEmpty(selectedIP))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        await conn.OpenAsync();
                        SqlCommand cmd = new SqlCommand("SELECT plc_port FROM plcip WHERE plc_ip = @ip", conn);
                        cmd.Parameters.AddWithValue("@ip", selectedIP);
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        bool found = false;
                        while (await reader.ReadAsync())
                        {
                            found = true;
                            string port = reader["plc_port"].ToString();
                            if (!comboBox2.Items.Contains(port))
                            {
                                comboBox2.Items.Add(port);
                            }
                        }
                        reader.Close();
                        if (!found)
                            MessageBox.Show("SQL'den hiç port dönmedi!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bağlantı hatası: " + ex.Message);
                    return;
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Visible = false;
        }
    }
}
