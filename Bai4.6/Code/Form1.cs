using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace QuanLySinhVien
{
    public partial class Form1 : Form
    {
        // Khai báo danh sách dữ liệu BindingList và cầu nối BindingSource
        private BindingList<Student> listStudents;
        private BindingSource bindingSource;

        public Form1()
        {
            InitializeComponent();

            // Gọi hàm khởi tạo dữ liệu mẫu và thiết lập liên kết
            InitData();
            SetupDataBinding();
        }

        private void InitData()
        {
            // Tạo sẵn một vài dữ liệu mẫu để kiểm tra giao diện
            listStudents = new BindingList<Student>
            {
                new Student { StudentId = "SV01", FullName = "Nguyễn Văn An", Age = 20, Gender = "Nam", AvatarPath = "" },
                new Student { StudentId = "SV02", FullName = "Trần Thị Bình", Age = 21, Gender = "Nữ", AvatarPath = "" }
            };

            bindingSource = new BindingSource();
            bindingSource.DataSource = listStudents;

            // Đưa BindingSource vào DataGridView để hiển thị danh sách[cite: 1]
            dataGridView1.DataSource = bindingSource;
        }

        private void SetupDataBinding()
        {
            // Liên kết 2 chiều giữa các ô nhập liệu (TextBox, ComboBox) với BindingSource[cite: 1]
            txtStudentId.DataBindings.Add("Text", bindingSource, "StudentId", true, DataSourceUpdateMode.OnPropertyChanged);
            txtFullName.DataBindings.Add("Text", bindingSource, "FullName", true, DataSourceUpdateMode.OnPropertyChanged);
            txtAge.DataBindings.Add("Text", bindingSource, "Age", true, DataSourceUpdateMode.OnPropertyChanged);
            cmbGender.DataBindings.Add("Text", bindingSource, "Gender", true, DataSourceUpdateMode.OnPropertyChanged);

            // Liên kết PictureBox để tự động load đường dẫn ảnh (AvatarPath) của sinh viên đang chọn[cite: 1]
            picAvatar.DataBindings.Add("ImageLocation", bindingSource, "AvatarPath", true);
        }

        // --- SỰ KIỆN NÚT THÊM ---
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int.TryParse(txtAge.Text, out int age);

            Student newStudent = new Student
            {
                StudentId = txtStudentId.Text,
                FullName = txtFullName.Text,
                Age = age,
                Gender = cmbGender.Text,
                AvatarPath = picAvatar.ImageLocation ?? ""
            };

            listStudents.Add(newStudent);
            bindingSource.ResetBindings(false);
        }

        // --- SỰ KIỆN NÚT XÓA ---
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (bindingSource.Current != null)
            {
                bindingSource.RemoveCurrent();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng sinh viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // --- SỰ KIỆN NÚT CẬP NHẬT ---
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bindingSource.EndEdit();
            dataGridView1.Refresh();
            MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --- SỰ KIỆN CHỌN ẢNH AVATAR (Dùng OpenFileDialog) ---
        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files (*.jpg; *.png; *.bmp)|*.jpg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Gán đường dẫn vào PictureBox để hiển thị ảnh ngay lập tức
                    picAvatar.ImageLocation = ofd.FileName;

                    // Cập nhật đường dẫn file ảnh vào đối tượng sinh viên hiện tại đang chọn
                    if (bindingSource.Current is Student currentStudent)
                    {
                        currentStudent.AvatarPath = ofd.FileName;
                    }
                }
            }
        }

        // --- SỰ KIỆN XUẤT FILE CSV (Dùng SaveFileDialog & Hỗ trợ tiếng Việt UTF-8) ---
        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV file (*.csv)|*.csv";
                sfd.FileName = "DanhSachSinhVien.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Sử dụng Encoding.UTF8 để lưu tiếng Việt có dấu chuẩn xác
                        using (StreamWriter sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                        {
                            // Ghi dòng tiêu đề cột
                            sw.WriteLine("StudentId,FullName,Age,Gender,AvatarPath");

                            // Duyệt qua danh sách sinh viên và ghi từng dòng vào file
                            foreach (var sv in listStudents)
                            {
                                sw.WriteLine($"{sv.StudentId},{sv.FullName},{sv.Age},{sv.Gender},{sv.AvatarPath}");
                            }
                        }
                        MessageBox.Show("Xuất file CSV thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {

        }

        private void btnChonAnh_Click_1(object sender, EventArgs e)
        {

        }

        private void btnExportCSV_Click_1(object sender, EventArgs e)
        {

        }
    }
}
