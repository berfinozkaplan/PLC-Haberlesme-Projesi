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
    public partial class Form2 : Form
    {
        SqlConnection Connection = Form1.Connection;

        public Form2()
        {
            InitializeComponent();
            this.AcceptButton = this.button1;
            this.CancelButton = this.button2;

            // Placeholder davranışı
            textBox1.Text = "Kullanıcı adı giriniz";
            textBox1.ForeColor = Color.Gray;
            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;

            textBox2.Text = "Şifre giriniz";
            textBox2.ForeColor = Color.Gray;
            textBox2.Enter += textBox2_Enter;
            textBox2.Leave += textBox2_Leave;

            textBox3.Text = "E posta giriniz";
            textBox3.ForeColor = Color.Gray;
            textBox3.Enter += textBox3_Enter;
            textBox3.Leave += textBox3_Leave;

            // Form Load eventini ekle
            this.Load += Form2_Load;
            this.Shown += Form2_Shown;
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult onay = MessageBox.Show("Geriye gitmek istediğinizden emin misiniz?", "Program", MessageBoxButtons.YesNo);

            if (onay == DialogResult.Yes)
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Tüm kutuların herhangi biri boşsa veya placeholder metin varsa uyarı ver
            if (string.IsNullOrWhiteSpace(textBox1.Text) || textBox1.Text == "Kullanıcı adı giriniz" ||
                string.IsNullOrWhiteSpace(textBox2.Text) || textBox2.Text == "Şifre giriniz" ||
                string.IsNullOrWhiteSpace(textBox3.Text) || textBox3.Text == "E posta giriniz")
            {
                MessageBox.Show("Lütfen kayıt oluşturunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string username = Cryptology.Encryption(textBox1.Text.Trim(), 2);
            string pass = Cryptology.Encryption(textBox2.Text.Trim(), 2);
            string email = textBox3.Text.Trim();

            Connection.Open();

            // 3. Hem kullanıcı adı+şifre hem de e-posta kayıtlıysa
            SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM yenikayit WHERE username=@username AND pass=@pass", Connection);
            checkCmd.Parameters.AddWithValue("@username", username);
            checkCmd.Parameters.AddWithValue("@pass", pass);
            int userExists = (int)checkCmd.ExecuteScalar();

            SqlCommand emailCheckCmd2 = new SqlCommand("SELECT COUNT(*) FROM yenikayit WHERE e_mail=@e_mail", Connection);
            emailCheckCmd2.Parameters.AddWithValue("@e_mail", email);
            int emailExists2 = (int)emailCheckCmd2.ExecuteScalar();

            if (userExists > 0 && emailExists2 > 0)
            {
                MessageBox.Show("Bu Kayıt Zaten Var. Yeni Kayıt Oluşturunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Connection.Close();
                return;
            }

            // 4. Sadece e-posta kayıtlıysa
            if (emailExists2 > 0)
            {
                MessageBox.Show("Girdiğiniz e-posta ile kayıt yapılmıştır. Lütfen tekrar deneyiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Connection.Close();
                return;
            }

            // Kayıt ekle
            SqlCommand command = new SqlCommand("INSERT INTO yenikayit (username, pass, e_mail) VALUES (@username, @pass, @e_mail)", Connection);
            command.Parameters.AddWithValue("@username", textBox1.Text.Trim());
            command.Parameters.AddWithValue("@pass", Cryptology.Encryption(textBox2.Text.Trim(), 2));
            command.Parameters.AddWithValue("@e_mail", email);
            command.ExecuteNonQuery();

            Connection.Close();
            MessageBox.Show("Kayıt Başarılı.", "Program");
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
            DialogResult onay = MessageBox.Show("Çıkmak istediğinizden emin misiniz?", "Program", MessageBoxButtons.YesNo);
            if (onay == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

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
        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.Text == "E posta giriniz")
            {
                textBox3.Text = "";
                textBox3.ForeColor = Color.Black;
            }
        }
        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                textBox3.Text = "E posta giriniz";
                textBox3.ForeColor = Color.Gray;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Form ilk açıldığında placeholder'ları ayarla
            TextBoxlariSifirla();
        }


        bool move;
        int mouse_x;
        int mouse_y;
        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);

            }
        }

        public void TextBoxlariSifirla()
        {
            textBox1.Text = "Kullanıcı adı giriniz";
            textBox1.ForeColor = Color.Gray;
            textBox2.Text = "Şifre giriniz";
            textBox2.ForeColor = Color.Gray;
            textBox2.PasswordChar = '\0';
            textBox3.Text = "E posta giriniz";
            textBox3.ForeColor = Color.Gray;
        }


    }
    
}
