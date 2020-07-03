using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    class IniFile
    {
        #region ini
        [DllImport("kernel32")]
        internal static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        internal static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName);

        static string configFile = null;
        public static string ConfigFileName
        {
            get
            {
                if (string.IsNullOrEmpty(configFile))
                {
                    string configFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "VisualCodeEditor");
                    if (!Directory.Exists(configFolder))
                        Directory.CreateDirectory(configFolder);
                    configFile= System.IO.Path.Combine(configFolder, ".config");
                }
                return configFile;
            }
        }
        public static void WriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, ConfigFileName);
        }

        public static string ReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp,
                                            255, ConfigFileName);
            if (i <= 0)
                return null;
            String Value = temp.ToString();
            return Value;

        }
        #endregion ini
    }
}
