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
    public partial class FormCategoryEditor : Form
    {
        private Form1 form;
        private Informer info;
        public int id=-1;

        public FormCategoryEditor(Form1 form)
        {
            this.form = form;
            info = Informer.getInstance();
            InitializeComponent();
        }

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
            if (button2.Text.Length<=100)
            {
                string temp = "";
                if (id==-1) //add new one
                {
                    temp="name=" + textBox1.Text;
                    info.GET("http://localhost:32768/api/Editor/addCategories", temp);
                } else //edit exists one
                {
                    temp = "id=" + label4.Text + "&name=" + textBox1.Text;
                    info.GET("http://localhost:32768/api/Editor/editCategories", temp);
                }

                this.Hide();
                form.Left = this.Left;
                form.Top = this.Top;
                form.Invalidate();
                form.Show();
                Thread.Sleep(1000);
                form.refreshInfo();
            } else
            {
                MessageBox.Show("Name length>100 symbols.");
            }
        }
    }
}
