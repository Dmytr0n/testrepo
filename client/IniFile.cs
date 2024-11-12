using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace game_client
{
    public interface IIniFile
    {
        string Read(string section, string key, string defaultValue);
    }

    public class IniFile:IIniFile
    {
        private string path;

        public IniFile(string iniPath)
        {
            path = iniPath;
        }

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32.dll")]
        private static extern bool WritePrivateProfileString(string section, string key, string value, string filePath);

        public string Read(string section, string key, string defaultValue = "")
        {
            StringBuilder result = new StringBuilder(255);
            GetPrivateProfileString(section, key, defaultValue, result, 255, path);
            return result.ToString();
        }

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, path);
        }
        public string[] ReadKeys(string section)
        {
            // Створюємо список для зберігання ключів
            List<string> keys = new List<string>();

            // Читаємо всі рядки з файлу
            var lines = File.ReadAllLines(path);

            bool isInSection = false;

            foreach (var line in lines)
            {
                // Якщо рядок - це секція, встановлюємо прапорець
                if (line.StartsWith("[" + section + "]"))
                {
                    isInSection = true; // Ми знайшли потрібну секцію
                    continue; // Перейдемо до наступного рядка
                }

                // Якщо ми знаходимося в секції, обробляємо ключі
                if (isInSection)
                {
                    // Якщо знайшли нову секцію, виходимо з циклу
                    if (line.StartsWith("["))
                        break;

                    // Розділяємо рядок на ключ та значення
                    var parts = line.Split('=');
                    if (parts.Length > 0)
                    {
                        keys.Add(parts[0].Trim()); // Додаємо ключ до списку
                    }
                }
            }

            return keys.ToArray();
        }
        

    }
}
