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

        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            ischanged = true;
        }
    }
}
