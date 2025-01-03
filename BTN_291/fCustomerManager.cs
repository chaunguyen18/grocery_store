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

namespace BTN_291
{
    public partial class fCustomerManager : Form
    {
        SqlConnection conn = new SqlConnection();
        KetNoiSQL kn = new KetNoiSQL();

        public fCustomerManager()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fCustomerManager_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            kn.Ketnoi(conn); // Kết nối cơ sở dữ liệu
            HienThiKhachHang(dgTTKhachHang, conn);
            dgTTKhachHang.CellClick += dataKhachHang_CellClick;
        }

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

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^0\d{9}$";

            return Regex.IsMatch(phoneNumber, pattern);
        }

        public void HienThiKhachHang(DataGridView dg, SqlConnection conn)
        {
            string sql = @"
                            SELECT KH_MA, KH_HOTEN, KH_NGAYSINH, KH_GIOI, KH_SDT
                            FROM KhachHang 
                            ORDER BY KH_MA";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet table = new DataSet();
            adapter.Fill(table, "ThongTinKH");
            dg.DataSource = table;
            dg.DataMember = "ThongTinKH";

            dg.Columns["KH_MA"].HeaderText = "Mã KH";
            dg.Columns["KH_HOTEN"].HeaderText = "Tên KH";
            dg.Columns["KH_NGAYSINH"].HeaderText = "Ngày Sinh";
            dg.Columns["KH_GIOI"].HeaderText = "Giới tính";
            dg.Columns["KH_SDT"].HeaderText = "Số ĐT";
        }

        private void dataKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgTTKhachHang.Rows[e.RowIndex];
                txtMaKH.Text = row.Cells["KH_MA"].Value?.ToString() ?? string.Empty;
                txtTenKH.Text = row.Cells["KH_HOTEN"].Value?.ToString() ?? string.Empty;
                dateNgaySinh.Value = row.Cells["KH_NGAYSINH"].Value is DateTime date ? date : DateTime.Now;
                cbGioiTinh.Text = row.Cells["KH_GIOI"].Value?.ToString() ?? string.Empty;
                txtSDT.Text = row.Cells["KH_SDT"].Value?.ToString() ?? string.Empty;
            }
        }

        private int count = 0; // Đặt biến count ở ngoài phương thức

        private void TimKiem()
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                MessageBox.Show("Vui lòng nhập thông tin tìm kiếm.");
                return;
            }

            SqlConnection conn = new SqlConnection();
            kn.Ketnoi(conn); // Kết nối cơ sở dữ liệu

            try
            {
                string sql = @"
                            SELECT KH_MA, KH_HOTEN, KH_NGAYSINH, KH_GIOI, KH_SDT
                            FROM KhachHang 
                            WHERE KH_HOTEN LIKE @TenKH OR KH_SDT LIKE @SDT
                            ORDER BY KH_MA";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@TenKH", "%" + txtSearch.Text + "%");
                adapter.SelectCommand.Parameters.AddWithValue("@SDT", "%" + txtSearch.Text + "%");

                DataTable table = new DataTable();
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    dgTTKhachHang.DataSource = null;
                    dgTTKhachHang.DataSource = table;

                    // Thiết lập tiêu đề cột và định dạng sau khi đã gán dữ liệu
                    dgTTKhachHang.Columns["KH_MA"].HeaderText = "Mã KH";
                    dgTTKhachHang.Columns["KH_HOTEN"].HeaderText = "Tên KH";
                    dgTTKhachHang.Columns["KH_NGAYSINH"].HeaderText = "Ngày Sinh";
                    dgTTKhachHang.Columns["KH_GIOI"].HeaderText = "Giới tính";
                    dgTTKhachHang.Columns["KH_SDT"].HeaderText = "Số ĐT";


                    // Hiển thị nút Reset sau khi có kết quả tìm kiếm
                    btnReset.Visible = true;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy kết quả.");
                    dgTTKhachHang.DataSource = null;
                    btnReset.Visible = false; // Ẩn nút Reset nếu không có kết quả
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                conn.Close(); // Đóng kết nối sau khi tìm kiếm
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TimKiem(); // Gọi phương thức tìm kiếm
                e.SuppressKeyPress = true; // Ngăn chặn âm thanh "ding" khi nhấn Enter
            }
        }

        private void ResetDataGridView()
        {
            SqlConnection conn = new SqlConnection();
            kn.Ketnoi(conn); // Kết nối cơ sở dữ liệu

            try
            {
                string sql = @"
                            SELECT KH_MA, KH_HOTEN, KH_NGAYSINH, KH_GIOI, KH_SDT
                            FROM KhachHang 
                            ORDER BY KH_MA";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dgTTKhachHang.DataSource = table;

                // Thiết lập tiêu đề cột
                dgTTKhachHang.Columns["KH_MA"].HeaderText = "Mã KH";
                dgTTKhachHang.Columns["KH_HOTEN"].HeaderText = "Tên KH";
                dgTTKhachHang.Columns["KH_NGAYSINH"].HeaderText = "Ngày Sinh";
                dgTTKhachHang.Columns["KH_GIOI"].HeaderText = "Giới tính";
                dgTTKhachHang.Columns["KH_SDT"].HeaderText = "Số ĐT";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                conn.Close(); // Đóng kết nối sau khi tải lại dữ liệu
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ResetDataGridView();
            txtSearch.Clear(); // Xóa nội dung tìm kiếm
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-TNSKCBGM\\SQLEXPRESS; Initial Catalog=QLCuaHangTienLoi;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            try
            {
                string sql_max_ms = "SELECT max(substring(KH_MA, 4, 6)) + 1 from KhachHang";
                SqlCommand comd = new SqlCommand(sql_max_ms, conn);
                SqlDataReader reader = comd.ExecuteReader();

                if (reader.Read())
                {
                    string value = reader[0]?.ToString();
                    int sohientai = string.IsNullOrEmpty(value) ? 1 : int.Parse(value);

                    if (sohientai <= 9)
                        txtMaKH.Text = "KH00" + sohientai.ToString();
                    else if (sohientai <= 99)
                        txtMaKH.Text = "KH0" + sohientai.ToString();
                    else if (sohientai <= 999)
                        txtMaKH.Text = "KH" + sohientai.ToString();
                    else
                        MessageBox.Show("Cần reset lại");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            txtMaKH.Enabled = false;
         }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            kn.Ketnoi(conn);

            // Kiểm tra định dạng số điện thoại
            if (!IsValidPhoneNumber(txtSDT.Text))
            {
                MessageBox.Show("Số điện thoại không đúng định dạng. Số điện thoại phải bắt đầu bằng 0 và có 10 chữ số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Dừng thực hiện nếu số điện thoại không hợp lệ
            }

            try
            {
                string sql = @"
                                INSERT INTO KhachHang (KH_MA, KH_HOTEN, KH_NGAYSINH, KH_GIOI, KH_SDT)
                                VALUES (@MaKH, @TenKH, @NgaySinh, @GioiTinh, @SDT)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaKH", txtMaKH.Text);
                cmd.Parameters.AddWithValue("@TenKH", txtTenKH.Text);
                cmd.Parameters.AddWithValue("@NgaySinh", dateNgaySinh.Value);
                cmd.Parameters.AddWithValue("@GioiTinh", cbGioiTinh.Text);
                cmd.Parameters.AddWithValue("@SDT", txtSDT.Text);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Thêm thành công!");
                    HienThiKhachHang(dgTTKhachHang, conn);
                }
                else
                {
                    MessageBox.Show("Thêm thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            kn.Ketnoi(conn);

            // Kiểm tra định dạng số điện thoại
            if (!IsValidPhoneNumber(txtSDT.Text))
            {
                MessageBox.Show("Số điện thoại không đúng định dạng. Số điện thoại phải bắt đầu bằng 0 và có 10 chữ số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Dừng thực hiện nếu số điện thoại không hợp lệ
            }

            try
            {
                string sql = @"
                                UPDATE KhachHang
                                SET KH_HOTEN = @TenKH, KH_NGAYSINH = @NgaySinh, KH_GIOI = @GioiTinh, KH_SDT = @SDT
                                WHERE KH_MA = @MaKH";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaKH", txtMaKH.Text);
                cmd.Parameters.AddWithValue("@TenKH", txtTenKH.Text);
                cmd.Parameters.AddWithValue("@NgaySinh", dateNgaySinh.Value);
                cmd.Parameters.AddWithValue("@GioiTinh", cbGioiTinh.Text);
                cmd.Parameters.AddWithValue("@SDT", txtSDT.Text);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Sửa thành công!");
                    HienThiKhachHang(dgTTKhachHang, conn);
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

        private void btnReset_Click(object sender, EventArgs e)
        {
            // Xóa nội dung các TextBox
            txtMaKH.Text = string.Empty;
            txtTenKH.Text = string.Empty;
            dateNgaySinh.Value = DateTime.Now;
            cbGioiTinh.Text = string.Empty;
            txtSDT.Text = string.Empty;

            // Đặt lại focus về ô họ tên
            txtMaKH.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            kn.Ketnoi(conn);

            if (string.IsNullOrWhiteSpace(txtMaKH.Text))
            {
                MessageBox.Show("Vui lòng chọn Khách Hàng để xóa!");
                return;
            }

            try
            {
                string sql = "DELETE FROM KhachHang WHERE KH_MA = @MaKH";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaKH", txtMaKH.Text);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Xóa thành công!");
                    HienThiKhachHang(dgTTKhachHang, conn);
                    txtMaKH.Clear();
                    txtTenKH.Clear();
                    dateNgaySinh.Value = DateTime.Now;
                    cbGioiTinh.SelectedIndex = -1;
                    txtSDT.Clear();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
