using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTap4_8_Async
{
    public partial class FormStudent : Form
    {
        private BindingList<Student> listStudents;
        private BindingSource bindingSource;

        private DataGridView dataGridView1;
        private TextBox txtStudentId, txtFullName, txtAge;
        private ComboBox cmbGender;
        private PictureBox picAvatar;
        private Button btnChonAnh, btnAdd, btnDelete, btnUpdate, btnExportCSV, btnLoadAsync;
        private ProgressBar progressBar1;
        private Label lblPercent;
        private ContextMenuStrip contextMenuStrip1;

        public FormStudent()
        {
            this.Text = "Quản lý Sinh viên - Lập trình Bất đồng bộ";
            this.Width = 920;
            this.Height = 600;

            InitControlsAndLayout();
            InitData();
            SetupDataBinding();
        }

        private void InitControlsAndLayout()
        {
            dataGridView1 = new DataGridView { Location = new System.Drawing.Point(12, 260), Width = 880, Height = 260, Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top };
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Nhãn và ô nhập liệu
            Label lblId = new Label { Text = "Mã SV:", Location = new System.Drawing.Point(20, 23), Width = 70 };
            Label lblName = new Label { Text = "Họ tên:", Location = new System.Drawing.Point(20, 63), Width = 70 };
            Label lblAge = new Label { Text = "Tuổi:", Location = new System.Drawing.Point(20, 103), Width = 70 };
            Label lblGender = new Label { Text = "Giới tính:", Location = new System.Drawing.Point(20, 143), Width = 70 };

            txtStudentId = new TextBox { Location = new System.Drawing.Point(100, 20), Width = 160 };
            txtFullName = new TextBox { Location = new System.Drawing.Point(100, 60), Width = 160 };
            txtAge = new TextBox { Location = new System.Drawing.Point(100, 100), Width = 160 };

            cmbGender = new ComboBox { Location = new System.Drawing.Point(100, 140), Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbGender.Items.AddRange(new string[] { "Nam", "Nữ" });

            picAvatar = new PictureBox { Location = new System.Drawing.Point(280, 20), Width = 110, Height = 130, BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.StretchImage };
            btnChonAnh = new Button { Text = "Chọn ảnh", Location = new System.Drawing.Point(280, 160), Width = 110 };

            // Các nút chức năng chuẩn
            btnAdd = new Button { Text = "Thêm", Location = new System.Drawing.Point(420, 20), Width = 130 };
            btnDelete = new Button { Text = "Xóa", Location = new System.Drawing.Point(420, 60), Width = 130 };
            btnUpdate = new Button { Text = "Cập nhật", Location = new System.Drawing.Point(420, 100), Width = 130 };
            btnExportCSV = new Button { Text = "Xuất file CSV", Location = new System.Drawing.Point(420, 140), Width = 130 };

            // --- CÁC CONTROL CHO BÀI 4.8 (Async/Await & ProgressBar) ---
            btnLoadAsync = new Button { Text = "Nạp dữ liệu (Async)", Location = new System.Drawing.Point(420, 185), Width = 130, BackColor = System.Drawing.Color.LightSkyBlue };

            Label lblProg = new Label { Text = "Tiến trình:", Location = new System.Drawing.Point(20, 210), Width = 70 };
            progressBar1 = new ProgressBar { Location = new System.Drawing.Point(100, 208), Width = 230, Height = 23 };
            lblPercent = new Label { Text = "0%", Location = new System.Drawing.Point(345, 210), Width = 45, Font = new System.Drawing.Font("Microsoft Sans Serif", 9, System.Drawing.FontStyle.Bold) };

            this.Controls.AddRange(new Control[] {
                dataGridView1,
                lblId, lblName, lblAge, lblGender,
                txtStudentId, txtFullName, txtAge, cmbGender,
                picAvatar, btnChonAnh,
                btnAdd, btnDelete, btnUpdate, btnExportCSV,
                btnLoadAsync, lblProg, progressBar1, lblPercent
            });

            // Menu chuột phải cho DataGridView
            contextMenuStrip1 = new ContextMenuStrip();
            contextMenuStrip1.Items.Add("Xóa dòng này", null, (s, e) => btnDelete_Click(null, null));
            contextMenuStrip1.Items.Add("Làm mới dữ liệu", null, (s, e) => bindingSource.ResetBindings(false));
            dataGridView1.ContextMenuStrip = contextMenuStrip1;

            // Đăng ký sự kiện
            btnAdd.Click += btnAdd_Click;
            btnDelete.Click += btnDelete_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnChonAnh.Click += btnChonAnh_Click;
            btnExportCSV.Click += btnExportCSV_Click;
            btnLoadAsync.Click += btnLoadAsync_Click;
        }

        private void InitData()
        {
            listStudents = new BindingList<Student>
            {
                new Student { StudentId = "SV01", FullName = "Nguyễn Văn An", Age = 20, Gender = "Nam", AvatarPath = "" },
                new Student { StudentId = "SV02", FullName = "Trần Thị Bình", Age = 21, Gender = "Nữ", AvatarPath = "" }
            };

            bindingSource = new BindingSource();
            bindingSource.DataSource = listStudents;
            dataGridView1.DataSource = bindingSource;

            if (dataGridView1.Columns["StudentId"] != null)
            {
                dataGridView1.Columns["StudentId"].HeaderText = "Mã SV";
                dataGridView1.Columns["FullName"].HeaderText = "Họ và tên";
                dataGridView1.Columns["Age"].HeaderText = "Tuổi";
                dataGridView1.Columns["Gender"].HeaderText = "Giới tính";
                dataGridView1.Columns["AvatarPath"].HeaderText = "Đường dẫn ảnh";
            }
        }

        private void SetupDataBinding()
        {
            txtStudentId.DataBindings.Add("Text", bindingSource, "StudentId", true, DataSourceUpdateMode.OnPropertyChanged);
            txtFullName.DataBindings.Add("Text", bindingSource, "FullName", true, DataSourceUpdateMode.OnPropertyChanged);
            txtAge.DataBindings.Add("Text", bindingSource, "Age", true, DataSourceUpdateMode.OnPropertyChanged);
            cmbGender.DataBindings.Add("Text", bindingSource, "Gender", true, DataSourceUpdateMode.OnPropertyChanged);
            picAvatar.DataBindings.Add("ImageLocation", bindingSource, "AvatarPath", true);
        }

        // =========================================================================
        // YÊU CẦU BÀI 4.8: LẬP TRÌNH BẤT ĐỒNG BỘ (ASYNC/AWAIT) & PROGRESSBAR
        // =========================================================================
        private async void btnLoadAsync_Click(object sender, EventArgs e)
        {
            // 1. Quản lý trạng thái: Khóa nút bấm để tránh người dùng kích hoạt trùng lặp[cite: 12]
            btnLoadAsync.Enabled = false;
            progressBar1.Value = 0;
            lblPercent.Text = "0%";

            try
            {
                // 2. Sử dụng IProgress<int> (Progress<int>) để truyền và cập nhật tiến trình về UI Thread theo thời gian thực[cite: 12]
                var progress = new Progress<int>(percent =>
                {
                    progressBar1.Value = percent;
                    lblPercent.Text = $"{percent}%";

                    // Cập nhật đồng thời trạng thái phần trăm lên StatusStrip của Form cha MDI[cite: 12]
                    if (this.MdiParent is FormMain mainForm)
                    {
                        mainForm.UpdateMainStatus($"Tiến trình nạp dữ liệu: {percent}%");
                    }
                });

                // 3. Sử dụng Task.Run kết hợp await để giả lập tác vụ nạp dữ liệu nặng trong tiến trình ngầm[cite: 11]
                List<Student> loadedData = await Task.Run(() => LoadDataFromDatabaseSimulation(progress));

                // 4. Đổ dữ liệu nạp được vào BindingList
                foreach (var sv in loadedData)
                {
                    if (!listStudents.Any(s => s.StudentId == sv.StudentId))
                    {
                        listStudents.Add(sv);
                    }
                }
                bindingSource.ResetBindings(false);

                MessageBox.Show("Nạp dữ liệu bất đồng bộ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 5. Mở lại nút bấm (Enabled = true) ngay sau khi kết thúc hoặc gặp lỗi bằng khối try...catch...finally[cite: 12]
                btnLoadAsync.Enabled = true;
            }
        }

        // Phương thức giả lập tác vụ nạp dữ liệu lớn dạng async Task<List<Student>>[cite: 11]
        private List<Student> LoadDataFromDatabaseSimulation(IProgress<int> progress)
        {
            List<Student> tempList = new List<Student>();

            for (int i = 1; i <= 10; i++)
            {
                // Giả lập tác vụ nặng mất thời gian bằng Thread.Sleep trong tiến trình ngầm[cite: 11]
                Thread.Sleep(200);

                tempList.Add(new Student
                {
                    StudentId = $"SV0{i + 2}",
                    FullName = "Sinh Viên Async " + (i + 2),
                    Age = 19 + (i % 3),
                    Gender = (i % 2 == 0) ? "Nữ" : "Nam",
                    AvatarPath = ""
                });

                int percentComplete = i * 10;
                progress?.Report(percentComplete); // Báo cáo tiến trình về UI
            }

            return tempList;
        }

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (bindingSource.Current != null)
            {
                bindingSource.RemoveCurrent();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bindingSource.EndEdit();
            dataGridView1.Refresh();
            MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files (*.jpg; *.png; *.bmp)|*.jpg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    picAvatar.ImageLocation = ofd.FileName;
                    if (bindingSource.Current is Student currentStudent)
                    {
                        currentStudent.AvatarPath = ofd.FileName;
                    }
                }
            }
        }

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
                        using (StreamWriter sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                        {
                            sw.WriteLine("StudentId,FullName,Age,Gender,AvatarPath");
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
    }
}
