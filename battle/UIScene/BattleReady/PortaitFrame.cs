using Godot;
using System;

public partial class PortaitFrame : Node2D
{
    public Charater Charater;
    public Button PortaitButton => field??= GetNode("Button") as Button;
    public TextureRect PortaitRect => field??= GetNode("TextureRect") as TextureRect;
}
