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
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Lab07_KT_NguyenVuThuyAnh_2180607272
{
    public partial class frmSinhVien : Form
    {
        public frmSinhVien()
        {
            InitializeComponent();
        }
        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            cmbLop.SelectedIndex = 0;
            dtNgaySinh.ValueChanged += new EventHandler(dtNgaySinh_ValueChanged);
            LoadData();

        }

        private int GetSelectedRow(string MaSV)
        {
            for (int i = 0; i < dgvSinhVien.Rows.Count; i++)
            {
                if (dgvSinhVien.Rows[i].Cells[0].Value?.ToString() == MaSV)
                {
                    return i;
                }
            }
            return -1;
        }
        private void InsertUpdate(int selectedRow)
        {
            dgvSinhVien.Rows[selectedRow].Cells[0].Value =txtMaSV.Text;
            dgvSinhVien.Rows[selectedRow].Cells[1].Value = txtHotenSV.Text;
            dgvSinhVien.Rows[selectedRow].Cells[2].Value = cmbLop.Text;
            dgvSinhVien.Rows[selectedRow].Cells[3].Value = dtNgaySinh.Value.ToString("dd/MM/yyyy");

        }

        

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra các trường thông tin
                if (string.IsNullOrEmpty(txtMaSV.Text) ||
                    string.IsNullOrEmpty(txtHotenSV.Text) ||
                    string.IsNullOrEmpty(cmbLop.Text) ||
                    dtNgaySinh.Value == null)
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin sinh viên (Mã SV, Họ tên, Lớp, Ngày sinh)!");
                }

                // Kiểm tra sinh viên đã tồn tại chưa
                int selectedRow = GetSelectedRow(txtMaSV.Text);
                if (selectedRow == -1)
                {
                    // Thêm hàng mới nếu chưa tồn tại
                    selectedRow = dgvSinhVien.Rows.Add();
                    InsertUpdate(selectedRow);
                    MessageBox.Show("Thêm mới thành công!", "THÔNG BÁO", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Sinh viên đã tồn tại! Vui lòng kiểm tra lại.", "THÔNG BÁO", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaSV.Text) ||
                    string.IsNullOrEmpty(txtHotenSV.Text) ||
                    string.IsNullOrEmpty(cmbLop.Text) ||
                    dtNgaySinh.Value == null)
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin sinh viên (Mã SV, Họ tên, Lớp, Ngày sinh)!");
                }

                int selectedRow = GetSelectedRow(txtMaSV.Text);
                if (selectedRow == -1)
                {
                    // Thêm hàng mới nếu sinh viên chưa tồn tại
                    selectedRow = dgvSinhVien.Rows.Add();
                    InsertUpdate(selectedRow);
                    MessageBox.Show("Thêm mới dữ liệu thành công!", "THÔNG BÁO", MessageBoxButtons.OK);
                }
                else
                {
                    // Cập nhật hàng nếu sinh viên đã tồn tại
                    InsertUpdate(selectedRow);
                    MessageBox.Show("Cập nhật dữ liệu thành công!", "THÔNG BÁO", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát hay không ?", "THÔNG BÁO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int selectRow = GetSelectedRow(txtMaSV.Text);
            if (selectRow == -1)
            {
                throw new Exception("ko thấy MSSV cần xóa");
            }
            else
            {
                DialogResult dr = MessageBox.Show("Bạn có muốn xóa ?", "YES/No", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    dgvSinhVien.Rows.RemoveAt(selectRow);
                    MessageBox.Show("Xóa sinh viên thành công !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra nếu không nhập mã sinh viên
                if (string.IsNullOrEmpty(txtMaSV.Text))
                {
                    throw new Exception("Vui lòng nhập mã sinh viên để tìm kiếm!");
                }

                // Tìm kiếm hàng có mã sinh viên trùng
                int selectedRow = GetSelectedRow(txtMaSV.Text);
                if (selectedRow == -1)
                {
                    // Nếu không tìm thấy
                    MessageBox.Show("Không tìm thấy sinh viên với mã: " + txtMaSV.Text, "KẾT QUẢ TÌM KIẾM",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // Nếu tìm thấy, hiển thị thông tin sinh viên
                    DataGridViewRow row = dgvSinhVien.Rows[selectedRow];
                    txtHotenSV.Text = row.Cells[1].Value.ToString(); // Họ tên
                    cmbLop.Text = row.Cells[2].Value.ToString();     // Lớp
                    dtNgaySinh.Value = DateTime.Parse(row.Cells[3].Value.ToString()); // Ngày sinh

                    MessageBox.Show("Tìm thấy sinh viên thành công!","THÔNG BÁO",MessageBoxButtons.OK,MessageBoxIcon.Information );
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message,"LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtNgaySinh_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dtNgaySinh.Value;

            // Kiểm tra nếu ngày sinh lớn hơn ngày hiện tại
            if (selectedDate > DateTime.Now)
            {
                MessageBox.Show(
                    "Ngày sinh không hợp lệ! Vui lòng chọn ngày sinh nhỏ hơn ngày hiện tại.",
                    "LỖI",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                dtNgaySinh.Value = DateTime.Now; // Reset về ngày hiện tại
            }
            else
            {
                // Xử lý nếu ngày sinh hợp lệ (ví dụ, lưu tạm thời hoặc hiển thị)
                MessageBox.Show(
                    "Bạn đã chọn ngày sinh: " + selectedDate.ToString("dd/MM/yyyy"),
                    "THÔNG BÁO",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra nếu thông tin chưa đầy đủ
                if (string.IsNullOrEmpty(txtMaSV.Text) ||
                    string.IsNullOrEmpty(txtHotenSV.Text) ||
                    string.IsNullOrEmpty(cmbLop.Text))
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin trước khi lưu!");
                }

                // Kiểm tra ngày sinh hợp lệ
                if (dtNgaySinh.Value > DateTime.Now)
                {
                    throw new Exception("Ngày sinh không hợp lệ! Vui lòng chọn ngày nhỏ hơn ngày hiện tại.");
                }

                // Kiểm tra xem sinh viên đã tồn tại chưa
                int selectedRow = GetSelectedRow(txtMaSV.Text);
                if (selectedRow == -1)
                {
                    // Thêm mới nếu chưa tồn tại
                    selectedRow = dgvSinhVien.Rows.Add();
                }

                // Gọi phương thức InsertUpdate để lưu thông tin
                InsertUpdate(selectedRow);
                MessageBox.Show("Lưu dữ liệu thành công!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Hiển thị lỗi nếu có
                MessageBox.Show(ex.Message, "LỖI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnKhong_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
        "Bạn có chắc chắn muốn hủy bỏ và không lưu thông tin đã nhập?",
        "XÁC NHẬN",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning
    );

            if (dr == DialogResult.Yes)
            {
                // Reset các trường dữ liệu về mặc định
                txtMaSV.Text = string.Empty;
                txtHotenSV.Text = string.Empty;
                cmbLop.SelectedIndex = -1; // Đặt về trạng thái chưa chọn
                dtNgaySinh.Value = DateTime.Now; // Đặt lại ngày mặc định

                MessageBox.Show("Thông tin đã được hủy bỏ!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void LoadData()
        {
            string connectionString = "Server=YOUR_SERVER_NAME;Database=YOUR_DATABASE_NAME;Trusted_Connection=True;";
            string query = "SELECT MaSV, HoTen, Lop, NgaySinh FROM SinhVien";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable); // Lấy dữ liệu vào DataTable

                    // Gắn DataTable vào DataGridView
                    dgvSinhVien.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi kết nối cơ sở dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
