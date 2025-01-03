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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BTN_291
{
    public partial class fLogin : Form
    {
        SqlConnection conn = new SqlConnection();
        KetNoiSQL kn = new KetNoiSQL();

        public fLogin()
        {
            InitializeComponent();
            userName = string.Empty; // Gán giá trị mặc định cho userName
        }
        private void fLogin_Load(object sender, EventArgs e)
        {
            kn.Ketnoi(conn);
            txtUserName.Focus(); 
        }

        private string userName; // Biến để lưu tên đăng nhập, có thể null

        private void linkDangKy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            fRegister f = new fRegister();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassWord.Text))
            {
                MessageBox.Show("Chưa nhập thông tin");
            }
            else
            {
                ChucNang k = new ChucNang();

                // Kiểm tra tên đăng nhập
                if (!k.CheckUserName(txtUserName.Text, conn))
                {
                    MessageBox.Show("Sai thông tin. Yêu cầu nhập lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!k.CheckPassword(txtUserName.Text, txtPassWord.Text, conn))
                {
                    MessageBox.Show("Sai thông tin. Yêu cầu nhập lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Đăng nhập thành công
                    userName = txtUserName.Text; // Lưu tên đăng nhập

                    // Lấy MaNV và TenNV
                    string maNV = k.GetMaNV(userName, conn); // Lấy MaNV từ username
                    if (!string.IsNullOrEmpty(maNV)) // Kiểm tra giá trị của maNV
                    {
                        string tenNV = k.GetTenNV(maNV, conn); // Lấy TenNV từ MaNV

                        // Chỉ hiển thị FormManager nếu tenNV không null
                        if (!string.IsNullOrEmpty(tenNV))
                        {
                            // Sau khi đăng nhập thành công và mở form fManager, không gọi fLogOut trừ khi cần
                            this.Hide();
                            fManager formManager = new fManager(tenNV, maNV);
                            formManager.ShowDialog(); // Đợi người dùng đóng form Manager trước khi hiển thị lại

                            this.Close(); // Đóng form fLogin sau khi fManager đóng
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thông tin nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
