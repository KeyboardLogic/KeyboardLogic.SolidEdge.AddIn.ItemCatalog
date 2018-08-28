using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using log4net;

namespace KeyboardLogic.SolidEdge.AddIn.ItemCatalog {
    class Settings {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly KeyValueConfigurationCollection _appSettings;
        private readonly KeyValueConfigurationCollection _variables;

        public Settings(Configuration configuration) {
            this._appSettings = ((AppSettingsSection)configuration.Sections["appSettings"]).Settings;
            if (this._appSettings.Count == 0) {
                Log.Warn("appSettings is empty.");
            }
            this._variables = ((AppSettingsSection)configuration.Sections["variables"]).Settings;
            if (this._variables.Count == 0) {
                Log.Warn("variables is empty.");
            }
        }

        public string getRootFolder() {
            string result = Directory.GetCurrentDirectory();
            try {
                result = this._appSettings["motherPartFolder"].Value;
                if (!Directory.Exists(result)) {
                    MessageBox.Show("motherPartFolder: " + result + " is not a valid path.\nPlease update the motherPartFolder to reference a valid path.");
                    result = Directory.GetCurrentDirectory();
                }
            } catch (Exception ex) {
                Log.Warn("motherPartFolder: " + result + " is not a valid path.\nPlease update the motherPartFolder to reference a valid path. | " + ex.Message);
                result = Directory.GetCurrentDirectory();
            }

            return result;
        }

        public bool isVariableDefined(string name) {
            return this._variables[name] != null && !string.IsNullOrEmpty(this._variables[name].Value);
        }

        public string getFileNameSeparator() {
            return this._appSettings["fileNameSeparator"].Value;
        }

        public string getAssemblyPartFolderName() {
            return this._appSettings["assemblyPartFolderName"].Value;
        }
    }
}
