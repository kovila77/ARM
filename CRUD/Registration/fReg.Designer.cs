namespace Registration
{
    partial class fReg
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
            this.components = new System.ComponentModel.Container();
            this.pnLogin = new System.Windows.Forms.Panel();
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.lLogin = new System.Windows.Forms.Label();
            this.btRegister = new System.Windows.Forms.Button();
            this.epMain = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnPassword = new System.Windows.Forms.Panel();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lPassword = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.epMain)).BeginInit();
            this.pnPassword.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnLogin
            // 
            this.pnLogin.Controls.Add(this.tbLogin);
            this.pnLogin.Controls.Add(this.lLogin);
            this.pnLogin.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnLogin.Location = new System.Drawing.Point(0, 0);
            this.pnLogin.Name = "pnLogin";
            this.pnLogin.Padding = new System.Windows.Forms.Padding(10, 10, 30, 10);
            this.pnLogin.Size = new System.Drawing.Size(414, 47);
            this.pnLogin.TabIndex = 0;
            // 
            // tbLogin
            // 
            this.tbLogin.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbLogin.Location = new System.Drawing.Point(58, 10);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(326, 20);
            this.tbLogin.TabIndex = 0;
            this.tbLogin.TextChanged += new System.EventHandler(this.tbLogin_TextChanged);
            // 
            // lLogin
            // 
            this.lLogin.Dock = System.Windows.Forms.DockStyle.Left;
            this.lLogin.Location = new System.Drawing.Point(10, 10);
            this.lLogin.Name = "lLogin";
            this.lLogin.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lLogin.Size = new System.Drawing.Size(48, 27);
            this.lLogin.TabIndex = 1;
            this.lLogin.Text = "Логин:";
            // 
            // btRegister
            // 
            this.btRegister.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btRegister.Enabled = false;
            this.btRegister.Location = new System.Drawing.Point(0, 167);
            this.btRegister.Name = "btRegister";
            this.btRegister.Size = new System.Drawing.Size(414, 23);
            this.btRegister.TabIndex = 1;
            this.btRegister.Text = "Зарегистрироваться";
            this.btRegister.UseVisualStyleBackColor = true;
            this.btRegister.Click += new System.EventHandler(this.btRegister_Click);
            // 
            // epMain
            // 
            this.epMain.ContainerControl = this;
            // 
            // pnPassword
            // 
            this.pnPassword.Controls.Add(this.tbPassword);
            this.pnPassword.Controls.Add(this.lPassword);
            this.pnPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnPassword.Location = new System.Drawing.Point(0, 47);
            this.pnPassword.Name = "pnPassword";
            this.pnPassword.Padding = new System.Windows.Forms.Padding(10, 10, 30, 10);
            this.pnPassword.Size = new System.Drawing.Size(414, 47);
            this.pnPassword.TabIndex = 2;
            // 
            // tbPassword
            // 
            this.tbPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbPassword.Location = new System.Drawing.Point(58, 10);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(326, 20);
            this.tbPassword.TabIndex = 0;
            this.tbPassword.UseSystemPasswordChar = true;
            this.tbPassword.TextChanged += new System.EventHandler(this.tbPassword_TextChanged);
            // 
            // lPassword
            // 
            this.lPassword.Dock = System.Windows.Forms.DockStyle.Left;
            this.lPassword.Location = new System.Drawing.Point(10, 10);
            this.lPassword.Name = "lPassword";
            this.lPassword.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lPassword.Size = new System.Drawing.Size(48, 27);
            this.lPassword.TabIndex = 1;
            this.lPassword.Text = "Пароль:";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(30, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(354, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "Дата регистрации";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // dtpDate
            // 
            this.dtpDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.dtpDate.Location = new System.Drawing.Point(30, 29);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(354, 20);
            this.dtpDate.TabIndex = 2;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtpDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 94);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(30, 10, 30, 10);
            this.panel1.Size = new System.Drawing.Size(414, 96);
            this.panel1.TabIndex = 3;
            // 
            // fReg
            // 
            this.AcceptButton = this.btRegister;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 190);
            this.Controls.Add(this.btRegister);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnPassword);
            this.Controls.Add(this.pnLogin);
            this.MinimumSize = new System.Drawing.Size(300, 150);
            this.Name = "fReg";
            this.Text = "Регистрация нового пользователя";
            this.Load += new System.EventHandler(this.fReg_Load);
            this.pnLogin.ResumeLayout(false);
            this.pnLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.epMain)).EndInit();
            this.pnPassword.ResumeLayout(false);
            this.pnPassword.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnLogin;
        private System.Windows.Forms.TextBox tbLogin;
        private System.Windows.Forms.Label lLogin;
        private System.Windows.Forms.Button btRegister;
        private System.Windows.Forms.ErrorProvider epMain;
        private System.Windows.Forms.Panel pnPassword;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Panel panel1;
    }
}

