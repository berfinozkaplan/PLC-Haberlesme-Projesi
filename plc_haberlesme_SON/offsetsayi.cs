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

namespace plc_haberlesme_SON
{
    public partial class offsetsayi : Form
    {
        private string mevcutDeger = null;
        private int offsetId;

        public offsetsayi(int offsetId)
        {
            InitializeComponent();
            comboBoxSayi.DrawItem += new DrawItemEventHandler(comboBoxSayi_DrawItem);
            comboBoxSayi.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSayi.DrawMode = DrawMode.OwnerDrawFixed;
            this.CancelButton = this.button3; // ESC tuşu için
            this.offsetId = offsetId;
            LoadSayilar();

        }

        public offsetsayi()
        {
            InitializeComponent();
            comboBoxSayi.DrawItem += new DrawItemEventHandler(comboBoxSayi_DrawItem);
            comboBoxSayi.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSayi.DrawMode = DrawMode.OwnerDrawFixed;
            this.CancelButton = this.button3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Çıkmak istediğinizden emin misiniz?", "Program", MessageBoxButtons.YesNo);
            if (onay == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Geriye gitmek istediğinizden emin misiniz?", "Program", MessageBoxButtons.YesNo);
            if (onay == DialogResult.Yes)
            {
                this.ComboBoxSifirla();
                this.Hide();
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is anasayfacs anaFrm)
                    {
                        anaFrm.ComboBoxSifirla();
                        anaFrm.Show();
                        anaFrm.BringToFront();
                        return;
                    }
                }
                anasayfacs yeniAna = new anasayfacs();
                yeniAna.ComboBoxSifirla();
                yeniAna.Show();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                button3.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        bool move;
        int mouse_x;
        int mouse_y;
        private void offsetsayi_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;
        }

        private void offsetsayi_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void offsetsayi_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);

            }
        }

        private string connectionString = "Server=BERFIN\\SQLEXPRESS;Database=plc_haberlesme;Trusted_Connection=True;";




        private void LoadSayilar()
        {

            comboBoxSayi.Items.Clear();
            comboBoxSayi.Text = "Sayı giriniz";
            if (offsetId > 0)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT sayi FROM offsetsayi WHERE offset_id = @offset_id", conn);
                    cmd.Parameters.AddWithValue("@offset_id", offsetId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        object value = reader[0];
                        if (value != DBNull.Value)
                        {
                            string sayiStr = value.ToString();
                            if (!comboBoxSayi.Items.Contains(sayiStr))
                                comboBoxSayi.Items.Add(sayiStr);

                        }
                    }
                    reader.Close();
                }
            }



        }



        public void ComboBoxSifirla()
        {

            comboBoxSayi.SelectedIndex = -1;
            comboBoxSayi.Text = "Sayı giriniz";
            label1.Visible = true;
        }

        private void comboBoxSayi_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            bool isDropDown = (e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            Color backColor;
            Color foreColor;

            if (isDropDown)
            {
                // ComboBox kapalı, sadece seçili öğe gösteriliyor
                backColor = Color.White;
                foreColor = Color.Black;
            }
            else if (isSelected)
            {
                // Açılır listede seçim yapılıyor
                backColor = SystemColors.Highlight;
                foreColor = SystemColors.HighlightText;
            }
            else
            {
                backColor = Color.White;
                foreColor = Color.Black;
            }

            using (SolidBrush backgroundBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
            }
            using (SolidBrush textBrush = new SolidBrush(foreColor))
            {
                string text = comboBoxSayi.Items[e.Index].ToString();
                e.Graphics.DrawString(text, e.Font, textBrush, e.Bounds);
            }
            e.DrawFocusRectangle();

        }



        private void comboBoxSayi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSayi.SelectedIndex >= 0)
            {
                label1.Visible = false;

            }
            else
            {
                label1.Visible = true;
                comboBoxSayi.Text = "Sayı giriniz";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxSayi.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen sayı giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Seçilen değeri simüle olarak kaydet
            mevcutDeger = comboBoxSayi.SelectedItem.ToString();

            // Burada PLC'ye veri gönderme kodu olacak (şu an yok)
            MessageBox.Show("Başlatıldı.", "Program");

            // PLC bağlantısı olduğunda buraya kod eklenecek
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBoxSayi.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen sayı giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // PLC'den değer okuma simülasyonu
            if (mevcutDeger == null)
            {
                MessageBox.Show("Mevcut Değer: Başlatılmadı", "Program");
            }
            else
            {
                MessageBox.Show("Mevcut Değer: " + mevcutDeger, "Program");
            }

            // PLC bağlantısı olduğunda buraya kod eklenecek
        }
    }
}
