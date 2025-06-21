using Godot;
using System;
using System.Threading.Tasks;

public partial class Kasiya : PlayerCharater
{
    Label label => field ??= GetNode<Label>("Label");
    public override PackedScene CharaterScene { get; set; } = GD.Load<PackedScene>("res://charater/PlayerCharater/Kasiya/kasiya.tscn");
    public override string CharaterName { get; set; }="Kasiya";

    public override void Initialize()
    {
        base.Initialize();
        if(Istest) test();
        
        
        BattleNode.UsedSkills.ItemAdded += item => { Passive(item);};
    }

    public void test()
    {
        Skills = new Skill[] { new ReNewedSpirit(this),new Determination(this),new Defense(this),new Smite(this) };
        BattleLifemax = 3000;
        BattlePower = 1300;
        BattleDefense = 1700;
        Speed = 20;
    }

    public override void Passive(Skill skill)
    {
        if (skill.OwnerCharater == this & skill.SkillType == Skill.SkillTypes.Defence)
        {
            Recovery(500);
        }
    }
}
