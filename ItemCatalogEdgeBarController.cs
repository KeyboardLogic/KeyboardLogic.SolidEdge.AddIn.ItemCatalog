using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SolidEdgeCommunity.AddIn;
using log4net;

namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    public partial class ItemCatalogEdgeBarController : EdgeBarControl {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Configuration myDllConfig;
        private string rootFolderPath;
        private string currentPath;
        private string filePath;

        public ItemCatalogEdgeBarController() {
            // Open the configuration file using the dll location
            this.myDllConfig = ConfigurationManager.OpenExeConfiguration(this.GetType().Assembly.Location);
            KeyValueConfigurationCollection settings = ((AppSettingsSection)this.myDllConfig.Sections["appSettings"]).Settings;
            if (settings.Count == 0) {
                log.Warn("AppSettings is empty.");
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
            this.currentDirectory.Text = this.currentPath.Substring(this.currentPath.LastIndexOf("\\") + 1);
            this.UpdateDirectories();
            log.Info("After Initilization: Complete");
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

        /// <summary>
        /// Displays the configuration container used for changing values associated with part properties
        /// </summary>
        private void HideConfigurationContainer() {
            this.edgeBar.Panel2Collapsed = true;
            this.partPropertyBindingSource.Clear();
        }

        /// <summary>
        /// Displays the configuration container used for changing values associated with part properties
        /// </summary>
        private void ShowConfigurationContainer(string filePath) {
            log.Debug("filePath: " + filePath);
            this.filePath = filePath;
            KeyValueConfigurationCollection settings = ((AppSettingsSection)this.myDllConfig.Sections["variables"]).Settings;
            this.partPropertyBindingSource.Clear();
            // TODO: fix crashing issue from heap corruption when opening part document
            SolidEdgePart.PartDocument partDocument = (SolidEdgePart.PartDocument)this.Document.Application.Documents.Open(filePath);//, "8");
            this.Document.Application.DoIdle();
            // Get a reference to the Variables collection.
            SolidEdgeFramework.Variables variables = (SolidEdgeFramework.Variables)partDocument.Variables;
            // Get a reference to the variablelist.
            SolidEdgeFramework.VariableList variableList = (SolidEdgeFramework.VariableList)variables.Query("*", SolidEdgeConstants.VariableNameBy.seVariableNameByBoth, SolidEdgeConstants.VariableVarType.SeVariableVarTypeBoth, false);
            Marshal.FinalReleaseComObject(variables);
            PartProperty partProperty = null;
            foreach (var property in variableList) {
                partProperty = new PartProperty(property, partDocument.UnitsOfMeasure);
                if (settings[partProperty.Name] != null) {
                    log.Debug("property: " + partProperty.Name);
                    this.partPropertyBindingSource.Add(partProperty);
                }
            }
            Marshal.FinalReleaseComObject(variableList);
            // TODO: figure out what causes this to throw "Exception thrown: 'System.MissingMemberException' in Microsoft.VisualBasic.dll"
            partDocument.Close(false);
            log.Info("this.Document.Name: " + this.Document.Name);
            this.Document.Application.DoIdle();
            Marshal.FinalReleaseComObject(partDocument);
            partDocument = null;
            this.edgeBar.Panel2Collapsed = false;
        }

        private void OkButton_Click(object sender, EventArgs e) {
            KeyValueConfigurationCollection settings = ((AppSettingsSection)this.myDllConfig.Sections["appSettings"]).Settings;
            log.Debug("assemblyDocument: " + this.Document.Name + " path: " + this.Document.Path);
            if (this.Document.Path == null || this.Document.Path == "") {
                MessageBox.Show("You must save the current assembly before adding parts");
            } else {
                // TODO: fix crashing issue from heap corruption when opening part document
                SolidEdgePart.PartDocument partDocument = (SolidEdgePart.PartDocument)this.Document.Application.Documents.Open(this.filePath);//, "8");
                this.Document.Application.DoIdle();
                log.Debug("partDocument: " + partDocument.Name);
                // Handles naming convention when it is not followed
                int length = partDocument.Name.IndexOf(settings["fileNameToReplace"].Value);
                if (length <= 0) {
                    length = partDocument.Name.Length - 4;
                }
                string fileName = partDocument.Name.Substring(0, length);
                KeyValueConfigurationCollection partNameSettings = ((AppSettingsSection)this.myDllConfig.Sections["partName"]).Settings;
                foreach (PartProperty partProperty in this.partPropertyBindingSource) {
                    if (partNameSettings[partProperty.Name] != null && partProperty.Value != 0) {
                        // Get a reference to the Variables collection.
                        SolidEdgeFramework.Variables variables = (SolidEdgeFramework.Variables)partDocument.Variables;
                        // Get a reference to the variablelist.
                        SolidEdgeFramework.VariableList variableList = (SolidEdgeFramework.VariableList)variables.Query(partProperty.Name, SolidEdgeConstants.VariableNameBy.seVariableNameByBoth, SolidEdgeConstants.VariableVarType.SeVariableVarTypeBoth, false);
                        foreach (var property in variableList) {
                            try {
                                Type type = property.GetType();
                                SolidEdgeFramework.ObjectType objectType = (SolidEdgeFramework.ObjectType)type.InvokeMember("Type", System.Reflection.BindingFlags.GetProperty, null, property, null);
                                switch (objectType) {
                                    case SolidEdgeFramework.ObjectType.igDimension:
                                        SolidEdgeFrameworkSupport.Dimension dimension = (SolidEdgeFrameworkSupport.Dimension)property;
                                        dimension.Value = partProperty.GetSolidEdgeStoredValue(partDocument.UnitsOfMeasure);
                                        log.Info("Value: " + dimension.Value);
                                        Marshal.FinalReleaseComObject(dimension);
                                        break;
                                    case SolidEdgeFramework.ObjectType.igVariable:
                                        SolidEdgeFramework.variable variable = (SolidEdgeFramework.variable)property;
                                        variable.Value = partProperty.GetSolidEdgeStoredValue(partDocument.UnitsOfMeasure);
                                        log.Info("Value: " + variable.Value);
                                        Marshal.FinalReleaseComObject(variable);
                                        break;
                                }
                                //TODO: update partDocument values before saving
                                fileName += "_" + partProperty.Value;
                            } catch (Exception ex) {
                                log.Error(ex.Message);
                            }
                        }
                        Marshal.FinalReleaseComObject(variables);
                        Marshal.FinalReleaseComObject(variableList);
                    }
                }
                // Remove invalid fileName characters
                fileName = string.Join("", fileName.Split(Path.GetInvalidFileNameChars()));
                log.Debug("fileName: " + fileName);
                string fullPathName = this.Document.Path;
                string assemblyPartFolderName = settings["assemblyPartFolderName"].Value;
                if (assemblyPartFolderName != null & assemblyPartFolderName != "") {
                    assemblyPartFolderName = string.Join("", assemblyPartFolderName.Split(Path.GetInvalidFileNameChars()));
                    fullPathName += "\\" + assemblyPartFolderName;
                    Directory.CreateDirectory(fullPathName);
                }
                fullPathName += "\\" + fileName + ".par";
                log.Debug("fullPathName: " + fullPathName);
                //fullPathName = partDocument.FullName;
                partDocument.SaveCopyAs(fullPathName);
                this.Document.Application.DoIdle();
                partDocument.Close(false);
                this.Document.Application.DoIdle();
                Marshal.FinalReleaseComObject(partDocument);
                this.HideConfigurationContainer();
                int count = ((SolidEdgeAssembly.AssemblyDocument)this.Document).Occurrences.Count;
                if (count > 0) {
                    SolidEdgeAssembly.Occurrence occurrence = ((SolidEdgeAssembly.AssemblyDocument)this.Document).Occurrences.Item(count);
                    occurrence.GetTransform(out double OriginX, out double OriginY, out double OriginZ, out double AngleX, out double AngleY, out double AngleZ);
                    Marshal.ReleaseComObject(occurrence);
                    double offset = 0.05;
                    occurrence = ((SolidEdgeAssembly.AssemblyDocument)this.Document).Occurrences.AddWithTransform(fullPathName, OriginX + offset, OriginY + offset, OriginZ + offset, AngleX, AngleY, AngleZ);
                    // delete grounded relationship from occurrence.Relations3d
                    Marshal.FinalReleaseComObject(occurrence);
                } else {
                    ((SolidEdgeAssembly.AssemblyDocument)this.Document).Occurrences.AddWithTransform(fullPathName, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            this.HideConfigurationContainer();
        }

        private void BackButton_Click(object sender, EventArgs e) {
            int index = this.currentPath.LastIndexOf("\\");
            if (index >= this.rootFolderPath.Length) {
                string[] folders = this.currentPath.Split('\\');
                this.currentDirectory.Text = folders[folders.Length - 2];
                this.currentPath = this.currentPath.Substring(0, index);
                this.UpdateDirectories();
            }
        }

        /**
        private void Mouse_MouseUp(short sButton, short sShift, double dX, double dY, double dZ, object pWindowDispatch, int lKeyPointType, object pGraphicDispatch) {
            // Note: Thread.CurrentThread.IsBackground = true so we must Invoke a call back to the main GUI thread.
            log.Info("MouseUp: " + sButton + ", " + sShift + ", " + dX + ", " + dY + ", " + dZ + ", " + pWindowDispatch + ", " + lKeyPointType + ", " + pGraphicDispatch);
            //this.OkButton_Click(null, null);
        }

        private void PartLibrary_ItemDrag(object sender, ItemDragEventArgs e) {
            log.Info("e.Item: " + e.Item);
            // Attempt to drag and drop part into diagram
            SolidEdgeFramework.Command _command = this.Document.Application.CreateCommand((int)SolidEdgeConstants.seCmdFlag.seNoDeactivate);
            _command.Start();
            SolidEdgeFramework.Mouse _mouse = _command.Mouse;
            _mouse.LocateMode = (int)SolidEdgeConstants.seLocateModes.seLocateQuickPick;
            _mouse.EnabledMove = true;
            _mouse.EnabledDrag = true;
            _mouse.ScaleMode = 1;   // Design model coordinates.
            _mouse.WindowTypes = 1; // Graphic window's only.
            _mouse.MouseUp += Mouse_MouseUp;
        }
        **/

        private void PartLibrary_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            log.Debug("Documents.Count: " + this.Document.Application.Documents.Count);
            log.Info("Item Selection Changed To: " + e.Item.Text);
            if (e.Item.Text != null && File.Exists(this.currentPath + "\\" + e.Item.Text)) { // Display preview of part
                // TODO: display preview of selected part
                this.ShowConfigurationContainer(this.currentPath + "\\" + e.Item.Text);
            } else {
                this.HideConfigurationContainer();
            }
        }

        private void PartLibrary_DoubleClick(object sender, EventArgs e) {
            log.Debug("DoubleClick: Documents.Count: " + this.Document.Application.Documents.Count);
            foreach (ListViewItem item in this.partLibrary.SelectedItems) {
                if (item.Text != null && Directory.Exists(this.currentPath + "\\" + item.Text)) {
                    this.currentPath += "\\" + item.Text;
                    this.currentDirectory.Text = item.Text;
                    this.UpdateDirectories();
                } else if (item.Text != null && File.Exists(this.currentPath + "\\" + item.Text)) {
                    this.ShowConfigurationContainer(this.currentPath + "\\" + item.Text);
                }
            }
        }

        private void PartProperties_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (Char)Keys.Enter) {
                this.OkButton_Click(sender, e);
            }
        }

        private void PartProperties_SelectionChanged(object sender, EventArgs e) {
            log.Info(e.GetType() + ": " + e.ToString());
            try {
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
            } catch (Exception ex) {
                log.Error(ex.Message);
            }
        }
    }
}
