using Godot;

namespace y1000.Source.Control.LeftSide;

public partial class LeftsideTextControl : VBoxContainer
{
    private LeftsideTextLabel _label1;
    private LeftsideTextLabel _label2;
    private LeftsideTextLabel _label3;
    private LeftsideTextLabel _label4;
    private LeftsideTextLabel _label5;

    public override void _Ready()
    {
        _label1 = GetNode<LeftsideTextLabel>("Label1");
        _label2 = GetNode<LeftsideTextLabel>("Label2");
        _label3 = GetNode<LeftsideTextLabel>("Label3");
        _label4 = GetNode<LeftsideTextLabel>("Label4");
        _label5 = GetNode<LeftsideTextLabel>("Label5");
    }


    public void Display(string text)
    {
        Visible = true;
        LeftsideTextLabel[] labels = { _label1, _label2, _label3, _label4, _label5 };
        for (int i = 0; i < labels.Length; i++)
        {
            if (!labels[i].Visible)
            {
                labels[i].Display(text);
                return;
            }
        }
        Compact();
        labels[^1].Display(text);
    }

    private void Compact()
    {
        LeftsideTextLabel[] labels = { _label1, _label2, _label3, _label4, _label5 };
        for (int i = 1; i < labels.Length; i++)
        {
            labels[i - i].Copy(labels[i]);
            labels[i].Clean();
        }
    }

    public override void _Process(double delta)
    {
        if (!Visible)
            return;
        LeftsideTextLabel[] labels = { _label1, _label2, _label3, _label4, _label5 };
        bool needHide = true;
        for (int i = 0; i < labels.Length; i++)
        {
            labels[i].Update(delta);
            if (labels[i].Visible)
            {
                needHide = false;
            }
        }
        if (needHide)
        {
            Visible = false;
        }
        else
        {
            if (!_label1.Visible)
                Compact();
        }
    }
}