using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace y1000.Source.Util;

public static class StringUtil
{
    
    public static bool DigitOnly(this string str)
    {
        foreach (char c in str)
        {
            if (c < '0' || c > '9')
                return false;
        }
        return true;
    }

    public static List<string> SplitByNewline(this string str)
    {
        return new List<string>(str.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None));
    }

    public static string ReplaceBr(this string str)
    {
		return Regex.Replace(str, "<br>", "\n");
    }
}