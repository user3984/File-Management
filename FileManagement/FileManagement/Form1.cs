using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Windows.Forms;

namespace FileManagement
{
    public class Form1 : Form
    {
        private Disk disk;
        private Directory dir;
        private Node current;

        public Form1()
        {
            InitializeComponent();
            this.disk = new Disk(1024, 4);
            this.dir = new Directory();
            if (File.Exists(Application.StartupPath + "\\dir.dat"))
            {
                dir.ReadDirectory();
            }
            else
            {
                dir = new Directory(new FCB("Root", 1, DateTime.Now.ToString(), -1, -1));
            }
            if (File.Exists(Application.StartupPath + "\\disk.dat"))
            {
                disk.ReadDisk();
            }
            if (File.Exists(Application.StartupPath + "\\fat.dat"))
            {
                disk.ReadFAT();
            }
            current = dir.root;
            pathBox.Text = "Root";
            totalSpaceLbl.Text = "磁盘容量:" + disk.size.ToString() + "字节";
            usedSpaceLbl.Text = "可用空间: " + (disk.blockSize * disk.blocksRemain).ToString() + "字节";
            progressBar1.Maximum = disk.blockNum;
            progressBar1.Value = disk.blockNum - disk.blocksRemain;
            DisplayDirView();
        }

        private void DisplayDirView()
        {
            Node n = this.current.firstChild;
            dirView.Rows.Clear();
            while (n != null)
            {
                int i = dirView.Rows.Add();
                dirView.Rows[i].Cells[0].Value = n.fcb.fileName;
                dirView.Rows[i].Cells[1].Value = n.fcb.time;
                dirView.Rows[i].Cells[2].Value = n.fcb.type == 0 ? "文本文档" : "文件夹";
                dirView.Rows[i].Cells[3].Value = n.fcb.type == 0 ? (n.fcb.size + "B") : "";
                n = n.nextSibling;
            }
            string path = "/" + current.fcb.fileName;
            n = current.parent;
            while (n != null)
            {
                path = "/" + n.fcb.fileName + path;
                n = n.parent;
            }
            pathBox.Text = path;
            totalSpaceLbl.Text = "磁盘容量:" + disk.size.ToString() + "字节";
            usedSpaceLbl.Text = "可用空间: " + (disk.blockSize * disk.blocksRemain).ToString() + "字节";
            progressBar1.Maximum = disk.blockNum;
            progressBar1.Value = disk.blockNum - disk.blocksRemain;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.contextMemuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripRename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.backBtn = new System.Windows.Forms.Button();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.totalSpaceLbl = new System.Windows.Forms.Label();
            this.usedSpaceLbl = new System.Windows.Forms.Label();
            this.formatDiskBtn = new System.Windows.Forms.Button();
            this.createFileBtn = new System.Windows.Forms.Button();
            this.createFolderBtn = new System.Windows.Forms.Button();
            this.dirView = new System.Windows.Forms.DataGridView();
            this.fileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.contextMemuStrip1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dirView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ContextMenuStrip = this.contextMemuStrip1;
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(782, 553);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint_1);
            // 
            // contextMemuStrip1
            // 
            this.contextMemuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMemuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripRename,
            this.toolStripDelete});
            this.contextMemuStrip1.Name = "contextMemuStrip1";
            this.contextMemuStrip1.Size = new System.Drawing.Size(124, 52);
            this.contextMemuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMemuStrip1_Opening);
            this.contextMemuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMemuStrip1_ItemClicked);
            // 
            // toolStripRename
            // 
            this.toolStripRename.Name = "toolStripRename";
            this.toolStripRename.Size = new System.Drawing.Size(123, 24);
            this.toolStripRename.Text = "重命名";
            // 
            // toolStripDelete
            // 
            this.toolStripDelete.Name = "toolStripDelete";
            this.toolStripDelete.Size = new System.Drawing.Size(123, 24);
            this.toolStripDelete.Text = "删除";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.backBtn, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.pathBox, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(776, 34);
            this.tableLayoutPanel2.TabIndex = 0;
            this.tableLayoutPanel2.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel2_Paint);
            // 
            // backBtn
            // 
            this.backBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backBtn.Location = new System.Drawing.Point(3, 3);
            this.backBtn.Name = "backBtn";
            this.backBtn.Size = new System.Drawing.Size(114, 28);
            this.backBtn.TabIndex = 0;
            this.backBtn.Text = "返回上级目录";
            this.backBtn.UseVisualStyleBackColor = true;
            this.backBtn.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // pathBox
            // 
            this.pathBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pathBox.Location = new System.Drawing.Point(123, 3);
            this.pathBox.Name = "pathBox";
            this.pathBox.ReadOnly = true;
            this.pathBox.Size = new System.Drawing.Size(650, 27);
            this.pathBox.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.dirView, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 43);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(776, 507);
            this.tableLayoutPanel3.TabIndex = 1;
            this.tableLayoutPanel3.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel3_Paint);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.totalSpaceLbl);
            this.panel1.Controls.Add(this.usedSpaceLbl);
            this.panel1.Controls.Add(this.formatDiskBtn);
            this.panel1.Controls.Add(this.createFileBtn);
            this.panel1.Controls.Add(this.createFolderBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(579, 3);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(194, 501);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(5, 427);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(184, 29);
            this.progressBar1.TabIndex = 5;
            // 
            // totalSpaceLbl
            // 
            this.totalSpaceLbl.AutoSize = true;
            this.totalSpaceLbl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.totalSpaceLbl.Location = new System.Drawing.Point(5, 456);
            this.totalSpaceLbl.Name = "totalSpaceLbl";
            this.totalSpaceLbl.Size = new System.Drawing.Size(84, 20);
            this.totalSpaceLbl.TabIndex = 4;
            this.totalSpaceLbl.Text = "磁盘容量：";
            this.totalSpaceLbl.Click += new System.EventHandler(this.totalSpaceLbl_Click);
            // 
            // usedSpaceLbl
            // 
            this.usedSpaceLbl.AutoSize = true;
            this.usedSpaceLbl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.usedSpaceLbl.Location = new System.Drawing.Point(5, 476);
            this.usedSpaceLbl.Name = "usedSpaceLbl";
            this.usedSpaceLbl.Size = new System.Drawing.Size(84, 20);
            this.usedSpaceLbl.TabIndex = 3;
            this.usedSpaceLbl.Text = "可用空间：";
            this.usedSpaceLbl.Click += new System.EventHandler(this.label1_Click);
            // 
            // formatDiskBtn
            // 
            this.formatDiskBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.formatDiskBtn.Location = new System.Drawing.Point(5, 85);
            this.formatDiskBtn.Name = "formatDiskBtn";
            this.formatDiskBtn.Size = new System.Drawing.Size(184, 40);
            this.formatDiskBtn.TabIndex = 2;
            this.formatDiskBtn.Text = "格式化磁盘";
            this.formatDiskBtn.UseVisualStyleBackColor = true;
            this.formatDiskBtn.Click += new System.EventHandler(this.formatDiskBtn_Click);
            // 
            // createFileBtn
            // 
            this.createFileBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.createFileBtn.Location = new System.Drawing.Point(5, 45);
            this.createFileBtn.Name = "createFileBtn";
            this.createFileBtn.Size = new System.Drawing.Size(184, 40);
            this.createFileBtn.TabIndex = 1;
            this.createFileBtn.Text = "新建文本文档";
            this.createFileBtn.UseVisualStyleBackColor = true;
            this.createFileBtn.Click += new System.EventHandler(this.createFileBtn_Click);
            // 
            // createFolderBtn
            // 
            this.createFolderBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.createFolderBtn.Location = new System.Drawing.Point(5, 5);
            this.createFolderBtn.Name = "createFolderBtn";
            this.createFolderBtn.Size = new System.Drawing.Size(184, 40);
            this.createFolderBtn.TabIndex = 0;
            this.createFolderBtn.Text = "新建文件夹";
            this.createFolderBtn.UseVisualStyleBackColor = true;
            this.createFolderBtn.Click += new System.EventHandler(this.createFolderBtn_Click);
            // 
            // dirView
            // 
            this.dirView.AllowUserToAddRows = false;
            this.dirView.AllowUserToDeleteRows = false;
            this.dirView.AllowUserToResizeColumns = false;
            this.dirView.AllowUserToResizeRows = false;
            this.dirView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dirView.ColumnHeadersHeight = 29;
            this.dirView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dirView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fileName,
            this.date,
            this.type,
            this.size});
            this.dirView.ContextMenuStrip = this.contextMemuStrip1;
            this.dirView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dirView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dirView.Location = new System.Drawing.Point(3, 3);
            this.dirView.MultiSelect = false;
            this.dirView.Name = "dirView";
            this.dirView.ReadOnly = true;
            this.dirView.RowHeadersVisible = false;
            this.dirView.RowHeadersWidth = 51;
            this.dirView.RowTemplate.Height = 29;
            this.dirView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dirView.Size = new System.Drawing.Size(570, 501);
            this.dirView.TabIndex = 1;
            this.dirView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dirView_CellContentClick);
            this.dirView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dirView_CellDoubleClick);
            this.dirView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dirView_CellMouseDown);
            // 
            // fileName
            // 
            this.fileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fileName.FillWeight = 2F;
            this.fileName.HeaderText = "名称";
            this.fileName.MinimumWidth = 6;
            this.fileName.Name = "fileName";
            this.fileName.ReadOnly = true;
            // 
            // date
            // 
            this.date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.date.FillWeight = 2F;
            this.date.HeaderText = "修改日期";
            this.date.MinimumWidth = 6;
            this.date.Name = "date";
            this.date.ReadOnly = true;
            // 
            // type
            // 
            this.type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.type.FillWeight = 1F;
            this.type.HeaderText = "类型";
            this.type.MinimumWidth = 6;
            this.type.Name = "type";
            this.type.ReadOnly = true;
            // 
            // size
            // 
            this.size.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.size.FillWeight = 1F;
            this.size.HeaderText = "大小";
            this.size.MinimumWidth = 6;
            this.size.Name = "size";
            this.size.ReadOnly = true;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "文件管理器";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.contextMemuStrip1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dirView)).EndInit();
            this.ResumeLayout(false);

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.disk.WriteDisk();
            this.disk.WriteFAT();
            this.dir.WriteDirectory();
        }
        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private Button backBtn;
        private TextBox pathBox;

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (current != dir.root)
            {
                current = current.parent;
                DisplayDirView();
            }
        }

        private TableLayoutPanel tableLayoutPanel3;

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private Panel panel1;
        private Label totalSpaceLbl;
        private Label usedSpaceLbl;
        private Button formatDiskBtn;
        private Button createFileBtn;
        private Button createFolderBtn;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private DataGridView dirView;
        private DataGridViewTextBoxColumn fileName;
        private DataGridViewTextBoxColumn date;
        private DataGridViewTextBoxColumn type;
        private DataGridViewTextBoxColumn size;
        

        private void dirView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string fileName = dirView.Rows[e.RowIndex].Cells[0].Value.ToString();
            int type;
            if (dirView.Rows[e.RowIndex].Cells[3].Value.ToString() == "")   // 文件夹
            {
                type = 1;
                Node n = this.current.firstChild;
                while (n != null)
                {
                    if (n.fcb.fileName == fileName && n.fcb.type == type)
                    {
                        current = n;
                        break;
                    }
                    n = n.nextSibling;
                }
            }
            else   // 文件
            {
                type = 0;
                Node n = this.current.firstChild;
                while (n != null)
                {
                    if (n.fcb.fileName == fileName && n.fcb.type == type)
                    {
                        break;
                    }
                    n = n.nextSibling;
                }
                if (n != null)
                {
                    string fileContent = disk.GetFileContent(n.fcb);
                    FormNotepad notePad = new FormNotepad(fileContent, n.fcb, this.disk);   // 打开记事本
                    notePad.ShowDialog();
                }
            }
            DisplayDirView();
        }

        private void totalSpaceLbl_Click(object sender, EventArgs e)
        {

        }

        private void createFolderBtn_Click(object sender, EventArgs e)
        {
            string folderName = Interaction.InputBox("请输入文件夹名称:", "新建文件夹");
            if (folderName == "")
            {
                MessageBox.Show("文件夹名称不得为空！");
                
            }
            else if(dir.NameExist(folderName, current, 1))
            {
                MessageBox.Show("文件夹名称已存在！");
            }
            else
            {
                FCB newFCB = new FCB(folderName, 1, DateTime.Now.ToString(), 0, -1);
                dir.CreateNode(current, newFCB);
            }
            DisplayDirView();
        }

        private void createFileBtn_Click(object sender, EventArgs e)
        {
            string folderName = Interaction.InputBox("请输入文件名:", "新建文本文档");
            if (folderName == "")
            {
                MessageBox.Show("文件名不得为空！");
                
            }
            else if (dir.NameExist(folderName, current, 0))
            {
                MessageBox.Show("文件名已存在！");
            }
            else
            {
                FCB newFCB = new FCB(folderName, 0, DateTime.Now.ToString(), 0, -1);
                dir.CreateNode(current, newFCB);
            }
            DisplayDirView();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void formatDiskBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("格式化磁盘将删除磁盘上的所有文件。", "格式化", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dir.DeleteNode(dir.root, this.disk);
                current = dir.root;
                DisplayDirView();
            }
        }

        private void dirView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private IContainer components;

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private ContextMenuStrip contextMemuStrip1;
        private ToolStripMenuItem toolStripRename;
        private ToolStripMenuItem toolStripDelete;
        private Node selected;

        private void contextMemuStrip1_Opening(object sender, CancelEventArgs e)
        {
            
        }

        private void dirView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dirView.Rows[e.RowIndex].Selected == false)
                    {
                        dirView.ClearSelection();
                        dirView.Rows[e.RowIndex].Selected = true;
                    }
                    string fileName = dirView.Rows[e.RowIndex].Cells[0].Value.ToString();
                    int type = (dirView.Rows[e.RowIndex].Cells[3].Value.ToString() == "") ? 1 : 0;

                    Node n = this.current.firstChild;
                    while (n != null)
                    {
                        if (n.fcb.fileName == fileName && n.fcb.type == type)
                        {
                            break;
                        }
                        n = n.nextSibling;
                    }
                    this.selected = n;
                    dirView.ContextMenuStrip.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void contextMemuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == toolStripRename)
            {
                string newName = Interaction.InputBox((this.selected.fcb.type == 0) ? "请输入文件名:" : "请输入文件夹名称:", "重命名");
                if (newName == "")
                {
                    MessageBox.Show("不得为空！");

                }
                else if (dir.NameExist(newName, current, selected.fcb.type))
                {
                    MessageBox.Show("该名称已存在！");
                }
                else
                {
                    selected.fcb.fileName = newName;
                }
            }
            else if (e.ClickedItem == toolStripDelete)
            {
                if (MessageBox.Show("确认删除 " + selected.fcb.fileName + " ？", "删除", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dir.DeleteNode(selected, disk);
                }
            }
            DisplayDirView();
        }

        private ProgressBar progressBar1;
    }
}
