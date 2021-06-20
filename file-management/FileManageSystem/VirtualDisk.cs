using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManageSystem {
    public class VirtualDisk {
        public const int EMPTY = -1; // 表示当前块为空
        public const int END = -2; // 结束标识符

        public int size; // 磁盘容量
        public int remain; // 磁盘剩余空间
        public int blockSize; // 块大小
        public int blockNum; // 磁盘块数
        public string[] memory = new string[] { }; // 内存空间
        public int[] bitMap = new int[] { }; //位图表

        public VirtualDisk(int size, int blockSize) {
            this.size = size;
            this.blockSize = blockSize;
            this.blockNum = size / blockSize;
            this.remain = this.blockNum;

            this.memory = new string[this.blockNum];
            this.bitMap = new int[this.blockNum];
            for(int i = 0; i < this.blockNum; i++) {
                this.bitMap[i] = EMPTY; // 初始化位图表全部为空
                this.memory[i] = ""; // 初始化内存内容为空
            }
        }

        // 更新文件内容
        public void fileUpdate(int oldStart, int oldSize, FCB newFcb, string newContent) {
            this.deleteFileContent(oldStart, oldSize);
            this.giveSpace(newFcb, newContent);
        }

        // 给文件分配空间并添加内容
        public bool giveSpace(FCB fcb, string content) {
            int blocks = this.getBlockSize(fcb.size);
            if(blocks <= this.remain) {
                int start = 0; // 记录起始位置
                for(; start < this.blockNum; start++) {
                    if(bitMap[start] == EMPTY) {
                        this.remain--;
                        fcb.start = start;
                        this.memory[start] = content.Substring(0, Math.Min(this.blockSize, content.Length));
                        break;
                    }
                }
                for(int j = 1, i = start + 1; j < blocks && i < this.blockNum; i++) {
                    if(this.bitMap[i] == EMPTY) {
                        this.remain--;
                        this.bitMap[start] = i; // 以链接的方式存储每位数据
                        start = i;

                        if (j != blocks - 1)
                            this.memory[i] = content.Substring(j * this.blockSize, this.blockSize);
                        else {
                            this.bitMap[i] = END;
                            if (fcb.size % this.blockSize != 0)
                                this.memory[i] = content.Substring(j * this.blockSize, content.Length - j * blockSize);
                            else
                                this.memory[i] = content.Substring(j * this.blockSize, Math.Min(this.blockSize, content.Length));
                        }
                        j++;
                    }
                }
                return true;
            }
            return false;
        }

        // 删除文件内容
        public void deleteFileContent(int start, int size) {
            int blocks = this.getBlockSize(size);
            int count = 0, i = start;
            while(i < this.blockNum) {
                if (count == blocks)
                    break;
                else {
                    this.memory[i] = ""; // 清空所占内存
                    this.remain++;
                    int next = this.bitMap[i];
                    this.bitMap[i] = EMPTY; // 清空所占位图
                    i = next;
                    count++;
                }
            }
        }

        // 获取所占块数
        private int getBlockSize(int size) {
            return size / this.blockSize + (size % this.blockSize == 0 ? 0 : 1);
        }
    }
}
