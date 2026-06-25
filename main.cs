using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace LabelPlusMark
{
    public partial class main : Form
    {
        private const int WS_EX_NOACTIVATE = 0x08000000;
        private const int WM_MOUSEACTIVATE = 0x0021;
        private const int MA_NOACTIVATE = 3;
        private string DefaultFilePath = "symbols.txt";
        public main()
        {
            InitializeComponent();
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            LoadSymbols(DefaultFilePath);// 加载符号列表

            listBox1.MouseDoubleClick += listView1_SelectedIndexChanged;// 双击事件
            button1.Click += button1_Click;// 点击事件
            button2.Click += button2_Click;// 删除事件
            //button3.Click += button3_Click;// 选择文件导入
        }
/*********************窗体最上层******************************************************/
        // 窗口显示时不抢焦点
        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        // 给窗口添加“不激活”样式
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_NOACTIVATE;
                return cp;
            }
        }

        // 鼠标点击窗体时，不让窗体获得焦点
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE)
            {
                m.Result = (IntPtr)MA_NOACTIVATE;
                return;
            }

            base.WndProc(ref m);
        }
        /*********************窗体最上层******************************************************/

        private void LoadSymbols(string filePath)
        {
            listBox1.Items.Clear();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        listBox1.Items.Add(line);
                    }
                }
            }
            else
            {
                SaveSymbols();
            }
        }

        private void SaveSymbols()
        {
            string[] symbols = new string[listBox1.Items.Count];

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                symbols[i] = listBox1.Items[i].ToString();
            }

            File.WriteAllLines(DefaultFilePath, symbols);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddSymbolForm addForm = new AddSymbolForm();
            addForm.TopMost = true;

            // 弹出窗口，并等待用户点确定或取消
            DialogResult result = addForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                string newText = addForm.SymbolText;

                if (!string.IsNullOrWhiteSpace(newText))
                {
                    listBox1.Items.Add(newText);
                    SaveSymbols();
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("请先选择要删除的项目。");
                return;
            }
            int index = listBox1.SelectedIndex;
            listBox1.Items.Remove(listBox1.SelectedItem);

            if (listBox1.Items.Count == 0)
            {
                listBox1.ClearSelected();
                return;
            }

            // 删除后选中上一条
            int newIndex = index - 1;
            // 如果原来删的是第一条，就选中新的第一条
            if (newIndex < 0)
            {
                newIndex = 0;
            }
            listBox1.SelectedIndex = newIndex;

            SaveSymbols();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "选择符号文件";
            openFileDialog.Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
            openFileDialog.Multiselect = false;

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                DefaultFilePath = openFileDialog.FileName;

                label1.Text = Path.GetFileName(DefaultFilePath);

                LoadSymbols(DefaultFilePath);
            }
        }
        private void listView1_SelectedIndexChanged(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.Location);

            // 双击空白区域，不执行复制
            if (index == ListBox.NoMatches)
            {
                return;
            }

            // 选中鼠标双击的那一项
            listBox1.SelectedIndex = index;

            string text = listBox1.Items[index].ToString();

            Clipboard.SetText(text);
            SendKeys.SendWait("^v");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
