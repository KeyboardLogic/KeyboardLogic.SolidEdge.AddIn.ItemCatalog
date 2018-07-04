using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods

namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    public partial class ConfigurationDialog : Form {
        // Connect to or start Solid Edge.
        private SolidEdgeFramework.Application application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);
        private SolidEdgeAssembly.AssemblyDocument assemblyDocument;
        private SolidEdgePart.PartDocument partDocument;
        private String originalFileName;
        
        public ConfigurationDialog(string path) {
            InitializeComponent();
            // Get a reference to the documents collection.
            SolidEdgeFramework.Documents documents = this.application.Documents;
            // Get a refrence to the active assembly document.
            this.assemblyDocument = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);
            try {
                this.partDocument = (SolidEdgePart.PartDocument)documents.Open(path, "8");
                this.originalFileName = this.partDocument.Name.Substring(0,this.partDocument.Name.IndexOf(", natural_"));
                
                SolidEdgeFramework.Variables variables = partDocument.GetVariables();
                SolidEdgeFramework.VariableList variableList = (SolidEdgeFramework.VariableList)variables.Query("*", SolidEdgeConstants.VariableNameBy.seVariableNameByBoth, SolidEdgeConstants.VariableVarType.SeVariableVarTypeBoth, false);
                SolidEdgeFramework.UnitsOfMeasure unitesOfMeasure = this.partDocument.UnitsOfMeasure;
                PartProperty partProperty;
                foreach (var item in variableList) {
                    partProperty = null;
                    // Determine the runtime type of the object.
                    Type itemType = item.GetType();
                    SolidEdgeFramework.ObjectType objectType = (SolidEdgeFramework.ObjectType)itemType.InvokeMember("Type", System.Reflection.BindingFlags.GetProperty, null, item, null);
                    
                    switch (objectType) {
                        case SolidEdgeFramework.ObjectType.igDimension:
                            SolidEdgeFrameworkSupport.Dimension dimension = (SolidEdgeFrameworkSupport.Dimension)item;
                            partProperty = new PartDimension(dimension, unitesOfMeasure);
                            break;
                        // Currently only supporting dimension properties
                        // case SolidEdgeFramework.ObjectType.igVariable:
                            // SolidEdgeFramework.variable variable = (SolidEdgeFramework.variable)item;
                            // partProperty = new PartVariable(variable, unitesOfMeasure);
                            // break;
                    }
                    if (partProperty != null && partProperty.displayForConfiguration()) {
                        this.partPropertyBindingSource.Add(partProperty);
                    }
                }
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            }
        }

        private void okButton_Click(object sender, EventArgs e) {
            string fileName = "";
            foreach (PartProperty partProperty in this.partPropertyBindingSource) {
                fileName += ", " + partProperty.Name + "=" + partProperty.Value + partProperty.Units;
            }
            fileName = string.Join("", fileName.Split(Path.GetInvalidFileNameChars()));
            string fullPathName = this.assemblyDocument.Path + "\\" + this.originalFileName + fileName + ".par";
            MessageBox.Show(fullPathName);
            this.partDocument.SaveCopyAs(fullPathName);
            this.partDocument.Close(false);
            this.application.DoIdle();
            this.assemblyDocument.Occurrences.AddByFilename(fullPathName);
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.partDocument.Close(false);
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
