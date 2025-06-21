using Godot;
using System;

public partial class SkillButton : Button
{
    public Frame SelfFrame => field ??= GetParent().GetParent() as Frame;
    public VBoxContainer SelfContainer => field ??= GetParent() as VBoxContainer;
    public Label NameLabel => field ??= GetChild(0) as Label;
    public Area2D Detector => field??= GetChild(1) as Area2D;
    public Vector2 PositionIndex;
    public Skill SelfSkill;
}
