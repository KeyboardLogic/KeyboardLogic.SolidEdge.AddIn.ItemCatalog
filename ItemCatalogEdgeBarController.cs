using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using SolidEdgeCommunity.AddIn;
using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System.IO;
using log4net;
using System.Reflection;

namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    public partial class ItemCatalogEdgeBarController : EdgeBarControl {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Configuration myDllConfig;
        private string rootFolderPath;
        private string currentPath;
        private SolidEdgeFramework.Application application;
        private SolidEdgePart.PartDocument partDocument;

        public ItemCatalogEdgeBarController() {
            //Open the configuration file using the dll location
            this.myDllConfig = ConfigurationManager.OpenExeConfiguration(this.GetType().Assembly.Location);
            KeyValueConfigurationCollection settings = ((AppSettingsSection)this.myDllConfig.Sections["appSettings"]).Settings;
            if (settings.Count == 0) {
                log.Info("AppSettings is empty.");
            } else {
                foreach (string key in settings.AllKeys) {
                    log.Info("Key: " + key + " Value: " + settings[key].Value);
                }
            }
            // Set root folder for mother parts
            this.rootFolderPath = settings["motherPartFolder"].Value;
            this.currentPath = this.rootFolderPath;

            InitializeComponent();
            log.Info("Item Catalog Loaded");
        }

        private void ControllerLoad(object sender, EventArgs e) {
            // Trick to use the default system font.
            this.Font = SystemFonts.MessageBoxFont;
        }

        private void ControllerAfterInitialize(object sender, EventArgs e) {
            log.Info("After Initilization: Started");
            // These properties are not initialized until AfterInitialize is called.
            if (partLibraryImageList.Images.ContainsKey(".par") == false) {
                Icon icon = IconTools.GetIconForExtension(".par", ShellIconSize.SmallIcon);
                if (icon != null) {
                    partLibraryImageList.Images.Add(icon);
                    partLibraryImageList.Images.SetKeyName(1, ".par");
                }
            }
            this.HideConfigurationContainer();
            // Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register();
            // Connect to or start Solid Edge.
            this.application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);
            
            this.currentDirectory.Text = this.currentPath.Substring(this.currentPath.LastIndexOf("\\") + 1);
            this.UpdateDirectories();
            log.Info("AFter Initilization: Complete");
        }

        private void PartLibrary_SelectedIndexChanged(object sender, EventArgs e) {
            log.Info("Documents.Count: " + this.application.Documents.Count);
            foreach (ListViewItem item in this.partLibrary.SelectedItems) {
                if (item.Text != null && File.Exists(this.currentPath + "\\" + item.Text)) { // Display preview of part
                    // TODO: display preview of selected part
                    this.ShowConfigurationContainer(this.currentPath + "\\" + item.Text);
                } else {
                    this.HideConfigurationContainer();
                }
            }
        }

        private void PartLibrary_DoubleClick(object sender, EventArgs e) {
            log.Info("Documents.Count: " + this.application.Documents.Count);
            foreach (ListViewItem item in this.partLibrary.SelectedItems) {
                if (item.Text != null && Directory.Exists(this.currentPath + "\\" + item.Text)) {
                    this.currentPath += "\\" + item.Text;
                    this.currentDirectory.Text = item.Text;
                    this.UpdateDirectories();
                } else if (item.Text != null && File.Exists(this.currentPath + "\\" + item.Text)) {
                    this.ShowConfigurationContainer(this.currentPath + "\\" + item.Text);
                    this.partProperties.Focus();
                    this.partProperties.BeginEdit(true);
                }
            }
        }

        private void BackButton_Click(object sender, EventArgs e) {
            int index = this.currentPath.LastIndexOf("\\");
            if (index >= this.rootFolderPath.Length) {
                string[] folders = this.currentPath.Split('\\');
                this.currentDirectory.Text = folders[folders.Length-2];
                this.currentPath = this.currentPath.Substring(0, index);
                this.UpdateDirectories();
            }
        }

        private void UpdateDirectories() {
            this.partLibrary.Items.Clear();
            foreach (string str in Directory.GetDirectories(this.currentPath)) {
                this.partLibrary.Items.Add(Path.GetFileName(str), 0);
            }
            foreach (string str in Directory.GetFiles(this.currentPath, "*.par")) {
                this.partLibrary.Items.Add(Path.GetFileName(str), 1);
            }
        }

        private void OkButton_Click(object sender, EventArgs e) {
            KeyValueConfigurationCollection settings = ((AppSettingsSection)this.myDllConfig.Sections["appSettings"]).Settings;
            // Get a refrence to the active assembly document.
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);
            log.Debug("assemblyDocument: " + assemblyDocument.Name + " path: " + assemblyDocument.Path);
            if (assemblyDocument.Path == null || assemblyDocument.Path == "") {
                MessageBox.Show("You must save the current assembly before adding parts");
            } else {
                log.Debug("partDocument: " + this.partDocument.Name);
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
                log.Debug("fileName: " + fileName);
                string fullPathName = assemblyDocument.Path;
                string assemblyPartFolderName = settings["assemblyPartFolderName"].Value;
                if (assemblyPartFolderName != null & assemblyPartFolderName != "") {
                    assemblyPartFolderName = string.Join("", assemblyPartFolderName.Split(Path.GetInvalidFileNameChars()));
                    fullPathName += "\\" + assemblyPartFolderName;
                    Directory.CreateDirectory(fullPathName);
                }
                fullPathName += "\\" + fileName + ".par";
                log.Debug("fullPathName: " + fullPathName);
                this.partDocument.SaveCopyAs(fullPathName);
                this.HideConfigurationContainer();
                assemblyDocument.Occurrences.AddWithTransform(fullPathName, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
                this.application.DoIdle();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            this.HideConfigurationContainer();
        }

        private void ClearPartDocument() {
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
        private void HideConfigurationContainer() {
            this.edgeBar.Panel2Collapsed = true;
            this.ClearPartDocument();
        }

        /// <summary>
        /// Displays the configuration container used for changing values associated with part properties
        /// </summary>
        private void ShowConfigurationContainer(string filePath) {
            try {
                KeyValueConfigurationCollection settings = ((AppSettingsSection)this.myDllConfig.Sections["variables"]).Settings;
                if (settings.Count == 0) {
                    log.Info("AppSettings is empty.");
                } else {
                    foreach (string key in settings.AllKeys) {
                        log.Info("Key: " + key + " Value: " + settings[key].Value);
                    }
                }
                this.ClearPartDocument();
                this.partDocument = (SolidEdgePart.PartDocument)this.application.Documents.Open(filePath, "8");
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
                this.edgeBar.Panel2Collapsed = false;
            } catch (Exception ex) {
                log.Error(ex.Message);
            }
        }

        private void PartLibrary_ItemDrag(object sender, ItemDragEventArgs e) {
            log.Info("e.Item: " + e.Item);
            foreach (ListViewItem item in this.partLibrary.SelectedItems) {
                if (item.Text != null && File.Exists(this.currentDirectory.Text + "\\" + item.Text)) {
                    this.partLibrary.DoDragDrop(@"" + this.currentDirectory.Text + "\\" + item.Text, DragDropEffects.Copy);
                    SolidEdgeAssembly.AssemblyDocument assemblyDocument = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);
                    log.Debug("assemblyDocument: " + assemblyDocument.Name);
                    log.Debug("partDocument: " + this.partDocument.Name);
                    assemblyDocument.Activate();
                }
            }
        }

        private void partProperties_Enter(object sender, EventArgs e) {
            log.Info("Configure the part");
        }

        private void partProperties_CurrentCellChanged(object sender, EventArgs e) {
            // log.Info("Current Cell: " + this.partProperties.CurrentCell);
        }
    }
}
