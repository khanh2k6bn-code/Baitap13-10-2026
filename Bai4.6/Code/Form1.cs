using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace BaiTap4_6_Binding
{
    public partial class Form1 : Form
    {
        private BindingList<Student> listStudents;
        private BindingSource bindingSource;

        private DataGridView dataGridView1;
        private TextBox txtStudentId, txtFullName, txtAge;
        private ComboBox cmbGender;
        private PictureBox picAvatar;
        private Button btnChonAnh, btnAdd, btnDelete, btnUpdate, btnExportCSV;
        private ContextMenuStrip contextMenuStrip1;

        public Form1()
        {
            this.Text = "Quản lý Sinh viên - BindingSource & File (Bài 4.6)";
            this.Width = 900;
            this.Height = 560;

            InitControlsAndLayout();
            InitData();
            SetupDataBinding();
        }

        private void InitControlsAndLayout()
        {
            dataGridView1 = new DataGridView { Location = new System.Drawing.Point(12, 230), Width = 860, Height = 270, Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top };
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

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

            btnAdd = new Button { Text = "Thêm", Location = new System.Drawing.Point(420, 20), Width = 120 };
            btnDelete = new Button { Text = "Xóa", Location = new System.Drawing.Point(420, 60), Width = 120 };
            btnUpdate = new Button { Text = "Cập nhật", Location = new System.Drawing.Point(420, 100), Width = 120 };
            btnExportCSV = new Button { Text = "Xuất file CSV", Location = new System.Drawing.Point(420, 140), Width = 120 };

            this.Controls.AddRange(new Control[] {
                dataGridView1,
                lblId, lblName, lblAge, lblGender,
                txtStudentId, txtFullName, txtAge, cmbGender,
                picAvatar, btnChonAnh,
                btnAdd, btnDelete, btnUpdate, btnExportCSV
            });

            contextMenuStrip1 = new ContextMenuStrip();
            contextMenuStrip1.Items.Add("Xóa dòng này", null, (s, e) => btnDelete_Click(null, null));
            contextMenuStrip1.Items.Add("Làm mới dữ liệu", null, (s, e) => bindingSource.ResetBindings(false));
            dataGridView1.ContextMenuStrip = contextMenuStrip1;

            btnAdd.Click += btnAdd_Click;
            btnDelete.Click += btnDelete_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnChonAnh.Click += btnChonAnh_Click;
            btnExportCSV.Click += btnExportCSV_Click;
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
