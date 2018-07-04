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
        public ItemCatalogEdgeBarController() {
            InitializeComponent();
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
            foreach (ListViewItem item in this.partLibrary.SelectedItems) {
                if (item.Text == null) {
                    // do nothing
                } else if (File.Exists(this.currentDirectory.Text + "\\" + item.Text)) { // Display preview of part
                    // TODO: display preview of selected part
                } else { // Directories do not have displays

                }
            }
        }

        private void partLibrary_DoubleClick(object sender, EventArgs e) {
            foreach (ListViewItem item in this.partLibrary.SelectedItems) {
                if (item.Text == null) {
                    // do nothing
                } else if (File.Exists(this.currentDirectory.Text + "\\" + item.Text)) {
                    ConfigurationDialog configurationDialog = new ConfigurationDialog(this.currentDirectory.Text + "\\" + item.Text);
                    configurationDialog.ShowDialog();
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
    }
}
