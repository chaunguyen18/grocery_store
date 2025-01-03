using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTN_291
{
    public partial class fProductTypeManager : Form
    {
        SqlConnection conn = new SqlConnection();
        KetNoiSQL kn = new KetNoiSQL();

        public fProductTypeManager()
        {
            InitializeComponent();
        }

        private void fProductTypeManager_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            kn.Ketnoi(conn); // Kết nối cơ sở dữ liệu
            HienThiLSP(dgTTLoaiSP, conn);
            dgTTLoaiSP.CellClick += dataLoaiSP_CellClick;

            //txtSearch.KeyDown += txtSearch_KeyDown;
        }

        public void HienThiLSP(DataGridView dg, SqlConnection conn)
        {
            string sql = @"
            SELECT LSP_MA, LSP_TEN
            FROM LoaiSP
            ORDER BY LSP_MA";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet table = new DataSet();
            adapter.Fill(table, "ThongTinLSP");
            dg.DataSource = table;
            dg.DataMember = "ThongTinLSP";

            dg.Columns["LSP_MA"].HeaderText = "Mã Loại";
            dg.Columns["LSP_TEN"].HeaderText = "Tên Loại";

            dgTTLoaiSP.Columns["LSP_MA"].Width = 100; 
            dgTTLoaiSP.Columns["LSP_TEN"].Width = 180; 
        }

        private void dataLoaiSP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgTTLoaiSP.Rows[e.RowIndex];
                txtMaLSP.Text = row.Cells["LSP_MA"].Value?.ToString() ?? string.Empty; // Sử dụng toán tử null-conditional
                txtTenLSP.Text = row.Cells["LSP_TEN"].Value?.ToString() ?? string.Empty;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-TNSKCBGM\\SQLEXPRESS; Initial Catalog=QLCuaHangTienLoi; Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql_max_ms = "SELECT max(substring(LSP_MA, 2, 4)) from LoaiSP";
                    SqlCommand comd = new SqlCommand(sql_max_ms, conn);
                    SqlDataReader reader = comd.ExecuteReader();

                    if (reader.Read())
                    {
                        string value = reader[0]?.ToString();
                        int sohientai = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);

                        // Tạo mã độc giả mới
                        sohientai++;  // Tăng mã độc giả lên 1
                        if (sohientai <= 9)
                            txtMaLSP.Text = "L00" + sohientai.ToString();
                        else if (sohientai <= 99)
                            txtMaLSP.Text = "L0" + sohientai.ToString();
                        else if (sohientai <= 999)
                            txtMaLSP.Text = "L" + sohientai.ToString();
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
            txtMaLSP.Enabled = false; // Khóa ô txtMaNV để không cho người dùng sửa
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            kn.Ketnoi(conn);

            try
            {
                string sql = @"
                                INSERT INTO LoaiSP (LSP_MA, LSP_TEN)
                                VALUES (@MaLoaiSP, @TenLoaiSP)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaLoaiSP", txtMaLSP.Text);
                cmd.Parameters.AddWithValue("@TenLoaiSP", txtTenLSP.Text);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Thêm thành công!");
                    HienThiLSP(dgTTLoaiSP, conn);
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
                                UPDATE LoaiSP SET LSP_TEN = @TenLSP WHERE LSP_MA = @MaLSP";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaLSP", txtMaLSP.Text);
                cmd.Parameters.AddWithValue("@TenLSP", txtTenLSP.Text);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Sửa thành công!");
                    HienThiLSP(dgTTLoaiSP, conn); // Cập nhật lại danh sách sách
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            kn.Ketnoi(conn);

            if (string.IsNullOrWhiteSpace(txtMaLSP.Text))
            {
                MessageBox.Show("Vui lòng chọn Loại SP để xóa!");
                return;
            }

            try
            {
                string sql = "DELETE FROM LoaiSP WHERE LSP_MA = @MaLSP";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaLSP", txtMaLSP.Text);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Xóa thành công!");
                    HienThiLSP(dgTTLoaiSP, conn);
                    txtMaLSP.Clear();
                    txtTenLSP.Clear();
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
