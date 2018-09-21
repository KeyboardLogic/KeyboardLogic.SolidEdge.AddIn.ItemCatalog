using log4net;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    class PartProperty {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int UnitType = 1;

        public PartProperty(object property, SolidEdgeFramework.UnitsOfMeasure unitesOfMeasure) {
            // TODO: Fix error in ST9 with UnitsType not accessible
            /**
            try {
                this.unitType = dimension.UnitsType;
            } catch (Exception ex) {
                log.Error(ex.Message);
            }
            **/
            try {
                Type type = property.GetType();
                SolidEdgeFramework.ObjectType objectType = (SolidEdgeFramework.ObjectType)type.InvokeMember("Type", System.Reflection.BindingFlags.GetProperty, null, property, null);
                switch (objectType) {
                    case SolidEdgeFramework.ObjectType.igDimension:
                        SolidEdgeFrameworkSupport.Dimension dimension = (SolidEdgeFrameworkSupport.Dimension)property;
                        this.Name = dimension.DisplayName.ToString();
                        this.Value = GetValueofCurrentMeasure(unitesOfMeasure, this.UnitType, dimension.Value);
                        this.Units = GetUnitsOfCurrentMeasure(unitesOfMeasure, this.UnitType, dimension.Value);
                        Marshal.FinalReleaseComObject(dimension);
                        break;
                        // Currently only supporting dimension properties
                    case SolidEdgeFramework.ObjectType.igVariable:
                        SolidEdgeFramework.variable variable = (SolidEdgeFramework.variable)property;
                        this.Name = variable.DisplayName.ToString();
                        this.Value = GetValueofCurrentMeasure(unitesOfMeasure, this.UnitType, variable.Value);
                        this.Units = GetUnitsOfCurrentMeasure(unitesOfMeasure, this.UnitType, variable.Value);
                        Marshal.FinalReleaseComObject(variable);
                        break;
                }
            } catch (Exception ex) {
                log.Error(ex.Message);
            }
        }

        public string Name { get; set; }

        public double Value { get; set; }

        public string Units { get; set; }

        public double GetSolidEdgeStoredValue(SolidEdgeFramework.UnitsOfMeasure unitesOfMeasure) {
            return (double)unitesOfMeasure.ParseUnit(this.UnitType, this.Value.ToString());
        }

        public static string GetUnitsOfCurrentMeasure(SolidEdgeFramework.UnitsOfMeasure unitesOfMeasure, int unitsType, double value) {
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

        public static double GetValueofCurrentMeasure(SolidEdgeFramework.UnitsOfMeasure unitesOfMeasure, int unitsType, double value) {
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
