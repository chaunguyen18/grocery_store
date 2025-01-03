using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTN_291
{
    public partial class fNhapSoLuong : Form
    {
        public int SoLuong { get; private set; }

        public fNhapSoLuong(int tonKho)
        {
            InitializeComponent();
            lblThongBao.Text = $"Nhập số lượng (Tồn kho: {tonKho})";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtSoLuong.Text, out int soLuong) && soLuong > 0)
            {
                SoLuong = soLuong;
                DialogResult = DialogResult.OK;  // Đặt DialogResult để form tự đóng
                this.Close(); // Đóng form sau khi chọn số lượng
            }
            else
            {
                MessageBox.Show("Số lượng không hợp lệ. Vui lòng nhập số dương.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
