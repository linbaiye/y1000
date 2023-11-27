using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.player.skill
{
    public interface IBufa
    {
        string Name {get;}

        float Speed {get;}

        public bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            return Name.Equals(((IBufa)obj).Name);
        }
        
        public int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}