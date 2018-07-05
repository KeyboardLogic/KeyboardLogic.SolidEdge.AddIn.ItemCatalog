namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    abstract class PartProperty {
        protected SolidEdgeFramework.UnitsOfMeasure unitesOfMeasure;

        public abstract string Name { get;}

        public abstract double Value { get; set; }

        public abstract string Units { get;}

        public abstract bool displayForConfiguration();

        protected string getUnitsOfCurrentMeasure(int unitsType, double value) {
            string result = unitesOfMeasure.FormatUnit(unitsType, value).ToString();
            string[] strArray = result.Split(' ');
            if (strArray.Length == 2 && strArray[0] != "") {
                result = strArray[1];
            } else if (strArray.Length == 3 && strArray[1] != "") {
                result = strArray[2];
            } else {
                result = "";
            }
            return result;
        }

        protected double getValueofCurrentMeasure(int unitsType, double value) {
            string result = unitesOfMeasure.FormatUnit(unitsType, value).ToString();
            string[] strArray = result.Split(' ');
            if (strArray.Length == 2 && strArray[0] != "") {
                result = strArray[0];
            } else if (strArray.Length == 3 && strArray[1] != "") {
                result = strArray[1];
            }
            return double.Parse(result);
        }
    }
}
