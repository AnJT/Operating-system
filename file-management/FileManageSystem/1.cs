using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.VisualBasic;

namespace FileManageSystem {
    public partial class MainForm : Form {
        public Category category = new Category(); // 创建目录
        public Category.Node rootNode = new Category.Node(); // 目录的根节点
        public Category.Node currentRoot = new Category.Node();// 当前根节点为root
        public VirtualDisk myDisk = new VirtualDisk(1000, 2); // 申请内存空间

        public string nowPath;
        public string change;
        public MainForm() {
            this.StartPosition = FormStartPosition.CenterScreen;
            FCB root = new FCB("root", FCB.FOLDER, "", 1);
            this.rootNode = new Category.Node(root);
            this.currentRoot = rootNode;
            this.category.root = rootNode;

            // 恢复文件管理系统
            readFormDisk(); // 读取目录信息文件
            readBitMap(); // 读取位图文件
            readmyDisk(); // 读取虚拟磁盘文件

            InitializeComponent();
        }



        private void label1_Click(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {

        }

        private void label5_Click(object sender, EventArgs e) {

        }

        //格式化
        private void buttonDelete_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("确定清空磁盘？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK) {
                category.freeCategory(category.root);
                for (int i = 0; i < myDisk.blockSize; i++) {
                    myDisk.memory[i] = "";
                    myDisk.bitMap[i] = -1;
                    myDisk.remain = myDisk.blockNum;
                }
                MessageBox.Show("磁盘已清空。");
                //fileFormInit(rootNode);

                nowPath = "";
                textBoxSearch.Text = "";
                treeView.Nodes.Clear();

                updateLog();        //清空所有日志文件
            }
        }

        //创建目录树
        public void createTreeView(Category.Node node) {
            if (node == null)      //目录为空, 不需要创建目录树
                return;

            // 文件夹和文本文件分别创建目录树结点
            TreeNode tn = new TreeNode();
            if (node.fcb.type == FCB.FOLDER) {
                tn.Name = node.fcb.fileName;
                tn.Text = node.fcb.fileName;
                tn.Tag = 1;
                tn.ImageIndex = 1;
                tn.SelectedImageIndex = 1;
            }
            else if (node.fcb.type == FCB.TXTFILE) {
                tn.Name = node.fcb.fileName + ".txt";
                tn.Text = node.fcb.fileName + ".txt";
                tn.Tag = 0;
                tn.ImageIndex = 0;
                tn.SelectedImageIndex = 0;
            }

            if (node.parent == this.rootNode) {
                treeView.Nodes.Add(tn);
            }
            else {
                CallAddNode(treeView, node.parent.fcb.fileName, tn);
            }
            this.createTreeView(node.child);
            this.createTreeView(node.brother);
        }

        public TreeNode CallAddNode(TreeView tree, string tnStr, TreeNode newTn) {
            foreach (TreeNode n in tree.Nodes) {
                TreeNode temp = AddNode(n, tnStr, newTn);
                if (temp != null)
                    return temp;
            }
            return null;
        }

        //递归查找往目录树中加入结点
        public TreeNode AddNode(TreeNode tnParent, string tnStr, TreeNode newTn) {
            if (tnParent == null)
                return null;
            if (tnParent.Name == tnStr) {
                tnParent.Nodes.Add(newTn);
            }

            TreeNode tnRet = null;
            foreach (TreeNode tn in tnParent.Nodes) {
                tnRet = AddNode(tn, tnStr, newTn);
                if (tnRet != null)
                    break;
            }
            return tnRet;
        }

        // CategoryInfo.txt中信息格式 
        // 当前目录下的根节点
        // 文件名称
        // 文件类型
        // 上次修改时间
        // 文件大小
        // 文件起始位置
        // #分隔符

