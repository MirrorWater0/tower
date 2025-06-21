using Godot;
using System;

public partial class Test1 : EnemyTemplate
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BattleLifemax = 100;
		BattlePower = 40;
		BattleDefense = 40;
		Speed = 18;
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
