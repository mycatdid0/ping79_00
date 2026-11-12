using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ping79_00
{
    internal static class IniSettings
    {
        internal static string Read(string path, string section, string key)
        {
            if (!File.Exists(path)) return null;
            bool inSection = false;
            foreach (string line in File.ReadLines(path, Encoding.UTF8))
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                    inSection = string.Equals(trimmed, "[" + section + "]", StringComparison.OrdinalIgnoreCase);
                else if (inSection)
                {
                    int equals = line.IndexOf('=');
                    if (equals >= 0 && string.Equals(line.Substring(0, equals).Trim(), key,
                        StringComparison.OrdinalIgnoreCase)) return line.Substring(equals + 1).Trim();
                }
            }
            return null;
        }

        // Update only the requested section; keep other settings and comments intact.
        internal static void WriteSection(string path, string section, IEnumerable<string> values)
        {
            var lines = File.Exists(path) ? File.ReadAllLines(path, Encoding.UTF8).ToList() : new List<string>();
            int start = lines.FindIndex(line => string.Equals(line.Trim(), "[" + section + "]",
                StringComparison.OrdinalIgnoreCase));
            if (start >= 0)
            {
                int end = start + 1;
                while (end < lines.Count && !lines[end].TrimStart().StartsWith("[")) end++;
                lines.RemoveRange(start, end - start);
            }
            else start = lines.Count;
            lines.InsertRange(start, new[] { "[" + section + "]" }.Concat(values));
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path)));
            File.WriteAllLines(path, lines, Encoding.UTF8);
        }
    }

    internal static class IpHistorySettings
    {
        internal static List<string> Read(string path)
        {
            var values = new List<string>();
            for (int i = 1; i <= 10; i++) values.Add(IniSettings.Read(path, "Ping", "IP" + i));
            return Normalize(values);
        }

        internal static List<string> Normalize(IEnumerable<string> values)
        {
            var result = new List<string>();
            foreach (string value in values)
            {
                IPAddress address;
                if (string.IsNullOrWhiteSpace(value) || !IPAddress.TryParse(value.Trim(), out address)) continue;
                string ip = address.ToString();
                if (!result.Contains(ip)) result.Add(ip);
                if (result.Count == 10) break;
            }
            return result;
        }

        internal static void Save(string path, IEnumerable<string> history)
        {
            IniSettings.WriteSection(path, "Ping", Normalize(history).Select((ip, index) =>
                "IP" + (index + 1) + "=" + ip));
        }
    }
}