        // 目录信息
        public void readFormDisk() {
            string path = Application.StartupPath + "\\CategoryInfo.txt";
            if (File.Exists(path)) {
                StreamReader reader = new StreamReader(path);
                string parentName = "", name = "";
                string lastModify = "";
                int type = -1, size = 0, start = -1, infoNum = 1;

                string str = reader.ReadLine();
                while (str != null) {
                    switch (infoNum) {
                        case 1:
                            parentName = str;
                            infoNum++;
                            break;
                        case 2:
                            name = str;
                            infoNum++;
                            break;
                        case 3:
                            type = int.Parse(str);
                            infoNum++;
                            break;
                        case 4:
                            lastModify = str;
                            infoNum++;
                            break;
                        case 5:
                            size = int.Parse(str);
                            infoNum++;
                            break;
                        case 6:
                            start = int.Parse(str);
                            infoNum++;
                            break;
                        case 7:
                            infoNum = 1;
                            FCB now = new FCB(name, type, lastModify, size, start);
                            this.category.createFile(parentName, now);
                            break;
                        default:
                            break;
                    }
                    str = reader.ReadLine();
                }
                reader.Close();
            }
        }

        public void writeCategory(Category.Node node) {
            Category.Node parentNode = category.currentRootName(this.rootNode, node.fcb.fileName, node.fcb.type);
            string InfoPath = Application.StartupPath + "\\CategoryInfo.txt";
            StreamWriter writer = File.AppendText(InfoPath);

            writer.WriteLine(parentNode.fcb.fileName); //写入父结点的名字
            writer.WriteLine(node.fcb.fileName); //写入文件的名字
            writer.WriteLine(node.fcb.type); //写入文件的类型
            writer.WriteLine(node.fcb.lastModify); //写入最后修改的时间
            writer.WriteLine(node.fcb.size); // 写入文件的大小
            if (node.fcb.type == FCB.TXTFILE)
                writer.WriteLine(node.fcb.start); // 写入文件的开始位置
            else
                writer.WriteLine(-1); // 如果是文件夹则写入 -1
            writer.WriteLine("#"); // 一个节点写完
        }

        // 位图文件
        public void readBitMap() {
            string path = Application.StartupPath + "\\BitMapInfo.txt";
            if (File.Exists(path)) {
                StreamReader reader = new StreamReader(path);
                for (int i = 0; i < myDisk.blockNum; i++) {
                    myDisk.bitMap[i] = int.Parse(reader.ReadLine());
                }
                reader.Close();
            }
        }

        public void writeBitMap() {
            if (File.Exists(Application.StartupPath + "\\BitMapInfo.txt"))
                File.Delete(Application.StartupPath + "\\BitMapInfo.txt");
            StreamWriter writer = new StreamWriter(Application.StartupPath + "\\BitMapInfo.txt");

            for (int i = 0; i < myDisk.blockNum; i++) {
                writer.WriteLine(myDisk.bitMap[i]);
            }
            writer.Close();
        }

        // 虚拟磁盘文件
        public void readmyDisk() {
            string path = Application.StartupPath + "\\MyDiskInfo.txt";
            Console.WriteLine(path);
            if (File.Exists(path)) {
                StreamReader reader = new StreamReader(path);
                for (int i = 0; i < myDisk.blockNum; i++) {
                    string str = reader.ReadLine();
                    if (str == "#")
                        myDisk.memory[i] = "";
                    else
                        myDisk.memory[i] = str;
                }
                reader.Close();
            }
        }

        public void writeMyDisk() {
            if (File.Exists(Application.StartupPath + "\\MyDiskInfo.txt"))
                File.Delete(Application.StartupPath + "\\MyDiskInfo.txt");
            StreamWriter writer = new StreamWriter(Application.StartupPath + "\\MyDiskInfo.txt");

            for (int i = 0; i < myDisk.blockNum; i++) {
                if (myDisk.memory[i] == "")
                    writer.WriteLine("#");
                else
                    writer.WriteLine(myDisk.memory[i]);
            }
            writer.Close();
        }

        //主窗体关闭时，把所有的内容写入到本地   
        protected override void OnFormClosing(FormClosingEventArgs e) {
            updateLog();
        }

        // 更新所有日志信息,将所有的内容写入到本地   
        public void updateLog() {
            if (File.Exists(Application.StartupPath + "\\CategoryInfo.txt"))
                File.Delete(Application.StartupPath + "\\CategoryInfo.txt");
            Queue<Category.Node> q = new Queue<Category.Node>();
            q.Enqueue(this.category.root);

            while (q.Count() != 0) {
                Category.Node temp = q.Dequeue();
                temp = temp.child;
                while (temp != null) {
                    q.Enqueue(temp);
                    writeCategory(temp);
                    temp = temp.brother;
                }
            }

            writeBitMap();
            writeMyDisk();
        }
    }
}
