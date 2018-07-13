using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SolidEdgeCommunity.AddIn;
using SolidEdgeFramework;

namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
	
	[ComVisible(true)]
	[Guid("B440CD64-4926-446A-AA6E-A5115E21F43D")] // Must be unique!
	[ProgId("KeyboardLogic.SolidEdge.AddIn.ItemCatalog")] // Must be unique!
	public class ItemCatalogAddIn : SolidEdgeAddIn {
        /// <summary>
        /// Called when the addin is first loaded by Solid Edge.
        /// </summary>
        public override void OnConnection(SolidEdgeFramework.Application application, SeConnectMode ConnectMode, SolidEdgeFramework.AddIn AddInInstance) {
            // If you makes changes to your ribbon, be sure to increment the GuiVersion or your ribbon
            // will not initialize properly.
            AddInEx.GuiVersion = 1;
        }

        /// <summary>
        /// Called when the addin first connects to a new Solid Edge environment.
        /// </summary>
        public override void OnConnectToEnvironment(SolidEdgeFramework.Environment environment, bool firstTime) { }

        /// <summary>
        /// Called when the addin is about to be unloaded by Solid Edge.
        /// </summary>
        public override void OnDisconnection(SeDisconnectMode DisconnectMode) { }

        /// <summary>
        /// Called when Solid Edge raises the SolidEdgeFramework.ISEAddInEdgeBarEvents[Ex].AddPage() event.
        /// </summary>
        public override void OnCreateEdgeBarPage(EdgeBarController controller, SolidEdgeDocument document) {
            // Note: Confirmed with Solid Edge development, OnCreateEdgeBarPage does not get called when Solid Edge is first open and the first document is open.
            // i.e. Under the hood, SolidEdgeFramework.ISEAddInEdgeBarEvents[Ex].AddPage() is not getting called.
            // As an alternative, you can call DemoAddIn.Instance.EdgeBarController.Add() in some other event if you need.
            // Get the document type of the passed in document.
            var documentType = document.Type;
            
            // Depending on the document type, you may have different edgebar controls.
            switch (documentType) {
                case DocumentTypeConstants.igAssemblyDocument:
                    controller.Add<ItemCatalogEdgeBarController>(document, 1);
                    break;
                case DocumentTypeConstants.igDraftDocument:
                    break;
                case DocumentTypeConstants.igPartDocument:
                    break;
                case DocumentTypeConstants.igSheetMetalDocument:
                    break;
            }
        }


        /// <summary>
        /// Called when regasm.exe is executed against the assembly.
        /// </summary>
        [ComRegisterFunction]
        public static void OnRegister(Type t) {
            string title = "AddIn ItemCatalog";
            string summary = "Solid Edge Item Catalog Addin in .NET 4.0.";
            var enabled = true; // You have the option to register the addin in a disabled state.

            // List of environments that your addin supports.
            Guid[] environments = {
                SolidEdgeSDK.EnvironmentCategories.Application,
                SolidEdgeSDK.EnvironmentCategories.AllDocumentEnvrionments
            };

            try {
                Register(t, title, summary, enabled, environments);
            } catch (Exception ex) {
                MessageBox.Show(ex.StackTrace, ex.Message);
            }
        }

        /// <summary>
        /// Called when regasm.exe /u is executed against the assembly.
        /// </summary>
        [ComUnregisterFunction]
        public static void OnUnregister(Type t) {
            Unregister(t);
        }
    }
}
