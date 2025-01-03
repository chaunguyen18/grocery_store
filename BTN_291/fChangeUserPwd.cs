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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BTN_291
{
    public partial class fChangeUserPwd : Form
    {
        SqlConnection conn = new SqlConnection();
        KetNoiSQL kn = new KetNoiSQL();

        private string maNV;

        public fChangeUserPwd(string maNV) // Thêm tham số cho constructor
        {
            InitializeComponent();
            this.maNV = maNV; 
        }

        private void fChangeUserPwd_Load(object sender, EventArgs e)
        {
            txtMaNV.Text = maNV;
            txtMaNV.Enabled = false;
            txtOldPass.Focus();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Kết nối cơ sở dữ liệu
                kn.Ketnoi(conn);

                // Bước 1: Lấy mật khẩu cũ từ cơ sở dữ liệu dựa trên maNV
                string query = "SELECT NV_PASSWORD FROM NhanVien WHERE NV_Ma = @maNV";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@maNV", maNV);

                string currentPassword = cmd.ExecuteScalar()?.ToString();

                if (currentPassword == null)
                {
                    MessageBox.Show("Không tìm thấy nhân viên với mã này.", "Lỗi");
                    return;
                }

                // Bước 2: Kiểm tra mật khẩu cũ có khớp với txtPassOld không
                if (txtOldPass.Text != currentPassword)
                {
                    MessageBox.Show("Mật khẩu cũ không chính xác.", "Lỗi");
                    return;
                }

                // Bước 3: Kiểm tra mật khẩu mới có khớp nhau không
                if (txtNewPass.Text != txtReNewPass.Text)
                {
                    MessageBox.Show("Mật khẩu mới không khớp nhau.", "Lỗi");
                    return;
                }

                // Bước 4: Cập nhật mật khẩu mới
                string updateQuery = "UPDATE NhanVien SET NV_PASSWORD = @newPassword WHERE NV_Ma = @maNV";
                SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@newPassword", txtNewPass.Text);
                updateCmd.Parameters.AddWithValue("@maNV", maNV);

                int rowsAffected = updateCmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Đổi mật khẩu thất bại.", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi");
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
