using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;

namespace y1000.Source.Storage
{
    public class Configuration
    {
        public static readonly Configuration Instance = new();

        private const string FileName = "config.txt";

        public string ServerAddr { get; }


        private Configuration()
        {
            var path = OS.GetExecutablePath().GetBaseDir();
            var fullPath = path + "/" + FileName;
            if (!File.Exists(fullPath))
            {
                ServerAddr = "193.112.251.231";
            }
            else
            {
                var content = File.ReadAllText(fullPath);
                if (string.IsNullOrEmpty(content))
                    throw new NotSupportedException("Bad configuration file.");
                var result = JsonSerializer.Deserialize<Dictionary<string, string>>(content);
                if (result == null || !result.ContainsKey("ServerAddr"))
                    throw new NotSupportedException("Bad configuration file.");
                ServerAddr = result.GetValueOrDefault("ServerAddr", "");
            }
        }
    }
}