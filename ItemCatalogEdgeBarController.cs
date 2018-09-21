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
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Settings _settings;
        private string _currentPath;
        private string _filePath;

        public ItemCatalogEdgeBarController() {
            Log.Info("SolidEdge Version # " + SolidEdgeCommunity.SolidEdgeUtils.GetVersion());
            // Open the configuration file using the dll location
            this._settings = new Settings(ConfigurationManager.OpenExeConfiguration(this.GetType().Assembly.Location));
            this._currentPath = this._settings.getRootFolder();
            Log.Info("rootFolderPath: " + this._settings.getRootFolder());
            InitializeComponent();
        }

        /// <summary>
        /// Used the default system font.
        /// </summary>
        private void ControllerLoad(object sender, EventArgs e) {
            this.Font = SystemFonts.MessageBoxFont;
        }

        private void ControllerAfterInitialize(object sender, EventArgs e) {
            Log.Info("After Initialization: Started");
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
            this.currentDirectory.Text = this._currentPath.Substring(this._currentPath.LastIndexOf("\\") + 1);
            this.UpdateDirectories();
            Log.Info("After Initialization: Complete");
        }

        private void UpdateDirectories() {
            this.partLibrary.Items.Clear();
            foreach (var str in Directory.GetDirectories(this._currentPath)) {
                this.partLibrary.Items.Add(Path.GetFileName(str), 0);
            }
            foreach (var str in Directory.GetFiles(this._currentPath, "*.par")) {
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
            try {
                Log.Debug("filePath: " + filePath);
                this._filePath = filePath;
                this.partPropertyBindingSource.Clear();
                SolidEdgePart.PartDocument partDocument = (SolidEdgePart.PartDocument)this.Document.Application.Documents.Open(filePath, 0x00000008);//, "8");
                this.Document.Application.DoIdle();
                // Get a reference to the Variables collection.
                SolidEdgeFramework.Variables variables = (SolidEdgeFramework.Variables)partDocument.Variables;
                // Get a reference to the variableList.
                SolidEdgeFramework.VariableList variableList = (SolidEdgeFramework.VariableList)variables.Query("*", SolidEdgeConstants.VariableNameBy.seVariableNameByBoth, SolidEdgeConstants.VariableVarType.SeVariableVarTypeBoth, false);
                Marshal.FinalReleaseComObject(variables);
                PartProperty partProperty = null;                
                foreach (var property in variableList) {
                    partProperty = new PartProperty(property, partDocument.UnitsOfMeasure);
                    if (this._settings.isVariableDefined(partProperty.Name)) {
                        Log.Debug("partProperty: " + partProperty.Name);
                        this.partPropertyBindingSource.Add(partProperty);
                    }
                }
                Marshal.FinalReleaseComObject(variableList);
                // TODO: figure out what causes this to throw "Exception thrown: 'System.MissingMemberException' in Microsoft.VisualBasic.dll"
                partDocument.Close(false);
                Log.Debug("this.Document.Name: " + this.Document.Name);
                this.Document.Application.DoIdle();
                Marshal.FinalReleaseComObject(partDocument);
                this.edgeBar.Panel2Collapsed = false;
            } catch (Exception ex) {
                Log.Error("Unable to open solid edge partDocument | " + ex.Message);
            }
            
        }

        private void OkButton_Click(object sender, EventArgs e) {
            Log.Debug("assemblyDocument: " + this.Document.Name + " path: " + this.Document.Path);
            if (string.IsNullOrEmpty(this.Document.Path)) {
                MessageBox.Show("You must save the current assembly before adding parts");
            } else {
                SolidEdgePart.PartDocument partDocument = (SolidEdgePart.PartDocument)this.Document.Application.Documents.Open(this._filePath, 0x00000008);//, "8");
                this.Document.Application.DoIdle();
                Log.Debug("partDocument: " + partDocument.Name);
                // Get file properties of the partDocument
                SolidEdgeFramework.Properties objProperties = ((SolidEdgeFramework.PropertySets)partDocument.Properties).Item("Custom");
                // Handles naming convention when it is not followed
                int length = partDocument.Name.IndexOf(".par");
                if (length <= 0) {
                    Log.Warn("selected file is not a Part Document");
                    length = partDocument.Name.Length;
                }
                string fileName = partDocument.Name.Substring(0, length);
                foreach (PartProperty partProperty in this.partPropertyBindingSource) {
                    if (this._settings.isVariableDefined(partProperty.Name)) {
                        // Get a reference to the Variables collection.
                        SolidEdgeFramework.Variables variables = (SolidEdgeFramework.Variables)partDocument.Variables;
                        // Get a reference to the variableList.
                        SolidEdgeFramework.VariableList variableList = (SolidEdgeFramework.VariableList)variables.Query(partProperty.Name, SolidEdgeConstants.VariableNameBy.seVariableNameByBoth, SolidEdgeConstants.VariableVarType.SeVariableVarTypeBoth, false);
                        foreach (var property in variableList) {
                            try {
                                Type type = property.GetType();
                                SolidEdgeFramework.ObjectType objectType = (SolidEdgeFramework.ObjectType)type.InvokeMember("Type", System.Reflection.BindingFlags.GetProperty, null, property, null);
                                switch (objectType) {
                                    case SolidEdgeFramework.ObjectType.igDimension:
                                        SolidEdgeFrameworkSupport.Dimension dimension = (SolidEdgeFrameworkSupport.Dimension)property;
                                        dimension.Value = partProperty.GetSolidEdgeStoredValue(partDocument.UnitsOfMeasure);
                                        Marshal.FinalReleaseComObject(dimension);
                                        break;
                                    case SolidEdgeFramework.ObjectType.igVariable:
                                        SolidEdgeFramework.variable variable = (SolidEdgeFramework.variable)property;
                                        variable.Value = partProperty.GetSolidEdgeStoredValue(partDocument.UnitsOfMeasure);
                                        Marshal.FinalReleaseComObject(variable);
                                        break;
                                }
                                // TODO: allow for order of appended values to be determined (L_x before B_x)
                                fileName += this._settings.getFileNameSeparator() + partProperty.Value + " " + partProperty.Units;
                                // Update file property
                                try {
                                    // TODO: Fix exception when partProperty.Name is not defined in objProperties.Item
                                    SolidEdgeFramework.Property objProperty = objProperties.Item(partProperty.Name);
                                    objProperty.set_Value(partProperty.Value.ToString());
                                    Marshal.FinalReleaseComObject(objProperty);
                                } catch (Exception ex) {
                                    Log.Warn("no file property exists for " + partProperty.Name + " | " + ex.Message);
                                }
                            } catch (Exception ex) {
                                Log.Error(ex.Message);
                            }
                        }
                        Marshal.FinalReleaseComObject(variables);
                        Marshal.FinalReleaseComObject(variableList);
                    }
                }
                objProperties.Save();
                Marshal.FinalReleaseComObject(objProperties);
                // Remove invalid fileName characters
                fileName = string.Join("", fileName.Split(Path.GetInvalidFileNameChars()));
                Log.Debug("fileName: " + fileName);
                string fullPathName = this.Document.Path;
                string assemblyPartFolderName = _settings.getAssemblyPartFolderName();
                if (assemblyPartFolderName != null & assemblyPartFolderName != "") {
                    assemblyPartFolderName = string.Join("", assemblyPartFolderName.Split(Path.GetInvalidFileNameChars()));
                    fullPathName += "\\" + assemblyPartFolderName;
                    Directory.CreateDirectory(fullPathName);
                }
                fullPathName += "\\" + fileName + ".par";
                Log.Debug("fullPathName: " + fullPathName);

                if (!File.Exists(fullPathName)) {
                    partDocument.SaveCopyAs(fullPathName);
                }
                
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
                    SolidEdgeAssembly.Relations3d relation3d = (SolidEdgeAssembly.Relations3d)occurrence.Relations3d;
                    foreach (var relation in relation3d) {
                        Type type = relation.GetType();
                        // Get the value of the Type proprety via Reflection
                        SolidEdgeFramework.ObjectType objectType = (SolidEdgeFramework.ObjectType)type.InvokeMember("Type", BindingFlags.GetProperty, null, relation, null);
                        // Determine the relation type
                        switch (objectType) {
                            case SolidEdgeFramework.ObjectType.igGroundRelation3d:
                                SolidEdgeAssembly.GroundRelation3d groundRelation3d = (SolidEdgeAssembly.GroundRelation3d)relation;
                                groundRelation3d.Delete();
                                break;
                        }
                    }
                    Marshal.FinalReleaseComObject(relation3d);
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
            int index = this._currentPath.LastIndexOf("\\");
            if (index >= this._settings.getRootFolder().Length) {
                string[] folders = this._currentPath.Split('\\');
                this.currentDirectory.Text = folders[folders.Length - 2];
                this._currentPath = this._currentPath.Substring(0, index);
                this.UpdateDirectories();
            }
        }

        #region PartLibrary
        private void PartLibrary_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            Log.Debug("ItemSelectionChanged: Documents.Count: " + this.Document.Application.Documents.Count);
            Log.Info("ItemSelectionChanged: e.Item.Text: " + e.Item.Text);
            if (e.Item.Text != null && File.Exists(this._currentPath + "\\" + e.Item.Text)) { // Display preview of part
                // TODO: display preview of selected part
                this.ShowConfigurationContainer(this._currentPath + "\\" + e.Item.Text);
            } else {
                this.HideConfigurationContainer();
            }
        }

        private void PartLibrary_DoubleClick(object sender, EventArgs e) {
            Log.Debug("DoubleClick: Documents.Count: " + this.Document.Application.Documents.Count);
            Log.Debug("DoubleClick: currentPath" + this._currentPath);
            foreach (ListViewItem item in this.partLibrary.SelectedItems) {
                Log.Info("DoubleClick: Item.Text: " + item.Text);
                if (item.Text != null && Directory.Exists(this._currentPath + "\\" + item.Text)) {
                    this._currentPath += "\\" + item.Text;
                    this.currentDirectory.Text = item.Text;
                    this.UpdateDirectories();
                    this.HideConfigurationContainer();
                } else if (item.Text != null && File.Exists(this._currentPath + "\\" + item.Text)) {
                    this.ShowConfigurationContainer(this._currentPath + "\\" + item.Text);
                    this.partProperties.BeginEdit(true);
                }
            }
        }

        private void PartLibrary_MouseDown(object sender, MouseEventArgs e) {
            Log.Debug("PartLibrary_MouseDown: filePath: " + this._filePath);
            if (this._filePath != null) {
                DataObject dataObject = new DataObject();
                String[] seFile = new String[] { this._filePath };
                // Commented out to fix issue with code crashing
                // Cannot DoDragDrop without causing DoubleClick becoming disabled
                // TODO: fix drag and drop
//                dataObject.SetData(DataFormats.FileDrop, seFile);
//                this.partLibrary.DoDragDrop(dataObject, DragDropEffects.All);
            }
        }
        #endregion

        #region PartProperties
        private void PartProperties_KeyPress(object sender, KeyPressEventArgs e) {
            switch (e.KeyChar) {
                case (char)Keys.Enter:
                    this.OkButton_Click(sender, e);
                    break;
                case (char)Keys.Tab:
                    PartProperties_SelectionChanged(sender, e);
                    break;
                default:
                    break;
            }
        }

        private void PartProperties_SelectionChanged(object sender, EventArgs e) {
            try {
                DataGridViewCell currentCell = this.partProperties.CurrentCell;
                Log.Debug("currentCell: " + currentCell);
                if (currentCell == null) {
                    // do nothing
                } else if (currentCell.ReadOnly) {
                    DataGridViewCell nextCell = null;
                    Boolean loopToBeginning = true;
                    int nextRow = currentCell.RowIndex;
                    int nextCol = currentCell.ColumnIndex;
                    while (nextCell == null || nextCell.ReadOnly) {
                        Log.Debug("nextCell: " + nextCell);
                        nextCol = nextCol + 1;
                        if (nextCol >= this.partProperties.ColumnCount) {
                            nextCol = 0;
                            nextRow++;
                        }
                        if (loopToBeginning && nextRow >= this.partProperties.RowCount) {
                            nextRow = 0;
                            loopToBeginning = false;
                        }
                        nextCell = this.partProperties.Rows[nextRow].Cells[nextCol];

                    }
                    this.partProperties.CurrentCell = nextCell;
                } else {
                    Log.Debug("currentCell is editable");
                }
            } catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        private void PartProperties_CellEnter(object sender, DataGridViewCellEventArgs e) {
            DataGridViewCell currentCell = this.partProperties.CurrentCell;
            Log.Debug("PartProperties_CellEnter: currentCell: " + currentCell);
            //SelectNextEditableCell();
        }
        #endregion
    }
}
