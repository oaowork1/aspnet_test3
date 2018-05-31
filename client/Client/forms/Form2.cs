using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Client
{
    public partial class Form2 : Form
    {
        private Form1 form;
        private FormProductEditor editor;
        private Informer info;

        public Form2(Form1 form)
        {
            InitializeComponent();
            this.Text = "Products";
            this.form = form;

            editor = new FormProductEditor(this);
            editor.Activate();
            editor.Show();
            editor.Hide();
            editor.Left = this.Left;
            editor.Top = this.Top;
            editor.Invalidate();

            button2.Hide();

            info = Informer.getInstance();

            refreshInfo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            form.Left = this.Left;
            form.Top = this.Top;
            form.Invalidate();
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();

            editor.Text = "Add new Product";
            editor.label7.Text = "";
            editor.textBox2.Text = "";
            editor.textBox3.Text = "";
            editor.textBox4.Text = "";
            editor.textBox6.Text = "";
            editor.richTextBox1.Text = "";
            editor.id = -1;
            editor.Left = this.Left;
            editor.Top = this.Top;
            editor.Invalidate();
            editor.Show();
        }

        public void refreshInfo()
        {
            bool loadComplete = info.GET("http://localhost:32768/api/Editor/getProducts");
            if (!loadComplete)
            {
                MessageBox.Show(info.error);
                info.productList = new List<Product>();
            }
            else
            {
                info.productList = new List<Product>();
                info.productList = Product.parser(info.xmlString);
            }
            showNew();
        }

        private void showNew()
        {
            checkedListBox1.Items.Clear();
            for (int i = 0; i < info.productList.Count; i++)
            {
                checkedListBox1.Items.Add("id=" + info.productList[i].id +
                    "; name=" + info.productList[i].name +
                    "; price=" + info.productList[i].price +
                     "; count=" + info.productList[i].count +
                      "; categoryId=" + info.productList[i].categoryId);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int curItem = checkedListBox1.SelectedIndex;
            richTextBox1.Text = info.productList[curItem].description;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            refreshInfo();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int curItem = checkedListBox1.SelectedIndex;
            richTextBox1.Text = info.productList[curItem].description;
        }

        private void checkedListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                int curItem = checkedListBox1.SelectedIndex;
                this.Hide();
                editor.Text = "Edit current Product";
                editor.label7.Text = info.productList[curItem].id;
                editor.textBox2.Text = info.productList[curItem].name;
                editor.textBox3.Text = info.productList[curItem].price;
                editor.textBox4.Text = info.productList[curItem].count;
                editor.textBox6.Text = info.productList[curItem].categoryId;
                editor.richTextBox1.Text = info.productList[curItem].description;
                editor.id = curItem;
                editor.Left = this.Left;
                editor.Top = this.Top;
                editor.Invalidate();
                editor.Show();
            }
            catch (Exception ex)
            {

            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bool show = false;
            int curItem = checkedListBox1.SelectedIndex;
            info.productList[curItem].check = !info.productList[curItem].check;
            for (int i = 0; i < info.productList.Count; i++)
            {
                if (info.productList[i].check)
                {
                    button2.Show();
                    show = true;
                    break;
                }
            }
            if (!show)
            {
                button2.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < info.productList.Count; i++)
            {
                if (info.productList[i].check)
                {
                    info.GET("http://localhost:32768/api/Editor/delProduct",
                        "id=" + info.productList[i].id);
                }
            }
            Thread.Sleep(1000);
            refreshInfo();
        }

        
    }
}
