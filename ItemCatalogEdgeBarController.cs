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
        private string currentDirectory;

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

            this.currentDirectory = "Y:\\Google Drive\\Customers\\Innovated Solutions\\01 Profile 8";
            updatelistViewToCurrentDirectory();
            // this.webBrowser1.Url = new Uri(this.currentDirectory);
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            // Set the help text description for the FolderBrowserDialog.
            folderBrowserDialog1.Description = "Select the directory that you want to use as the default.";
            // Do not allow the user to create new files via the FolderBrowserDialog.
            folderBrowserDialog1.ShowNewFolderButton = false;
            // Default to the My Documents folder.
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            // Show the FolderBrowserDialog.
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                this.currentDirectory = folderBrowserDialog1.SelectedPath;
            }

            if (this.currentDirectory != null) {
                updatelistViewToCurrentDirectory();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e) {
            foreach (ListViewItem item in this.listView1.SelectedItems) {
                if (item.Text == null) {
                    this.richTextBox1.Text = "";
                } else if (File.Exists(this.currentDirectory + "\\" + item.Text)) { // Display preview of part
                    this.richTextBox1.Text = item.Text;
                } else { // Directories do not have displays

                }
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e) {
            foreach (ListViewItem item in this.listView1.SelectedItems) {
                if (item.Text == null) {
                    this.richTextBox1.Text = "";
                } else if (File.Exists(this.currentDirectory + "\\" + item.Text)) {
                    ConfigurationDialog configurationDialog = new ConfigurationDialog(this.currentDirectory + "\\" + item.Text);
                    configurationDialog.ShowDialog();
                } else { // change current directory to selected directory
                    this.currentDirectory = this.currentDirectory + "\\" + item.Text;
                    updatelistViewToCurrentDirectory();
                }
            }
        }

        private void updatelistViewToCurrentDirectory() {
            this.listView1.Items.Clear();
            foreach (string str in Directory.GetDirectories(this.currentDirectory)) {
                this.listView1.Items.Add(Path.GetFileName(str));
            }
            foreach (string str in Directory.GetFiles(this.currentDirectory)) {
                this.listView1.Items.Add(Path.GetFileName(str));
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {

        }
    }
}
