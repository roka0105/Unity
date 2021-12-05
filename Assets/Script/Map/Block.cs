using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
	
	float HP;
	int Type;
	int Position;
    protected BlockManager M_Block => BlockManager.Instance;
	protected MapManager M_Map => MapManager.Instance;
	public int P_Type
	{
		get{ return Type; }
		set{ Type = value; }
	}
	public int P_Position
	{
		get { return Position;}
		set { Position = value; }
	}
	public float P_HP
	{
		get { return HP; }
		set { HP = value; }
	}

	private void Awake()
	{
		Type = -1;
		Position = -1;
	}

}
