using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTN_291
{
    public partial class fProductManager : Form
    {
        SqlConnection conn = new SqlConnection();
        KetNoiSQL kn = new KetNoiSQL();

        public fProductManager()
        {
            InitializeComponent();
        }
        private void fProductManager_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            kn.Ketnoi(conn); // Kết nối cơ sở dữ liệu
            HienThiLoaiSP(cbLoaiSP, conn);
            HienThiSanPham(dgTTSanPham, conn);
            dgTTSanPham.CellClick += dataSanPham_CellClick;
            txtMaSP.Focus();
        }

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
                        SELECT sp.SP_MA, sp.SP_TEN, lsp.LSP_TEN, sp.SP_TONKHO, sp.SP_DVT, sp.SP_Gia, sp.SP_HINHANH, lsp.LSP_MA
                        FROM SanPham sp, LoaiSP lsp
                        WHERE sp.LSP_MA = lsp.LSP_MA 
                        AND (sp.SP_TEN LIKE @TenSP OR sp.SP_MA LIKE @MaSP)
                        ORDER BY sp.SP_MA";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@TenSP", "%" + txtSearch.Text + "%");
                adapter.SelectCommand.Parameters.AddWithValue("@MaSP", "%" + txtSearch.Text + "%");

                DataTable table = new DataTable();
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    dgTTSanPham.DataSource = null;
                    dgTTSanPham.DataSource = table;

                    // Thiết lập tiêu đề cột và định dạng sau khi đã gán dữ liệu
                    dgTTSanPham.Columns["SP_MA"].HeaderText = "Mã SP";
                    dgTTSanPham.Columns["SP_TEN"].HeaderText = "Tên SP";
                    dgTTSanPham.Columns["SP_TONKHO"].HeaderText = "Tồn kho";
                    dgTTSanPham.Columns["SP_DVT"].HeaderText = "ĐVT";
                    dgTTSanPham.Columns["SP_Gia"].HeaderText = "Đơn giá";
                    dgTTSanPham.Columns["SP_Gia"].DefaultCellStyle.Format = "N0";
                    dgTTSanPham.Columns["SP_HINHANH"].HeaderText = "Ảnh SP";
                    dgTTSanPham.Columns["LSP_TEN"].HeaderText = "Tên Loại";

                    dgTTSanPham.Columns["LSP_MA"].Visible = false;


                    // Hiển thị nút Reset sau khi có kết quả tìm kiếm
                    btnReset.Visible = true;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy kết quả.");
                    dgTTSanPham.DataSource = null;
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            ResetDataGridView();
            txtSearch.Clear(); // Xóa nội dung tìm kiếm
        }

        private void ResetDataGridView()
        {
            SqlConnection conn = new SqlConnection();
            kn.Ketnoi(conn); // Kết nối cơ sở dữ liệu

            try
            {
                string sql = @"
                        SELECT sp.SP_MA, sp.SP_TEN, lsp.LSP_TEN, sp.SP_TONKHO, sp.SP_DVT, sp.SP_Gia, sp.SP_HINHANH, lsp.LSP_MA
                        FROM SanPham sp, LoaiSP lsp
                        WHERE sp.LSP_MA = lsp.LSP_MA 
                        ORDER BY sp.SP_MA";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dgTTSanPham.DataSource = table;

                // Thiết lập tiêu đề cột
                dgTTSanPham.Columns["SP_MA"].HeaderText = "Mã SP";
                dgTTSanPham.Columns["SP_TEN"].HeaderText = "Tên SP";
                dgTTSanPham.Columns["SP_TONKHO"].HeaderText = "Tồn kho";
                dgTTSanPham.Columns["SP_DVT"].HeaderText = "ĐVT";
                dgTTSanPham.Columns["SP_Gia"].HeaderText = "Đơn giá";
                dgTTSanPham.Columns["SP_Gia"].DefaultCellStyle.Format = "N0";
                dgTTSanPham.Columns["SP_HINHANH"].HeaderText = "Ảnh SP";
                dgTTSanPham.Columns["LSP_TEN"].HeaderText = "Tên Loại";
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAllProducts_Click(object sender, EventArgs e)
        {
            fProduct f = new fProduct();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        public void HienThiLoaiSP(System.Windows.Forms.ComboBox cb, SqlConnection conn)
        {
            string sql = "SELECT LSP_MA, LSP_TEN FROM LoaiSP";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataTable table = new DataTable();
            adapter.Fill(table);

            // Gán dữ liệu vào ComboBox
            cb.DataSource = table;
            cb.DisplayMember = "LSP_TEN";
            cb.ValueMember = "LSP_MA";
            cb.SelectedIndex = -1;
        }

        public void HienThiSanPham(DataGridView dg, SqlConnection conn)
        {
            string sql = @"
                            SELECT sp.SP_MA, sp.SP_TEN, lsp.LSP_TEN, sp.SP_TONKHO, sp.SP_DVT, sp.SP_Gia, sp.SP_HINHANH, lsp.LSP_MA
                            FROM SanPham sp, LoaiSP lsp
                            WHERE sp.LSP_MA = lsp.LSP_MA
                            ORDER BY SP_MA";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet table = new DataSet();
            adapter.Fill(table, "ThongTinSP");
            dg.DataSource = table;
            dg.DataMember = "ThongTinSP";

            dg.Columns["SP_MA"].HeaderText = "Mã SP";
            dg.Columns["SP_TEN"].HeaderText = "Tên SP";
            dg.Columns["SP_TONKHO"].HeaderText = "Tồn kho";
            dg.Columns["SP_DVT"].HeaderText = "ĐVT";
            dg.Columns["SP_Gia"].HeaderText = "Đơn giá";
            dg.Columns["SP_Gia"].DefaultCellStyle.Format = "N0";
            dg.Columns["SP_HINHANH"].HeaderText = "Ảnh SP";
            dg.Columns["LSP_TEN"].HeaderText = "Tên Loại";

            dg.Columns["LSP_MA"].Visible = false;
        }
       
        private string imagePath = string.Empty; // Biến toàn cục để lưu đường dẫn hình ảnh

        private void dataSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgTTSanPham.Rows[e.RowIndex];
                txtMaSP.Text = row.Cells["SP_MA"].Value?.ToString() ?? string.Empty;
                txtTenSP.Text = row.Cells["SP_TEN"].Value?.ToString() ?? string.Empty;
                txtSoLuong.Text = row.Cells["SP_TONKHO"].Value?.ToString() ?? string.Empty;
                txtDVT.Text = row.Cells["SP_DVT"].Value?.ToString() ?? string.Empty;
                txtGia.Text = row.Cells["SP_Gia"].Value?.ToString() ?? string.Empty;

                // Cập nhật đường dẫn hình ảnh
                string hinhAnh = row.Cells["SP_HINHANH"].Value as string; // Sử dụng as để gán giá trị có thể là null
                if (!string.IsNullOrEmpty(hinhAnh))
                {
                    imagePath = hinhAnh; // Lưu đường dẫn vào biến toàn cục
                    ptSanPham.Image = new Bitmap(hinhAnh); // Hiển thị hình ảnh trong PictureBox
                }
                else
                {
                    ptSanPham.Image = null; // Nếu không có hình ảnh, set null
                }

                cbLoaiSP.SelectedValue = row.Cells["LSP_MA"].Value?.ToString() ?? string.Empty; // Cũng sử dụng null-conditional
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png"; // Định dạng file hình ảnh
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string sourcePath = dialog.FileName; // Lưu đường dẫn hình ảnh nguồn
                string fileName = Path.GetFileName(sourcePath); // Lấy tên file
                imagePath = Path.Combine(@"C:\Users\ASUS\source\repos\BTN_291\AnhSanPham", fileName); // Tạo đường dẫn mới

                // Lưu hình ảnh vào đường dẫn mới
                File.Copy(sourcePath, imagePath, true); // true để ghi đè nếu file đã tồn tại
                ptSanPham.Image = new Bitmap(imagePath); // Hiển thị hình ảnh trong PictureBox
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-T01GUO8\\MASTERMOS; Initial Catalog=QLCuaHangTienLoi;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connectionString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            try
            {
                string sql_max_ms = "SELECT max(substring(SP_MA, 4, 6)) + 1 from SanPham";
                SqlCommand comd = new SqlCommand(sql_max_ms, conn);
                SqlDataReader reader = comd.ExecuteReader();

                if (reader.Read())
                {
                    string value = reader[0]?.ToString();
                    int sohientai = string.IsNullOrEmpty(value) ? 1 : int.Parse(value);

                    if (sohientai <= 9)
                        txtMaSP.Text = "SP00" + sohientai.ToString();
                    else if (sohientai <= 99)
                        txtMaSP.Text = "SP0" + sohientai.ToString();
                    else if (sohientai <= 999)
                        txtMaSP.Text = "SP" + sohientai.ToString();
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
            txtMaSP.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            kn.Ketnoi(conn);

            try
            {
                string sql = @"
                                INSERT INTO SanPham (SP_MA, SP_TEN, SP_TONKHO, SP_DVT, SP_Gia, SP_HINHANH, LSP_MA)
                                VALUES (@MaSP, @TenSP, @SoLuong, @DVT, @Gia, @HinhAnh, @MaLoaiSP)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaSP", txtMaSP.Text);
                cmd.Parameters.AddWithValue("@TenSP", txtTenSP.Text);
                cmd.Parameters.AddWithValue("@SoLuong", txtSoLuong.Text);
                cmd.Parameters.AddWithValue("@Gia", txtGia.Text);
                cmd.Parameters.AddWithValue("@DVT", txtDVT.Text);
                cmd.Parameters.AddWithValue("@HinhAnh", imagePath);
                cmd.Parameters.AddWithValue("@MaLoaiSP", cbLoaiSP.SelectedValue);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Thêm thành công!");
                    HienThiSanPham(dgTTSanPham, conn);
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

            try
            {
                string sql = @"
                                UPDATE SanPham
                                SET SP_TEN = @TenSP, SP_TONKHO = @SoLuong, SP_DVT = @DVT, SP_Gia = @Gia, LSP_MA = @MaLoaiSP, SP_HINHANH = @HinhAnh
                                WHERE SP_MA = @MaSP";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaSP", txtMaSP.Text);
                cmd.Parameters.AddWithValue("@TenSP", txtTenSP.Text);
                cmd.Parameters.AddWithValue("@SoLuong", txtSoLuong.Text);
                cmd.Parameters.AddWithValue("@Gia", txtGia.Text);
                cmd.Parameters.AddWithValue("@DVT", txtDVT.Text);
                cmd.Parameters.AddWithValue("@MaLoaiSP", cbLoaiSP.SelectedValue);

                // Cập nhật đường dẫn hình ảnh
                cmd.Parameters.AddWithValue("@HinhAnh", string.IsNullOrEmpty(imagePath) ? (object)DBNull.Value : imagePath);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Sửa thành công!");
                    HienThiSanPham(dgTTSanPham, conn); // Cập nhật lại danh sách
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
            txtMaSP.Text = string.Empty;
            txtTenSP.Text = string.Empty;
            txtSoLuong.Text = string.Empty;
            txtGia.Text = string.Empty;
            txtDVT.Text = string.Empty;
            cbLoaiSP.SelectedIndex = -1; // Đặt combobox về trạng thái chưa chọn
            ptSanPham.Image = null; // Xóa ảnh trong PictureBox

            // Đặt lại focus về ô họ tên
            txtMaSP.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            kn.Ketnoi(conn);

            if (string.IsNullOrWhiteSpace(txtMaSP.Text))
            {
                MessageBox.Show("Vui lòng chọn Sản phẩm để xóa!");
                return;
            }

            try
            {
                string sql = "DELETE FROM SanPham WHERE SP_MA = @MaSP";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaSP", txtMaSP.Text);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Xóa thành công!");
                    HienThiSanPham(dgTTSanPham, conn);
                    txtMaSP.Clear();
                    txtTenSP.Clear();
                    txtSoLuong.Clear();
                    txtDVT.Clear();
                    txtGia.Clear();
                    cbLoaiSP.SelectedIndex = -1;
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
