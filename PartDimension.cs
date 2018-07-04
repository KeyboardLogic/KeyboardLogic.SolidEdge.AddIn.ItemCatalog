using System.Windows.Forms;
namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    class PartDimension : PartProperty{
        private SolidEdgeFrameworkSupport.Dimension dimension;
        private SolidEdgeFramework.UnitsOfMeasure unitesOfMeasure;

        public PartDimension(SolidEdgeFrameworkSupport.Dimension dimension, SolidEdgeFramework.UnitsOfMeasure unitesOfMeasure) {
            this.dimension = dimension;
            this.unitesOfMeasure = unitesOfMeasure;
        }

        public string Name {
            get { return this.dimension.DisplayName; }
        }

        public double Value {
            get {
                string result = unitesOfMeasure.FormatUnit(dimension.UnitsType, dimension.Value).ToString();
                string[] strArray = result.Split(' ');
                if (strArray.Length == 2 && strArray[0] != "") {
                    result = strArray[0];
                }
                return double.Parse(result);
            }
            set {
                this.dimension.Value = (double)unitesOfMeasure.ParseUnit(dimension.UnitsType, value.ToString());
            }
        }

        public string Units {
            get {
                string result = unitesOfMeasure.FormatUnit(dimension.UnitsType, dimension.Value).ToString();
                string[] strArray = result.Split(' ');
                if (strArray.Length == 2 && strArray[0] != "") {
                    result = strArray[1];
                } else {
                    result = "";
                }
                return result;
            }
        }

        public bool displayForConfiguration() {
            return !(this.dimension.Constraint || this.dimension.IsReadOnly);
        }
    }
}
