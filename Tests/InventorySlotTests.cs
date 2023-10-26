using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Xunit;

namespace Tests
{
    public class InventorySlotTests
    {

        [Fact]
       public void Test1()
        {
            //InventorySlot slot = new InventorySlot();
            Texture2D texture2D = new Texture2D();
            Assert.Equal(4, Character.Add(1, 2));
            //slot.PutItem(item);
            //Assert.True(slot._CanDropData(Vector2.Zero, Variant.CreateFrom(item)));
        }
        
    }
}