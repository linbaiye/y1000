using y1000.Source.Animation;
using y1000.Source.Item;
using y1000.Source.Util;

namespace y1000.Source.Player;

public abstract partial class AbstractDyablePartSprite : AbstractPartSprite, IDyableSprite
{
    public void Dye(int color)
    {
        if (color == 0)
        {
            Material = null;
        }
        else
        {
            Material = DyeShader.CreateShaderMaterial(color);
        }
    }


    public void OnEquipmentChanged(AbstractArmor? equipment)
    {
        if (equipment == null)
        {
            Visible = false;
            return;
        }
        Dye(equipment.Color);
        Visible = true;
    }
}