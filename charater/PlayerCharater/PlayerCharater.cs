using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class PlayerCharater : Charater
{
	public Frame SelfFrame;
	public Control SkillButton;
	public List<Skill> UntakeSkills;
	public bool Istest;
	public async override void Initialize()
	{
		base.Initialize();
		GD.Print("read",BattleNode);
		IsPlayer = true;
		await Task.Delay(200);//等待CharaterControl执行connnect
		GD.Print("battle:",BattleNode);
		SelfFrame.CombNum.Text = ComboAbleNum.ToString();
		SelfFrame.CarryNum.Text = CarryAbleNum.ToString();
		SelfFrame.Power.Text = BattlePower.ToString();
		SelfFrame.Defence.Text = BattleDefense.ToString();
	}
	

	public override void StartAction()
	{
		base.StartAction();
		for (int j = 0; j < SkillButton.GetChildCount(); j++)
		{
			SkillButton.GetChild<Button>(j).Disabled = false;
			SkillButton.GetChild<Button>(j).GetChild<Label>(0).Modulate = new Color(1, 1, 1, 1f);
			
		}

		BattleNode.RetreatButton.Disabled = false;
		UpdateComboNumber(0);
		UpdateCarryNumber(0);
	}

	public override void EndAction()
	{
		BattleNode.RetreatButton.Disabled = true;
		DisableSkill();
		base.EndAction();
	}

	public override void GetHurt(float damage)
	{
		base.GetHurt(damage);
		Tween tween = CreateTween();
		tween.TweenProperty(this,"position",Position + 20*Vector2.Left,0.3f);
		tween.TweenProperty(this,"position",Position + 20*Vector2.Right,0.2f);
	}

	public override void DisableSkill()
	{
		GD.Print("ButtonNum:",SkillButton.GetChildCount());
		for (int j = 0; j < SkillButton.GetChildCount(); j++)
		{
			SkillButton.GetChild<Button>(j).Disabled = true;
			SkillButton.GetChild<Button>(j).GetChild<Label>(0).Modulate = new Color(1, 1, 1, 0.3f);
		}
	}

	public override void Dying()
	{
		BattleNode.PlayerDyingNum++;
		base.Dying();
	}

	public override void UpdateComboNumber(int num)
	{
		ComboAbleNum += num;
		SelfFrame.CombNum.Text = ComboAbleNum.ToString();
	}
	
	public override void UpdateCarryNumber(int num)
	{
		CarryAbleNum += num;
		SelfFrame.CarryNum.Text = CarryAbleNum.ToString();
	}
}
