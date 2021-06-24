using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManagement
{
    public partial class FormNotepad : Form
    {
        bool changed = false;
        FCB fcb;
        Disk disk;
        public FormNotepad(string content, FCB fcb, Disk disk)
        {
            InitializeComponent(content);
            this.fcb = fcb;
            this.Text = fcb.fileName + " - 记事本";
            this.disk = disk;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void FormNotepad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changed)
            {
                if (MessageBox.Show("是否将更改保存到" + fcb.fileName + "中？", "记事本", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (!disk.UpdateFile(this.fcb, this.textBox1.Text))
                    {
                        MessageBox.Show("保存失败, 磁盘空间不足！");
                    }
                }
            }
        }

        private void FormNotepad_Load(object sender, EventArgs e)
        {
            
        }
    }
}
