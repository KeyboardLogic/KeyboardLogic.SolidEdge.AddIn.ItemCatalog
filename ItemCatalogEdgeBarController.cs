using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SolidEdgeCommunity.AddIn;
using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System.IO;

namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    public partial class ItemCatalogEdgeBarController : EdgeBarControl {
        private string rootFolderPath = "Y:\\Google Drive\\Customers\\Innovated Solutions\\01 Profile 8";
        private SolidEdgeFramework.Application application;
        private SolidEdgeAssembly.AssemblyDocument assemblyDocument;
        private SolidEdgePart.PartDocument partDocument;
        private String originalFileName;

        public ItemCatalogEdgeBarController() {
            InitializeComponent();
            if (partLibraryImageList.Images.ContainsKey(".par") == false) {
                Icon icon = IconTools.GetIconForFile("test.par", ShellIconSize.SmallIcon);
                if (icon != null) {
                    partLibraryImageList.Images.Add(icon);
                    partLibraryImageList.Images.SetKeyName(1, ".par");
                }
            }
            this.hideConfigurationContainer();
            // Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register();
            // Connect to or start Solid Edge.
            this.application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);
            // Get a refrence to the active assembly document.
            this.assemblyDocument = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);
        }

        private void controllerLoad(object sender, EventArgs e) {
            // Trick to use the default system font.
            this.Font = SystemFonts.MessageBoxFont;
        }

        private void controllerAfterInitialize(object sender, EventArgs e) {
            // These properties are not initialized until AfterInitialize is called.
            var application = this.Document.Application;
            this.currentDirectory.Text = this.rootFolderPath;
        }

        private void partLibrary_SelectedIndexChanged(object sender, EventArgs e) {
            System.Diagnostics.Debug.WriteLine("partLibrary_SelectedIndexChanged: Documents.Count: " + this.application.Documents.Count);
            foreach (ListViewItem item in this.partLibrary.SelectedItems) {
                if (item.Text != null && File.Exists(this.currentDirectory.Text + "\\" + item.Text)) { // Display preview of part
                    // TODO: display preview of selected part
                    this.showConfigurationContainer(this.currentDirectory.Text + "\\" + item.Text);
                } else { // Directories do not have displays
                    this.hideConfigurationContainer();
                }
            }
        }

        private void partLibrary_DoubleClick(object sender, EventArgs e) {
            System.Diagnostics.Debug.WriteLine("partLibrary_DoubleClick: Documents.Count: " + this.application.Documents.Count);
            foreach (ListViewItem item in this.partLibrary.SelectedItems) {
                if (item.Text == null) {
                    // do nothing
                } else if (File.Exists(this.currentDirectory.Text + "\\" + item.Text)) {
                    // TODO: display preview of selected part
                    //this.showConfigurationContainer(this.currentDirectory.Text + "\\" + item.Text);
                    //this.okButton_Click(sender, e);
                } else { // change current directory to selected directory
                    this.currentDirectory.Text = this.currentDirectory.Text + "\\" + item.Text;
                }
            }
        }

        private void backButton_Click(object sender, EventArgs e) {
            this.currentDirectory.Text = this.currentDirectory.Text.Substring(0,this.currentDirectory.Text.LastIndexOf("\\"));
        }

        private void currentDirectory_TextChanged(object sender, EventArgs e) {
            if (this.currentDirectory.Text == null || !this.currentDirectory.Text.Contains(this.rootFolderPath)) {
                this.currentDirectory.Text = this.rootFolderPath;
            } else {
                this.partLibrary.Items.Clear();
                foreach (string str in Directory.GetDirectories(this.currentDirectory.Text)) {
                    this.partLibrary.Items.Add(Path.GetFileName(str), 0);
                }
                foreach (string str in Directory.GetFiles(this.currentDirectory.Text, "*.par")) {
                    this.partLibrary.Items.Add(Path.GetFileName(str), 1);
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e) {
            string fileName = "";
            foreach (PartProperty partProperty in this.partPropertyBindingSource) {
                fileName += ", " + partProperty.Name + "=" + partProperty.Value + partProperty.Units;
            }
            // Remove invalid fileName characters
            fileName = string.Join("", fileName.Split(Path.GetInvalidFileNameChars()));
            string fullPathName = this.assemblyDocument.Path + "\\" + this.originalFileName + fileName + ".par";
            this.partDocument.SaveCopyAs(fullPathName);
            this.hideConfigurationContainer();
            this.assemblyDocument.Occurrences.AddByFilename(fullPathName);
            this.application.DoIdle();
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.hideConfigurationContainer();
        }

        private void clearPartDocument() {
            this.partPropertyBindingSource.Clear();
            if (this.partDocument != null) {
                this.partDocument.Close(false);
                this.application.DoIdle();
                this.partDocument = null;
            }
        }

        /// <summary>
        /// Displays the configuration container used for changing values associated with part properties
        /// </summary>
        private void hideConfigurationContainer() {
            this.edgeBar.Panel2Collapsed = true;
            this.clearPartDocument();
        }

        /// <summary>
        /// Displays the configuration container used for changing values associated with part properties
        /// </summary>
        private void showConfigurationContainer(string filePath) {
            try {
                this.clearPartDocument();

                // Get a reference to the documents collection.
                SolidEdgeFramework.Documents documents = this.application.Documents;
                this.partDocument = (SolidEdgePart.PartDocument)documents.Open(filePath, "8");
                this.originalFileName = this.partDocument.Name.Substring(0, this.partDocument.Name.IndexOf(", natural_"));

                SolidEdgeFramework.Variables variables = partDocument.GetVariables();
                SolidEdgeFramework.VariableList variableList = (SolidEdgeFramework.VariableList)variables.Query("*", SolidEdgeConstants.VariableNameBy.seVariableNameByBoth, SolidEdgeConstants.VariableVarType.SeVariableVarTypeBoth, false);
                SolidEdgeFramework.UnitsOfMeasure unitesOfMeasure = this.partDocument.UnitsOfMeasure;
                PartProperty partProperty;
                foreach (var property in variableList) {
                    partProperty = null;
                    // Determine the runtime type of the object.
                    Type type = property.GetType();
                    SolidEdgeFramework.ObjectType objectType = (SolidEdgeFramework.ObjectType)type.InvokeMember("Type", System.Reflection.BindingFlags.GetProperty, null, property, null);

                    switch (objectType) {
                        case SolidEdgeFramework.ObjectType.igDimension:
                            SolidEdgeFrameworkSupport.Dimension dimension = (SolidEdgeFrameworkSupport.Dimension)property;
                            partProperty = new PartDimension(dimension, unitesOfMeasure);
                            break;
                            // Currently only supporting dimension properties
                            // case SolidEdgeFramework.ObjectType.igVariable:
                            // SolidEdgeFramework.variable variable = (SolidEdgeFramework.variable)property;
                            // partProperty = new PartVariable(variable, unitesOfMeasure);
                            // break;
                    }
                    if (partProperty != null && partProperty.displayForConfiguration()) {
                        this.partPropertyBindingSource.Add(partProperty);
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            this.edgeBar.Panel2Collapsed = false;
        }
    }
}
