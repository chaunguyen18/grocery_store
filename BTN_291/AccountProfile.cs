using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTN_291
{
    public partial class fAccountProfile : Form
    {
        SqlConnection conn = new SqlConnection();
        KetNoiSQL kn = new KetNoiSQL();

        public fAccountProfile()
        {
            InitializeComponent();
        }

        private string maNV;

        public fAccountProfile(string maNV) // Thêm tham số cho constructor
        {
            InitializeComponent();
            this.maNV = maNV; // Gán mã độc giả vào biến
        }

        private void fAccountProfile_Load(object sender, EventArgs e)
        {
            txtMaNV.Text = maNV;
            txtMaNV.Enabled = false; // Khóa ô txtMaNV để không cho người dùng sửa

            // Kết nối cơ sở dữ liệu và lấy thông tin nhân viên
            try
            {
                kn.Ketnoi(conn); // Kết nối tới CSDL

                // Câu lệnh SQL lấy thông tin nhân viên dựa vào maNV
                string query = "SELECT NV_HOTEN, NV_NGAYSINH, NV_GIOI, NV_SDT, NV_EMAIL, NV_AVATAR FROM NhanVien WHERE NV_MA = @maNV";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@maNV", maNV);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Gán giá trị vào các ô điều khiển tương ứng
                    txtHoTen.Text = reader["NV_HOTEN"].ToString();
                    dateNgaySinh.Value = DateTime.Parse(reader["NV_NGAYSINH"].ToString());
                    cbGioiTinh.SelectedItem = reader["NV_GIOI"].ToString();
                    txtSDT.Text = reader["NV_SDT"].ToString();
                    txtEmail.Text = reader["NV_EMAIL"].ToString();
                    string avatarPath = reader["NV_AVATAR"].ToString();
                    if (!string.IsNullOrEmpty(avatarPath) && File.Exists(avatarPath)) // Kiểm tra nếu đường dẫn hợp lệ và tồn tại
                    {
                        ptAnhNV.Image = new Bitmap(avatarPath); // Hiển thị ảnh trong PictureBox
                    }
                    else
                    {
                        ptAnhNV.Image = null; // Nếu không có ảnh, đặt về null hoặc hình mặc định
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi");
            }
            finally
            {
                conn.Close(); // Đảm bảo đóng kết nối sau khi truy vấn
            }
        }

        private string imagePath = string.Empty; // Biến toàn cục để lưu đường dẫn hình ảnh

        private void btnDown_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png"; // Định dạng file hình ảnh
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string sourcePath = dialog.FileName; // Lưu đường dẫn hình ảnh nguồn
                string fileName = Path.GetFileName(sourcePath); // Lấy tên file
                imagePath = Path.Combine(@"C:\Users\ASUS\source\repos\BTN_291\AnhNhanVien", fileName); // Tạo đường dẫn mới

                // Lưu hình ảnh vào đường dẫn mới
                File.Copy(sourcePath, imagePath, true); // true để ghi đè nếu file đã tồn tại
                ptAnhNV.Image = new Bitmap(imagePath); // Hiển thị hình ảnh trong PictureBox
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            kn.Ketnoi(conn);

            try
            {
                string sql = @"
                    UPDATE NhanVien
                    SET NV_HOTEN = @TenNV, NV_NGAYSINH = @NgaySinh, NV_GIOI = @GioiTinh, NV_SDT = @SDT, NV_EMAIL = @Email" +
                    (string.IsNullOrEmpty(imagePath) ? "" : ", NV_AVATAR = @Avatar") +
                    " WHERE NV_MA = @MaNV";


                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", txtMaNV.Text);
                cmd.Parameters.AddWithValue("@TenNV", txtHoTen.Text);
                cmd.Parameters.AddWithValue("@NgaySinh", dateNgaySinh.Value); // Sử dụng `Value` thay vì `Text`
                cmd.Parameters.AddWithValue("@SDT", txtSDT.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@GioiTinh", cbGioiTinh.SelectedItem.ToString()); // Sử dụng `SelectedItem`

                // Cập nhật đường dẫn hình ảnh nếu có
                if (!string.IsNullOrEmpty(imagePath))
                {
                    cmd.Parameters.AddWithValue("@Avatar", imagePath);
                }

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Sửa thành công!");
                }
                else
                {
                    MessageBox.Show("Sửa thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                conn.Close(); // Đóng kết nối
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
