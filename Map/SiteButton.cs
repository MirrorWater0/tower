using Godot;
using System;
using System.Threading.Tasks;

public partial class SiteButton : Button
{
    public ColorRect Mask => field??= GetNode("/root/Map/UI/ColorRect") as ColorRect;
    public override void _Ready()
    {
        ButtonDown += GotoSite;
        base._Ready();
    }

    public async void GotoSite()
    {
        GD.Print("Goto Site");
        CreateTween().TweenProperty(Mask, "color", new Color(0, 0, 0, 1), 0.3f);
        await Task.Delay(300);
        GetTree().ChangeSceneToFile("res://battle/Battle.tscn");
    }
}
