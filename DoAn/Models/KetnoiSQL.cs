using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace Doan.Models
{
    public class Ketnoisql : IDisposable
    {
        private readonly SqlConnection sqlconn;
        private readonly string chuoiKetNoi = "Data Source=LAPTOP-GOC9P0UG;Initial Catalog=QLCHHoa;User ID=sa;Password=123;Encrypt=False;TrustServerCertificate=True;";

        // Constructor khởi tạo kết nối
        public Ketnoisql()
        {
            sqlconn = new SqlConnection(chuoiKetNoi);
        }

        // Mở kết nối SQL Server
        private void OpenSQL()
        {
            if (sqlconn.State == ConnectionState.Closed)
            {
                sqlconn.Open();
            }
        }

        // Đóng kết nối SQL Server
        private void CloseSQL()
        {
            if (sqlconn.State == ConnectionState.Open)
            {
                sqlconn.Close();
            }
        }

        // Kiểm tra kết nối
        public void TestConnection()
        {
            using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
            {
                try
                {
                    conn.Open();
                    MessageBox.Show("Kết nối thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Lấy dữ liệu dưới dạng DataTable (tốt nhất cho DataGridView)
        public DataTable GetDataTable(string query, Dictionary<string, object>? parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                OpenSQL();
                using (SqlCommand cmd = new SqlCommand(query, sqlconn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi truy vấn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseSQL();
            }
            return dt;
        }

        // Lấy dữ liệu dưới dạng List (không tối ưu cho DataGridView)
        public List<Dictionary<string, object>> ExecuteQuery(string query, Dictionary<string, object>? parameters = null)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            try
            {
                OpenSQL();
                using (SqlCommand cmd = new SqlCommand(query, sqlconn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);
                            }
                            result.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi truy vấn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseSQL();
            }
            return result;
        }

        // Thực thi truy vấn INSERT, UPDATE, DELETE
        public int ExecuteNonQuery(string query, Dictionary<string, object>? parameters = null)
        {
            int affectedRows = 0;
            try
            {
                OpenSQL();
                using (SqlCommand cmd = new SqlCommand(query, sqlconn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }
                    affectedRows = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thực thi truy vấn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseSQL();
            }
            return affectedRows;
        }

        public object ExecuteScalar(string query, Dictionary<string, object> parameters)
        {
            using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }
        // Giải phóng tài nguyên
        public void Dispose()
        {
            sqlconn.Dispose();
        }

        // 🟢 Phương thức làm mới dữ liệu
        public DataTable RefreshData(string query, Dictionary<string, object>? parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                OpenSQL();
                using (SqlCommand cmd = new SqlCommand(query, sqlconn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        // Làm mới dữ liệu và điền vào DataTable
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi truy vấn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseSQL();
            }
            return dt;
        }

    }
}
