using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class EnemyTemplate : Charater
{
	private ProgressBar _lifebar;
	public Battle Battle => field ??= GetNode("/root/Battle") as Battle;
	Label label => field ??= GetNode<Label>("Label");

	public override void _Ready()
	{
		IsPlayer = false;
		Lifemax = 2500;
		Power = 1200;
		Defense = 900;
		Speed = 18;
	}

	public override void Initialize()
	{
		base.Initialize();

		Skills = new Skill[] {new Attack(this),new Combo(this) };
		label.Text = PositionIndex.ToString();
		DyingBuffs.Add(new Buff(this,new Charater[]{this},Buff.BuffType.Rebirth,1));
	}
	

	public async override void StartAction()
	{
		base.StartAction();
		Random random = new Random();
		int i = random.Next(0, Skills.Length);
		Skills[i].Effect();
	}
	
	public override void GetHurt(float damage)
	{
		base.GetHurt(damage);
		Tween tween = CreateTween();
		tween.TweenProperty(this,"position",Position + 20*Vector2.Right,0.3f);
		tween.TweenProperty(this,"position",Position + 20*Vector2.Left,0.2f);
	}
	
	public override void Dying()
	{
		BattleNode.EnemiesDyingNum++;
		base.Dying();
	}
}
