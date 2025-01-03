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
using Microsoft.VisualBasic;

namespace BTN_291
{
    public partial class fSellerManager : Form
    {
        SqlConnection conn = new SqlConnection();
        KetNoiSQL kn = new KetNoiSQL();
        private string HD_MA;

        public fSellerManager(string HD_MA)
        {
            InitializeComponent();
            this.HD_MA = HD_MA;
            MessageBox.Show("Mã hóa đơn: " + HD_MA);
            LoadData(dgTTHangHoa, conn);  // Tải dữ liệu sản phẩm vào dgTTHangHoa
            HienThiTenNhanVienTheoHoaDon(HD_MA);
            if (!string.IsNullOrEmpty(HD_MA))
            {
                txtMaHD.Text = HD_MA;
            }
        }


        private void LoadData(DataGridView dgTTHangHoa, SqlConnection conn)
        {
            try
            {
                // Kết nối cơ sở dữ liệu
                kn.Ketnoi(conn);

                // Truy vấn lấy tất cả sản phẩm
                string sql = "SELECT SP_MA, SP_TEN, SP_TONKHO, SP_GIA FROM SanPham";

                // Tạo đối tượng SqlDataAdapter để điền dữ liệu vào DataTable
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgTTHangHoa.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu sản phẩm: " + ex.Message);
            }
            finally
            {
                // Đảm bảo kết nối được đóng sau khi sử dụng
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show(
                "Thoát sẽ hủy hóa đơn này. Bạn có chắc chắn muốn thoát không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Kết nối cơ sở dữ liệu
                    kn.Ketnoi(conn);

                    // Lấy mã hóa đơn hiện tại (giả sử lưu trong một biến "hoaDonID")
                    string hoaDonID = txtMaHD.Text; // Hoặc bạn có thể truyền giá trị này từ nơi khác

                    // 1. Cập nhật lại số lượng sản phẩm từ ChiTietHD vào SanPham
                    string updateProductQuery = @"
                        UPDATE SanPham
SET SP_TonKho = SP_TonKho + cthd.CTHD_SoLuong
                        FROM SanPham sp
                        INNER JOIN ChiTietHD cthd ON sp.SP_MA = cthd.SP_MA
                        WHERE cthd.HD_MA = @hoaDonID";


                    SqlCommand updateProductCmd = new SqlCommand(updateProductQuery, conn);
                    updateProductCmd.Parameters.AddWithValue("@hoaDonID", hoaDonID);
                    updateProductCmd.ExecuteNonQuery();

                    // 2. Xóa chi tiết hóa đơn
                    string deleteChiTietHDQuery = "DELETE FROM ChiTietHD WHERE HD_MA = @hoaDonID";
                    SqlCommand deleteChiTietHDCmd = new SqlCommand(deleteChiTietHDQuery, conn);
                    deleteChiTietHDCmd.Parameters.AddWithValue("@hoaDonID", hoaDonID);
                    deleteChiTietHDCmd.ExecuteNonQuery();

                    // 3. Xóa hóa đơn
                    string deleteHoaDonQuery = "DELETE FROM HoaDon WHERE HD_MA = @hoaDonID";
                    SqlCommand deleteHoaDonCmd = new SqlCommand(deleteHoaDonQuery, conn);
                    deleteHoaDonCmd.Parameters.AddWithValue("@hoaDonID", hoaDonID);
                    deleteHoaDonCmd.ExecuteNonQuery();


                    // Hiển thị thông báo thành công
                    MessageBox.Show("Hóa đơn đã được hủy và số lượng sản phẩm đã được cập nhật.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Đóng form
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xử lý: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Đảm bảo kết nối được đóng
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            else
            {
                // Người dùng chọn "No", không làm gì cả
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dgTTHangHoa.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow selectedRow in dgTTHangHoa.SelectedRows)
                {
                    string SP_MA = selectedRow.Cells["SP_MA"].Value.ToString();
                    string tenSP = selectedRow.Cells["SP_TEN"].Value.ToString();
                    int stockQuantity = GetProductStockQuantity(SP_MA);
                    decimal price = GetProductPrice(SP_MA);

                    using (fNhapSoLuong frmNhapSoLuong = new fNhapSoLuong(stockQuantity))
                    {
                        if (frmNhapSoLuong.ShowDialog() == DialogResult.OK)
                        {
                            int quantity = frmNhapSoLuong.SoLuong;

                            if (quantity <= stockQuantity)
                            {
                                if (IsProductAlreadyInBill(SP_MA))
                                {
                                    UpdateProductQuantityInBill(HD_MA, SP_MA, quantity);
                                }
                                else
                                {
                                    // Thêm sản phẩm vào chi tiết hóa đơn
                                    AddProductToBill(SP_MA, quantity, price);
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Số lượng đặt hàng cho sản phẩm {tenSP} không thể lớn hơn số lượng tồn kho ({stockQuantity})!");
                            }
                        }
                    }
                }
                LoadChiTietHD(HD_MA); // Tải lại chi tiết hóa đơn

                // Gọi lại LoadData để làm mới dữ liệu sản phẩm
                LoadData(dgTTHangHoa, conn);  // Làm mới bảng sản phẩm
                CalculateTotalAmount();

            }
            else
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sản phẩm.");
            }
        }



        private void HienThiTenNhanVienTheoHoaDon(string HD_MA)
        {
            try
            {
                string query = @"
            SELECT NV_HOTEN 
            FROM HOADON HD 
            JOIN NHANVIEN NV ON HD.NV_MA = NV.NV_MA 
            WHERE HD.HD_MA = @HD_MA";

                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-T01GUO8\\MASTERMOS; Initial Catalog=QLCuaHangTienLoi;Integrated Security=True"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@HD_MA", HD_MA);
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            txtTenNV.Text = result.ToString(); // Hiển thị tên nhân viên trong txtTenNV
                        }
                        else
                        {
                            txtTenNV.Text = "Không tìm thấy nhân viên!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy tên nhân viên: " + ex.Message);
            }
        }

