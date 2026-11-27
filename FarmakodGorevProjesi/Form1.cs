using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FarmakodGorevProjesi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string userNameDt = "mustafa";
        public string passwordDt = "kale";


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
                {
                    var userName = textBox1.Text;
                    var password = textBox2.Text;

                    if (userName == userNameDt && password == passwordDt)
                    {

                        Form2 form2 = new Form2();
                        form2.userName = userName;
                        this.Hide();
                        form2.ShowDialog();

                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Hatali Kullanici Adı veya Şifre");

                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Lütfen Kullanici Adı ve Şifre Girin");
                }
            }
            catch
            {

            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
