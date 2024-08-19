using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace y1000.code.util
{
    public interface I2JsonString
    {
        public string JsonString() { return JsonSerializer.Serialize(this); }
    }
}