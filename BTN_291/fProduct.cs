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
    public partial class fProduct : Form
    {
        KetNoiSQL kn = new KetNoiSQL();
        SqlConnection conn = new SqlConnection();

        public fProduct()
        {
            InitializeComponent();
            LoadHang();
        }

        private Image LoadAndResizeImage(string path, int width, int height)
        {
            try
            {
                using (var originalImage = Image.FromFile(path))
                {
                    var resizedImage = new Bitmap(width, height);
                    using (var graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.DrawImage(originalImage, 0, 0, width, height);
                    }
                    return resizedImage;
                }
            }
            catch
            {
                return null;
            }
        }

        private void GetDataAndDisplayImages(string loaiSPMa)
        {
            dgTatCaSP.Controls.Clear();

            // Mở kết nối
            kn.Ketnoi(conn);

            // Câu lệnh SQL để lấy dữ liệu sản phẩm theo LSP_MA
            string sql = "SELECT * FROM SanPham WHERE LSP_MA = @LSP_MA";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@LSP_MA", loaiSPMa);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                PictureBox pic = new PictureBox
                {
                    Width = 200,
                    Height = 200,
                    BackgroundImageLayout = ImageLayout.Stretch
                };

                Label tensp = new Label
                {
                    Text = reader["SP_TEN"].ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Bottom,
                    BackColor = Color.FromArgb(192, 192, 192),
                    ForeColor = Color.White
                };

                pic.Click += (sender, args) =>
                {
                    fProductDetails form = new fProductDetails(tensp.Text);
                    form.ShowDialog();
                };

                pic.Controls.Add(tensp);

                string imagePath = reader["SP_HINHANH"].ToString();
                if (File.Exists(imagePath))
                {
                    pic.Image = LoadAndResizeImage(imagePath, 200, 200);
                }
                else
                {
                    pic.Image = null;
                }

                dgTatCaSP.Controls.Add(pic);
            }

            reader.Close();
            conn.Close();
        }

        private void LoadHang()
        {
            try
            {
                kn.Ketnoi(conn);

                string sql = "SELECT LSP_MA, LSP_TEN FROM LoaiSP";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cbChonLoaiHH.DataSource = dt;
                cbChonLoaiHH.DisplayMember = "LSP_TEN";
                cbChonLoaiHH.ValueMember = "LSP_MA";

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải loại hàng hóa: " + ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cbChonLoaiHH.SelectedValue != null)
            {
                string loaiSPMa = cbChonLoaiHH.SelectedValue.ToString();
                GetDataAndDisplayImages(loaiSPMa);
            }
        }
    }
}
