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
    public partial class fInventory : Form
    {
        SqlConnection conn = new SqlConnection();
        KetNoiSQL kn = new KetNoiSQL();

        public fInventory()
        {
            InitializeComponent();
            LoadData(dgTTLoaiHH, conn);  // Gọi phương thức LoadData khi form được tải
        }
        private void LoadData(DataGridView dgTonKho, SqlConnection conn)
        {
            try
            {
                // Thực hiện truy vấn để lấy dữ liệu từ bảng SanPham
                kn.Ketnoi(conn);
                string sql = "SELECT * FROM SanPham";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();

                // Điền dữ liệu vào DataTable
                da.Fill(dt);

                // Kiểm tra nếu DataTable có dữ liệu
                if (dt.Rows.Count > 0)
                {
                    // Gán DataTable cho DataGridView dgTonKho
                    dgTonKho.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu trong bảng SanPham!");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và hiển thị thông báo lỗi
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                // Đảm bảo kết nối được đóng sau khi thực thi
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Kiểm tra nếu ô tìm kiếm không trống
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                // Tạo câu truy vấn SQL để tìm kiếm sản phẩm theo mã hoặc tên sản phẩm
                string query = "SELECT * FROM SanPham WHERE SP_MA LIKE @search OR SP_TEN LIKE @search";

                try
                {
                    // Kết nối đến cơ sở dữ liệu
                    kn.Ketnoi(conn);
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);

                    // Thêm tham số vào câu truy vấn
                    da.SelectCommand.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Cập nhật DataGridView với kết quả tìm kiếm
                    if (dt.Rows.Count > 0)
                    {
                        dgTTLoaiHH.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm khớp với tìm kiếm!");
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi và hiển thị thông báo lỗi
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            else
            {
                // Nếu ô tìm kiếm trống, tải lại tất cả sản phẩm
                LoadData(dgTTLoaiHH, conn);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Gọi sự kiện TextChanged khi người dùng nhấn Enter
                txtSearch_TextChanged(sender, e);
            }
        }
    }
}
