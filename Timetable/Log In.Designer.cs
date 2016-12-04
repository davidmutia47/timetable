namespace Timetable
{
    partial class Log_In
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.login = new iTalk.iTalk_Button_2();
            this.iTalk_Panel2 = new iTalk.iTalk_Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tbLoginPass = new iTalk.iTalk_TextBox_Small();
            this.iTalk_Label2 = new iTalk.iTalk_Label();
            this.iTalk_Panel1 = new iTalk.iTalk_Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbLoginId = new iTalk.iTalk_TextBox_Small();
            this.iTalk_Label1 = new iTalk.iTalk_Label();
            this.panel1.SuspendLayout();
            this.iTalk_Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.iTalk_Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(590, 83);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Lucida Bright", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(168, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Log in to System";
            // 
            // login
            // 
            this.login.BackColor = System.Drawing.Color.Transparent;
            this.login.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.login.ForeColor = System.Drawing.Color.White;
            this.login.Image = null;
            this.login.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.login.Location = new System.Drawing.Point(291, 375);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(199, 57);
            this.login.TabIndex = 3;
            this.login.Text = "Login";
            this.login.TextAlignment = System.Drawing.StringAlignment.Center;
            this.login.Click += new System.EventHandler(this.login_Click);
            // 
            // iTalk_Panel2
            // 
            this.iTalk_Panel2.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_Panel2.Controls.Add(this.pictureBox2);
            this.iTalk_Panel2.Controls.Add(this.tbLoginPass);
            this.iTalk_Panel2.Controls.Add(this.iTalk_Label2);
            this.iTalk_Panel2.Location = new System.Drawing.Point(21, 259);
            this.iTalk_Panel2.Name = "iTalk_Panel2";
            this.iTalk_Panel2.Padding = new System.Windows.Forms.Padding(5);
            this.iTalk_Panel2.Size = new System.Drawing.Size(535, 43);
            this.iTalk_Panel2.TabIndex = 2;
            this.iTalk_Panel2.Text = "iTalk_Panel2";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Timetable.Properties.Resources.padlock;
            this.pictureBox2.Location = new System.Drawing.Point(483, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(45, 36);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // tbLoginPass
            // 
            this.tbLoginPass.BackColor = System.Drawing.Color.Transparent;
            this.tbLoginPass.Font = new System.Drawing.Font("Tahoma", 11F);
            this.tbLoginPass.ForeColor = System.Drawing.Color.DimGray;
            this.tbLoginPass.Location = new System.Drawing.Point(145, 8);
            this.tbLoginPass.MaxLength = 32767;
            this.tbLoginPass.Multiline = false;
            this.tbLoginPass.Name = "tbLoginPass";
            this.tbLoginPass.ReadOnly = false;
            this.tbLoginPass.Size = new System.Drawing.Size(331, 28);
            this.tbLoginPass.TabIndex = 5;
            this.tbLoginPass.Tag = "";
            this.tbLoginPass.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbLoginPass.UseSystemPasswordChar = true;
            // 
            // iTalk_Label2
            // 
            this.iTalk_Label2.AutoSize = true;
            this.iTalk_Label2.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_Label2.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iTalk_Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.iTalk_Label2.Location = new System.Drawing.Point(18, 5);
            this.iTalk_Label2.Name = "iTalk_Label2";
            this.iTalk_Label2.Size = new System.Drawing.Size(109, 28);
            this.iTalk_Label2.TabIndex = 0;
            this.iTalk_Label2.Text = "Password:";
            // 
            // iTalk_Panel1
            // 
            this.iTalk_Panel1.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_Panel1.Controls.Add(this.pictureBox1);
            this.iTalk_Panel1.Controls.Add(this.tbLoginId);
            this.iTalk_Panel1.Controls.Add(this.iTalk_Label1);
            this.iTalk_Panel1.Location = new System.Drawing.Point(21, 133);
            this.iTalk_Panel1.Name = "iTalk_Panel1";
            this.iTalk_Panel1.Padding = new System.Windows.Forms.Padding(5);
            this.iTalk_Panel1.Size = new System.Drawing.Size(535, 43);
            this.iTalk_Panel1.TabIndex = 1;
            this.iTalk_Panel1.Text = "iTalk_Panel1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Timetable.Properties.Resources.user;
            this.pictureBox1.Location = new System.Drawing.Point(483, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(45, 36);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // tbLoginId
            // 
            this.tbLoginId.BackColor = System.Drawing.Color.Transparent;
            this.tbLoginId.Font = new System.Drawing.Font("Tahoma", 11F);
            this.tbLoginId.ForeColor = System.Drawing.Color.DimGray;
            this.tbLoginId.Location = new System.Drawing.Point(145, 8);
            this.tbLoginId.MaxLength = 32767;
            this.tbLoginId.Multiline = false;
            this.tbLoginId.Name = "tbLoginId";
            this.tbLoginId.ReadOnly = false;
            this.tbLoginId.Size = new System.Drawing.Size(331, 28);
            this.tbLoginId.TabIndex = 5;
            this.tbLoginId.Tag = "";
            this.tbLoginId.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbLoginId.UseSystemPasswordChar = false;
            // 
            // iTalk_Label1
            // 
            this.iTalk_Label1.AutoSize = true;
            this.iTalk_Label1.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_Label1.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iTalk_Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.iTalk_Label1.Location = new System.Drawing.Point(18, 5);
            this.iTalk_Label1.Name = "iTalk_Label1";
            this.iTalk_Label1.Size = new System.Drawing.Size(87, 28);
            this.iTalk_Label1.TabIndex = 0;
            this.iTalk_Label1.Text = "User Id:";
            // 
            // Log_In
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(590, 504);
            this.Controls.Add(this.login);
            this.Controls.Add(this.iTalk_Panel2);
            this.Controls.Add(this.iTalk_Panel1);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.MaximumSize = new System.Drawing.Size(606, 543);
            this.MinimumSize = new System.Drawing.Size(606, 543);
            this.Name = "Log_In";
            this.Text = "Log In";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Log_In_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.iTalk_Panel2.ResumeLayout(false);
            this.iTalk_Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.iTalk_Panel1.ResumeLayout(false);
            this.iTalk_Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private iTalk.iTalk_Panel iTalk_Panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private iTalk.iTalk_TextBox_Small tbLoginId;
        private iTalk.iTalk_Label iTalk_Label1;
        private iTalk.iTalk_Panel iTalk_Panel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private iTalk.iTalk_TextBox_Small tbLoginPass;
        private iTalk.iTalk_Label iTalk_Label2;
        private System.Windows.Forms.Label label2;
        private iTalk.iTalk_Button_2 login;
    }
}