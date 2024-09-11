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
    
}