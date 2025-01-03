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
using Microsoft.Reporting.WinForms;

namespace BTN_291
{
    public partial class BillReport : Form
    {
        private string maHD;

        public BillReport(string maHD)
        {
            InitializeComponent();
            this.maHD = maHD;
        }

        private void BillReport_Load(object sender, EventArgs e)
        {

            try
            {
                // Thiết lập file báo cáo
                reportViewer1.LocalReport.ReportEmbeddedResource = "BTN_291.PrintBill.rdlc";

                string connectionString = "Data Source=DESKTOP-T01GUO8\\MASTERMOS; Initial Catalog=QLCuaHangTienLoi;Integrated Security=True";

                // 1. Lấy dữ liệu Chi tiết hóa đơn
                string query1 = "SELECT * FROM ChiTietHD WHERE HD_MA = @HD_MA";
                DataTable cthd = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    cmd1.Parameters.AddWithValue("@HD_MA", maHD);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    da1.Fill(cthd);
                }

                // 2. Lấy dữ liệu sản phẩm liên quan
                string query2 = "SELECT * FROM SanPham JOIN ChiTietHD ON ChiTietHD.SP_MA = SanPham.SP_MA WHERE ChiTietHD.HD_MA = @HD_MA";
                DataTable sp = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("@HD_MA", maHD);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    da2.Fill(sp);
                }

                // 3. Lấy dữ liệu hóa đơn
                string query3 = "SELECT * FROM HoaDon WHERE HD_MA = @HD_MA";
                DataTable hd = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd3 = new SqlCommand(query3, conn);
                    cmd3.Parameters.AddWithValue("@HD_MA", maHD);
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    da3.Fill(hd);
                }

                // 4. Lấy thông tin nhân viên liên quan
                string query4 = "SELECT * FROM NhanVien JOIN HoaDon ON HoaDon.NV_MA = NhanVien.NV_MA WHERE HoaDon.HD_MA = @HD_MA";
                DataTable nv = new DataTable();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd4 = new SqlCommand(query4, conn);
                    cmd4.Parameters.AddWithValue("@HD_MA", maHD);
                    SqlDataAdapter da4 = new SqlDataAdapter(cmd4);
                    da4.Fill(nv);
                }

                // Tạo nguồn dữ liệu cho báo cáo
                ReportDataSource rp1 = new ReportDataSource("ChiTietHD", cthd);
                ReportDataSource rp2 = new ReportDataSource("SanPham", sp);
                ReportDataSource rp3 = new ReportDataSource("HoaDon", hd);
                ReportDataSource rp4 = new ReportDataSource("NhanVien", nv);

                // Thêm nguồn dữ liệu vào ReportViewer
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rp1);
                reportViewer1.LocalReport.DataSources.Add(rp2);
                reportViewer1.LocalReport.DataSources.Add(rp3);
                reportViewer1.LocalReport.DataSources.Add(rp4);

                // Làm mới báo cáo
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
