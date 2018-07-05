namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    partial class ItemCatalogEdgeBarController {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemCatalogEdgeBarController));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.partLibrary = new System.Windows.Forms.ListView();
            this.partLibraryImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.currentDirectory = new System.Windows.Forms.ToolStripTextBox();
            this.backButton = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.partPreview = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.partPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.partLibrary);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(278, 630);
            this.splitContainer1.SplitterDistance = 394;
            this.splitContainer1.TabIndex = 0;
            // 
            // partLibrary
            // 
            this.partLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.partLibrary.HideSelection = false;
            this.partLibrary.LabelWrap = false;
            this.partLibrary.LargeImageList = this.partLibraryImageList;
            this.partLibrary.Location = new System.Drawing.Point(0, 25);
            this.partLibrary.MultiSelect = false;
            this.partLibrary.Name = "partLibrary";
            this.partLibrary.RightToLeftLayout = true;
            this.partLibrary.ShowGroups = false;
            this.partLibrary.Size = new System.Drawing.Size(278, 369);
            this.partLibrary.SmallImageList = this.partLibraryImageList;
            this.partLibrary.TabIndex = 1;
            this.partLibrary.UseCompatibleStateImageBehavior = false;
            this.partLibrary.View = System.Windows.Forms.View.List;
            this.partLibrary.SelectedIndexChanged += new System.EventHandler(this.partLibrary_SelectedIndexChanged);
            this.partLibrary.DoubleClick += new System.EventHandler(this.partLibrary_DoubleClick);
            // 
            // partLibraryImageList
            // 
            this.partLibraryImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("partLibraryImageList.ImageStream")));
            this.partLibraryImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.partLibraryImageList.Images.SetKeyName(0, "Folder_16.png");
            this.partLibraryImageList.Images.SetKeyName(1, "Notepad_32x32.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.currentDirectory,
            this.backButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(278, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Enabled = false;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(34, 22);
            this.toolStripLabel1.Text = "Path:";
            // 
            // currentDirectory
            // 
            this.currentDirectory.Enabled = false;
            this.currentDirectory.Name = "currentDirectory";
            this.currentDirectory.Size = new System.Drawing.Size(175, 25);
            this.currentDirectory.TextChanged += new System.EventHandler(this.currentDirectory_TextChanged);
            // 
            // backButton
            // 
            this.backButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.backButton.Image = ((System.Drawing.Image)(resources.GetObject("backButton.Image")));
            this.backButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(36, 22);
            this.backButton.Text = "Back";
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.partPreview);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(278, 232);
            this.panel1.TabIndex = 0;
            // 
            // partPreview
            // 
            this.partPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.partPreview.Location = new System.Drawing.Point(0, 0);
            this.partPreview.Name = "partPreview";
            this.partPreview.Size = new System.Drawing.Size(278, 232);
            this.partPreview.TabIndex = 1;
            this.partPreview.TabStop = false;
            // 
            // ItemCatalogEdgeBarController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ItemCatalogEdgeBarController";
            this.Size = new System.Drawing.Size(278, 630);
            this.ToolTip = "Item Catalog";
            this.AfterInitialize += new System.EventHandler(this.controllerAfterInitialize);
            this.Load += new System.EventHandler(this.controllerLoad);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.partPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ListView partLibrary;
        private System.Windows.Forms.PictureBox partPreview;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox currentDirectory;
        private System.Windows.Forms.ToolStripButton backButton;
        private System.Windows.Forms.ImageList partLibraryImageList;
    }
}
