using log4net;
using System;
using System.Reflection;

namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    class PartVariable : PartProperty {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private SolidEdgeFramework.variable variable;
        
        public PartVariable(SolidEdgeFramework.variable variable, SolidEdgeFramework.UnitsOfMeasure unitesOfMeasure) {
            this.variable = variable;
            this.unitesOfMeasure = unitesOfMeasure;
            // TODO: Fix error in ST9 with UnitsType not accessible
            /**
            try {
                this.unitType = variable.UnitsType;
            } catch (Exception ex) {
                log.Error(ex.Message);
            }
            **/
        }

        public override string Name {
            get { return this.variable.DisplayName; }
        }

        public override double Value {
            get {
                return this.getValueofCurrentMeasure(this.unitType, variable.Value);
            }
            set {
                this.variable.Value = (double)unitesOfMeasure.ParseUnit(this.unitType, value.ToString());
            }
        }

        public override string Units {
            get {
                return this.getUnitsOfCurrentMeasure(this.unitType, variable.Value);
            }
        }

        public override bool displayForConfiguration() {
            return !this.variable.IsReadOnly;
        }
    }
}
