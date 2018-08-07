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
            this.edgeBar = new System.Windows.Forms.SplitContainer();
            this.partLibrary = new System.Windows.Forms.ListView();
            this.partLibraryImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.currentDirectory = new System.Windows.Forms.ToolStripTextBox();
            this.backButton = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.configurationContainer = new System.Windows.Forms.SplitContainer();
            this.partProperties = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unitsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partPropertyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.edgeBar)).BeginInit();
            this.edgeBar.Panel1.SuspendLayout();
            this.edgeBar.Panel2.SuspendLayout();
            this.edgeBar.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configurationContainer)).BeginInit();
            this.configurationContainer.Panel1.SuspendLayout();
            this.configurationContainer.Panel2.SuspendLayout();
            this.configurationContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.partProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.partPropertyBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // edgeBar
            // 
            this.edgeBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edgeBar.Location = new System.Drawing.Point(0, 0);
            this.edgeBar.Name = "edgeBar";
            this.edgeBar.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // edgeBar.Panel1
            // 
            this.edgeBar.Panel1.Controls.Add(this.partLibrary);
            this.edgeBar.Panel1.Controls.Add(this.toolStrip1);
            // 
            // edgeBar.Panel2
            // 
            this.edgeBar.Panel2.Controls.Add(this.panel1);
            this.edgeBar.Size = new System.Drawing.Size(278, 630);
            this.edgeBar.SplitterDistance = 350;
            this.edgeBar.TabIndex = 0;
            // 
            // partLibrary
            // 
            this.partLibrary.AllowDrop = true;
            this.partLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.partLibrary.HideSelection = false;
            this.partLibrary.LabelWrap = false;
            this.partLibrary.LargeImageList = this.partLibraryImageList;
            this.partLibrary.Location = new System.Drawing.Point(0, 25);
            this.partLibrary.MultiSelect = false;
            this.partLibrary.Name = "partLibrary";
            this.partLibrary.RightToLeftLayout = true;
            this.partLibrary.ShowGroups = false;
            this.partLibrary.Size = new System.Drawing.Size(278, 325);
            this.partLibrary.SmallImageList = this.partLibraryImageList;
            this.partLibrary.TabIndex = 1;
            this.partLibrary.UseCompatibleStateImageBehavior = false;
            this.partLibrary.View = System.Windows.Forms.View.List;
            this.partLibrary.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.PartLibrary_ItemSelectionChanged);
            this.partLibrary.DoubleClick += new System.EventHandler(this.PartLibrary_DoubleClick);
            // 
            // partLibraryImageList
            // 
            this.partLibraryImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("partLibraryImageList.ImageStream")));
            this.partLibraryImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.partLibraryImageList.Images.SetKeyName(0, "Folder_16.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.AllowMerge = false;
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.currentDirectory,
            this.backButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(278, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Enabled = false;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel1.Text = "Folder:";
            // 
            // currentDirectory
            // 
            this.currentDirectory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.currentDirectory.Enabled = false;
            this.currentDirectory.HideSelection = false;
            this.currentDirectory.Name = "currentDirectory";
            this.currentDirectory.ReadOnly = true;
            this.currentDirectory.Size = new System.Drawing.Size(125, 25);
            // 
            // backButton
            // 
            this.backButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.backButton.Image = ((System.Drawing.Image)(resources.GetObject("backButton.Image")));
            this.backButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(36, 22);
            this.backButton.Text = "Back";
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.configurationContainer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(278, 276);
            this.panel1.TabIndex = 0;
            // 
            // configurationContainer
            // 
            this.configurationContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configurationContainer.Location = new System.Drawing.Point(0, 0);
            this.configurationContainer.Name = "configurationContainer";
            this.configurationContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // configurationContainer.Panel1
            // 
            this.configurationContainer.Panel1.Controls.Add(this.partProperties);
            // 
            // configurationContainer.Panel2
            // 
            this.configurationContainer.Panel2.Controls.Add(this.cancelButton);
            this.configurationContainer.Panel2.Controls.Add(this.okButton);
            this.configurationContainer.Size = new System.Drawing.Size(278, 276);
            this.configurationContainer.SplitterDistance = 235;
            this.configurationContainer.TabIndex = 2;
            // 
            // partProperties
            // 
            this.partProperties.AllowUserToAddRows = false;
            this.partProperties.AllowUserToDeleteRows = false;
            this.partProperties.AllowUserToResizeColumns = false;
            this.partProperties.AllowUserToResizeRows = false;
            this.partProperties.AutoGenerateColumns = false;
            this.partProperties.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.partProperties.BackgroundColor = System.Drawing.SystemColors.Control;
            this.partProperties.CausesValidation = false;
            this.partProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.partProperties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.valueDataGridViewTextBoxColumn,
            this.unitsDataGridViewTextBoxColumn});
            this.partProperties.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.partProperties.DataSource = this.partPropertyBindingSource;
            this.partProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.partProperties.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.partProperties.GridColor = System.Drawing.SystemColors.Control;
            this.partProperties.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.partProperties.Location = new System.Drawing.Point(0, 0);
            this.partProperties.MultiSelect = false;
            this.partProperties.Name = "partProperties";
            this.partProperties.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.partProperties.ShowCellErrors = false;
            this.partProperties.ShowEditingIcon = false;
            this.partProperties.ShowRowErrors = false;
            this.partProperties.Size = new System.Drawing.Size(278, 235);
            this.partProperties.TabIndex = 0;
            this.partProperties.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.PartProperties_CellEnter);
            this.partProperties.SelectionChanged += new System.EventHandler(this.PartProperties_SelectionChanged);
            this.partProperties.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PartProperties_KeyPress);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // valueDataGridViewTextBoxColumn
            // 
            this.valueDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
            this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
            this.valueDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
            this.valueDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // unitsDataGridViewTextBoxColumn
            // 
            this.unitsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.unitsDataGridViewTextBoxColumn.DataPropertyName = "Units";
            this.unitsDataGridViewTextBoxColumn.FillWeight = 75F;
            this.unitsDataGridViewTextBoxColumn.HeaderText = "Units";
            this.unitsDataGridViewTextBoxColumn.MinimumWidth = 75;
            this.unitsDataGridViewTextBoxColumn.Name = "unitsDataGridViewTextBoxColumn";
            this.unitsDataGridViewTextBoxColumn.ReadOnly = true;
            this.unitsDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.unitsDataGridViewTextBoxColumn.Width = 75;
            // 
            // partPropertyBindingSource
            // 
            this.partPropertyBindingSource.DataSource = typeof(KeyboardLogic.SolidEdge.AddIn.ItemCatalog.PartProperty);
            // 
            // cancelButton
            // 
            this.cancelButton.AutoSize = true;
            this.cancelButton.Location = new System.Drawing.Point(84, 2);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.AutoSize = true;
            this.okButton.Location = new System.Drawing.Point(3, 2);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // ItemCatalogEdgeBarController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.edgeBar);
            this.Name = "ItemCatalogEdgeBarController";
            this.Size = new System.Drawing.Size(278, 630);
            this.ToolTip = "Item Catalog";
            this.AfterInitialize += new System.EventHandler(this.ControllerAfterInitialize);
            this.Load += new System.EventHandler(this.ControllerLoad);
            this.edgeBar.Panel1.ResumeLayout(false);
            this.edgeBar.Panel1.PerformLayout();
            this.edgeBar.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.edgeBar)).EndInit();
            this.edgeBar.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.configurationContainer.Panel1.ResumeLayout(false);
            this.configurationContainer.Panel2.ResumeLayout(false);
            this.configurationContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configurationContainer)).EndInit();
            this.configurationContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.partProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.partPropertyBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer edgeBar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox currentDirectory;
        private System.Windows.Forms.ToolStripButton backButton;
        private System.Windows.Forms.ImageList partLibraryImageList;
        private System.Windows.Forms.SplitContainer configurationContainer;
        private System.Windows.Forms.DataGridView partProperties;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        protected System.Windows.Forms.ListView partLibrary;
        private System.Windows.Forms.BindingSource partPropertyBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn unitsDataGridViewTextBoxColumn;
    }
}
