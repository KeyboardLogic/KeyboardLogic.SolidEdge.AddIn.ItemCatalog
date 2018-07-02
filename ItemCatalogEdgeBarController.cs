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

namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    public partial class ItemCatalogEdgeBarController : EdgeBarControl {
        public ItemCatalogEdgeBarController() {
            InitializeComponent();
        }

        private void controllerLoad(object sender, EventArgs e) {
            // Trick to use the default system font.
            this.Font = SystemFonts.MessageBoxFont;
        }

        private void controllerAfterInitialize(object sender, EventArgs e) {
            // These properties are not initialized until AfterInitialize is called.
            var edgeBarPage = this.EdgeBarPage;
            var document = this.Document;
            var application = document.Application;

            // Populate the richtextbox with some text.
            this.richTextBox1.Text = application.GetGlobalParameter<string>(SolidEdgeFramework.ApplicationGlobalConstants.seApplicationGlobalSystemInfo);
        }
    }
}
