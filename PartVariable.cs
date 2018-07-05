using System.Windows.Forms;
namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    class PartVariable : PartProperty {
        private SolidEdgeFramework.variable variable;
        
        public PartVariable(SolidEdgeFramework.variable variable, SolidEdgeFramework.UnitsOfMeasure unitesOfMeasure) {
            this.variable = variable;
            this.unitesOfMeasure = unitesOfMeasure;
        }

        public override string Name {
            get { return this.variable.DisplayName; }
        }

        public override double Value {
            get {
                return this.getValueofCurrentMeasure(variable.UnitsType, variable.Value);
            }
            set {
                this.variable.Value = (double)unitesOfMeasure.ParseUnit(variable.UnitsType, value.ToString());
            }
        }

        public override string Units {
            get {
                return this.getUnitsOfCurrentMeasure(variable.UnitsType, variable.Value);
            }
        }

        public override bool displayForConfiguration() {
            return !this.variable.IsReadOnly;
        }
    }
}
