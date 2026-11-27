using FarmakodGorevProjesi.Domain;
using FarmakodGorevProjesi.Services;
using FarmakodGorevProjesi.Results;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace FarmakodGorevProjesi
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            configureDataGridView();
            configureRobotPosition();
            timer1.Start();

        }
        CalculateDistanceService calculateDistance = new CalculateDistanceService();
        FindProductService findProductService = new FindProductService();
        Robot robot = new Robot();
        List<Product> productList = new List<Product>();

        public string userName = "";
        public int rowCount = 11;
        public int columnCount = 3;

        private void Form2_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                label1.Text = "Hoş Geldiniz: " + userName;
            }
        }
        private void btnCreateProduct_Click(object sender, EventArgs e)
        {
            int X;
            int Y;
            //tüm alanlar dolu mu?
            if (string.IsNullOrEmpty(txtBoxCode.Text) || string.IsNullOrEmpty(textBoxName.Text) || !int.TryParse(textBox3.Text,out X) || !int.TryParse(textBox4.Text, out Y))
            {
                MessageBox.Show("Lütfen Tüm Alanları doldurun");
                return;
            }
            //depo sinirlari icinde mi veri girildi?
            if (X >= rowCount && Y >= columnCount)
            {
                MessageBox.Show("Hatali Veri Girdiniz");
                return;
            }

            //ayni koda sahip ürün var mi?
            if (productList.Any(c => c.code == txtBoxCode.Text))
            {
                MessageBox.Show("Bu Ürün Zaten Ekli");
                return;
            }

            //ayni konumda başka ürün var mı?
            if (productList.Any(c => c.X == X && c.Y == Y))
            {
                MessageBox.Show("Bu Konumda Ürün Mevcut");
                return;
            }
            Product product = new Product();
            product.code = txtBoxCode.Text;
            product.name = textBoxName.Text;
            product.X = X;
            product.Y = Y;
            productList.Add(product);

            foreach (var item in productList)
            {
                dataGridView1.Rows[item.X].Cells[item.Y].Value = item.code;

            }
        }


        public void configureDataGridView()
        {
            dataGridView1.RowCount = rowCount;
            dataGridView1.ColumnCount = columnCount;
        }

        public void configureRobotPosition()
        {
            label7.Text = "Robot: " + "(" + Convert.ToString(robot.X) + "," + Convert.ToString(robot.Y) + ")";
            var selectedCell = dataGridView1.Rows[robot.X].Cells[robot.Y];
            selectedCell.Selected = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            var content = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            if (content != null && !listBox1.Items.Contains(content.ToString()))
            {
                listBox1.Items.AddRange(content.ToString());
            }
            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = null;
        }

        private void btnGoToProduct_Click(object sender, EventArgs e)
        {
            //urun var mı kontrolü
            var result = findProductService.FindProduct(txtBoxCode.Text,productList);
            if (result.success)
            {
                int distance = calculateDistance.calculate(robot.X, robot.Y, result.product.X, result.product.Y);
                infoLabel.Text = "Hedefe Uzaklik: " + distance + " Birim";
                robot.X = result.product.X;
                robot.Y = result.product.Y;
            }
            else
            {
                MessageBox.Show("Ürün Bulunamadi");
            }
        }

        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            var result = findProductService.FindProduct(txtBoxCode.Text, productList);
            if (result.success)
            {
                txtBoxCode.Text = result.product.code;
                textBoxName.Text = result.product.name;
                textBox3.Text = Convert.ToString(result.product.X);
                textBox4.Text = Convert.ToString(result.product.Y);
                infoLabel.Text = "Ürün Bulundu: " + "Konum " + "(" + textBox3.Text + "," + textBox4.Text + ")";
            }
            else
            {
                MessageBox.Show("Ürün Bulunamadi");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            configureRobotPosition();
        }

        private async void btnCreateOrder_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1)
            {
                MessageBox.Show("Listeye bir ürün ekleyin..");
                return;
            }

            logMessage.Text = "Sipariş Oluşturuluyor...";
            await Task.Delay(1500);


            //listBOX'da ki  siparişleri liste ye alalım.
            List<Product> orderList = new List<Product>();
            if (listBox1.Items.Count > 0)
                foreach (var item in listBox1.Items)
                {
                    orderList.Add(productList.First(c => c.code == item.ToString()));
                }



            //boş bir sözlük oluşturalım.
            var orderDistance = new Dictionary<String, int>
            {

            };


            logMessage.Text = "Robot Hazir...";
            await Task.Delay(1000);

            //ana algoritmayı çalıştıralım..
            while (listBox1.Items.Count > 0)
            {
                foreach (var item in orderList)
                {
                    logMessage.Text = "En yakın ürün Hesaplaniyor...";
                    await Task.Delay(1000);
                    logMessage.Text = "En yakın ürün Hesaplaniyor..";
                    var distance = calculateDistance.calculate(robot.X, robot.Y, item.X, item.Y);
                    orderDistance.Add(item.code, distance);
                }
                var orderedDistance = orderDistance.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                var firstProduct = orderList.Find(x => x.code == orderedDistance.Keys.First());


                //log mesajı
                logMessage.Text = "(" + firstProduct.X + "," + firstProduct.Y + ")" + "Konumundaki Ürüne Gidiliyor...";
                await Task.Delay(1000);


                robot.X = firstProduct.X;
                robot.Y = firstProduct.Y;

                var selectedCell = dataGridView1.Rows[robot.X].Cells[robot.Y];
                selectedCell.Selected = true;


                listBox1.Items.Remove(firstProduct.code);
                orderList.Remove(firstProduct);
                orderDistance.Clear();


                logMessage.Text = "Ürün Başarıyla Toplandi...";
                await Task.Delay(1000);

                logMessage.Text = "Robot Sonraki Ürüne Gidiyor...";
                await Task.Delay(1000);
                logMessage.Text = " ";
            }
            MessageBox.Show("Sipariş Toplama İşlemi Tamamlandi...");
            await Task.Delay(1000);

        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem!=null)
                listBox1.Items.Remove(listBox1.SelectedItem);
            else
            {
                MessageBox.Show("Silinecek Bir Ürün Seçin");
            }
        }
    }
}
