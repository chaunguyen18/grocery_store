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
    public partial class fLogOut : Form
    {
        public fLogOut()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            fLogin f = new fLogin();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }    
        private void btnRegister_Click(object sender, EventArgs e)
        {
            fRegister f = new fRegister();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }        

        private void btnExit_Click(object sender, EventArgs e)
        {
            // Xác nhận người dùng có muốn thoát không
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn đóng cửa sổ không?",
                                                 "Xác nhận", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                this.Close(); // Đóng cửa sổ hiện tại
            }
        }
    }
}
