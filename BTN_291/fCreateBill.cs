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
    public partial class fCreateBill : Form
    {
        SqlConnection conn = new SqlConnection();
        KetNoiSQL kn = new KetNoiSQL();

        public fCreateBill(string maNV)
        {
            InitializeComponent();
            this.dgTTHoaDon.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgTTHoaDon_CellClick);

            this.maNV = maNV;
            string connectionString = "Data Source=DESKTOP-T01GUO8\\MASTERMOS; Initial Catalog=QLCuaHangTienLoi;Integrated Security=True";
            conn.ConnectionString = connectionString;
            LoadData(dgTTHoaDon, conn);
        }

        private string maNV;
        private string maKH;

        private void fCreateBill_Load(object sender, EventArgs e)
        {
            txtMaNV.Text = maNV;
            txtMaNV.Enabled = false;
        }

        private void LoadData(DataGridView dg, SqlConnection conn)
        {
            try
            {
                kn.Ketnoi(conn);

                string sql = "SELECT * FROM HoaDon";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgTTHoaDon.DataSource = dt;

                //conn.Close();
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            //}
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void TimKiemSDT()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string sql = "SELECT KH_MA, KH_HOTEN FROM KhachHang WHERE KH_SDT = @KH_SDT";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@KH_SDT", txtSearch.Text.Trim());

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtTenKH.Text = reader["KH_HOTEN"].ToString();
                    maKH = reader["KH_MA"].ToString();
                }
                else
                {
                    MessageBox.Show("Lỗi không tìm thấy khách hàng");
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không tìm thấy khách hàng: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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
                string sql_max_ms = "SELECT max(substring(HD_MA, 4, 6)) + 1 from HoaDon";
                SqlCommand comd = new SqlCommand(sql_max_ms, conn);
                SqlDataReader reader = comd.ExecuteReader();

                if (reader.Read())
                {
                    string value = reader[0]?.ToString();
                    int sohientai = string.IsNullOrEmpty(value) ? 1 : int.Parse(value);

                    if (sohientai <= 9)
                        txtMaHD.Text = "HD00" + sohientai.ToString();
                    else if (sohientai <= 99)
                        txtMaHD.Text = "HD0" + sohientai.ToString();
                    else if (sohientai <= 999)
                        txtMaHD.Text = "HD" + sohientai.ToString();
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
            txtMaHD.Enabled = false;
        }

        private void btnNewCus_Click(object sender, EventArgs e)
        {
            fCustomerManager f = new fCustomerManager();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnSaveBill_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaHD.Text) || string.IsNullOrEmpty(txtSearch.Text) ||
                    string.IsNullOrEmpty(txtTenKH.Text) || string.IsNullOrEmpty(txtMaNV.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                    return;
                }

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string sql = "INSERT INTO HoaDon (HD_MA, KH_MA, NV_MA, HD_NGAYGIOLAP) VALUES (@HD_MA, @KH_MA, @NV_MA, GETDATE())";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@HD_MA", txtMaHD.Text.Trim());
                cmd.Parameters.AddWithValue("@KH_MA", maKH);
                cmd.Parameters.AddWithValue("@NV_MA", txtMaNV.Text.Trim());

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Thêm thành công!");
                    LoadData(dgTTHoaDon, conn);  // Reload data to the DataGridView

                    // Sau khi thêm hóa đơn, chuyển đến form chi tiết hóa đơn
                    string maHD = txtMaHD.Text.Trim();
                    fSellerManager f = new fSellerManager(maHD);  // Truyền mã hóa đơn vào constructor
                    this.Hide();  // Ẩn form hiện tại
                    f.ShowDialog(); // Mở form chi tiết hóa đơn
                    this.Show();  // Hiển thị lại form hiện tại sau khi đóng form chi tiết
                }
                else
                {
                    MessageBox.Show("Thêm thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message);
            }
            finally
            {
                // Đảm bảo đóng kết nối sau khi xong
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        //nut in hoa don
        private void btnLoadBill_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaHD.Text))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để in!");
                return;
            }

            //truyen ma hoa don vao form report
            string maHD = txtMaHD.Text.Trim();
            BillReport billReport = new BillReport(maHD); 
            billReport.ShowDialog();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TimKiemSDT();
                e.SuppressKeyPress = true;
            }
        }

        //nut load hoa don
        private void btnLoadBill_Click_1(object sender, EventArgs e)
        {
            LoadData(dgTTHoaDon, conn);
        }

        private void dgTTHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
                DataGridViewRow row = dgTTHoaDon.Rows[e.RowIndex];
                txtMaHD.Text = row.Cells["HD_MA"].Value.ToString(); //lay ma hoa don
            }
        }
    }
}
