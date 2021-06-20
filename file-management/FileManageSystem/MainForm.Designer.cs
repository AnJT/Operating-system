
using System.Windows.Forms;

namespace FileManageSystem {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonCreateFolder = new System.Windows.Forms.Button();
            this.buttonCreateFile = new System.Windows.Forms.Button();
            this.labelDiskSize = new System.Windows.Forms.Label();
            this.labelBlockSize = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.buttonBack = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.fileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastModify = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(960, 98);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(182, 39);
            this.buttonDelete.TabIndex = 0;
            this.buttonDelete.Text = "格式化";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click_1);
            // 
            // buttonCreateFolder
            // 
            this.buttonCreateFolder.Location = new System.Drawing.Point(960, 180);
            this.buttonCreateFolder.Name = "buttonCreateFolder";
            this.buttonCreateFolder.Size = new System.Drawing.Size(182, 44);
            this.buttonCreateFolder.TabIndex = 1;
            this.buttonCreateFolder.Text = "新建文件夹";
            this.buttonCreateFolder.UseVisualStyleBackColor = true;
            this.buttonCreateFolder.Click += new System.EventHandler(this.buttonCreateFolder_Click);
            // 
            // buttonCreateFile
            // 
            this.buttonCreateFile.Location = new System.Drawing.Point(960, 267);
            this.buttonCreateFile.Name = "buttonCreateFile";
            this.buttonCreateFile.Size = new System.Drawing.Size(182, 47);
            this.buttonCreateFile.TabIndex = 2;
            this.buttonCreateFile.Text = "新建文本文件";
            this.buttonCreateFile.UseVisualStyleBackColor = true;
            this.buttonCreateFile.Click += new System.EventHandler(this.buttonCreateFile_Click);
            // 
            // labelDiskSize
            // 
            this.labelDiskSize.AutoSize = true;
            this.labelDiskSize.Location = new System.Drawing.Point(957, 388);
            this.labelDiskSize.Name = "labelDiskSize";
            this.labelDiskSize.Size = new System.Drawing.Size(62, 18);
            this.labelDiskSize.TabIndex = 3;
            this.labelDiskSize.Text = "label1";
            // 
            // labelBlockSize
            // 
            this.labelBlockSize.AutoSize = true;
            this.labelBlockSize.Location = new System.Drawing.Point(957, 466);
            this.labelBlockSize.Name = "labelBlockSize";
            this.labelBlockSize.Size = new System.Drawing.Size(62, 18);
            this.labelBlockSize.TabIndex = 4;
            this.labelBlockSize.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "当前路径";
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(217, 35);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(461, 28);
            this.textBoxSearch.TabIndex = 6;
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(715, 12);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(225, 51);
            this.buttonBack.TabIndex = 7;
            this.buttonBack.Text = "返回上级目录";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fileName,
            this.lastModify,
            this.type,
            this.size});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listView1.Location = new System.Drawing.Point(217, 123);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(723, 433);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // fileName
            // 
            this.fileName.Text = "文件名";
            this.fileName.Width = 122;
            // 
            // lastModify
            // 
            this.lastModify.Text = "修改日期";
            this.lastModify.Width = 115;
            // 
            // type
            // 
            this.type.Text = "类型";
            this.type.Width = 74;
            // 
            // size
            // 
            this.size.Text = "大小";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 656);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelBlockSize);
            this.Controls.Add(this.labelDiskSize);
            this.Controls.Add(this.buttonCreateFile);
            this.Controls.Add(this.buttonCreateFolder);
            this.Controls.Add(this.buttonDelete);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonCreateFolder;
        private System.Windows.Forms.Button buttonCreateFile;
        private System.Windows.Forms.Label labelDiskSize;
        private System.Windows.Forms.Label labelBlockSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.ListView listView1;
        private ColumnHeader fileName;
        private ColumnHeader lastModify;
        private ColumnHeader type;
        private ColumnHeader size;
    }
}