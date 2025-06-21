using Godot;
using System;

public partial class HurtBuff
{
    public enum BuffType
    {
        Rebirth,
    }
    public Charater Owner;
    public Charater[] Target;
    public BuffType Type;
    public int Stack;
    public HurtBuff(Charater owner, Charater[] target, BuffType type, int stack)
    {
        Owner = owner;
        Target = target;
        Type = type;
        Stack = stack;
    }
}