        private void CalculateTotalAmount()
        {
            try
            {
                decimal totalAmount = 0;

                // Kiểm tra mã hóa đơn có hợp lệ không
                if (string.IsNullOrEmpty(txtMaHD.Text.Trim()))
                {
                    MessageBox.Show("Mã hóa đơn không hợp lệ.");
                    return;
                }

                // Truy vấn chi tiết hóa đơn để tính tổng tiền
                string sql = "SELECT CT.CTHD_SOLUONG, CT.SP_GIA FROM ChiTietHD CT WHERE CT.HD_MA = @HD_MA";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@HD_MA", txtMaHD.Text.Trim());

                // Kiểm tra kết nối
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                SqlDataReader reader = cmd.ExecuteReader();

                // Tính tổng tiền
                while (reader.Read())
                {
                    // Kiểm tra giá trị CTHD_SOLUONG và SP_GIA có hợp lệ không trước khi chuyển đổi
                    int quantity = 0;
                    decimal price = 0;

                    if (reader["CTHD_SOLUONG"] != DBNull.Value)
                        quantity = Convert.ToInt32(reader["CTHD_SOLUONG"]);

                    if (reader["SP_GIA"] != DBNull.Value)
                        price = Convert.ToDecimal(reader["SP_GIA"]);

                    totalAmount += quantity * price; // Tính tổng tiền (SP_GIA * CTHD_SOLUONG)
                }
                reader.Close();

                // Định dạng và hiển thị tổng tiền vào TextBox txtMaNV
                txtTongTien.Text = totalAmount.ToString("#,0") + " VND"; // Định dạng số với dấu phân cách ngàn và thêm "VND"
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có
                MessageBox.Show("Lỗi khi tính tổng tiền: " + ex.Message);
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }





        private bool IsProductAlreadyInBill(string SP_MA)
        {
            bool exists = false;
            try
            {
                string connectionString = "Data Source=DESKTOP-T01GUO8\\MASTERMOS; Initial Catalog=QLCuaHangTienLoi; Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Truy vấn kiểm tra sản phẩm đã có trong chi tiết hóa đơn chưa
                    string sql = "SELECT COUNT(*) FROM ChiTietHD WHERE HD_MA = @HD_MA AND SP_MA = @SP_MA";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@HD_MA", txtMaHD.Text.Trim());
                    cmd.Parameters.AddWithValue("@SP_MA", SP_MA);

                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        exists = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kiểm tra sản phẩm trong hóa đơn: " + ex.Message);
            }

            return exists;
        }

        private void AddProductToBill(string SP_MA, int quantity, decimal price)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-T01GUO8\\MASTERMOS; Initial Catalog=QLCuaHangTienLoi; Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Thực hiện thêm sản phẩm vào chi tiết hóa đơn
                    string sqlInsert = "INSERT INTO ChiTietHD (HD_MA, SP_MA, CTHD_SOLUONG, SP_GIA) VALUES (@HD_MA, @SP_MA, @CTHD_SOLUONG, @SP_GIA)";
                    SqlCommand cmdInsert = new SqlCommand(sqlInsert, conn);
                    cmdInsert.Parameters.AddWithValue("@HD_MA", txtMaHD.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@SP_MA", SP_MA);
                    cmdInsert.Parameters.AddWithValue("@CTHD_SOLUONG", quantity);
                    cmdInsert.Parameters.AddWithValue("@SP_GIA", price);

                    int resultInsert = cmdInsert.ExecuteNonQuery();
                    if (resultInsert > 0)
                    {
                        // Sau khi thêm vào ChiTietHD, trừ số lượng sản phẩm trong bảng SanPham
                        UpdateProductStock(conn, SP_MA, quantity);
                        CalculateTotalAmount();

                        MessageBox.Show("Sản phẩm đã được thêm vào hóa đơn.");
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi thêm sản phẩm vào hóa đơn.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sản phẩm vào hóa đơn: " + ex.Message);
            }
        }

        private void UpdateProductStock(SqlConnection conn, string SP_MA, int quantity)
        {
            try
            {
                // Cập nhật số lượng tồn kho trong bảng SanPham
                string sqlUpdate = "UPDATE SanPham SET SP_TONKHO = SP_TONKHO - @CTHD_SOLUONG WHERE SP_MA = @SP_MA";
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn);
                cmdUpdate.Parameters.AddWithValue("@CTHD_SOLUONG", quantity);
                cmdUpdate.Parameters.AddWithValue("@SP_MA", SP_MA);

                int resultUpdate = cmdUpdate.ExecuteNonQuery();
                if (resultUpdate > 0)
                {
                    MessageBox.Show("Số lượng tồn kho đã được cập nhật.");
                }
                else
                {
                    MessageBox.Show("Lỗi khi cập nhật số lượng tồn kho.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật số lượng tồn kho: " + ex.Message);
            }
        }

        private void UpdateProductQuantityInBill(string HD_MA, string SP_MA, int quantity)
        {
            string connectionString = "Data Source=DESKTOP-T01GUO8\\MASTERMOS; Initial Catalog=QLCuaHangTienLoi; Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Kiểm tra xem sản phẩm đã tồn tại trong ChiTietHD hay chưa
                string checkQuery = "SELECT COUNT(*) FROM ChiTietHD WHERE HD_MA = @HD_MA AND SP_MA = @SP_MA";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@HD_MA", HD_MA);
                checkCmd.Parameters.AddWithValue("@SP_MA", SP_MA);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    // Cập nhật số lượng sản phẩm trong ChiTietHD nếu sản phẩm đã có
                    string updateQuery = "UPDATE ChiTietHD SET CTHD_SOLUONG = CTHD_SOLUONG + @CTHD_SOLUONG WHERE HD_MA = @HD_MA AND SP_MA = @SP_MA";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@CTHD_SOLUONG", quantity);
                    updateCmd.Parameters.AddWithValue("@HD_MA", HD_MA);
                    updateCmd.Parameters.AddWithValue("@SP_MA", SP_MA);
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    // Thêm sản phẩm vào ChiTietHD nếu sản phẩm chưa có
                    string insertQuery = "INSERT INTO ChiTietHD (HD_MA, SP_MA, CTHD_SOLUONG) VALUES (@HD_MA, @SP_MA, @CTHD_SOLUONG)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@HD_MA", HD_MA);
                    insertCmd.Parameters.AddWithValue("@SP_MA", SP_MA);
                    insertCmd.Parameters.AddWithValue("@CTHD_SOLUONG", quantity);
                    insertCmd.ExecuteNonQuery();
                }

