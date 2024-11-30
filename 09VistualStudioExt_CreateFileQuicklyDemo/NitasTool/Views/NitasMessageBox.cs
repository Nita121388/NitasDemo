using System;
using System.Drawing;
using System.Windows.Forms;

namespace NitasTool.Views
{
    public class NitasMessageBox : Form
    {
        private Button Button1;
        private Button Button2;
        private Button Button3;
        private Label Message;

        public NitasMessageBox(string title, string message)
        {
            InitializeComponent();
            Text = title;
            this.Message.Text = message;
        }

        private void InitializeComponent()
        {
            this.Message = new Label();
            this.Button1 = new Button();
            this.Button2 = new Button();
            this.Button3 = new Button();
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();

            // 
            // Message
            // 
            this.Message.AutoSize = true;
            this.Message.Font = new Font("宋体", 14F, FontStyle.Bold, GraphicsUnit.Point, 134);
            this.Message.TextAlign = ContentAlignment.MiddleCenter;
            this.Message.Dock = DockStyle.Fill;

            // 
            // Button1
            // 
            this.Button1.Text = "Overwrite";
            this.Button1.DialogResult = DialogResult.Yes;
            this.Button1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.Button1.Size = new Size(150, 50);
            this.Button1.Anchor = AnchorStyles.None;

            // 
            // Button2
            // 
            this.Button2.Text = "AutoRename";
            this.Button2.DialogResult = DialogResult.No;
            this.Button2.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.Button2.Size = new Size(150, 50);
            this.Button2.Anchor = AnchorStyles.None;

            // 
            // Button3
            // 
            this.Button3.Text = "Cancel";
            this.Button3.DialogResult = DialogResult.Cancel;
            this.Button3.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.Button3.Size = new Size(150, 50);
            this.Button3.Anchor = AnchorStyles.None;

            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 3;
            tableLayoutPanel.RowCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Controls.Add(this.Message, 0, 0);
            tableLayoutPanel.SetColumnSpan(this.Message, 3);
            tableLayoutPanel.Controls.Add(this.Button1, 0, 1);
            tableLayoutPanel.Controls.Add(this.Button2, 1, 1);
            tableLayoutPanel.Controls.Add(this.Button3, 2, 1);

            // 
            // NitasMessageBox
            // 
            this.ClientSize = new System.Drawing.Size(686, 200);
            this.Controls.Add(tableLayoutPanel);
            this.Name = "NitasMessageBox";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }
    }
}
