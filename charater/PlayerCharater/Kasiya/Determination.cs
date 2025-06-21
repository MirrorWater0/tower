using Godot;
using System;

public partial class Determination : Skill
{
    public Determination(Charater owner): base(SkillTypes.Defence, owner) 
    {
        OwnerCharater = owner;
    }

    public override string SkillName { get; set; } = "剑意已决";
    public override void Effect()
    {
        base.Effect();
        Attack1(0.5f);
        OwnerCharater.HurtBuffs.Add(new Buff(OwnerCharater,[],Buff.BuffType.DamageImmune,1));
        OwnerCharater.UpdataBlock((int)(OwnerCharater.BattleDefense*0.5f));
        OwnerCharater.EndAction();
    }
}
