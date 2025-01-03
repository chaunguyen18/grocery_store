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
    public partial class fBillDetailsCustomer : Form
    {
        SqlConnection conn = new SqlConnection();
        KetNoiSQL kn = new KetNoiSQL();

        public fBillDetailsCustomer()
        {
            InitializeComponent();
            LoadComboBox();
        }

        private void LoadComboBox()
        {
            try
            {
                kn.Ketnoi(conn); // Kết nối CSDL
                string query = "SELECT HD_MA FROM HoaDon"; // Lấy danh sách mã hóa đơn
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Gán dữ liệu vào ComboBox
                cbMaHD.DataSource = dt;
                cbMaHD.DisplayMember = "HD_MA";
                cbMaHD.ValueMember = "HD_MA";
            }
            finally
            {
                conn.Close(); // Đóng kết nối
            }
        }
        private void cbMaHD_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbMaHD.SelectedValue == null) return;

                string selectedMaHD = cbMaHD.SelectedValue.ToString(); // Lấy mã hóa đơn được chọn
                kn.Ketnoi(conn); // Kết nối CSDL

                // Truy vấn thông tin liên quan đến hóa đơn
                string query = @"
                    SELECT NV.NV_HOTEN, HD.HD_TONGTIEN, HD.HD_NGAYGIOLAP
                    FROM HoaDon HD
                    INNER JOIN NhanVien NV ON HD.NV_MA = NV.NV_MA
                    WHERE HD.HD_MA = @maHD";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@maHD", selectedMaHD);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Hiển thị dữ liệu vào các TextBox và DateTimePicker
                    txtMaNV.Text = reader["NV_HOTEN"].ToString(); // Tên nhân viên
                    txtTongTien.Text = reader["HD_TONGTIEN"].ToString(); // Tổng tiền
                    dateNgayLapHD.Value = Convert.ToDateTime(reader["HD_NGAYGIOLAP"]); // Ngày lập hóa đơn

                }

                reader.Close();
                LoadChiTietHD(selectedMaHD);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xử lý: " + ex.Message);
            }
            finally
            {
                conn.Close(); // Đóng kết nối
            }
        }
        private void LoadChiTietHD(string maHD)
        {
            try
            {
                kn.Ketnoi(conn); // Kết nối CSDL

                // Truy vấn chi tiết hóa đơn
                string query = @"
            SELECT 
                SP.SP_TEN AS [Tên Sản Phẩm],
                SP.SP_DVT AS [Đơn Vị Tính],
                CT.CTHD_SOLUONG AS [Số Lượng],
                CT.SP_GIA AS [Đơn Giá],
                (CT.CTHD_SOLUONG * CT.SP_GIA) AS [Thành Tiền]
            FROM ChiTietHD CT
            INNER JOIN SanPham SP ON CT.SP_MA = SP.SP_MA
            WHERE CT.HD_MA = @maHD";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@maHD", maHD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Gán dữ liệu vào DataGridView
                dgTTChiTietHD.DataSource = dt;

                // Định dạng cột "Đơn Giá" và "Thành Tiền"
                dgTTChiTietHD.Columns["Đơn Giá"].DefaultCellStyle.Format = "N0"; // Định dạng số nguyên có dấu phân cách
                dgTTChiTietHD.Columns["Thành Tiền"].DefaultCellStyle.Format = "N0"; // Định dạng số nguyên có dấu phân cách

                // Thêm hậu tố "VND" vào các cột
                dgTTChiTietHD.Columns["Đơn Giá"].DefaultCellStyle.FormatProvider =
                    new System.Globalization.CultureInfo("vi-VN");
                dgTTChiTietHD.Columns["Thành Tiền"].DefaultCellStyle.FormatProvider =
                    new System.Globalization.CultureInfo("vi-VN");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết hóa đơn: " + ex.Message);
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
