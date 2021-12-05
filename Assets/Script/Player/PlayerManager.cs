using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    Player M_Playerobj => Player.Instance;
    CameraManager M_Camera => CameraManager.Instance;
    public bool ActiveSelf()
	{
        return M_Playerobj.ActiveSelf;
	}
    public void SetActivePlayer(bool active)
	{
        M_Playerobj.gameObject.SetActive(active);
	}
    public void SetPos(float x, float y,float z)
	{
        M_Playerobj.SetPlayerPos(x,y,z);
	}
    public Vector3 GetPos()
	{
        return M_Playerobj.GetPlayerPos;
	}
    public GameObject GetObjPlayer
    {
        get { return M_Playerobj.gameObject; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ActiveSelf())
		{
            M_Camera.Movecheck = true;
		}
    }
}
