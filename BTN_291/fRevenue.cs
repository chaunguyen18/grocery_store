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
using System.Windows.Forms.DataVisualization.Charting;

namespace BTN_291
{
    public partial class fRevenue : Form
    {
        public fRevenue()
        {
            InitializeComponent();
        }

        string connectionString = "Data Source=DESKTOP-T01GUO8\\MASTERMOS; Initial Catalog=QLCuaHangTienLoi;Integrated Security=True";

        public void LoadChart(Chart chart, string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                chart.Series.Clear();
                Series series = chart.Series.Add("Tổng Tiền");
                series.ChartType = SeriesChartType.Column; // Loại biểu đồ (cột)

                foreach (DataRow row in dt.Rows)
                {
                    string label = row["Nam"].ToString();
                    if (row.Table.Columns.Contains("Thang"))
                    {
                        label += "/" + row["Thang"].ToString();
                    }

                    series.Points.AddXY(label, row["TongTien"]);
                }
            }
        }

        private void fRevenue_Load(object sender, EventArgs e)
        {
            fRevenue thongKe = new fRevenue();
            string queryTheoThang = "SELECT YEAR(HD_NGAYGIOLAP) AS Nam, MONTH(HD_NGAYGIOLAP) AS Thang, SUM(HD_TONGTIEN) AS TongTien FROM HoaDon GROUP BY YEAR(HD_NGAYGIOLAP), MONTH(HD_NGAYGIOLAP) ORDER BY Nam, Thang";
            thongKe.LoadChart(chart1, queryTheoThang);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFilterMonth_Click(object sender, EventArgs e)
        {
            fRevenue thongKe = new fRevenue();
            string queryTheoThang = "SELECT YEAR(HD_NGAYGIOLAP) AS Nam, MONTH(HD_NGAYGIOLAP) AS Thang, SUM(HD_TONGTIEN) AS TongTien FROM HoaDon GROUP BY YEAR(HD_NGAYGIOLAP), MONTH(HD_NGAYGIOLAP) ORDER BY Nam, Thang";
            thongKe.LoadChart(chart1, queryTheoThang);
        }

        private void btnFilterYear_Click(object sender, EventArgs e)
        {
            fRevenue thongKe = new fRevenue();
            string queryTheoNam = "SELECT YEAR(HD_NGAYGIOLAP) AS Nam, SUM(HD_TONGTIEN) AS TongTien FROM HoaDon GROUP BY YEAR(HD_NGAYGIOLAP) ORDER BY Nam";
            thongKe.LoadChart(chart1, queryTheoNam);
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
