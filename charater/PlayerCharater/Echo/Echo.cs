using Godot;
using System;

public partial class Echo : PlayerCharater
{
	public override PackedScene CharaterScene { get; set; } = GD.Load<PackedScene>("res://charater/PlayerCharater/Echo/Echo.tscn");
	Label label => field ??= GetNode<Label>("Label");
	public override string CharaterName { get; set; } = "Echo";

	public override void _Ready()
	{
		if(Istest) test();
		base._Ready();
		label.Text = PositionIndex.ToString();
		
	}

	public void test()
	{
		Skills = new Skill[] { new Attack(this),new FollowingLight(this),new SacredOnslaught(this),new Combo(this) };
		BattleLifemax = 2000;
		BattlePower = 1300;
		BattleDefense = 1400;
		Speed = 20;
	}

	public override void StartAction()
	{
		base.StartAction();
	}
}
