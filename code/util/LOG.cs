using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.util
{
    public class LOG
    {
        public static void LogMethod(string level, string message)
        {
            GD.Print("l: {0} m: {1}", level, message);
        }

        public static void Debug(string message)
        {
            GD.Print(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")  + " DEBUG " +         message);
        }
    }
}