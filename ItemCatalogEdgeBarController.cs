using System;
using System.Configuration;
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
        private Configuration myDllConfig;
        private string rootFolderPath;
        private SolidEdgeFramework.Application application;
        private SolidEdgeAssembly.AssemblyDocument assemblyDocument;
        private SolidEdgePart.PartDocument partDocument;

        public ItemCatalogEdgeBarController() {
            //Open the configuration file using the dll location
            this.myDllConfig = ConfigurationManager.OpenExeConfiguration(this.GetType().Assembly.Location);
            KeyValueConfigurationCollection settings = ((AppSettingsSection)this.myDllConfig.Sections["appSettings"]).Settings;
            if (settings.Count == 0) {
                System.Diagnostics.Debug.WriteLine("AppSettings is empty.");
            } else {
                foreach (string key in settings.AllKeys) {
                    System.Diagnostics.Debug.WriteLine("Key: {0} Value: {1}", key, settings[key].Value);
                }
            }
            // Set root folder for mother parts
            this.rootFolderPath = settings["motherPartFolder"].Value;

            InitializeComponent();
        }

        private void controllerLoad(object sender, EventArgs e) {
            // Trick to use the default system font.
            this.Font = SystemFonts.MessageBoxFont;
        }

        private void controllerAfterInitialize(object sender, EventArgs e) {
            // These properties are not initialized until AfterInitialize is called.
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
            //((SolidEdgeFramework.ISEApplicationEvents_Event)this.application.ApplicationEvents).AfterWindowActivate += test;
            this.currentDirectory.Text = this.rootFolderPath;
        }

        private void partLibrary_SelectedIndexChanged(object sender, EventArgs e) {
            System.Diagnostics.Debug.WriteLine("partLibrary_SelectedIndexChanged: Documents.Count: " + this.application.Documents.Count);
            foreach (ListViewItem item in this.partLibrary.SelectedItems) {
                if (item.Text != null && File.Exists(this.currentDirectory.Text + "\\" + item.Text)) { // Display preview of part
                    // TODO: display preview of selected part
                    this.showConfigurationContainer(this.currentDirectory.Text + "\\" + item.Text);
                } else {
                    this.hideConfigurationContainer();
                }
            }
        }

        private void partLibrary_DoubleClick(object sender, EventArgs e) {
            System.Diagnostics.Debug.WriteLine("partLibrary_DoubleClick: Documents.Count: " + this.application.Documents.Count);
            foreach (ListViewItem item in this.partLibrary.SelectedItems) {
                if (item.Text != null && Directory.Exists(this.currentDirectory.Text + "\\" + item.Text)) {
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
            KeyValueConfigurationCollection settings = ((AppSettingsSection)this.myDllConfig.Sections["appSettings"]).Settings;
            // Handles naming convention when it is not followed
            int length = this.partDocument.Name.IndexOf(settings["fileNameToReplace"].Value);
            if (length <= 0) {
                length = this.partDocument.Name.Length - 4;
            }
            string fileName = this.partDocument.Name.Substring(0, length);
            foreach (PartProperty partProperty in this.partPropertyBindingSource) {
                fileName += ", " + partProperty.Name + "=" + partProperty.Value + partProperty.Units;
            }
            // Remove invalid fileName characters
            fileName = string.Join("", fileName.Split(Path.GetInvalidFileNameChars()));
            string fullPathName = this.assemblyDocument.Path + "\\" + fileName + ".par";
            this.partDocument.SaveCopyAs(fullPathName);
            this.hideConfigurationContainer();
            this.assemblyDocument.Occurrences.AddWithTransform(fullPathName, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
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
                KeyValueConfigurationCollection settings = ((AppSettingsSection)this.myDllConfig.Sections["variables"]).Settings;
                if (settings.Count == 0) {
                    System.Diagnostics.Debug.WriteLine("AppSettings is empty.");
                } else {
                    foreach (string key in settings.AllKeys) {
                        System.Diagnostics.Debug.WriteLine("Key: {0} Value: {1}", key, settings[key].Value);
                    }
                }
                this.clearPartDocument();
                // Create a new instance of PropertySets.
                // SolidEdgeFileProperties.PropertySets objPropertySets = new SolidEdgeFileProperties.PropertySets();
                // objPropertySets.Open(filePath, true);
                // foreach (SolidEdgeFileProperties.Properties objProperties in objPropertySets) {
                    // foreach (SolidEdgeFileProperties.Property objProperty in objProperties) {
                        // System.Diagnostics.Debug.WriteLine("showConfigurationContainer: prop: " + objProperty.Name + " = " + objProperty.Value);
                    // }
                // }
                // objPropertySets.Close();

                this.partDocument = (SolidEdgePart.PartDocument)this.application.Documents.Open(filePath, "8");
                // this.partDocument = (SolidEdgePart.PartDocument)this.application.Documents.AddPartDocument(filePath);
                SolidEdgeFramework.VariableList variableList = (SolidEdgeFramework.VariableList)partDocument.GetVariables().Query("*", SolidEdgeConstants.VariableNameBy.seVariableNameByBoth, SolidEdgeConstants.VariableVarType.SeVariableVarTypeBoth, false);
                PartProperty partProperty;
                foreach (var property in variableList) {
                    partProperty = null;
                    // Determine the runtime type of the object.
                    Type type = property.GetType();
                    SolidEdgeFramework.ObjectType objectType = (SolidEdgeFramework.ObjectType)type.InvokeMember("Type", System.Reflection.BindingFlags.GetProperty, null, property, null);
                    switch (objectType) {
                        case SolidEdgeFramework.ObjectType.igDimension:
                            SolidEdgeFrameworkSupport.Dimension dimension = (SolidEdgeFrameworkSupport.Dimension)property;
                            partProperty = new PartDimension(dimension, this.partDocument.UnitsOfMeasure);
                            break;
                            // Currently only supporting dimension properties
                            // case SolidEdgeFramework.ObjectType.igVariable:
                            // SolidEdgeFramework.variable variable = (SolidEdgeFramework.variable)property;
                            // partProperty = new PartVariable(variable, unitesOfMeasure);
                            // break;
                    }
                    if (partProperty != null && settings[partProperty.Name] != null && partProperty.displayForConfiguration()) {
                        this.partPropertyBindingSource.Add(partProperty);
                    }
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("showConfigurationContainer: " + ex.Message);
            }
            this.edgeBar.Panel2Collapsed = false;
        }

        private void partLibrary_ItemDrag(object sender, ItemDragEventArgs e) {
            System.Diagnostics.Debug.WriteLine("partLibrary_ItemDrag: e.Item: " + e.Item);
            foreach (ListViewItem item in this.partLibrary.SelectedItems) {
                if (item.Text != null && File.Exists(this.currentDirectory.Text + "\\" + item.Text)) {
                    this.partLibrary.DoDragDrop(@"" + this.currentDirectory.Text + "\\" + item.Text, DragDropEffects.Copy);
                    this.assemblyDocument.Activate();
                }
            }
        }
    }
}
