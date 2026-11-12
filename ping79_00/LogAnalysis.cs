using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ping79_00
{
    internal static class LogAnalysis
    {
        // Remove the terminal's wall-clock prefix, preserving boot-relative timestamps.
        private static readonly Regex Timestamp = new Regex(
            @"^\s*(?:\[?\d{4}[-/]\d{2}[-/]\d{2}[ T])?\[?\d{2}:\d{2}:\d{2}(?:[.,]\d+)?\]?\s*",
            RegexOptions.Compiled);
        private static readonly Regex Commit = new Regex(
            @"^\s*(?:\[\s*\d+(?:\.\d+)?\]\s*)?\*\s+[0-9a-f]{7,40}\s+",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        internal static string Extract(string path)
        {
            var result = new StringBuilder("FILE: " + Path.GetFullPath(path));
            int remainingCommits = 0;
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read,
                FileShare.ReadWrite | FileShare.Delete))
            using (var reader = new StreamReader(stream, Encoding.UTF8, true))
            {
                string raw;
                while ((raw = reader.ReadLine()) != null)
                {
                    string line = Timestamp.Replace(raw, "");
                    bool git = line.IndexOf(" git (", StringComparison.OrdinalIgnoreCase) >= 0;
                    bool selected = git || Contains(line, "build time:") ||
                        Contains(line, "chip id (serial-number)") ||
                        Contains(line, "AI board:") || Contains(line, "Kernel command line:");
                    if (selected)
                    {
                        result.AppendLine().Append(line);
                        remainingCommits = git ? 3 : 0;
                    }
                    else if (remainingCommits > 0 && Commit.IsMatch(line))
                    {
                        result.AppendLine().Append(line);
                        remainingCommits--;
                    }
                    else if (!string.IsNullOrWhiteSpace(line))
                    {
                        remainingCommits = 0;
                    }
                }
            }
            return result.ToString();
        }

        private static bool Contains(string line, string text)
        {
            return line.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        internal static List<string> Load(string folder)
        {
            var files = new DirectoryInfo(folder).GetFiles()
                .OrderByDescending(file => file.LastWriteTimeUtc)
                .ThenBy(file => file.Name, StringComparer.OrdinalIgnoreCase).Take(10).ToList();
            var results = new List<string>();
            foreach (var file in files)
            {
                try { results.Add(Extract(file.FullName)); }
                catch (IOException ex) { results.Add("FILE: " + file.FullName + "\r\n읽기 실패: " + ex.Message); }
                catch (UnauthorizedAccessException ex) { results.Add("FILE: " + file.FullName + "\r\n읽기 실패: " + ex.Message); }
            }
            return results;
        }
    }

    internal static class LogFolderSettings
    {
        internal const string DefaultFolder = @"E:\temp\_uart_log";
        internal static string SettingsPath
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ping79_00", "settings.ini"); }
        }

        internal static string Read()
        {
            string folder = IniSettings.Read(SettingsPath, "LogAnalysis", "Folder");
            if (!string.IsNullOrWhiteSpace(folder)) return folder;
            Save(DefaultFolder);
            return DefaultFolder;
        }

        internal static void Save(string folder)
        {
            IniSettings.WriteSection(SettingsPath, "LogAnalysis", new[] { "Folder=" + folder });
        }
    }
}
