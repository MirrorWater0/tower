using Godot;
using System;
using System.Threading.Tasks;

public partial class ReadyButton : Button
{
    public Map ThisMap => field ??= GetNode("/root/Map") as Map;
    PackedScene _readyScene = GD.Load("res://battle/UIScene/BattleReady/battle_ready.tscn") as PackedScene;
    private BattleReady ThisBattleReady;
    public CanvasLayer Layer => field ??= GetNode("/root/Map/BattleReadyLayer") as CanvasLayer;
    public async override void _Ready()
    {
        ThisBattleReady = _readyScene.Instantiate() as BattleReady;
        ThisBattleReady.Modulate = new Color(1, 1, 1, 0);
        Layer.AddChild(ThisBattleReady);
        ButtonDown += Opean;
    }

    public void Opean()
    {
        if (ThisBattleReady.Modulate.A == 0)
        {
            CreateTween().TweenProperty(ThisBattleReady, "modulate", new Color(1, 1, 1, 1), 0.3f);
            Layer.Layer = 1;
        }
        else
        {
            CreateTween().TweenProperty(ThisBattleReady, "modulate", new Color(1, 1, 1, 0), 0.3f);
            Layer.Layer = -1;
            ThisBattleReady.ComfirmTactics();
        }
        
    }
}
