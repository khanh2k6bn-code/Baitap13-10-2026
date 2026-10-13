using System;
using System.Windows.Forms;

namespace BaiTap4_8_Async
{
    public partial class FormMain : Form
    {
        private MenuStrip menuStrip1;
        private ToolStripMenuItem mnuHeThong, mnuThoat;
        private ToolStripMenuItem mnuDanhMuc, mnuQuanLySinhVien;
        private ToolStrip toolStrip1;
        private ToolStripButton tsbQLSV;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblStatusUser, lblStatusProgress;

        public FormMain()
        {
            // Thiết lập MDI Parent
            this.IsMdiContainer = true;
            this.Text = "Hệ thống MDI - Bài 4.8 Async/Await";
            this.Width = 1050;
            this.Height = 720;

            InitMenuAndToolbar();
        }

        private void InitMenuAndToolbar()
        {
            // 1. MenuStrip
            menuStrip1 = new MenuStrip();
            mnuHeThong = new ToolStripMenuItem("Hệ thống");
            mnuThoat = new ToolStripMenuItem("Thoát");
            mnuHeThong.DropDownItems.Add(mnuThoat);

            mnuDanhMuc = new ToolStripMenuItem("Danh mục");
            mnuQuanLySinhVien = new ToolStripMenuItem("Quản lý Sinh viên");
            mnuDanhMuc.DropDownItems.Add(mnuQuanLySinhVien);

            menuStrip1.Items.AddRange(new ToolStripItem[] { mnuHeThong, mnuDanhMuc });
            this.MainMenuStrip = menuStrip1;

            // 2. ToolStrip
            toolStrip1 = new ToolStrip();
            tsbQLSV = new ToolStripButton("Quản lý Sinh viên");
            toolStrip1.Items.Add(tsbQLSV);

            // 3. StatusStrip (Hiển thị trạng thái tiến trình từ Form con theo yêu cầu)
            statusStrip1 = new StatusStrip();
            lblStatusUser = new ToolStripStatusLabel("Trạng thái: Sẵn sàng | ");
            lblStatusProgress = new ToolStripStatusLabel("Tiến trình nạp dữ liệu: 0%");
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatusUser, lblStatusProgress });

            this.Controls.Add(statusStrip1);
            this.Controls.Add(toolStrip1);
            this.Controls.Add(menuStrip1);

            // Sự kiện mở Form con
            mnuQuanLySinhVien.Click += (s, e) => OpenChildForm<FormStudent>();
            tsbQLSV.Click += (s, e) => OpenChildForm<FormStudent>();
            mnuThoat.Click += (s, e) => Application.Exit();
        }

        private void OpenChildForm<T>() where T : Form, new()
        {
            foreach (Form child in this.MdiChildren)
            {
                if (child is T)
                {
                    child.Activate();
                    return;
                }
            }
            T newForm = new T();
            newForm.MdiParent = this;
            newForm.Show();
        }

        // Phương thức công khai để Form con gọi cập nhật StatusStrip của Form cha[cite: 12]
        public void UpdateMainStatus(string text)
        {
            if (lblStatusProgress != null)
            {
                lblStatusProgress.Text = text;
            }
        }
    }
}
