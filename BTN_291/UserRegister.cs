using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace BTN_291
{
    public partial class fRegister : Form
    {
        SqlConnection conn = new SqlConnection();
        KetNoiSQL kn = new KetNoiSQL();

        public fRegister()
        {
            InitializeComponent();
        }

        private void fRegister_Load(object sender, EventArgs e)
        {
            GenerateMaNV();
        }

        private void GenerateMaNV()
        {
            string connectionString = "Data Source=LAPTOP-TNSKCBGM\\SQLEXPRESS; Initial Catalog=QLCuaHangTienLoi; Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql_max_ms = "SELECT max(substring(NV_MA, 3, 5)) from NhanVien";
                    SqlCommand comd = new SqlCommand(sql_max_ms, conn);
                    SqlDataReader reader = comd.ExecuteReader();

                    if (reader.Read())
                    {
                        string value = reader[0]?.ToString();
                        int sohientai = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);

                        // Tạo mã độc giả mới
                        sohientai++;  // Tăng mã độc giả lên 1
                        if (sohientai <= 9)
                            txtMaNV.Text = "NV00" + sohientai.ToString();
                        else if (sohientai <= 99)
                            txtMaNV.Text = "NV0" + sohientai.ToString();
                        else if (sohientai <= 999)
                            txtMaNV.Text = "NV" + sohientai.ToString();
                        else
                            MessageBox.Show("Cần reset lại");
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
            txtMaNV.Enabled = false; // Khóa ô txtMaNV để không cho người dùng sửa
        }

        private int count = 0; // Đặt biến count ở ngoài phương thức

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // Nếu không phải là số hoặc ký tự điều khiển, chặn ký tự đó
                e.Handled = true;
            }
            else if (char.IsDigit(e.KeyChar) && count >= 10)
            {
                // Ngăn chặn nhập khi đã đạt tối đa (10)
                e.Handled = true;
            }
            else if (char.IsDigit(e.KeyChar))
            {
                count++; // Tăng số lượng ký tự đã nhập nếu là chữ số và chưa đạt tối đa
            }
            else if (char.IsControl(e.KeyChar) && e.KeyChar == (char)Keys.Back && count > 0)
            {
                // Kiểm tra xem người dùng có nhấn phím xóa không và count > 0
                count--; // Giảm giá trị của count khi người dùng xóa
            }
        }

        public static bool IsValidEmail(string email)
        {
            // Biểu thức chính quy để kiểm tra định dạng email
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Kiểm tra xem email có khớp với biểu thức chính quy không
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            // Biểu thức chính quy để kiểm tra định dạng số điện thoại
            string pattern = @"^0\d{9}$";

            // Kiểm tra xem số điện thoại có khớp với biểu thức chính quy không
            return Regex.IsMatch(phoneNumber, pattern);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Khai báo SqlConnection
            SqlConnection conn = new SqlConnection();
            // Gọi phương thức Ketnoi để mở kết nối
            kn.Ketnoi(conn);

            // Kiểm tra định dạng email
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không đúng định dạng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Dừng thực hiện nếu email không hợp lệ
            }

            // Kiểm tra định dạng số điện thoại
            if (!IsValidPhoneNumber(txtSDT.Text))
            {
                MessageBox.Show("Số điện thoại không đúng định dạng. Số điện thoại phải bắt đầu bằng 0 và có 10 chữ số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Dừng thực hiện nếu số điện thoại không hợp lệ
            }

            // Kiểm tra xem kết nối có thành công không
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("Không thể kết nối tới cơ sở dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Dừng thực hiện nếu không thể kết nối
            }
            try
            {
                // Bắt đầu một transaction để đảm bảo tính nhất quán của dữ liệu
                SqlTransaction transaction = conn.BeginTransaction();

                // Chèn vào bảng NhanVien
                string sqlNhanVien = @"
                            INSERT INTO NhanVien (NV_MA, NV_HOTEN, NV_NGAYSINH, NV_GIOI, NV_SDT, NV_EMAIL, NV_USERNAME, NV_PASSWORD)
                            VALUES (@MaNV, @TenNV, @NgaySinh, @GioiTinh, @SDT, @Email, @Username, @Pass)";

                SqlCommand cmdNhanVien = new SqlCommand(sqlNhanVien, conn, transaction);
                cmdNhanVien.Parameters.AddWithValue("@MaNV", txtMaNV.Text);
                cmdNhanVien.Parameters.AddWithValue("@TenNV", txtHoTen.Text);
                cmdNhanVien.Parameters.AddWithValue("@NgaySinh", dateNgaySinh.Value);
                cmdNhanVien.Parameters.AddWithValue("@GioiTinh", cbGioiTinh.SelectedItem.ToString());
                cmdNhanVien.Parameters.AddWithValue("@SDT", txtSDT.Text);
                cmdNhanVien.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmdNhanVien.Parameters.AddWithValue("@Username", txtUsername.Text);
                cmdNhanVien.Parameters.AddWithValue("@Pass", txtPass.Text);

                int resultNhanVien = cmdNhanVien.ExecuteNonQuery();

                // Kiểm tra câu lệnh INSERT
                if (resultNhanVien > 0)
                {
                    transaction.Commit(); // Xác nhận transaction
                    MessageBox.Show("Đăng ký thành công!");
                }
                else
                {
                    transaction.Rollback(); // Hoàn tác transaction
                    MessageBox.Show("Đăng ký thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                conn.Close(); // Đảm bảo luôn đóng kết nối
            }
        }

    }
}
