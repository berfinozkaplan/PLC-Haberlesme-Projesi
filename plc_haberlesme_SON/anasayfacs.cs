using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections.Generic; // Added for Dictionary

namespace plc_haberlesme_SON
{
    public partial class anasayfacs : Form
    {
        public anasayfacs()
        {
            InitializeComponent();
            this.CancelButton = this.button3;
            comboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox1.DrawItem += comboBox1_DrawItem;
            button2.Click += buttonFalseGonder_Click;
            button1.Click += buttonTrueGonder_Click;
        }

        private void anasayfacs_Load(object sender, EventArgs e)
        {
            // ComboBox1'i offsettrue tablosuna bağla
            try
            {
                comboBox1.Items.Clear();
                using (SqlConnection conn = new SqlConnection("Data Source=berfin\\SQLEXPRESS; Initial Catalog=plc_haberlesme; Integrated Security=TRUE"))
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT offset_true FROM offsettrue ORDER BY id", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string offset = reader["offset_true"].ToString();
                        comboBox1.Items.Add(offset);
                    }
                    reader.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            // Placeholder label davranışı
            label1.Visible = string.IsNullOrEmpty(comboBox1.Text);
            comboBox1.TextChanged += (s, e2) =>
            {
                label1.Visible = string.IsNullOrEmpty(comboBox1.Text);
            };
            comboBox1.SelectedIndexChanged += (s, e2) =>
            {
                label1.Visible = string.IsNullOrEmpty(comboBox1.Text);
            };
            comboBox1.Leave += (s, e2) =>
            {
                label1.Visible = string.IsNullOrEmpty(comboBox1.Text);
            };
            label1.BackColor = SystemColors.Control;
            label1.ForeColor = Color.Gray;
           
            comboBox1.ForeColor = Color.Black;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Geriye gitmek istediğinizden emin misiniz?", "Program", MessageBoxButtons.YesNo);

            if (onay == DialogResult.Yes)
            {
                this.Hide();
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is ipadres ipFrm)
                    {
                        ipFrm.ComboBoxSifirla();
                        ipFrm.Show();
                        ipFrm.BringToFront();
                        return;
                    }
                }
                // Eğer hiç açık yoksa (ilk kez açılıyorsa)
                ipadres yeniFrm = new ipadres();
                yeniFrm.ComboBoxSifirla();
                yeniFrm.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e)
        {
            comboBox1.Focus();
        }

        

        

