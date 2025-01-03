using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTN_291
{
    public partial class fManager : Form
    {
        SqlConnection conn = new SqlConnection();
        KetNoiSQL kn = new KetNoiSQL();

        public fManager()
        {
            InitializeComponent();
        }

        private string userName;
        private string maNV;

        public fManager(string userName, string maNV) // Thêm tham số cho constructor
        {
            InitializeComponent();
            this.userName = userName; // Gán tên đăng nhập vào biến
            this.maNV = maNV; // Gán mã độc giả vào biến
        }

        private void fManager_Load(object sender, EventArgs e)
        {
            try
            {
                kn.Ketnoi(conn); // Kết nối cơ sở dữ liệu

                // Hiển thị lời chào
                lbLayTenNV.Text = userName;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }        
        
        private void MenuHT_TaiKhoan_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(maNV); // Truyền maNV vào fChangeUserPwd
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void MenuHT_DoiMatKhau_Click(object sender, EventArgs e)
        {
            fChangeUserPwd f = new fChangeUserPwd(maNV); // Truyền maNV vào fChangeUserPwd
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void MenuQL_HangHoa_Click(object sender, EventArgs e)
        {
            fProductManager f = new fProductManager();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void MenuQL_LoaiHangHoa_Click(object sender, EventArgs e)
        {
            fProductTypeManager f = new fProductTypeManager();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void MenuQL_KhachHang_Click(object sender, EventArgs e)
        {
            fCustomerManager f = new fCustomerManager();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void MenuTonKho_Click(object sender, EventArgs e)
        {
            fInventory f = new fInventory();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void MenuThongKe_Click(object sender, EventArgs e)
        {
            fRevenue f = new fRevenue();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void MenuHD_ThemHD_Click(object sender, EventArgs e)
        {
            fCreateBill f = new fCreateBill(maNV);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void MenuHD_LichSu_Click(object sender, EventArgs e)
        {
            fBillDetailsCustomer f = new fBillDetailsCustomer();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
    }
}
