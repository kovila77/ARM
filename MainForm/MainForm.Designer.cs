namespace MainForm
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.правкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьЗановоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.перезагрузитьВсеТаблицыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.инструментыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tbBR = new System.Windows.Forms.TabPage();
            this.dgvBR = new System.Windows.Forms.DataGridView();
            this.tpR = new System.Windows.Forms.TabPage();
            this.dgvR = new System.Windows.Forms.DataGridView();
            this.tpB = new System.Windows.Forms.TabPage();
            this.dgvB = new System.Windows.Forms.DataGridView();
            this.tpBRC = new System.Windows.Forms.TabPage();
            this.dgvBRC = new System.Windows.Forms.DataGridView();
            this.tpBRP = new System.Windows.Forms.TabPage();
            this.dgvBRP = new System.Windows.Forms.DataGridView();
            this.tpO = new System.Windows.Forms.TabPage();
            this.dgvO = new System.Windows.Forms.DataGridView();
            this.tpSR = new System.Windows.Forms.TabPage();
            this.dgvSR = new System.Windows.Forms.DataGridView();
            this.menuStrip1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tbBR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBR)).BeginInit();
            this.tpR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvR)).BeginInit();
            this.tpB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvB)).BeginInit();
            this.tpBRC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBRC)).BeginInit();
            this.tpBRP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBRP)).BeginInit();
            this.tpO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvO)).BeginInit();
            this.tpSR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSR)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.правкаToolStripMenuItem,
            this.инструментыToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // правкаToolStripMenuItem
            // 
            this.правкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.загрузитьЗановоToolStripMenuItem,
            this.перезагрузитьВсеТаблицыToolStripMenuItem});
            this.правкаToolStripMenuItem.Name = "правкаToolStripMenuItem";
            this.правкаToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.правкаToolStripMenuItem.Text = "Правка";
            // 
            // загрузитьЗановоToolStripMenuItem
            // 
            this.загрузитьЗановоToolStripMenuItem.Name = "загрузитьЗановоToolStripMenuItem";
            this.загрузитьЗановоToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.загрузитьЗановоToolStripMenuItem.Text = "Загрузить заново";
            this.загрузитьЗановоToolStripMenuItem.Click += new System.EventHandler(this.Reload);
            // 
            // перезагрузитьВсеТаблицыToolStripMenuItem
            // 
            this.перезагрузитьВсеТаблицыToolStripMenuItem.Name = "перезагрузитьВсеТаблицыToolStripMenuItem";
            this.перезагрузитьВсеТаблицыToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.перезагрузитьВсеТаблицыToolStripMenuItem.Text = "Перезагрузить все таблицы";
            this.перезагрузитьВсеТаблицыToolStripMenuItem.Click += new System.EventHandler(this.FullReload);
            // 
            // инструментыToolStripMenuItem
            // 
            this.инструментыToolStripMenuItem.Name = "инструментыToolStripMenuItem";
            this.инструментыToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.инструментыToolStripMenuItem.Text = "Инструменты";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tbBR);
            this.tabControl.Controls.Add(this.tpR);
            this.tabControl.Controls.Add(this.tpB);
            this.tabControl.Controls.Add(this.tpBRC);
            this.tabControl.Controls.Add(this.tpBRP);
            this.tabControl.Controls.Add(this.tpO);
            this.tabControl.Controls.Add(this.tpSR);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 426);
            this.tabControl.TabIndex = 1;
            // 
            // tbBR
            // 
            this.tbBR.Controls.Add(this.dgvBR);
            this.tbBR.Location = new System.Drawing.Point(4, 22);
            this.tbBR.Name = "tbBR";
            this.tbBR.Padding = new System.Windows.Forms.Padding(3);
            this.tbBR.Size = new System.Drawing.Size(792, 400);
            this.tbBR.TabIndex = 0;
            this.tbBR.Text = "Состояние ресурсов";
            this.tbBR.UseVisualStyleBackColor = true;
            // 
            // dgvBR
            // 
            this.dgvBR.AllowUserToAddRows = false;
            this.dgvBR.AllowUserToDeleteRows = false;
            this.dgvBR.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBR.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvBR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBR.Location = new System.Drawing.Point(3, 3);
            this.dgvBR.Name = "dgvBR";
            this.dgvBR.ReadOnly = true;
            this.dgvBR.Size = new System.Drawing.Size(786, 394);
            this.dgvBR.TabIndex = 0;
            // 
            // tpR
            // 
            this.tpR.Controls.Add(this.dgvR);
            this.tpR.Location = new System.Drawing.Point(4, 22);
            this.tpR.Name = "tpR";
            this.tpR.Padding = new System.Windows.Forms.Padding(3);
            this.tpR.Size = new System.Drawing.Size(792, 400);
            this.tpR.TabIndex = 1;
            this.tpR.Text = "Ресурсы";
            this.tpR.UseVisualStyleBackColor = true;
            // 
            // dgvR
            // 
            this.dgvR.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvR.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvR.Location = new System.Drawing.Point(3, 3);
            this.dgvR.Name = "dgvR";
            this.dgvR.Size = new System.Drawing.Size(786, 394);
            this.dgvR.TabIndex = 1;
            // 
            // tpB
            // 
            this.tpB.Controls.Add(this.dgvB);
            this.tpB.Location = new System.Drawing.Point(4, 22);
            this.tpB.Name = "tpB";
            this.tpB.Size = new System.Drawing.Size(792, 400);
            this.tpB.TabIndex = 2;
            this.tpB.Text = "Здания";
            this.tpB.UseVisualStyleBackColor = true;
            // 
            // dgvB
            // 
            this.dgvB.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvB.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvB.Location = new System.Drawing.Point(0, 0);
            this.dgvB.Name = "dgvB";
            this.dgvB.Size = new System.Drawing.Size(792, 400);
            this.dgvB.TabIndex = 1;
            // 
            // tpBRC
            // 
            this.tpBRC.Controls.Add(this.dgvBRC);
            this.tpBRC.Location = new System.Drawing.Point(4, 22);
            this.tpBRC.Name = "tpBRC";
            this.tpBRC.Size = new System.Drawing.Size(792, 400);
            this.tpBRC.TabIndex = 3;
            this.tpBRC.Text = "Потребление ресурсов зданиями";
            this.tpBRC.UseVisualStyleBackColor = true;
            // 
            // dgvBRC
            // 
            this.dgvBRC.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBRC.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvBRC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBRC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBRC.Location = new System.Drawing.Point(0, 0);
            this.dgvBRC.Name = "dgvBRC";
            this.dgvBRC.Size = new System.Drawing.Size(792, 400);
            this.dgvBRC.TabIndex = 1;
            // 
            // tpBRP
            // 
            this.tpBRP.Controls.Add(this.dgvBRP);
            this.tpBRP.Location = new System.Drawing.Point(4, 22);
            this.tpBRP.Name = "tpBRP";
            this.tpBRP.Size = new System.Drawing.Size(792, 400);
            this.tpBRP.TabIndex = 4;
            this.tpBRP.Text = "Производство зданиями ресурсов";
            this.tpBRP.UseVisualStyleBackColor = true;
            // 
            // dgvBRP
            // 
            this.dgvBRP.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBRP.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvBRP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBRP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBRP.Location = new System.Drawing.Point(0, 0);
            this.dgvBRP.Name = "dgvBRP";
            this.dgvBRP.Size = new System.Drawing.Size(792, 400);
            this.dgvBRP.TabIndex = 1;
            // 
            // tpO
            // 
            this.tpO.Controls.Add(this.dgvO);
            this.tpO.Location = new System.Drawing.Point(4, 22);
            this.tpO.Name = "tpO";
            this.tpO.Size = new System.Drawing.Size(792, 400);
            this.tpO.TabIndex = 5;
            this.tpO.Text = "Форпосты";
            this.tpO.UseVisualStyleBackColor = true;
            // 
            // dgvO
            // 
            this.dgvO.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvO.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvO.Location = new System.Drawing.Point(0, 0);
            this.dgvO.Name = "dgvO";
            this.dgvO.Size = new System.Drawing.Size(792, 400);
            this.dgvO.TabIndex = 0;
            // 
            // tpSR
            // 
            this.tpSR.Controls.Add(this.dgvSR);
            this.tpSR.Location = new System.Drawing.Point(4, 22);
            this.tpSR.Name = "tpSR";
            this.tpSR.Size = new System.Drawing.Size(792, 400);
            this.tpSR.TabIndex = 6;
            this.tpSR.Text = "Хранилище";
            this.tpSR.UseVisualStyleBackColor = true;
            // 
            // dgvSR
            // 
            this.dgvSR.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSR.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvSR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSR.Location = new System.Drawing.Point(0, 0);
            this.dgvSR.Name = "dgvSR";
            this.dgvSR.Size = new System.Drawing.Size(792, 400);
            this.dgvSR.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Управление ресурсами";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tbBR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBR)).EndInit();
            this.tpR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvR)).EndInit();
            this.tpB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvB)).EndInit();
            this.tpBRC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBRC)).EndInit();
            this.tpBRP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBRP)).EndInit();
            this.tpO.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvO)).EndInit();
            this.tpSR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem правкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem инструментыToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tbBR;
        private System.Windows.Forms.DataGridView dgvBR;
        private System.Windows.Forms.TabPage tpR;
        private System.Windows.Forms.TabPage tpB;
        private System.Windows.Forms.TabPage tpBRC;
        private System.Windows.Forms.TabPage tpBRP;
        private System.Windows.Forms.TabPage tpO;
        private System.Windows.Forms.DataGridView dgvO;
        private System.Windows.Forms.DataGridView dgvR;
        private System.Windows.Forms.DataGridView dgvB;
        private System.Windows.Forms.DataGridView dgvBRC;
        private System.Windows.Forms.DataGridView dgvBRP;
        private System.Windows.Forms.ToolStripMenuItem загрузитьЗановоToolStripMenuItem;
        private System.Windows.Forms.TabPage tpSR;
        private System.Windows.Forms.DataGridView dgvSR;
        private System.Windows.Forms.ToolStripMenuItem перезагрузитьВсеТаблицыToolStripMenuItem;
    }
}

