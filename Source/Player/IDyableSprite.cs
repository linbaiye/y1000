using y1000.Source.Item;

namespace y1000.Source.Player;

public interface IDyableSprite
{

    void OnEquipmentChanged(AbstractArmor? equipment);
}