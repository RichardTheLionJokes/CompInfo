
namespace CompInfo
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.GetAllNames = new System.Windows.Forms.Button();
            this.AllCompNames = new System.Windows.Forms.ListBox();
            this.Scan = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.Ping = new System.Windows.Forms.Button();
            this.View = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GetAllNames
            // 
            this.GetAllNames.Location = new System.Drawing.Point(12, 12);
            this.GetAllNames.Name = "GetAllNames";
            this.GetAllNames.Size = new System.Drawing.Size(84, 23);
            this.GetAllNames.TabIndex = 0;
            this.GetAllNames.Text = "GetAllNames";
            this.GetAllNames.UseVisualStyleBackColor = true;
            this.GetAllNames.Click += new System.EventHandler(this.GetAllNames_Click);
            // 
            // AllCompNames
            // 
            this.AllCompNames.FormattingEnabled = true;
            this.AllCompNames.Location = new System.Drawing.Point(118, 12);
            this.AllCompNames.Name = "AllCompNames";
            this.AllCompNames.Size = new System.Drawing.Size(173, 420);
            this.AllCompNames.Sorted = true;
            this.AllCompNames.TabIndex = 1;
            // 
            // Scan
            // 
            this.Scan.Location = new System.Drawing.Point(12, 91);
            this.Scan.Name = "Scan";
            this.Scan.Size = new System.Drawing.Size(84, 23);
            this.Scan.TabIndex = 2;
            this.Scan.Text = "Scan";
            this.Scan.UseVisualStyleBackColor = true;
            this.Scan.Click += new System.EventHandler(this.Scan_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(337, 14);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(337, 230);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // Ping
            // 
            this.Ping.Location = new System.Drawing.Point(12, 50);
            this.Ping.Name = "Ping";
            this.Ping.Size = new System.Drawing.Size(84, 23);
            this.Ping.TabIndex = 4;
            this.Ping.Text = "Ping";
            this.Ping.UseVisualStyleBackColor = true;
            this.Ping.Click += new System.EventHandler(this.Ping_Click);
            // 
            // View
            // 
            this.View.Location = new System.Drawing.Point(12, 131);
            this.View.Name = "View";
            this.View.Size = new System.Drawing.Size(84, 23);
            this.View.TabIndex = 5;
            this.View.Text = "View";
            this.View.UseVisualStyleBackColor = true;
            this.View.Click += new System.EventHandler(this.View_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.View);
            this.Controls.Add(this.Ping);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.Scan);
            this.Controls.Add(this.AllCompNames);
            this.Controls.Add(this.GetAllNames);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button GetAllNames;
        private System.Windows.Forms.ListBox AllCompNames;
        private System.Windows.Forms.Button Scan;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button Ping;
        private System.Windows.Forms.Button View;
    }
}

