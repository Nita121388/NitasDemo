using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace NitasTool.Views
{
    public class NitasMessageBox : Form
    {
        private Button Button1;
        private Button Button2;
        private Button Button3;
        private Label Message;

        public NitasMessageBox(string title,string message)
        {
            InitializeComponent();
            Text = title;
            this.Message.Text = message;

            Button1 = new Button() 
            { Text = "Overwrite", DialogResult = DialogResult.Yes, Left = 100, Width = 100,Height=40, Top = 100,
                Font = new Font("Microsoft YaHei UI",12F, FontStyle.Regular, GraphicsUnit.Point, 134) };

            Button2 = new Button() 
            { Text = "AutoRename", DialogResult = DialogResult.No, Left = 220, Width = 120,
                Height = 40,
                Top = 100,
                Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134)
            };

            Button3 = new Button() 
            { Text = "Cancel", DialogResult = DialogResult.Cancel, Left = 360, Width = 100,
                Height = 40,
                Top = 100,
                Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134)
            };

            Controls.Add(Button1);
            Controls.Add(Button2);
            Controls.Add(Button3);

            this.DialogResult = DialogResult.Cancel;
            Button1.Click += (sender, e) => { this.DialogResult = DialogResult.Yes; this.Close(); };
            Button2.Click += (sender, e) => { this.DialogResult = DialogResult.No; this.Close(); };
            Button3.Click += (sender, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NitasMessageBox));
            this.Message = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Message
            // 
            this.Message.AutoSize = true;
            this.Message.Location = new System.Drawing.Point(60, 20);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(47, 12);
            this.Message.TabIndex = 0;
            this.Message.Text = "Message";
            this.Message.Font = new Font("宋体", 14F, FontStyle.Bold, GraphicsUnit.Point, 134);
            this.Message.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // NitasMessageBox
            // 
            this.ClientSize = new System.Drawing.Size(586, 159);
            this.Controls.Add(this.Message);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NitasMessageBox";
            this.Opacity = 0.8D;
            this.Padding = new System.Windows.Forms.Padding(10);
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
