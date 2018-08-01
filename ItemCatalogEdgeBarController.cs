using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using SolidEdgeCommunity.AddIn;
using System.IO;
using log4net;
using System.Reflection;

namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    public partial class ItemCatalogEdgeBarController : EdgeBarControl {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Configuration myDllConfig;
        private string rootFolderPath;
        private string currentPath;
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
        }

        private void ControllerLoad(object sender, EventArgs e) {
            // Trick to use the default system font.
            this.Font = SystemFonts.MessageBoxFont;
        }

        private void ControllerAfterInitialize(object sender, EventArgs e) {
            log.Info("After Initilization: Started");
            // Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register();
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
            this.currentDirectory.Text = this.currentPath.Substring(this.currentPath.LastIndexOf("\\") + 1);
            this.UpdateDirectories();
            log.Info("AFter Initilization: Complete");
        }

        private void PartLibrary_SelectedIndexChanged(object sender, EventArgs e) {
            log.Debug("Documents.Count: " + this.Document.Application.Documents.Count);
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
            log.Debug("Documents.Count: " + this.Document.Application.Documents.Count);
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
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = (SolidEdgeAssembly.AssemblyDocument)this.Document;
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
                KeyValueConfigurationCollection partNameSettings = ((AppSettingsSection)this.myDllConfig.Sections["partName"]).Settings;
                foreach (PartProperty partProperty in this.partPropertyBindingSource) {
                    if (partNameSettings[partProperty.Name] != null && partProperty.Value != 0) {
                        //fileName += ", " + partProperty.Name + "=" + partProperty.Value + partProperty.Units;
                        fileName += "_" + partProperty.Value;
                    }
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
                int count = assemblyDocument.Occurrences.Count;
                if (count > 0) {
                    SolidEdgeAssembly.Occurrence occurrence = assemblyDocument.Occurrences.Item(count);
                    occurrence.GetTransform(out double OriginX, out double OriginY, out double OriginZ, out double AngleX, out double AngleY, out double AngleZ);
                    double offset = 0.05;
                    occurrence = assemblyDocument.Occurrences.AddWithTransform(fullPathName, OriginX + offset, OriginY + offset, OriginZ + offset, AngleX, AngleY, AngleZ);
                    //occurrence.Select(false);
                    //assemblyDocument.ClearCapturedRelationships();
                    //occurrence.ClearCapturedRelationships();
                } else {
                    assemblyDocument.Occurrences.AddWithTransform(fullPathName, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
                }
                //this.Document.Application.DoIdle();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            this.HideConfigurationContainer();
        }

        private void ClearPartDocument() {
            this.partPropertyBindingSource.Clear();
            if (this.partDocument != null) {
                //this.Document.Application.DoIdle();
                this.partDocument.Close(false);
                // Release our reference to the document.
                //Marshal.FinalReleaseComObject(this.partDocument);
                //this.Document.Application.DoIdle();
                this.partDocument = null;
            }
            log.Debug("Cleared PartDocument Info");
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
            log.Debug("filePath: " + filePath);
            try {
                KeyValueConfigurationCollection settings = ((AppSettingsSection)this.myDllConfig.Sections["variables"]).Settings;
                if (settings.Count == 0) {
                    log.Warn("AppSettings is empty.");
                } else {
                    foreach (string key in settings.AllKeys) {
                        log.Debug("Key: " + key + " Value: " + settings[key].Value);
                    }
                }
                this.ClearPartDocument();
                this.partDocument = (SolidEdgePart.PartDocument)this.Document.Application.Documents.Open(filePath, "8");
                //this.Document.Application.DoIdle();
                // Get a reference to the Variables collection.
                SolidEdgeFramework.Variables variables = (SolidEdgeFramework.Variables)this.partDocument.Variables;
                // Get a reference to the variablelist.
                SolidEdgeFramework.VariableList variableList = (SolidEdgeFramework.VariableList)variables.Query("*", SolidEdgeConstants.VariableNameBy.seVariableNameByBoth, SolidEdgeConstants.VariableVarType.SeVariableVarTypeDimension, false);
                PartProperty partProperty;
                foreach (var property in variableList) {
                    partProperty = null;
                    try {
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
                    } catch (Exception ex) {
                        log.Error(ex.Message);
                    }
                }
                this.edgeBar.Panel2Collapsed = false;
                //this.Document.Application.DoIdle();
            } catch (Exception ex) {
                log.Error(ex.Message);
            }
        }

        private void PartLibrary_ItemDrag(object sender, ItemDragEventArgs e) {
            log.Info("e.Item: " + e.Item);
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = (SolidEdgeAssembly.AssemblyDocument)this.Document;
            log.Debug("assemblyDocument: " + assemblyDocument.Name + " path: " + assemblyDocument.Path);

            // Attempt to drag and drop part into diagram
            /**
            SolidEdgeFramework.Command _command = this.Document.Application.CreateCommand((int)SolidEdgeConstants.seCmdFlag.seNoDeactivate);
            _command.Start();
            SolidEdgeFramework.Mouse _mouse = _command.Mouse;
            _mouse.LocateMode = (int)SolidEdgeConstants.seLocateModes.seLocateQuickPick;
            _mouse.EnabledMove = true;
            _mouse.EnabledDrag = true;
            _mouse.ScaleMode = 1;   // Design model coordinates.
            _mouse.WindowTypes = 1; // Graphic window's only.
            _mouse.MouseUp += Mouse_MouseUp;
            **/
        }

        private void Mouse_MouseUp(short sButton, short sShift, double dX, double dY, double dZ, object pWindowDispatch, int lKeyPointType, object pGraphicDispatch) {
            // Note: Thread.CurrentThread.IsBackground = true so we must Invoke a call back to the main GUI thread.
            log.Info("MouseUp: " + sButton + ", " + sShift + ", " + dX + ", " + dY + ", " + dZ + ", " + pWindowDispatch + ", " + lKeyPointType + ", " + pGraphicDispatch);
            this.OkButton_Click(null, null);
        }

        private void PartProperties_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (Char)Keys.Enter) {
                this.OkButton_Click(sender, e);
            } else if (e.KeyChar == (Char)Keys.Tab) {
                //this.PartProperties_SelectionChanged(sender, e);
            }
        }

        private void PartProperties_SelectionChanged(object sender, EventArgs e) {
            log.Info(e.GetType() + ": " + e.ToString());
            DataGridViewCell currentCell = this.partProperties.CurrentCell;
            if (currentCell != null) {
                log.Debug("currentCell: [" + currentCell.RowIndex + ", " + currentCell.ColumnIndex + "]");
                int nextRow = currentCell.RowIndex;
                int nextCol = currentCell.ColumnIndex;
                if (nextCol < 1) {
                    nextCol = 1;
                } else if (nextCol > 1) {
                    nextCol = 1;
                    nextRow++;
                }
                if (nextRow == this.partProperties.RowCount) {
                    nextRow = 0;
                }
                DataGridViewCell nextCell = this.partProperties.Rows[nextRow].Cells[nextCol];
                if (nextCell != null && nextCell.Visible) {
                    log.Debug("nextCell: [" + nextCell.RowIndex + ", " + nextCell.ColumnIndex + "]");
                    this.partProperties.CurrentCell = nextCell;
                }
            }
        }
    }
}
