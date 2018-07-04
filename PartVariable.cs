using System.Windows.Forms;
namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    class PartVariable : PartProperty {
        private SolidEdgeFramework.variable variable;
        private SolidEdgeFramework.UnitsOfMeasure unitesOfMeasure;

        public PartVariable(SolidEdgeFramework.variable variable, SolidEdgeFramework.UnitsOfMeasure unitesOfMeasure) {
            this.variable = variable;
            this.unitesOfMeasure = unitesOfMeasure;
        }

        public string Name {
            get { return this.variable.DisplayName; }
        }

        public double Value {
            get {
                string result = unitesOfMeasure.FormatUnit(variable.UnitsType, variable.Value).ToString();
                string[] strArray = result.Split(' ');
                if (strArray.Length == 2 && strArray[0] != "") {
                    result = strArray[0];
                }
                return double.Parse(result);
            }
            set {
                this.variable.Value = (double)unitesOfMeasure.ParseUnit(variable.UnitsType, value.ToString());
            }
        }

        public string Units {
            get {
                string result = unitesOfMeasure.FormatUnit(variable.UnitsType, variable.Value).ToString();
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
            return !this.variable.IsReadOnly;
        }
    }
}
