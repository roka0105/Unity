using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager :Singleton<CameraManager>
{
    protected PlayerManager M_Player => PlayerManager.Instance;
	bool movecheck;
    public Vector3 GetPos()
	{
        Vector3 temp= M_Player.GetPos();
        return temp;
	}
	public bool Movecheck
	{
		get { return movecheck; }
		set { movecheck = value; }
	}
	private void Update()
	{
	
	}
}
