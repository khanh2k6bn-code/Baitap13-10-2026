using System;
using System.Windows.Forms;

namespace BaiTap4_7_MDI
{
    public partial class FormMain : Form
    {
        private MenuStrip menuStrip1;
        private ToolStripMenuItem mnuHeThong, mnuDangXuat, mnuThoat;
        private ToolStripMenuItem mnuDanhMuc, mnuQuanLySinhVien;
        private ToolStripMenuItem mnuCuaSo, mnuCascade, mnuTileHorizontal, mnuTileVertical;

        private ToolStrip toolStrip1;
        private ToolStripButton tsbQLSV;

        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblStatusUser, lblStatusTime;

        public FormMain()
        {

            this.IsMdiContainer = true;
            this.Text = "Ứng dụng Quản lý Sinh viên - MDI Main";
            this.Width = 1000;
            this.Height = 700;

            InitMenuAndToolbars();
            SetupEvents();
            InitTimer();
        }

        private void InitMenuAndToolbars()
        {
            menuStrip1 = new MenuStrip();
            mnuHeThong = new ToolStripMenuItem("Hệ thống");
            mnuDangXuat = new ToolStripMenuItem("Đăng xuất");
            mnuThoat = new ToolStripMenuItem("Thoát");
            mnuHeThong.DropDownItems.AddRange(new ToolStripItem[] { mnuDangXuat, mnuThoat });

            mnuDanhMuc = new ToolStripMenuItem("Danh mục");
            mnuQuanLySinhVien = new ToolStripMenuItem("Quản lý Sinh viên");
            mnuDanhMuc.DropDownItems.Add(mnuQuanLySinhVien);

            mnuCuaSo = new ToolStripMenuItem("Cửa sổ (Window)");
            mnuCascade = new ToolStripMenuItem("Cascade");
            mnuTileHorizontal = new ToolStripMenuItem("Tile Horizontal");
            mnuTileVertical = new ToolStripMenuItem("Tile Vertical");
            mnuCuaSo.DropDownItems.AddRange(new ToolStripItem[] { mnuCascade, mnuTileHorizontal, mnuTileVertical });

            menuStrip1.Items.AddRange(new ToolStripItem[] { mnuHeThong, mnuDanhMuc, mnuCuaSo });
            this.MainMenuStrip = menuStrip1;

            toolStrip1 = new ToolStrip();
            tsbQLSV = new ToolStripButton("Quản lý Sinh viên");
            toolStrip1.Items.Add(tsbQLSV);

            statusStrip1 = new StatusStrip();
            lblStatusUser = new ToolStripStatusLabel("Đăng nhập: Nguyễn Thừa | ");
            lblStatusTime = new ToolStripStatusLabel();
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatusUser, lblStatusTime });

            this.Controls.Add(statusStrip1);
            this.Controls.Add(toolStrip1);
            this.Controls.Add(menuStrip1);
        }

        private void SetupEvents()
        {
            mnuQuanLySinhVien.Click += (s, e) => OpenChildForm<FormStudent>();
            tsbQLSV.Click += (s, e) => OpenChildForm<FormStudent>();

            mnuCascade.Click += (s, e) => this.LayoutMdi(MdiLayout.Cascade);
            mnuTileHorizontal.Click += (s, e) => this.LayoutMdi(MdiLayout.TileHorizontal);
            mnuTileVertical.Click += (s, e) => this.LayoutMdi(MdiLayout.TileVertical);

            mnuThoat.Click += (s, e) => Application.Exit();
            mnuDangXuat.Click += (s, e) => MessageBox.Show("Đã đăng xuất hệ thống!", "Thông báo");
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

        private void InitTimer()
        {
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += (s, e) => {
                lblStatusTime.Text = $"Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
            };
            timer.Start();
        }
    }
}
