using System.Windows.Forms;

namespace NitasTool.Views
{
    public class NitasMessageBox : Form
    {
        private Button Button1;
        private Button Button2;
        private Button Button3;
        private TableLayoutPanel tableLayoutPanel;
        private Label Message;
        private Label FileNameLabel;

        public NitasMessageBox(string title, string message, string fileName)
        {
            InitializeComponent();
            Text = title;
            this.Message.Text = message;
            this.FileNameLabel.Text = fileName;
            if (string.IsNullOrEmpty(fileName))
            {
                this.FileNameLabel.Visible = false;
            }
            else
            {
                this.FileNameLabel.Visible = true;
            }
        }

        private void InitializeComponent()
        {
            this.Message = new System.Windows.Forms.Label();
            this.FileNameLabel = new System.Windows.Forms.Label();
            this.Button1 = new System.Windows.Forms.Button();
            this.Button2 = new System.Windows.Forms.Button();
            this.Button3 = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Message
            // 
            this.Message.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.Message, 3);
            this.Message.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Message.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Message.Location = new System.Drawing.Point(3, 0);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(680, 50);
            this.Message.TabIndex = 0;
            this.Message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FileNameLabel
            // 
            this.FileNameLabel.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.FileNameLabel, 3);
            this.FileNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileNameLabel.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FileNameLabel.Location = new System.Drawing.Point(3, 50);
            this.FileNameLabel.Name = "FileNameLabel";
            this.FileNameLabel.Size = new System.Drawing.Size(680, 50);
            this.FileNameLabel.TabIndex = 4;
            this.FileNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Button1
            // 
            this.Button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Button1.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Button1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button1.Location = new System.Drawing.Point(39, 125);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(150, 50);
            this.Button1.TabIndex = 1;
            this.Button1.Text = "Overwrite";
            // 
            // Button2
            // 
            this.Button2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Button2.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Button2.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button2.Location = new System.Drawing.Point(267, 125);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(150, 50);
            this.Button2.TabIndex = 2;
            this.Button2.Text = "AutoRename";
            // 
            // Button3
            // 
            this.Button3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Button3.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button3.Location = new System.Drawing.Point(496, 125);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(150, 50);
            this.Button3.TabIndex = 3;
            this.Button3.Text = "Cancel";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel.Controls.Add(this.Message, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.FileNameLabel, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.Button1, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.Button2, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.Button3, 2, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(686, 200);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // NitasMessageBox
            // 
            this.ClientSize = new System.Drawing.Size(686, 250);
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "NitasMessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }
    }

}
