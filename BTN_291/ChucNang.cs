using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTN_291
{
    internal class ChucNang
    {
        // Kiểm tra tên đăng nhập
        public bool CheckUserName(string username, SqlConnection conn)
        {
            string sql = @"SELECT COUNT(*) 
                       FROM NhanVien 
                       WHERE NV_USERNAME = @username";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", username);

            int count = (int)cmd.ExecuteScalar();
            return count > 0;  // Trả về true nếu MaDG tồn tại
        }

        // Kiểm tra mật khẩu
        public bool CheckPassword(string username, string password, SqlConnection conn)
        {
            string sql = @"SELECT NV_PASSWORD 
                       FROM NhanVien 
                       WHERE NV_USERNAME = @username AND NV_PASSWORD = @password";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            SqlDataReader reader = cmd.ExecuteReader();
            bool result = reader.Read();
            reader.Close();
            return result;  // Trả về true nếu mật khẩu đúng
        }

        public string GetMaNV(string username, SqlConnection conn)
        {
            string maNV = null; // Sử dụng kiểu nullable
            string query = "SELECT NV_MA FROM NhanVien WHERE NV_USERNAME = @UserName";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserName", username);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        maNV = reader["NV_MA"]?.ToString(); // Dùng dấu hỏi để tránh null
                    }
                }
            }
            return maNV; // Có thể trả về null
        }

        public string GetTenNV(string maNV, SqlConnection conn)
        {
            string tenNV = null; // Sử dụng kiểu nullable
            string query = "SELECT NV_HOTEN FROM NhanVien WHERE NV_MA = @NV_MA";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@NV_MA", maNV);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tenNV = reader["NV_HOTEN"]?.ToString(); // Dùng dấu hỏi để tránh null
                    }
                }
            }
            return tenNV; // Có thể trả về null
        }
    }
}
