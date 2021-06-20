
namespace FileManageSystem {
    partial class MaiForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.treeView = new System.Windows.Forms.TreeView();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonCreateFolder = new System.Windows.Forms.Button();
            this.buttonCreateFile = new System.Windows.Forms.Button();
            this.labelDiskSIze = new System.Windows.Forms.Label();
            this.labelBlockSIze = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.buttonBack = new System.Windows.Forms.Button();
            this.fileName = new System.Windows.Forms.Label();
            this.modifyTime = new System.Windows.Forms.Label();
            this.fileType = new System.Windows.Forms.Label();
            this.fileSize = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Location = new System.Drawing.Point(-1, 2);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(232, 484);
            this.treeView.TabIndex = 0;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(938, 73);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(116, 30);
            this.buttonDelete.TabIndex = 1;
            this.buttonDelete.Text = "格式化";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonCreateFolder
            // 
            this.buttonCreateFolder.Location = new System.Drawing.Point(938, 140);
            this.buttonCreateFolder.Name = "buttonCreateFolder";
            this.buttonCreateFolder.Size = new System.Drawing.Size(136, 35);
            this.buttonCreateFolder.TabIndex = 2;
            this.buttonCreateFolder.Text = "新建文件夹";
            this.buttonCreateFolder.UseVisualStyleBackColor = true;
            // 
            // buttonCreateFile
            // 
            this.buttonCreateFile.Location = new System.Drawing.Point(938, 212);
            this.buttonCreateFile.Name = "buttonCreateFile";
            this.buttonCreateFile.Size = new System.Drawing.Size(136, 39);
            this.buttonCreateFile.TabIndex = 3;
            this.buttonCreateFile.Text = "新建文本文件";
            this.buttonCreateFile.UseVisualStyleBackColor = true;
            // 
            // labelDiskSIze
            // 
            this.labelDiskSIze.AutoSize = true;
            this.labelDiskSIze.Location = new System.Drawing.Point(935, 303);
            this.labelDiskSIze.Name = "labelDiskSIze";
            this.labelDiskSIze.Size = new System.Drawing.Size(55, 15);
            this.labelDiskSIze.TabIndex = 4;
            this.labelDiskSIze.Text = "label1";
            // 
            // labelBlockSIze
            // 
            this.labelBlockSIze.AutoSize = true;
            this.labelBlockSIze.Location = new System.Drawing.Point(935, 375);
            this.labelBlockSIze.Name = "labelBlockSIze";
            this.labelBlockSIze.Size = new System.Drawing.Size(55, 15);
            this.labelBlockSIze.TabIndex = 5;
            this.labelBlockSIze.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(249, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "当前路径";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(327, 12);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(415, 25);
            this.textBoxSearch.TabIndex = 7;
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(772, 12);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(150, 32);
            this.buttonBack.TabIndex = 8;
            this.buttonBack.Text = "返回上级目录";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.button1_Click);
            // 
            // fileName
            // 
            this.fileName.AutoSize = true;
            this.fileName.Location = new System.Drawing.Point(252, 57);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(52, 15);
            this.fileName.TabIndex = 9;
            this.fileName.Text = "文件名";
            // 
            // modifyTime
            // 
            this.modifyTime.AutoSize = true;
            this.modifyTime.Location = new System.Drawing.Point(424, 57);
            this.modifyTime.Name = "modifyTime";
            this.modifyTime.Size = new System.Drawing.Size(67, 15);
            this.modifyTime.TabIndex = 10;
            this.modifyTime.Text = "修改日期";
            // 
            // fileType
            // 
            this.fileType.AutoSize = true;
            this.fileType.Location = new System.Drawing.Point(566, 57);
            this.fileType.Name = "fileType";
            this.fileType.Size = new System.Drawing.Size(37, 15);
            this.fileType.TabIndex = 11;
            this.fileType.Text = "类型";
            // 
            // fileSize
            // 
            this.fileSize.AutoSize = true;
            this.fileSize.Location = new System.Drawing.Point(726, 57);
            this.fileSize.Name = "fileSize";
            this.fileSize.Size = new System.Drawing.Size(37, 15);
            this.fileSize.TabIndex = 12;
            this.fileSize.Text = "大小";
            this.fileSize.Click += new System.EventHandler(this.label5_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 485);
            this.Controls.Add(this.fileSize);
            this.Controls.Add(this.fileType);
            this.Controls.Add(this.modifyTime);
            this.Controls.Add(this.fileName);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelBlockSIze);
            this.Controls.Add(this.labelDiskSIze);
            this.Controls.Add(this.buttonCreateFile);
            this.Controls.Add(this.buttonCreateFolder);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.treeView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FileManageSystem";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonCreateFolder;
        private System.Windows.Forms.Button buttonCreateFile;
        private System.Windows.Forms.Label labelDiskSIze;
        private System.Windows.Forms.Label labelBlockSIze;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Label fileName;
        private System.Windows.Forms.Label modifyTime;
        private System.Windows.Forms.Label fileType;
        private System.Windows.Forms.Label fileSize;
    }
}