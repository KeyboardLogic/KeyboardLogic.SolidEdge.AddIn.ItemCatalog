using System;
using System.Runtime.InteropServices;
using SolidEdgeCommunity.AddIn;
using SolidEdgeFramework;
using log4net;
using System.Reflection;

// Logging configuration
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {

    [ComVisible(true)]
    [Guid("B440CD64-4926-446A-AA6E-A5115E21F43D")] // Must be unique!
    [ProgId("KeyboardLogic.SolidEdge.AddIn.ItemCatalog")] // Must be unique!
    public class ItemCatalogAddIn : SolidEdgeAddIn {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <inheritdoc />
        /// <summary>
        /// Called when the addin is first loaded by Solid Edge.
        /// </summary>
        public override void OnConnection(Application application, SeConnectMode connectMode, SolidEdgeFramework.AddIn addInInstance) {
            Log.Info("Item Catalog: OnConnection");
            // If you makes changes to your ribbon, be sure to increment the GuiVersion or your ribbon
            // will not initialize properly.
            AddInEx.GuiVersion = 1;
        }

        /// <inheritdoc />
        /// <summary>
        /// Called when the addin first connects to a new Solid Edge environment.
        /// </summary>
        public override void OnConnectToEnvironment(SolidEdgeFramework.Environment environment, bool firstTime) {
            Log.Info("Item Catalog Connected");
        }

        /// <inheritdoc />
        /// <summary>
        /// Called when the addin is about to be unloaded by Solid Edge.
        /// </summary>
        public override void OnDisconnection(SeDisconnectMode disconnectMode) {
            Log.Info("Item Catalog Disconnected");
        }

        /// <inheritdoc />
        /// <summary>
        /// Called when Solid Edge raises the SolidEdgeFramework.ISEAddInEdgeBarEvents[Ex].AddPage() event.
        /// </summary>
        public override void OnCreateEdgeBarPage(EdgeBarController controller, SolidEdgeDocument document) {
            Log.Info("EdgeBarPage Created");
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
                case DocumentTypeConstants.igUnknownDocument:
                    break;
                case DocumentTypeConstants.igWeldmentDocument:
                    break;
                case DocumentTypeConstants.igWeldmentAssemblyDocument:
                    break;
                case DocumentTypeConstants.igSyncPartDocument:
                    break;
                case DocumentTypeConstants.igSyncSheetMetalDocument:
                    break;
                case DocumentTypeConstants.igSyncAssemblyDocument:
                    break;
                case DocumentTypeConstants.igAssemblyViewerDocument:
                    break;
                case DocumentTypeConstants.igPartViewerDocument:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Called when regasm.exe is executed against the assembly.
        /// </summary>
        [ComRegisterFunction]
        public static void OnRegister(Type t) {
            const string title = "AddIn ItemCatalog";
            const string summary = "Solid Edge Item Catalog AddIn in .NET 4.7.2";
            const bool enabled = true; // You have the option to register the addin in a disabled state.
            // List of environments that your addin supports.
            Guid[] environments = {
                SolidEdgeSDK.EnvironmentCategories.Application,
                SolidEdgeSDK.EnvironmentCategories.Assembly
            };

            try {
                Register(t, title, summary, enabled, environments);
                Log.Info("Item Catalog Registered");
            } catch (Exception ex) {
                Log.Error("Could not register Item Catalog | " + ex.Message);
            }
        }

        /// <summary>
        /// Called when regasm.exe /u is executed against the assembly.
        /// </summary>
        [ComUnregisterFunction]
        public static void OnUnregister(Type t) {
            try {
                Unregister(t);
                Log.Info("Item Catalog Unregistered");
            } catch (Exception ex) {
                Log.Error("Could not Unregister Item Catalog | " + ex.Message);
            }
        }
    }
}
