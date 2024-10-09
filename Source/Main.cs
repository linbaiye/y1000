using Godot;
using y1000.Source.Control;

namespace y1000.Source;

public partial class Main : Node
{
    private UIController _controller;

    private Game _game;
    
    public override void _Ready()
    {
        _controller = GetNode<UIController>("UILayer");
        _game = GetNode<Game>("GameViewportContainer/GameViewport/Game");
    }

    public void Start(string token, string charName)
    {
		GetWindow().Size = new Vector2I(1024, 768);
		GetWindow().ContentScaleSize = new Vector2I(1024, 768);
        _game.Start(token, charName, _controller);
    }
}