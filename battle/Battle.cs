using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

public partial class Battle : Node2D
{
	static public bool Istest = true;
	[Signal]
	public delegate void NextEventHandler();
	PackedScene _echo = (PackedScene)ResourceLoader.Load("res://charater/PlayerCharater/Echo/Echo.tscn");
	PackedScene _test1 = (PackedScene)ResourceLoader.Load("res://charater/EnemyCharater/EnemyTemplate.tscn");
	private PackedScene _kasiya = ResourceLoader.Load<PackedScene>("res://charater/PlayerCharater/Kasiya/kasiya.tscn");
	public PlayerCharater[] Players = new PlayerCharater[]{};
	public Charater[] Enemies;
	public Node2D Right => field??= GetNode("Right") as Node2D;
	public Node2D Left => field??= GetNode("Left") as Node2D;
	
	public AnimationPlayer BattlePlayer => field??= GetNode("BattlePlayer") as AnimationPlayer;
	private int _turn;
	public CharaterControl CharaterControl => field??= GetNode("CharaterControl") as CharaterControl;

	public int PlayerDyingNum;
	public int EnemiesDyingNum;
	
	public ObservableList<Skill> UsedSkills = new ObservableList<Skill>();
	public Button RetreatButton ;
	
	public async override void _Ready()
	{
		RetreatButton = GetNode("Retreat") as Button;
		RetreatButton.ButtonDown += Retreat;
		UsedSkills.Clear();
		if(Istest) test();
		else
		{
			GD.Print(PlayerInfo.PlayerCharaters.Length);
			Players = PlayerInfo.PlayerCharaters.ToArray();
		}
		
		EnemyTemplate test1 = _test1.Instantiate<EnemyTemplate>();
		test1.PositionIndex = 1;
		EnemyTemplate test2 = _test1.Instantiate<EnemyTemplate>();
		test2.PositionIndex = 5;
		EnemyTemplate test3 = _test1.Instantiate<EnemyTemplate>();
		test3.PositionIndex = 2;
		EnemyTemplate test4 = _test1.Instantiate<EnemyTemplate>();
		test4.PositionIndex = 3;
		Enemies = new Charater[] { test1,test2,test3,test4 };

		for (int i = 0; i < Players.Length; i++)
		{
			Players[i].BattleNode = this;
			Players[i].Initialize();
		}


		
		Players = Players.OrderBy(x => x.PositionIndex).ToArray();
		Enemies = Enemies.OrderBy(x => x.PositionIndex).ToArray();
		SetCharaterPostion();//加入节点树
		
		for (int i = 0; i < Enemies.Length; i++)
		{
			Enemies[i].BattleNode = this;
			Enemies[i].Initialize();
		}
		
		await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
		
		CharaterControl.DisableAll();
		for (int i = 0; i < Players.Length; i++)
		{
			GD.Print(Players[i].State);
		}
		await Task.Delay(1000);

		BattleBegin1();
		
	}
	
	public void SetCharaterPostion()
	{
		Vector2 gapy = new Vector2(0, 160);
		Vector2 gapx = new Vector2(280, 0);
		for (int j = 0; j < Players.Length; j++)
		{
			int i = Players[j].PositionIndex;
			Left.AddChild(Players[j]);
			if (i <= 3)
			{
				Players[j].Position = gapy * (i-1);
			}
			else if(i <= 6)
			{
				Players[j].Position = -gapx + gapy * (i-4);
			}
			else
			{
				Players[j].Position = -2*gapx + gapy * (i-7);
			}
		}
		
		for (int j = 0; j < Enemies.Length; j++)
		{
			int i = Enemies[j].PositionIndex;
			Right.AddChild(Enemies[j]);
			if (i <= 3)
			{
				Enemies[j].Position =gapy * (i-1);
			}
			else if(i <= 6)
			{
				Enemies[j].Position =gapx + gapy * (i-4);
			}
			else
			{
				Enemies[j].Position =2*gapx + gapy * (i-7);
			}
		}
		
	}

	public void EmitS()
	{
		EmitSignal(SignalName.Next);
	}

	public async void BattleBegin1()
	{
		for(int i = 0; i < 100; i++){
			GD.Print("turn ",i);
			DyingDetector(Players);
			Players[0].StartAction();
			Array.Reverse(Players,1,Players.Length-1);
			Array.Reverse(Players);
			
			await ToSignal(this,SignalName.Next);
			await Task.Delay(300);
			
			if (Players.All(x => x.State == Charater.CharaterState.Dying) ||
			    Enemies.All(x => x.State == Charater.CharaterState.Dying))
			{
				GD.Print("over");
				return;
			}
			
			DyingDetector(Enemies);
			Enemies[0].StartAction();
			Array.Reverse(Enemies,1,Enemies.Length-1);
			Array.Reverse(Enemies);
			
			await ToSignal(this,SignalName.Next);
			await Task.Delay(1500);
			
			if (Players.All(x => x.State == Charater.CharaterState.Dying) ||
			    Enemies.All(x => x.State == Charater.CharaterState.Dying))
			{
				GD.Print("over");
				return;
			}
		}


	}

	public void DyingDetector(Charater[] c)
	{

		while(true){ 
			if (c[0].State == Charater.CharaterState.Dying)
			{
				Array.Reverse(c,1,c.Length-1);
				Array.Reverse(c);
				GD.Print("complate");
			}
			else
			{
				break;
			}
			
		}

	}

	public async void Retreat()
	{
		BattlePlayer.Play("end");
		for (int i = 0; i < Players.Length; i++)
		{
			Players[i].State = Charater.CharaterState.Dying;
		}
		
		EmitS();
		
		await Task.Delay(1500);
		for (int i = 0; i < Players.Length; i++)
		{
			Players[i].State = Charater.CharaterState.Normal;
			Left.RemoveChild(Players[i]);
		}
		GetTree().ChangeSceneToFile("res://Map/Map.tscn");
	}
	public void test()
	{
		Echo echo = _echo.Instantiate<Echo>();
		echo.PositionIndex = 7;
		Kasiya kasiya = _kasiya.Instantiate<Kasiya>();
		kasiya.PositionIndex = 2;
		Echo echo3 = _echo.Instantiate<Echo>();
		echo3.PositionIndex = 4;
		Echo echo4 = _echo.Instantiate<Echo>();
		echo4.PositionIndex = 6;
		
		Players = new PlayerCharater[] { echo,kasiya,echo3 ,echo4};
		for (int i = 0; i < Players.Length; i++)
		{
			Players[i].Istest = true;
		}
	}
	

	
	public class ObservableList<T> : List<T>
	{
		public event Action<T> ItemAdded;   // 新增元素事件
		public event Action<T> ItemRemoved; // 移除元素事件

		public new void Add(T item)
		{
			base.Add(item);
			ItemAdded?.Invoke(item); // 触发新增回调
		}

		public new void Remove(T item)
		{
			base.Remove(item);
			ItemRemoved?.Invoke(item); // 触发移除回调
		}
    
		// 可扩展其他方法（如 Insert、Clear 等）
	}

}
