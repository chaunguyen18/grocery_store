using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTN_291
{
    internal class KetNoiSQL
    {
        public void Ketnoi(SqlConnection conn)
        {
            try
            {
                // Kiểm tra nếu kết nối đã mở thì không cần thực hiện lại
                if (conn.State == ConnectionState.Open)
                {
                    return;
                }

                // Thiết lập chuỗi kết nối
                string chuoiketnoi = "Server=DESKTOP-T01GUO8\\MASTERMOS; Database=QLCuaHangTienLoi; Integrated Security=True";
                conn.ConnectionString = chuoiketnoi;

                // Mở kết nối
                conn.Open();
            }
            catch (Exception ex)
            {
                // Log lỗi (nếu cần) và hiển thị thông báo
                MessageBox.Show("Kết nối thất bại: " + ex.Message);
            }
        }
    }
}
