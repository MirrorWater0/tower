using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;

public partial class FollowingLight : Skill
{	
    public FollowingLight(Charater owner):base(Skill.SkillTypes.Attack,owner){}
    public override string SkillName { set; get; } = "流光日影";

    public async override void Effect()
    {
        base.Effect();
        Attack1(1);
        Charater[] targets = (OwnerCharater.IsPlayer) switch
        {
            true => OwnerCharater.BattleNode.Players, false => OwnerCharater.BattleNode.Enemies,
        };
        var MaxNum = targets.Max(x => x.PositionIndex);
        var target = targets.Where(x => x.PositionIndex == MaxNum).ToArray();

        await Task.Delay(1000);
        if(target[0].State != Charater.CharaterState.Dying & target[0] != OwnerCharater) Carry(target[0], 0);
        else OwnerCharater.EndAction();
    }
}
