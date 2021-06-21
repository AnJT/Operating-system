using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManageSystem {
    public partial class FileRWForm : Form {

        public static bool ischanged = false; // 用户打开记事本是否进行编辑
        public MainForm mainForm;
        public string filename;
        public FileRWForm(string name, MainForm mainForm) {
            this.mainForm = mainForm;
            this.filename = name;
            InitializeComponent();
        }

        private void FileRWForm_Load(object sender, EventArgs e) {
            string[] name = this.filename.Split('/');
            this.Text = name[name.Length - 2] + ".txt";
            FCB nowFcb = this.mainForm.category.search(this.mainForm.category.root, this.filename, FCB.TXTFILE).fcb;

            string content = this.mainForm.myDisk.getFileContent(nowFcb);
            this.textBox1.AppendText(content);
            ischanged = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            if (ischanged == true) {
                DialogResult result = MessageBox.Show("是否进行保存？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK) {
                    FCB nowFcb = this.mainForm.category.search(this.mainForm.category.root, this.filename, FCB.TXTFILE).fcb;
                    int oldSize = nowFcb.size, oldStart = nowFcb.start;
                    string content = this.textBox1.Text.Trim();

                    if (textBox1.Text.Trim().Length >= this.mainForm.myDisk.remain) {
                        MessageBox.Show("磁盘空间不足！");
                        e.Cancel = true;
                    }
                    else {
                        nowFcb.size = textBox1.Text.Trim().Length;
                        nowFcb.lastModify = DateTime.Now.ToLocalTime().ToString();
                        this.mainForm.myDisk.fileUpdate(oldStart, oldSize, nowFcb, this.textBox1.Text.Trim());
                        this.mainForm.init(this.mainForm.currentRoot);
                    }
                }
                else
                    e.Cancel = false;
            }
            else
                e.Cancel = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            ischanged = true;
        }
    }
}
