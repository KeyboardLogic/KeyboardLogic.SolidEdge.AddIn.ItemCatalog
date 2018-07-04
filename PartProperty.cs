namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    interface PartProperty {
        string Name { get;}

        double Value { get; set; }

        string Units { get;}

        bool displayForConfiguration();
    }
}
