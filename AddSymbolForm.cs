using System;
using System.Drawing;
using System.Windows.Forms;

namespace LabelPlusMark
{
    public partial class AddSymbolForm : Form
    {
        private TextBox textBoxInput;
        private Button buttonOK;
        private Button buttonCancel;

        public string SymbolText
        {
            get { return textBoxInput.Text.Trim(); }
        }

        public AddSymbolForm()
        {
            InitializeComponent();

            this.Text = "新增符号";
            this.Size = new Size(260, 150);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            textBoxInput = new TextBox();
            textBoxInput.Location = new Point(20, 20);
            textBoxInput.Size = new Size(200, 25);
            this.Controls.Add(textBoxInput);

            buttonOK = new Button();
            buttonOK.Text = "确定";
            buttonOK.Location = new Point(35, 65);
            buttonOK.Size = new Size(75, 30);
            buttonOK.Click += ButtonOK_Click;
            this.Controls.Add(buttonOK);

            buttonCancel = new Button();
            buttonCancel.Text = "取消";
            buttonCancel.Location = new Point(130, 65);
            buttonCancel.Size = new Size(75, 30);
            buttonCancel.Click += ButtonCancel_Click;
            this.Controls.Add(buttonCancel);

            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxInput.Text))
            {
                MessageBox.Show("请输入内容。");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}