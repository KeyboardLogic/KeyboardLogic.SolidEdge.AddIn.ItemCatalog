using log4net;
using System;
using System.Reflection;

namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    class PartDimension : PartProperty{
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private SolidEdgeFrameworkSupport.Dimension dimension;

        public PartDimension(SolidEdgeFrameworkSupport.Dimension dimension, SolidEdgeFramework.UnitsOfMeasure unitesOfMeasure) {
            this.dimension = dimension;
            this.unitesOfMeasure = unitesOfMeasure;
            // TODO: Fix error in ST9 with UnitsType not accessible
            /**
            try {
                this.unitType = dimension.UnitsType;
            } catch (Exception ex) {
                log.Error(ex.Message);
            }
            **/
        }

        public override string Name {
            get { return this.dimension.DisplayName; }
        }

        public override double Value {
            get {
                log.Debug("dimension.DisplayName: " + dimension.DisplayName);
                return this.getValueofCurrentMeasure(this.unitType, dimension.Value);
            }
            set {
                this.dimension.Value = (double)unitesOfMeasure.ParseUnit(this.unitType, value.ToString());
            }
        }

        public override string Units {
            get {
                return this.getUnitsOfCurrentMeasure(this.unitType, dimension.Value);
            }
        }

        public override bool displayForConfiguration() {
            return !this.dimension.Constraint;
        }
    }
}
