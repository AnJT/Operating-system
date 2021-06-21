using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManageSystem {
    public class Category {
        // 目录节点
        public class Node {
            public FCB fcb = new FCB();

            public Node child = null; // 左儿子
            public Node brother = null; // 右兄弟
            public Node parent = null; // 父节点

            public Node() { }
            public Node(FCB fcb) {
                this.fcb.fileName = fcb.fileName;
                this.fcb.lastModify = fcb.lastModify;
                this.fcb.type = fcb.type;
                this.fcb.size = fcb.size;
                this.fcb.start = fcb.start;
            }
            public Node(string name, int type) {
                this.fcb.fileName = name;
                this.fcb.type = type;
            }
        }

        public Node root; // 根节点

        public Category() {
            this.root = null;
        }
        public Category(FCB fcb) {
            this.root = new Node(fcb);
        }

        // 清空某目录
        public void freeCategory(ref Node node) {
            if (node == null)
                return;
            this.freeCategory(ref node.child);
            this.freeCategory(ref node.brother);
            node = null;
        }

        // 删除节点
        public void delete(Node node) {
            node = null;
        }

        // 搜索文件
        public Node search(Node node, string name, int type) {
            if (node == null)
                return null;
            if (node.fcb.fileName == name && node.fcb.type == type)
                return node;
            Node result = this.search(node.child, name, type);
            return result != null ? result : this.search(node.brother, name, type);
        }

        // 在文件夹中创建文件
        public void createFile(string parentFileName, FCB fcb) {
            if (this.root == null)
                return;
            Node parentNode = this.search(this.root, parentFileName, FCB.FOLDER);
            if (parentNode == null)
                return;
            if(parentNode.child == null) {
                parentNode.child = new Node(fcb);
                parentNode.child.parent = parentNode;
                return;
            }
            else {
                Node temp = parentNode.child;
                while (temp.brother != null)
                    temp = temp.brother;
                temp.brother = new Node(fcb);
                temp.brother.parent = parentNode;
            }
        }

        // 删除文件夹
        public void deleteFolder(string name) {
            Node currentNode = this.search(this.root, name, FCB.FOLDER);
            Node parentNode = currentNode.parent;
            if (parentNode.child == currentNode)
                parentNode.child = currentNode.brother;
            else {
                Node temp = parentNode.child;
                while (temp.brother != currentNode)
                    temp = temp.brother;
                temp.brother = currentNode.brother;
            }
            this.freeCategory(ref currentNode);
        }

        // 删除文件
        public void deleteFile(string name) {
            Node currentNode = this.search(this.root, name, FCB.TXTFILE);
            Node parentNode = currentNode.parent;
            if (parentNode.child == currentNode)
                parentNode.child = currentNode.brother;
            else {
                Node temp = parentNode.child;
                while (temp.brother != currentNode)
                    temp = temp.brother;
                temp.brother = currentNode.brother;
            }
            delete(currentNode);
        }

        // 判断同级目录下是否不重名
        public bool noSameName(Node node, string name, int type) {
            node = node.child;
            while(node != null) {
                if (node.fcb.fileName == name && node.fcb.type == type)
                    return false;
                node = node.brother;
            }
            return true;
        }

        // 寻找该目录下根目录的名称
        public Node currentRootName(Node node, string name, int type) {
            if (node == null)
                return null;
            if (node.child == null)
                return null;
            Node temp = node.child;
            while(temp != null) {
                if (temp.fcb.fileName == name && temp.fcb.type == type)
                    return node;
                temp = temp.brother;
            }
            Node result = this.currentRootName(node.child, name, type);
            return result != null ? result : this.currentRootName(node.brother, name, type);
        }
    }
}
