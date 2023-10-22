using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code
{
    public class GDNLogLogger
    {
        public static void LogMethod(string level, string message)
        {
            GD.Print("l: {0} m: {1}", level, message);
        }
    }
}