        bool move;
        int mouse_x;
        int mouse_y;
        private void anasayfacs_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;
        }

        private void anasayfacs_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void anasayfacs_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);

            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Çıkmak istediğinizden emin misiniz?", "Program", MessageBoxButtons.YesNo);
            if (onay == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Geriye gitmek istediğinizden emin misiniz?", "Program", MessageBoxButtons.YesNo);

            if (onay == DialogResult.Yes)
            {
                this.Hide();
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is ipadres ipFrm)
                    {
                        ipFrm.ComboBoxSifirla();
                        ipFrm.Show();
                        ipFrm.BringToFront();
                        return;
                    }
                }
                // Eğer hiç açık yoksa (ilk kez açılıyorsa)
                ipadres yeniFrm = new ipadres();
                yeniFrm.ComboBoxSifirla();
                yeniFrm.Show();
            }
        }

        public void ComboBoxSifirla()
        {
            comboBox1.SelectedIndex = -1;
            comboBox1.Text = "";
            label1.Visible = true;
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

        private void buttonFalseGonder_Click(object sender, EventArgs e)
        {
            string selectedOffset = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedOffset))
            {
                MessageBox.Show("Lütfen bir offset seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var mesajlar = new Dictionary<string, string>
            {
                { "0 (ışık)", "Işık kapatıldı." },
                { "1 (motor)", "Motor durduruldu." },
                { "2 (Acil Durum)", "Acil Durum durduruldu." },
                { "3 (Parlaklık)", "Parlaklık kapatıldı." },
                { "4 (Fan)", "Fan kapatıldı." },
                { "5 (Sistem)", "Sistem resetlendi." }
            };

            // 1. Birebir eşleşme varsa göster
            if (mesajlar.TryGetValue(selectedOffset, out string mesaj))
            {
                MessageBox.Show(mesaj, "Program");
                return;
            }

            // 2. Küçük harfe çevirip, baştaki ve sondaki boşlukları silerek eşleşme dene
            string selectedNormalized = selectedOffset.Trim().ToLowerInvariant();
            foreach (var key in mesajlar.Keys)
            {
                if (key.Trim().ToLowerInvariant() == selectedNormalized)
                {
                    MessageBox.Show(mesajlar[key], "Program");
                    return;
                }
            }

            // 3. Hiçbiri olmadıysa, Dictionary anahtarlarını göster (debug için)
            string anahtarlar = string.Join("\n", mesajlar.Keys);
            MessageBox.Show(
                $"Eşleşme bulunamadı!\nSeçilen: '{selectedOffset}'\nDictionary anahtarları:\n{anahtarlar}",
                "Debug"
            );

            // 4. Son çare: Genel mesaj
            MessageBox.Show("İşlem gerçekleştirildi.", "Program");
        }

        private void buttonTrueGonder_Click(object sender, EventArgs e)
        {
            string selectedOffset = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedOffset))
            {
                MessageBox.Show("Lütfen bir offset seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedOffsetId = -1;

            var mesajlar = new Dictionary<string, string>
            {
                { "0 (ışık)", "Işık açıldı." },
                { "1 (motor)", "Motor başlatıldı." },
                { "2 (Acil Durum)", "Acil Durum başlatıldı." },
                { "3 (Parlaklık)", "Parlaklık açıldı." },
                { "4 (Fan)", "Fan açıldı." },
                { "5 (Sistem)", "Sistem başlatıldı." }
            };

            if (mesajlar.TryGetValue(selectedOffset, out string mesaj))
            {
                MessageBox.Show(mesaj, "Program");
                this.Hide();
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is offsetsayi offFrm)
                    {
                        offFrm.Show();
                        offFrm.BringToFront();
                        return;
                    }
                }

                using (SqlConnection conn = new SqlConnection("Data Source=berfin\\SQLEXPRESS; Initial Catalog=plc_haberlesme; Integrated Security=TRUE"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT id FROM offsettrue WHERE offset_true = @offset", conn);
                    cmd.Parameters.AddWithValue("@offset", selectedOffset);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        selectedOffsetId = Convert.ToInt32(result);
                }
                if (selectedOffsetId > 0)
                {
                    offsetsayi yeniOffset = new offsetsayi(selectedOffsetId);
                    yeniOffset.Show();
                }
                else
                {
                    MessageBox.Show("Offset ID bulunamadı!");
                }
                return;
            }

            // Esnek karşılaştırma
            string selectedNormalized = selectedOffset.Trim().ToLowerInvariant();
            foreach (var key in mesajlar.Keys)
            {
                if (key.Trim().ToLowerInvariant() == selectedNormalized)
                {
                    MessageBox.Show(mesajlar[key], "Program");
                    this.Hide();
                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm is offsetsayi offFrm)
                        {
                            offFrm.Show();
                            offFrm.BringToFront();
                            return;
                        }
                    }


                    using (SqlConnection conn = new SqlConnection("Data Source=berfin\\SQLEXPRESS; Initial Catalog=plc_haberlesme; Integrated Security=TRUE"))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SELECT id FROM offsettrue WHERE offset_true = @offset", conn);
                        cmd.Parameters.AddWithValue("@offset", selectedOffset);
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                            selectedOffsetId = Convert.ToInt32(result);
                    }
                    if (selectedOffsetId > 0)
                    {
                        offsetsayi yeniOffset = new offsetsayi(selectedOffsetId);
                        yeniOffset.Show();
                    }
                    else
                    {
                        MessageBox.Show("Offset ID bulunamadı!");
                    }
                    return;
                }
            }

            MessageBox.Show("İşlem gerçekleştirildi.", "Program");
            this.Hide();
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is offsetsayi offFrm)
                {
                    offFrm.Show();
                    offFrm.BringToFront();
                    return;
                }
            }

            using (SqlConnection conn = new SqlConnection("Data Source=berfin\\SQLEXPRESS; Initial Catalog=plc_haberlesme; Integrated Security=TRUE"))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT id FROM offsettrue WHERE offset_true = @offset", conn);
                cmd.Parameters.AddWithValue("@offset", selectedOffset);
                var result = cmd.ExecuteScalar();
                if (result != null)
                    selectedOffsetId = Convert.ToInt32(result);
            }
            if (selectedOffsetId > 0)
            {
                offsetsayi yeniOffset2 = new offsetsayi(selectedOffsetId);
                yeniOffset2.Show();
            }
            else
            {
                MessageBox.Show("Offset ID bulunamadı!");
            }
        }
    }
}
