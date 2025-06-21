using Godot;
using System;
using System.Collections.Generic;	
using System.Linq;
using System.Threading.Tasks;

public partial class Buff
{

	public enum BuffType
	{
		Rebirth,
		DamageImmune,
	}
	public Charater Owner;
	public Charater[] Target;
	public BuffType Type;
	public int Stack;
	public Buff(Charater owner, Charater[] target, BuffType type, int stack)
	{
		Owner = owner;
		Target = target;
		Type = type;
		Stack = stack;
	}

	public async void DyingEffect()
	{
		switch (Type)
		{
			case BuffType.Rebirth:
				await Task.Delay(200);
				Target[0].CreateTween().TweenProperty(Target[0], "modulate", new Color(1,1,1,1), 0.5f);
				Target[0].Recovery(Target[0].BattleLifemax);
				Target[0].State = Charater.CharaterState.Normal;
				GD.Print("Rebirth",Owner.Life,"array");
				break;
		}
	}

	public void HurtEffect(ref float damage)
	{
		switch (Type)
		{
			case BuffType.DamageImmune :
				damage = 0;
				break;
		}
	}
	
}
