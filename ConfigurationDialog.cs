using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using RevisionManager;

namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    public partial class ConfigurationDialog : Form {
        public ConfigurationDialog(string path) {
            InitializeComponent();
            // PartDocument part = PartDocument.Open(path);
            this.textBox1.Text = path;

            SolidEdgeFileProperties.PropertySets objPropertySets = new SolidEdgeFileProperties.PropertySets();
            objPropertySets.Open(path, false);
            try {
                foreach (SolidEdgeFileProperties.Properties properties in objPropertySets) {
                    ListViewGroup group = new ListViewGroup(properties.Name);
                    foreach (SolidEdgeFileProperties.Property property in properties) {
                        try {
                            ListViewItem listViewItem = new ListViewItem(new string[] {property.Name, property.Value.ToString()}, -1);
                            listViewItem.Group = group;
                            this.listView1.Items.Add(listViewItem);
                        } catch (Exception e) {
                            Console.WriteLine(e.Message);
                            //MessageBox.Show(e.Message);
                        }
                    }
                    this.listView1.Groups.Add(group);
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                //MessageBox.Show(e.Message);
            }
            objPropertySets.Save();
            objPropertySets.Close();
        }

        private void okButton_Click(object sender, EventArgs e) {
            // Connect to or start Solid Edge.
            SolidEdgeFramework.Application application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);
            // Get a refrence to the active assembly document.
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);
            assemblyDocument.Occurrences.AddByFilename(this.textBox1.Text);
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
