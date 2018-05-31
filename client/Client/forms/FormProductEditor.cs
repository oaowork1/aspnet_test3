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
    public partial class FormProductEditor : Form
    {
        public Form2 form;
        private Informer info;
        public int id = -1;
        public FormProductEditor(Form2 form)
        {
            this.form = form;
            info = Informer.getInstance();
            InitializeComponent();
        }


        //back
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            form.Left = this.Left;
            form.Top = this.Top;
            form.Invalidate();
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text.Length <= 100)
            {
                string temp = "";
                if (id == -1) //add new one
                {
                    temp = "name=" + textBox2.Text +
                     "&price=" + textBox3.Text +
                     "&count=" + textBox4.Text +
                     "&categoryid=" + textBox6.Text +
                     "&description=" + richTextBox1.Text;
                    info.GET("http://localhost:32768/api/Editor/addProduct", temp);
                }
                else //edit exists one
                {
                    if (!info.productList[id].name.Equals(textBox2.Text))
                    {
                        temp = "id=" + label7.Text + "&field=name&value=" + textBox2.Text;
                        info.GET("http://localhost:32768/api/Editor/editProduct", temp);
                    }
                    if (!info.productList[id].price.Equals(textBox3.Text))
                    {
                        temp = "id=" + label7.Text + "&field=price&value=" + textBox3.Text;
                        info.GET("http://localhost:32768/api/Editor/editProduct", temp);
                    }
                    if (!info.productList[id].count.Equals(textBox4.Text))
                    {
                        temp = "id=" + label7.Text + "&field=count&value=" + textBox4.Text;
                        info.GET("http://localhost:32768/api/Editor/editProduct", temp);
                    }
                    if (!info.productList[id].categoryId.Equals(textBox6.Text))
                    {
                        temp = "id=" + label7.Text + "&field=categoryid&value=" + textBox6.Text;
                        info.GET("http://localhost:32768/api/Editor/editProduct", temp);
                    }
                    if (!info.productList[id].description.Equals(richTextBox1.Text))
                    {
                        temp = "id=" + label7.Text + "&field=description&value=" + richTextBox1.Text;
                        info.GET("http://localhost:32768/api/Editor/editProduct", temp);
                    }
                }

                this.Hide();
                form.Left = this.Left;
                form.Top = this.Top;
                form.Invalidate();
                form.Show();
                Thread.Sleep(1000);
                form.refreshInfo();
            }
            else
            {
                MessageBox.Show("Name length>100 symbols.");
            }
        }
    }
}
