using Godot;

namespace y1000.Source.Control.Map;

public partial class MapCreatureView : Panel
{
    private Label _label;

    private string _name = "";

    public override void _Ready()
    {
        _label = GetNode<Label>("Name");
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }
    
    private void OnMouseEntered()
    {
        _label.Text = _name;
        _label.Show();
    }


    private void OnMouseExited()
    {
        _label.Text = "";
        _label.Hide();
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public static MapCreatureView Create(string name)
    {
        var packedScene = ResourceLoader.Load<PackedScene>("res://Scenes/MapNpcView.tscn");
        var view = packedScene.Instantiate<MapCreatureView>();
        view._name = name;
        return view;
    }
}