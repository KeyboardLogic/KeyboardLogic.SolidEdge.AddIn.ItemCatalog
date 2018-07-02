using System;
using System.Runtime.InteropServices;
using SolidEdgeCommunity.AddIn;

namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
	
	[ComVisible(true)]
	[Guid("B440CD64-4926-446A-AA6E-A5115E21F43D")] // Must be unique!
	[ProgId("KeyboardLogic.SolidEdge.AddIn.ItemCatalog")] // Must be unique!
	public class ItemCatalogAddIn : SolidEdgeAddIn {
		public ItemCatalogAddIn() {
		}
	}
}
