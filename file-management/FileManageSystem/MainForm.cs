using System;
using System.Collections;
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
            FCB root = new FCB("./", FCB.FOLDER, "", 1);
            this.rootNode = new Category.Node(root);
            this.currentRoot = rootNode;
            this.category.root = rootNode;
            this.nowPath = rootNode.fcb.fileName;

            // 恢复文件管理系统
            readFormDisk(); // 读取目录信息文件
            readBitMap(); // 读取位图文件
            readmyDisk(); // 读取虚拟磁盘文件

            InitializeComponent();

            this.listView1.SmallImageList = this.imageList1;
            this.treeView.ImageList = this.imageList1;

            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.init(this.rootNode);
            this.initTreeView(this.rootNode);
            this.contextMenuStrip1.Closed += new ToolStripDropDownClosedEventHandler(contextMenuStrip1_Closed);
            this.listView1.ContextMenuStrip = this.contextMenuStrip2;

        }

        public void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e) {
            this.listView1.ContextMenuStrip = contextMenuStrip2;
        }


        public void init(Category.Node node) {
            this.nowPath = node.fcb.fileName;
            this.textBoxSearch.Text = this.nowPath;
            this.currentRoot = node;

            this.labelDiskSize.Text = "当前磁盘大小: " + myDisk.size.ToString() + "B";
            this.labelBlockSize.Text = "当前盘块大小: " + myDisk.blockSize.ToString() + "B";

            if (this.nowPath == "./")
                this.buttonBack.Enabled = false;
            else
                this.buttonBack.Enabled = true;

            this.initListView(node);
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
            while (temp != null) {
                ListViewItem li = new ListViewItem();
                string[] path = temp.fcb.fileName.Split('/');
                li.ImageIndex = temp.fcb.type;
                li.Text = path[path.Length - 2] + ((temp.fcb.type == FCB.FOLDER) ? "" :".txt");
                li.SubItems.Add(temp.fcb.lastModify);
                li.SubItems.Add(temp.fcb.type == FCB.FOLDER ? "Folder" : "File");
                li.SubItems.Add(temp.fcb.type == FCB.FOLDER ? "-" : (temp.fcb.size.ToString() + "B"));
                this.listView1.Items.Add(li);
                temp = temp.brother;
            }
            this.listView1.EndUpdate();
        }
        public void initTreeView(Category.Node node) {
            this.treeView.Nodes.Clear();
            this.treeView.BeginUpdate();
            TreeNode tn = new TreeNode();
            tn.Name = node.fcb.fileName;
            tn.Text = node.fcb.fileName;
            tn.Tag = FCB.FOLDER;
            tn.ImageIndex = FCB.FOLDER;
            tn.SelectedImageIndex = FCB.FOLDER;
            treeView.Nodes.Add(tn);
            this.initTreeNode(tn, node.child);
            this.treeView.ExpandAll();
            this.treeView.EndUpdate();
        }
        public void initTreeNode(TreeNode parentTn, Category.Node node) {
            if (node == null)
                return;
            TreeNode tn = new TreeNode();
            if (node.fcb.type == FCB.FOLDER) {
                string[] name = node.fcb.fileName.Split('/');
                tn.Name = name[name.Length - 2];
                tn.Text = name[name.Length - 2];
                tn.Tag = FCB.FOLDER;
                tn.ImageIndex = FCB.FOLDER;
                tn.SelectedImageIndex = FCB.FOLDER;
            }
            else {
                string[] name = node.fcb.fileName.Split('/');
                tn.Name = name[name.Length - 2] + ".txt";
                tn.Text = name[name.Length - 2] + ".txt";
                tn.Tag = FCB.TXTFILE;
                tn.ImageIndex = FCB.TXTFILE;
                tn.SelectedImageIndex = FCB.TXTFILE;
            }
            parentTn.Nodes.Add(tn);
            this.initTreeNode(parentTn, node.brother);
            this.initTreeNode(tn, node.child);
        }
        private void buttonBack_Click(object sender, EventArgs e) {
            this.currentRoot = this.currentRoot.parent;
            this.init(this.currentRoot);
        }

        // 创建文件夹
        public void buttonCreateFolder_Click(object sender, EventArgs e) {
            string str = Interaction.InputBox("请输入文件夹的名称", "创建文件夹", "", -1, -1);
            if(str.Contains('/'))
                MessageBox.Show("文件夹名称中不能包含'/'，创建失败！");
            else if (str != "") {
                if (category.noSameName(this.currentRoot, this.nowPath + str + "/", FCB.FOLDER)) {
                    string time = DateTime.Now.ToLocalTime().ToString();
                    this.category.createFile(this.currentRoot.fcb.fileName, new FCB(this.nowPath + str + "/", FCB.FOLDER, time, 0));  //文件夹加入到目录中
                    this.initListView(this.currentRoot);
                    this.initTreeView(this.rootNode);
                }
                else {
                    MessageBox.Show("已存在名为" + str + "的文件夹，创建失败！");
                }
            }
        }

        // 创建文本文件
        public void buttonCreateFile_Click(object sender, EventArgs e) {
            string str = Interaction.InputBox("请输入文件的名称", "创建文本文件", "", -1, -1);
            if (str.Contains('/'))
                MessageBox.Show("文本文件名称中不能包含'/'，创建失败！");
            else if (str != "") {
                if (this.category.noSameName(this.currentRoot, this.nowPath + str + "/", FCB.TXTFILE)) {
                    string time = DateTime.Now.ToLocalTime().ToString();    //获取时间信息
                    this.category.createFile(this.currentRoot.fcb.fileName, new FCB(this.nowPath + str + "/", FCB.TXTFILE, time, 0));  //文件加入到目录中
                    this.initListView(this.currentRoot);
                    this.initTreeView(this.rootNode);
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
                for (int i = 0; i < myDisk.blockNum; i++) {
                    myDisk.memory[i] = "";
                    myDisk.bitMap[i] = -1;
                }
                myDisk.remain = myDisk.blockNum;
                MessageBox.Show("磁盘已清空。");

                this.nowPath = "";
                this.textBoxSearch.Text = "";
                this.category.root = this.rootNode;
                updateLog();        //清空所有日志文件
                this.currentRoot = this.rootNode;
                this.init(this.currentRoot);
                this.initTreeView(this.rootNode);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e) {
            if (this.listView1.SelectedItems.Count > 0) {
                if (this.listView1.SelectedItems[0].SubItems[2].Text.Replace(".txt", "") == "File") {
                    FileRWForm file = new FileRWForm(this.nowPath + this.listView1.SelectedItems[0].Text.Replace(".txt", "") + "/", this);
                    file.Show();
                }
                else if (this.listView1.SelectedItems[0].SubItems[2].Text.Replace(".txt", "") == "Folder") {
                    this.currentRoot = this.category.search(this.rootNode, this.nowPath + this.listView1.SelectedItems[0].Text.Replace(".txt", "") + "/", FCB.FOLDER);
                    this.init(this.currentRoot);
                }
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                this.listView1.ContextMenuStrip = null;
                this.contextMenuStrip1.Show(this.listView1, e.Location);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e) {
            int type = this.listView1.SelectedItems[0].SubItems[2].Text.Replace(".txt", "") == "Folder" ? FCB.FOLDER : FCB.TXTFILE;
            Category.Node deleteNode = this.category.search(this.rootNode, this.nowPath + this.listView1.SelectedItems[0].Text.Replace(".txt", "") + "/", type);
            FCB deleteFcb = deleteNode.fcb;
            if (type == FCB.TXTFILE) {
                this.myDisk.deleteFileContent(deleteFcb.start, deleteFcb.size);
                this.category.deleteFile(this.nowPath + this.listView1.SelectedItems[0].Text.Replace(".txt", "") + "/");
            }
            else {
                this.myDisk.deleteFolderContent(deleteNode);
                this.category.deleteFolder(this.nowPath + this.listView1.SelectedItems[0].Text.Replace(".txt", "") + "/");
            }
            this.initListView(this.currentRoot);
            this.initTreeView(this.rootNode);
        }

        private void 新建文件夹ToolStripMenuItem_Click(object sender, EventArgs e) {
            this.buttonCreateFolder_Click(sender, e);
        }

        private void 新建文本文件ToolStripMenuItem_Click(object sender, EventArgs e) {
            this.buttonCreateFile_Click(sender, e);
        }

        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.listView1.SelectedItems.Count > 0) {
                int type = this.listView1.SelectedItems[0].SubItems[2].Text.Replace(".txt", "") == "Folder" ? FCB.FOLDER : FCB.TXTFILE;
                Category.Node node = this.category.search(this.rootNode, this.nowPath + this.listView1.SelectedItems[0].Text.Replace(".txt", "") + "/", type);
                string str = Interaction.InputBox("请输入新名称", "重命名", "", -1, -1);
                if (str.Contains('/'))
                    MessageBox.Show(String.Format("{0}名称中不能包含'/'，创建失败！", type == FCB.TXTFILE ? "文本文件" : "文件夹"));
                else if (str != "") {
                    if (category.noSameName(this.currentRoot, this.nowPath + str + "/", type)) {
                        string time = DateTime.Now.ToLocalTime().ToString();
                        node.fcb.fileName = this.nowPath + str + "/";
                        this.initListView(this.currentRoot);
                        this.initTreeView(this.rootNode);
                    }
                    else {
                        MessageBox.Show(String.Format("已存在名为" + str + "的{0}，重命名失败！", type == FCB.TXTFILE ? "文件" : "文件夹"));
                    }
                }
            }
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
            Category.Node parentNode = node.parent;
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

        private void treeView_DoubleClick(object sender, EventArgs e) {
            TreeNode temp = this.treeView.SelectedNode;
            ArrayList al = new ArrayList();
            while (temp != null) {
                al.Add(temp.Text);
                temp = temp.Parent;
            }
            string name = "./";
            for(int i = al.Count-2 ;i >= 0; i--) {
                name += al[i].ToString() + "/";
            }
            name = name.Replace(".txt", "");
            if(this.treeView.SelectedNode.Tag.ToString() == FCB.FOLDER.ToString()) {
                this.currentRoot = this.category.search(this.rootNode, name, FCB.FOLDER);
                this.init(this.currentRoot);
            }
            else {
                FileRWForm file = new FileRWForm(name, this);
                file.Show();
            }
        }
    }
}
