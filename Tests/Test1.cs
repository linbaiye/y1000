using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    public class Test1
    {


        [Test]
        public void Test()
        {
           var ret =  Character.Add(12, 3);
           Assert.AreEqual(15, ret);
        }
        
    }
}