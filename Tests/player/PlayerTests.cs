using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.player
{
    public class PlayerTests
    {

        [Test]
        public void Test1()
        {
           var ret =  Character.Add(12, 3);
           Assert.AreEqual(14, ret);
        }
        
    }
}