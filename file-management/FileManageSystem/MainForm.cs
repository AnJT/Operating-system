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
            this.initListView(this.currentRoot);
        }

        private void buttonBack_Click(object sender, EventArgs e) {
            this.nowPath = this.nowPath.Replace("> " + currentRoot.fcb.fileName, "");
            this.currentRoot = this.currentRoot.parent;
            this.initListView(this.currentRoot);
        }

        private void buttonCreateFolder_Click(object sender, EventArgs e) {
            string str = Interaction.InputBox("请输入文件夹的名称", "创建文件夹", "", 100, 100);
            if (str != "") {
                if (category.noSameName(this.currentRoot, str, FCB.FOLDER)) {
                    string time = DateTime.Now.ToLocalTime().ToString();
                    this.category.createFile(this.currentRoot.fcb.fileName, new FCB(str, FCB.FOLDER, time, 0));  //文件夹加入到目录中
                    this.initListView(this.currentRoot);
                }
                else {
                    MessageBox.Show("已存在名为" + str + "的文件夹，创建失败！");
                }
            }
        }

        private void buttonCreateFile_Click(object sender, EventArgs e) {
            string str = Interaction.InputBox("请输入文件的名称", "创建文本文件", "", 100, 100);
            if (str != "") {
                if (this.category.noSameName(this.currentRoot, str, FCB.TXTFILE)) {
                    string time = DateTime.Now.ToLocalTime().ToString();    //获取时间信息
                    this.category.createFile(this.currentRoot.fcb.fileName, new FCB(str, FCB.TXTFILE, time, 0));  //文件加入到目录中
                    this.initListView(this.currentRoot);
                }
                else {
                    MessageBox.Show("已存在名为" + str + ".txt的文件，创建失败！");
                }
            }
        }

        // 格式化
        private void buttonDelete_Click_1(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("确定清空磁盘？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK) {
                this.category.freeCategory(ref this.category.root);
                for (int i = 0; i < myDisk.blockSize; i++) {
                    myDisk.memory[i] = "";
                    myDisk.bitMap[i] = -1;
                    myDisk.remain = myDisk.blockNum;
                }
                MessageBox.Show("磁盘已清空。");
                //fileFormInit(rootNode);

                this.nowPath = "";
                this.textBoxSearch.Text = "";
                this.category.root = this.rootNode;
                updateLog();        //清空所有日志文件
                this.currentRoot = this.rootNode;
                this.initListView(this.currentRoot);
            }
        }

        public void initListView(Category.Node node) {
            this.listView1.BeginUpdate();
            this.listView1.Items.Clear();
            if (node == null) {
                this.listView1.EndUpdate();
                return;
            }
            if (node.child == null) {
                this.listView1.EndUpdate();
                return;
            }
            Category.Node temp = node.child;
            while(temp != null) {
                ListViewItem li = new ListViewItem();
                li.Text = temp.fcb.fileName;
                li.SubItems.Add(temp.fcb.lastModify);
                li.SubItems.Add(temp.fcb.type == FCB.FOLDER ? "Filder" : "File");
                li.SubItems.Add(temp.fcb.size.ToString() + "B");
                this.listView1.Items.Add(li);
                temp = temp.brother;
            }
            this.listView1.EndUpdate();
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
            writer.Close();
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
            Category.Node temp = new Category.Node();

            Queue<Category.Node> q = new Queue<Category.Node>();
            q.Enqueue(this.category.root);

            while (q.Count() != 0) {
                temp = q.Dequeue();
                if (temp == null)
                    continue;
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
