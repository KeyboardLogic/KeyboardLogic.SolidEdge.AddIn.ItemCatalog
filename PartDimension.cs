namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    class PartDimension : PartProperty{
        private SolidEdgeFrameworkSupport.Dimension dimension;

        public PartDimension(SolidEdgeFrameworkSupport.Dimension dimension, SolidEdgeFramework.UnitsOfMeasure unitesOfMeasure) {
            this.dimension = dimension;
            this.unitesOfMeasure = unitesOfMeasure;
        }

        public override string Name {
            get { return this.dimension.DisplayName; }
        }

        public override double Value {
            get {
                return this.getValueofCurrentMeasure(dimension.UnitsType, dimension.Value);
            }
            set {
                this.dimension.Value = (double)unitesOfMeasure.ParseUnit(dimension.UnitsType, value.ToString());
            }
        }

        public override string Units {
            get {
                return this.getUnitsOfCurrentMeasure(dimension.UnitsType, dimension.Value);
            }
        }

        public override bool displayForConfiguration() {
            return !(this.dimension.Constraint || this.dimension.IsReadOnly);
        }
    }
}
