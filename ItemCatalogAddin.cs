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
    public class ItemCatalogAddIn : SolidEdgeCommunity.AddIn.SolidEdgeAddIn // Solid Edge Community provided base class.
                                                                           //SolidEdgeFramework.ISEApplicationEvents,  // Solid Edge Application Events
                                                                           //SolidEdgeFramework.ISEApplicationWindowEvents, // Solid Edge Window Events
                                                                           //SolidEdgeFramework.ISEFeatureLibraryEvents, // Solid Edge Feature Library Events
                                                                           //SolidEdgeFramework.ISEFileUIEvents, // Solid Egde File UI Eventss
                                                                           //SolidEdgeFramework.ISENewFileUIEvents, // Solid Egde New File UI Events
                                                                           //SolidEdgeFramework.ISEECEvents, // Solid Edge EC Events
                                                                           //SolidEdgeFramework.ISEShortCutMenuEvents // Solid Edge Shortcut Menu Events 
        {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Called when the addin is first loaded by Solid Edge.
        /// </summary>
        public override void OnConnection(SolidEdgeFramework.Application application, SeConnectMode ConnectMode, SolidEdgeFramework.AddIn AddInInstance) {
            log.Info("Item Catalog: OnConnection");
            // If you makes changes to your ribbon, be sure to increment the GuiVersion or your ribbon
            // will not initialize properly.
            AddInEx.GuiVersion = 1;
            try {
                // Create an instance of the default connection point controller. It helps manage connections to COM events.
                //_connectionPointController = new SolidEdgeCommunity.ConnectionPointController(this);

                // Uncomment the following line to attach to the Solid Edge Application Events.
                //_connectionPointController.AdviseSink<SolidEdgeFramework.ISEApplicationEvents>(this.Application);

                // Not necessary unless you absolutely need to see low level windows messages.
                // Uncomment the following line to attach to the Solid Edge Application Window Events.
                //_connectionPointController.AdviseSink<SolidEdgeFramework.ISEApplicationWindowEvents>(this.Application);

                // Uncomment the following line to attach to the Solid Edge Feature Library Events.
                //_connectionPointController.AdviseSink<SolidEdgeFramework.ISEFeatureLibraryEvents>(this.Application);

                // Uncomment the following line to attach to the Solid Edge File UI Events.
                //_connectionPointController.AdviseSink<SolidEdgeFramework.ISEFileUIEvents>(this.Application);

                // Uncomment the following line to attach to the Solid Edge File New UI Events.
                //_connectionPointController.AdviseSink<SolidEdgeFramework.ISENewFileUIEvents>(this.Application);

                // Uncomment the following line to attach to the Solid Edge EC Events.
                //_connectionPointController.AdviseSink<SolidEdgeFramework.ISEECEvents>(this.Application);

                // Uncomment the following line to attach to the Solid Edge Shortcut Menu Events.
                //_connectionPointController.AdviseSink<SolidEdgeFramework.ISEShortCutMenuEvents>(this.Application);
            } catch (Exception ex) {
                log.Error(ex.Message);
            }
        }

        /// <summary>
        /// Called when the addin first connects to a new Solid Edge environment.
        /// </summary>
        public override void OnConnectToEnvironment(SolidEdgeFramework.Environment environment, bool firstTime) {
            log.Info("Item Catalog Connected");
        }

        /// <summary>
        /// Called when the addin is about to be unloaded by Solid Edge.
        /// </summary>
        public override void OnDisconnection(SeDisconnectMode DisconnectMode) {
            log.Info("Item Catalog Disconnected");
        }

        /// <summary>
        /// Called when Solid Edge raises the SolidEdgeFramework.ISEAddInEdgeBarEvents[Ex].AddPage() event.
        /// </summary>
        public override void OnCreateEdgeBarPage(EdgeBarController controller, SolidEdgeDocument document) {
            log.Info("EdgeBarPage Created");
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
            string summary = "Solid Edge Item Catalog AddIn in .NET 4.7.2";
            var enabled = true; // You have the option to register the addin in a disabled state.
            // List of environments that your addin supports.
            Guid[] environments = {
                SolidEdgeSDK.EnvironmentCategories.Application,
                SolidEdgeSDK.EnvironmentCategories.Assembly
            };

            try {
                Register(t, title, summary, enabled, environments);
                log.Info("Item Catalog Registered");
            } catch (Exception ex) {
                log.Error("Could not register Item Catalog | " + ex.Message);
            }
        }

        /// <summary>
        /// Called when regasm.exe /u is executed against the assembly.
        /// </summary>
        [ComUnregisterFunction]
        public static void OnUnregister(Type t) {
            try {
                Unregister(t);
                log.Info("Item Catalog Unregistered");
            } catch (Exception ex) {
                log.Error("Could not Unregister Item Catalog | " + ex.Message);
            }
        }
    }
}
