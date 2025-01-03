namespace BTN_291
{
    partial class fManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fManager));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuHeThong = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHT_DoiMatKhau = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHT_TaiKhoan = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuQuanLy = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuQL_KhachHang = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuQL_LoaiSanPham = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuQL_SanPham = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHoaDon = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHD_ThemHD = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHD_LichSu = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTonKho = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuThongKe = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.lbLayTenNV = new System.Windows.Forms.Label();
            this.pDefaultScreen = new System.Windows.Forms.PictureBox();
            this.pLogo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pDefaultScreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuHeThong,
            this.MenuQuanLy,
            this.MenuHoaDon,
            this.MenuTonKho,
            this.MenuThongKe});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(2018, 81);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuHeThong
            // 
            this.MenuHeThong.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuHT_TaiKhoan,
            this.MenuHT_DoiMatKhau});
            this.MenuHeThong.Font = new System.Drawing.Font("Segoe UI Black", 19.875F, System.Drawing.FontStyle.Bold);
            this.MenuHeThong.ForeColor = System.Drawing.Color.DodgerBlue;
            this.MenuHeThong.Name = "MenuHeThong";
            this.MenuHeThong.Size = new System.Drawing.Size(294, 75);
            this.MenuHeThong.Text = "Hệ thống";
            // 
            // MenuHT_DoiMatKhau
            // 
            this.MenuHT_DoiMatKhau.BackColor = System.Drawing.Color.White;
            this.MenuHT_DoiMatKhau.ForeColor = System.Drawing.Color.Black;
            this.MenuHT_DoiMatKhau.Name = "MenuHT_DoiMatKhau";
            this.MenuHT_DoiMatKhau.Size = new System.Drawing.Size(528, 80);
            this.MenuHT_DoiMatKhau.Text = "Đổi mật khẩu";
            this.MenuHT_DoiMatKhau.Click += new System.EventHandler(this.MenuHT_DoiMatKhau_Click);
            // 
            // MenuHT_TaiKhoan
            // 
            this.MenuHT_TaiKhoan.BackColor = System.Drawing.Color.White;
            this.MenuHT_TaiKhoan.ForeColor = System.Drawing.Color.Black;
            this.MenuHT_TaiKhoan.Name = "MenuHT_TaiKhoan";
            this.MenuHT_TaiKhoan.Size = new System.Drawing.Size(528, 80);
            this.MenuHT_TaiKhoan.Text = "Tài khoản";
            this.MenuHT_TaiKhoan.Click += new System.EventHandler(this.MenuHT_TaiKhoan_Click);
            // 
            // MenuQuanLy
            // 
            this.MenuQuanLy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuQL_LoaiSanPham,
            this.MenuQL_SanPham,
            this.MenuQL_KhachHang});
            this.MenuQuanLy.Font = new System.Drawing.Font("Segoe UI Black", 19.875F, System.Drawing.FontStyle.Bold);
            this.MenuQuanLy.ForeColor = System.Drawing.Color.DodgerBlue;
            this.MenuQuanLy.Name = "MenuQuanLy";
            this.MenuQuanLy.Size = new System.Drawing.Size(248, 75);
            this.MenuQuanLy.Text = "Quản lý";
            // 
            // MenuQL_KhachHang
            // 
            this.MenuQL_KhachHang.BackColor = System.Drawing.Color.White;
            this.MenuQL_KhachHang.ForeColor = System.Drawing.Color.Black;
            this.MenuQL_KhachHang.Name = "MenuQL_KhachHang";
            this.MenuQL_KhachHang.Size = new System.Drawing.Size(559, 80);
            this.MenuQL_KhachHang.Text = "Khách hàng";
            this.MenuQL_KhachHang.Click += new System.EventHandler(this.MenuQL_KhachHang_Click);
            // 
            // MenuQL_LoaiSanPham
            // 
            this.MenuQL_LoaiSanPham.BackColor = System.Drawing.Color.White;
            this.MenuQL_LoaiSanPham.ForeColor = System.Drawing.Color.Black;
            this.MenuQL_LoaiSanPham.Name = "MenuQL_LoaiSanPham";
            this.MenuQL_LoaiSanPham.Size = new System.Drawing.Size(559, 80);
            this.MenuQL_LoaiSanPham.Text = "Loại Sản Phẩm";
            this.MenuQL_LoaiSanPham.Click += new System.EventHandler(this.MenuQL_LoaiHangHoa_Click);
            // 
            // MenuQL_SanPham
            // 
            this.MenuQL_SanPham.BackColor = System.Drawing.Color.White;
            this.MenuQL_SanPham.ForeColor = System.Drawing.Color.Black;
            this.MenuQL_SanPham.Name = "MenuQL_SanPham";
            this.MenuQL_SanPham.Size = new System.Drawing.Size(559, 80);
            this.MenuQL_SanPham.Text = "Sản Phẩm";
            this.MenuQL_SanPham.Click += new System.EventHandler(this.MenuQL_HangHoa_Click);
            // 
            // MenuHoaDon
            // 
            this.MenuHoaDon.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuHD_ThemHD,
            this.MenuHD_LichSu});
            this.MenuHoaDon.Font = new System.Drawing.Font("Segoe UI Black", 19.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuHoaDon.ForeColor = System.Drawing.Color.DodgerBlue;
            this.MenuHoaDon.Name = "MenuHoaDon";
            this.MenuHoaDon.Size = new System.Drawing.Size(279, 75);
            this.MenuHoaDon.Text = "Hoá Đơn";
            // 
            // MenuHD_ThemHD
            // 
            this.MenuHD_ThemHD.Name = "MenuHD_ThemHD";
            this.MenuHD_ThemHD.Size = new System.Drawing.Size(366, 80);
            this.MenuHD_ThemHD.Text = "Thêm";
            this.MenuHD_ThemHD.Click += new System.EventHandler(this.MenuHD_ThemHD_Click);
            // 
            // MenuHD_LichSu
            // 
            this.MenuHD_LichSu.Name = "MenuHD_LichSu";
            this.MenuHD_LichSu.Size = new System.Drawing.Size(366, 80);
            this.MenuHD_LichSu.Text = "Lịch Sử";
            this.MenuHD_LichSu.Click += new System.EventHandler(this.MenuHD_LichSu_Click);
            // 
            // MenuTonKho
            // 
            this.MenuTonKho.Font = new System.Drawing.Font("Segoe UI Black", 19.875F, System.Drawing.FontStyle.Bold);
            this.MenuTonKho.ForeColor = System.Drawing.Color.DodgerBlue;
            this.MenuTonKho.Name = "MenuTonKho";
            this.MenuTonKho.Size = new System.Drawing.Size(264, 75);
            this.MenuTonKho.Text = "Tồn kho";
            this.MenuTonKho.Click += new System.EventHandler(this.MenuTonKho_Click);
            // 
            // MenuThongKe
            // 
            this.MenuThongKe.BackColor = System.Drawing.Color.White;
            this.MenuThongKe.Font = new System.Drawing.Font("Segoe UI Black", 19.875F, System.Drawing.FontStyle.Bold);
            this.MenuThongKe.ForeColor = System.Drawing.Color.DodgerBlue;
            this.MenuThongKe.Name = "MenuThongKe";
            this.MenuThongKe.Size = new System.Drawing.Size(294, 75);
            this.MenuThongKe.Text = "Thống kê";
            this.MenuThongKe.Click += new System.EventHandler(this.MenuThongKe_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("ROG Fonts", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(1066, 333);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(923, 230);
            this.label3.TabIndex = 21;
            this.label3.Text = "STORE \r\nMANAGEMENT";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbLayTenNV
            // 
            this.lbLayTenNV.AutoSize = true;
            this.lbLayTenNV.BackColor = System.Drawing.Color.White;
            this.lbLayTenNV.Font = new System.Drawing.Font("Tahoma", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLayTenNV.ForeColor = System.Drawing.Color.SkyBlue;
            this.lbLayTenNV.Location = new System.Drawing.Point(1316, 98);
            this.lbLayTenNV.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLayTenNV.Name = "lbLayTenNV";
            this.lbLayTenNV.Size = new System.Drawing.Size(276, 58);
            this.lbLayTenNV.TabIndex = 27;
            this.lbLayTenNV.Text = "UserName";
            // 
            // pDefaultScreen
            // 
            this.pDefaultScreen.BackColor = System.Drawing.Color.White;
            this.pDefaultScreen.Location = new System.Drawing.Point(0, 83);
            this.pDefaultScreen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pDefaultScreen.Name = "pDefaultScreen";
            this.pDefaultScreen.Size = new System.Drawing.Size(2011, 942);
            this.pDefaultScreen.TabIndex = 1;
            this.pDefaultScreen.TabStop = false;
            // 
            // pLogo
            // 
            this.pLogo.BackColor = System.Drawing.Color.White;
            this.pLogo.BackgroundImage = global::BTN_291.Properties.Resources.Screenshot_2024_11_05_1207051;
            this.pLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pLogo.Location = new System.Drawing.Point(0, 86);
            this.pLogo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pLogo.Name = "pLogo";
            this.pLogo.Size = new System.Drawing.Size(1033, 939);
            this.pLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pLogo.TabIndex = 28;
            this.pLogo.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Ebrima", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label1.Location = new System.Drawing.Point(1049, 94);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 65);
            this.label1.TabIndex = 29;
            this.label1.Text = "Nhân viên:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label2.Location = new System.Drawing.Point(1319, 607);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(394, 84);
            this.label2.TabIndex = 30;
            this.label2.Text = "Vui lòng chọn\r\nchức năng cần quản lý";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Black;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(1956, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(55, 56);
            this.btnExit.TabIndex = 31;
            this.btnExit.Text = "x";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // fManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(2018, 1054);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pLogo);
            this.Controls.Add(this.lbLayTenNV);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pDefaultScreen);
            this.Controls.Add(this.menuStrip1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "fManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "App Quản lý Cửa hàng tiện lợi";
            this.Load += new System.EventHandler(this.fManager_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pDefaultScreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuHeThong;
        private System.Windows.Forms.ToolStripMenuItem MenuHT_TaiKhoan;
        private System.Windows.Forms.ToolStripMenuItem MenuHT_DoiMatKhau;
        private System.Windows.Forms.ToolStripMenuItem MenuQuanLy;
        private System.Windows.Forms.ToolStripMenuItem MenuQL_KhachHang;
        private System.Windows.Forms.ToolStripMenuItem MenuQL_LoaiSanPham;
        private System.Windows.Forms.ToolStripMenuItem MenuQL_SanPham;
        private System.Windows.Forms.ToolStripMenuItem MenuHoaDon;
        private System.Windows.Forms.ToolStripMenuItem MenuTonKho;
        private System.Windows.Forms.ToolStripMenuItem MenuThongKe;
        private System.Windows.Forms.PictureBox pDefaultScreen;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbLayTenNV;
        private System.Windows.Forms.PictureBox pLogo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ToolStripMenuItem MenuHD_ThemHD;
        private System.Windows.Forms.ToolStripMenuItem MenuHD_LichSu;
    }
}