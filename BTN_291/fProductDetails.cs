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
using System.IO;

namespace BTN_291
{
    public partial class fProductDetails : Form
    {
        private string productName;
        KetNoiSQL kn = new KetNoiSQL();
        SqlConnection conn = new SqlConnection();

        public fProductDetails(string productName)
        {
            InitializeComponent();
            this.productName = productName;
            LoadProductDetails();
        }

        private void LoadProductDetails()
        {
            kn.Ketnoi(conn); // Mở kết nối
            string sql = "SELECT * FROM SanPham WHERE SP_TEN = @SP_TEN";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@SP_TEN", productName);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                // Gán giá trị cho các TextBox từ dữ liệu sản phẩm
                txtTenHH.Text = reader["SP_TEN"].ToString();
                txtDVT.Text = reader["SP_DVT"].ToString();
                txtSoLuong.Text = reader["SP_TONKHO"].ToString();
                txtDonGia.Text = reader["SP_GIA"].ToString();

                string lspMa = reader["LSP_MA"].ToString();

                string imagePath = reader["SP_HINHANH"].ToString();
                if (System.IO.File.Exists(imagePath))
                {
                    ptProduct.Image = Image.FromFile(imagePath); // Giả sử bạn đã tạo một PictureBox có tên picProductImage
                }
                else
                {
                    MessageBox.Show("Hình ảnh không tồn tại.");
                }

                reader.Close();

                LoadLoaiSPName(lspMa);
            }
            else
            {
                MessageBox.Show("Không tìm thấy sản phẩm.");
            }

            conn.Close(); // Đóng kết nối
        }

        private void LoadLoaiSPName(string lspMa)
        {
            string sqlLoaiSP = "SELECT LSP_TEN FROM LoaiSP WHERE LSP_MA = @LSP_MA";
            SqlCommand cmdLoaiSP = new SqlCommand(sqlLoaiSP, conn);
            cmdLoaiSP.Parameters.AddWithValue("@LSP_MA", lspMa);

            SqlDataReader readerLoaiSP = cmdLoaiSP.ExecuteReader();
            if (readerLoaiSP.Read())
            {
                txtTenLoaiHH.Text = readerLoaiSP["LSP_TEN"].ToString();
            }
            readerLoaiSP.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
