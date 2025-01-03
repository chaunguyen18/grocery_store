namespace BTN_291
{
    partial class fRevenue
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fRevenue));
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnFilterYear = new Guna.UI2.WinForms.Guna2Button();
            this.btnFilterMonth = new Guna.UI2.WinForms.Guna2Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Location = new System.Drawing.Point(8, 8);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1399, 946);
            this.panel2.TabIndex = 14;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnFilterYear);
            this.panel1.Controls.Add(this.btnFilterMonth);
            this.panel1.Controls.Add(this.chart1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(70, 139);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1244, 735);
            this.panel1.TabIndex = 37;
            // 
            // btnFilterYear
            // 
            this.btnFilterYear.BorderRadius = 10;
            this.btnFilterYear.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnFilterYear.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnFilterYear.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnFilterYear.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnFilterYear.FillColor = System.Drawing.Color.DodgerBlue;
            this.btnFilterYear.Font = new System.Drawing.Font("Microsoft Tai Le", 13.875F, System.Drawing.FontStyle.Bold);
            this.btnFilterYear.ForeColor = System.Drawing.Color.White;
            this.btnFilterYear.Location = new System.Drawing.Point(642, 45);
            this.btnFilterYear.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnFilterYear.Name = "btnFilterYear";
            this.btnFilterYear.Size = new System.Drawing.Size(247, 76);
            this.btnFilterYear.TabIndex = 48;
            this.btnFilterYear.Text = "Lọc theo NĂM";
            this.btnFilterYear.Click += new System.EventHandler(this.btnFilterYear_Click);
            // 
            // btnFilterMonth
            // 
            this.btnFilterMonth.BorderRadius = 10;
            this.btnFilterMonth.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnFilterMonth.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnFilterMonth.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnFilterMonth.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnFilterMonth.FillColor = System.Drawing.Color.SkyBlue;
            this.btnFilterMonth.Font = new System.Drawing.Font("Microsoft Tai Le", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilterMonth.ForeColor = System.Drawing.Color.White;
            this.btnFilterMonth.Location = new System.Drawing.Point(270, 45);
            this.btnFilterMonth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnFilterMonth.Name = "btnFilterMonth";
            this.btnFilterMonth.Size = new System.Drawing.Size(308, 76);
            this.btnFilterMonth.TabIndex = 47;
            this.btnFilterMonth.Text = "Lọc theo THÁNG";
            this.btnFilterMonth.Click += new System.EventHandler(this.btnFilterMonth_Click);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(193, 150);
            this.chart1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(898, 564);
            this.chart1.TabIndex = 46;
            this.chart1.Text = "chart1";
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(14, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 28);
            this.label1.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("Segoe UI Black", 36F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label3.Location = new System.Drawing.Point(238, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(896, 96);
            this.label3.TabIndex = 36;
            this.label3.Text = "THỐNG KÊ DOANH THU";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Black;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(1356, -2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(40, 46);
            this.btnExit.TabIndex = 26;
            this.btnExit.Text = "x";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // fRevenue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1423, 885);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "fRevenue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Doanh thu";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button btnFilterMonth;
        private Guna.UI2.WinForms.Guna2Button btnFilterYear;
    }
}