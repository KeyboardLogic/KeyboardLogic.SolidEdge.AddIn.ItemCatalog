using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    public partial class ConfigurationDialog : Form {
        public ConfigurationDialog() {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