                // Cập nhật số lượng tồn kho trong bảng SanPham
                string updateStockQuery = "UPDATE SanPham SET SP_TonKho = SP_TonKho - @Quantity WHERE SP_MA = @SP_MA";
                SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, conn);
                updateStockCmd.Parameters.AddWithValue("@Quantity", quantity);
                updateStockCmd.Parameters.AddWithValue("@SP_MA", SP_MA);
                updateStockCmd.ExecuteNonQuery();
            }
        }

        private void LoadChiTietHD(string HD_MA)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-T01GUO8\\MASTERMOS; Initial Catalog=QLCuaHangTienLoi; Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT HD_MA, SP_MA, CTHD_SoLuong, SP_Gia FROM ChiTietHD WHERE HD_MA = @HD_MA";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@HD_MA", HD_MA);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dgChiTietHD.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetProductStockQuantity(string SP_MA)
        {
            int stockQuantity = 0;
            try
            {
                // Kết nối cơ sở dữ liệu và lấy số lượng tồn kho của sản phẩm
                string connectionString = "Data Source=DESKTOP-T01GUO8\\MASTERMOS; Initial Catalog=QLCuaHangTienLoi; Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Truy vấn lấy số lượng tồn kho của sản phẩm
                    string sql = "SELECT SP_TONKHO FROM SanPham WHERE SP_MA = @SP_MA";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@SP_MA", SP_MA);

                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out stockQuantity))
                    {
                        return stockQuantity;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy số lượng tồn kho: " + ex.Message);
            }

            return stockQuantity; // Trả về 0 nếu có lỗi hoặc không tìm thấy sản phẩm
        }

        private decimal GetProductPrice(string SP_MA)
        {
            decimal price = 0;
            try
            {
                // Kết nối cơ sở dữ liệu và lấy giá của sản phẩm
                string connectionString = "Data Source=DESKTOP-T01GUO8\\MASTERMOS; Initial Catalog=QLCuaHangTienLoi; Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Truy vấn lấy giá của sản phẩm
                    string sql = "SELECT SP_GIA FROM SanPham WHERE SP_MA = @SP_MA";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@SP_MA", SP_MA);

                    object result = cmd.ExecuteScalar();
                    if (result != null && decimal.TryParse(result.ToString(), out price))
                    {
                        return price;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy giá sản phẩm: " + ex.Message);
            }

            return price;
        }

        private void dgTTHangHoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có phải là dòng hợp lệ không
            if (e.RowIndex >= 0)
            {
                // Lấy dòng hiện tại dựa trên chỉ số hàng
                DataGridViewRow selectedRow = dgTTHangHoa.Rows[e.RowIndex];

                // Lấy thông tin sản phẩm từ các cột (chỉnh lại tên cột theo bảng của bạn)
                string SP_MA = selectedRow.Cells["SP_MA"].Value.ToString();
                string tenSP = selectedRow.Cells["SP_TEN"].Value.ToString();
                int tonKho = int.Parse(selectedRow.Cells["SP_TONKHO"].Value.ToString());

                // Hiển thị thông tin sản phẩm lên MessageBox hoặc điều khiển khác
                MessageBox.Show($"Sản phẩm được chọn:\nMã SP: {SP_MA}\nTên SP: {tenSP}\nTồn kho: {tonKho}", "Thông tin sản phẩm");
            }
        }

        private void dgChiTietHD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtMaNV_TextChanged(object sender, EventArgs e)
        {

        }

        private void CalculateTotalAmount(string HD_MA)
        {
            string connectionString = "Data Source=DESKTOP-T01GUO8\\MASTERMOS; Initial Catalog=QLCuaHangTienLoi; Integrated Security=True";
            decimal totalAmount = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Truy vấn các chi tiết hóa đơn của mã hóa đơn hiện tại
                string query = @"
            SELECT CT.CTHD_SOLUONG, SP.SP_GIA
            FROM ChiTietHD CT
            INNER JOIN SanPham SP ON CT.SP_MA = SP.SP_MA
            WHERE CT.HD_MA = @HD_MA";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@HD_MA", HD_MA);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int quantity = reader.GetInt32(0);  // Số lượng sản phẩm
                        decimal price = reader.GetDecimal(1);  // Giá sản phẩm
                                                               // Cộng dồn tổng tiền
                        totalAmount += quantity * price;
                    }
                }
            }

            txtTongTien.Text = totalAmount.ToString("C");  // Định dạng tiền tệ
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra nếu không có dòng nào được chọn
                if (dgChiTietHD.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy thông tin từ dòng được chọn
                DataGridViewRow selectedRow = dgChiTietHD.SelectedRows[0];
                string HD_MA = selectedRow.Cells["HD_MA"].Value.ToString();
                string SP_MA = selectedRow.Cells["SP_MA"].Value.ToString();
                int soLuong = Convert.ToInt32(selectedRow.Cells["CTHD_SoLuong"].Value);

                // Xác nhận xóa
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa sản phẩm này khỏi chi tiết hóa đơn?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Chuỗi kết nối (cập nhật chuỗi này theo hệ thống của bạn)
                    string connectionString = "Data Source=DESKTOP-T01GUO8\\MASTERMOS;Initial Catalog=QLCuaHangTienLoi;Integrated Security=True;";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Bắt đầu transaction để đảm bảo tính toàn vẹn dữ liệu
                        SqlTransaction transaction = connection.BeginTransaction();

                        try
                        {
                            // Xóa sản phẩm khỏi bảng ChiTietHD
                            string deleteQuery = "DELETE FROM ChiTietHD WHERE HD_MA = @HD_MA AND SP_MA = @SP_MA";
                            using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection, transaction))
                            {
                                deleteCommand.Parameters.AddWithValue("@HD_MA", HD_MA);
                                deleteCommand.Parameters.AddWithValue("@SP_MA", SP_MA);
                                deleteCommand.ExecuteNonQuery();
                            }

                            // Cập nhật số lượng tồn kho trong bảng SanPham
                            string updateQuery = "UPDATE SanPham SET SP_TonKho = SP_TonKho + @SoLuong WHERE SP_MA = @SP_MA";
                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection, transaction))
                            {
                                updateCommand.Parameters.AddWithValue("@SoLuong", soLuong);
                                updateCommand.Parameters.AddWithValue("@SP_MA", SP_MA);
                                updateCommand.ExecuteNonQuery();
                            }

                            // Commit transaction
                            transaction.Commit();

                            LoadChiTietHD(HD_MA); // Tải lại chi tiết hóa đơn
                            LoadData(dgTTHangHoa, conn);  // Làm mới bảng sản phẩm
                            CalculateTotalAmount();


                            MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            // Rollback transaction nếu có lỗi
                            transaction.Rollback();
                            MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy giá trị tổng tiền từ txtTongTien
                string tongTienText = txtTongTien.Text;

                // Chuyển đổi sang số để lưu vào cơ sở dữ liệu
                decimal tongTien;
                if (decimal.TryParse(tongTienText.Replace(" VND", "").Replace(",", ""), out tongTien)) // Loại bỏ VND và dấu phẩy
                {
                    // Lấy mã hóa đơn (giả sử mã hóa đơn đã được lưu trong txtHD_MA)
                    string HD_MA = txtMaHD.Text;

                    // Kết nối đến cơ sở dữ liệu
                    string connectionString = "Data Source=DESKTOP-T01GUO8\\MASTERMOS;Initial Catalog=QLCuaHangTienLoi;Integrated Security=True;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Câu lệnh SQL để cập nhật tổng tiền vào cột HD_TONGTIEN
                        string updateQuery = "UPDATE HoaDon SET HD_TONGTIEN = @TongTien WHERE HD_MA = @HD_MA";
                        SqlCommand command = new SqlCommand(updateQuery, connection);
                        command.Parameters.AddWithValue("@TongTien", tongTien);
                        command.Parameters.AddWithValue("@HD_MA", HD_MA);
                        // Thực thi câu lệnh cập nhật
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Hiển thị thông báo thành công
                            MessageBox.Show("Thanh toán thành công. Tổng tiền đã được lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Đóng form sau khi lưu thành công
                            this.Close(); // Đóng form hiện tại
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy hóa đơn để cập nhật tổng tiền.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Giá trị tổng tiền không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }

}

