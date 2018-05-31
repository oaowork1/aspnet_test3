using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Threading;

namespace Client
{
    public partial class Form1 : Form
    {
        private Form2 form;
        private FormCategoryEditor editor;
        private Informer info;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Categories";
            this.Left = 10;
            this.Top = 10;
            this.Invalidate();
            button2.Hide();

            form = new Form2(this);
            form.Activate();
            form.Show();
            form.Hide();
            form.Left = this.Left;
            form.Top = this.Top;
            form.Invalidate();

            editor = new FormCategoryEditor(this);
            editor.Activate();
            editor.Show();
            editor.Hide();
            editor.Left = this.Left;
            editor.Top = this.Top;
            editor.Invalidate();

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
            form.refreshInfo();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            refreshInfo();
        }

        public void refreshInfo()
        {
            bool loadComplete = info.GET("http://localhost:32768/api/Editor/getCategories");
            if (!loadComplete)
            {
                MessageBox.Show(info.error);
                info.categoryList = new List<Categories>();
            } else
            {
                info.categoryList = new List<Categories>();
                info.categoryList = Categories.parser(info.xmlString);
            }
            showNew();
        }

        private void showNew()
        {
            for (int i = 0; i < info.categoryList.Count; i++ )
            {
                info.categoryList[i].check = false;
            }
            checkedListBox1.Items.Clear();
            for (int i = 0; i < info.categoryList.Count; i++)
            {
                checkedListBox1.Items.Add("id=" + info.categoryList[i].id + "; name=" + info.categoryList[i].name);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();

            editor.Text = "Add new Category";
            editor.label2.Text = "Name of new Category:";
            editor.label4.Text = "";
            editor.textBox1.Text = "";
            editor.id = -1;
            editor.Left = this.Left;
            editor.Top = this.Top;
            editor.Invalidate();
            editor.Show();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {                
                int curItem = checkedListBox1.SelectedIndex;
                this.Hide();
                editor.Text = "Edit current Category";
                editor.label2.Text = "Name of Category:";
                editor.label4.Text = info.categoryList[curItem].id;
                editor.textBox1.Text = info.categoryList[curItem].name;
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

        //delete element
        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < info.categoryList.Count; i++)
            {
                if (info.categoryList[i].check)
                {
                    info.GET("http://localhost:32768/api/Editor/delCategories", 
                        "id=" + info.categoryList[i].id);
                }
            }
            Thread.Sleep(1000);
            refreshInfo();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bool show = false;
            int curItem = checkedListBox1.SelectedIndex;
            info.categoryList[curItem].check = !info.categoryList[curItem].check;
            for (int i=0; i<info.categoryList.Count; i++)
            {
                if (info.categoryList[i].check)
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
    }
}