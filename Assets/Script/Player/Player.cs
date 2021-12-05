using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    PlayerManager M_Player => PlayerManager.Instance;
    Vector3 PlayerPos;
    float Speed;
    float Scale;
    public Vector3 GetPlayerPos
    {
        get { return PlayerPos; }
    }
    public bool ActiveSelf
    {
        get
        {
            return this.gameObject.activeSelf;
        }
    }
    public void SetPlayerPos(float x, float y, float z)
    {
        PlayerPos = this.transform.position;
        PlayerPos.x = x;
        PlayerPos.y = y;
        PlayerPos.z = z;
        this.transform.position = PlayerPos;
        //transform.localposition = PlayerPos;
    }
    //dir 0:x 1:z
    float RayCast(int dir, float number)
    {
        RaycastHit hitinfo;
        Vector3 temp = this.transform.position;
        float radius = Scale / 2;
        int mask = 1 << 7;
        switch (dir)
        {   //x
            case 0:
                if (number > 0)
                {

                    if (Physics.Raycast(temp, transform.right, out hitinfo, number + radius, mask))
                    {
                        Debug.DrawRay(temp, transform.right * (number + radius), Color.blue, float.PositiveInfinity);
                        //temp.x = temp.x + number - hitinfo.point.x;
                        temp.x = hitinfo.point.x - (temp.x + radius);
                        return temp.x;
                    }
                }
                else if (number < 0)
                {

                    if (Physics.Raycast(temp, -(transform.right), out hitinfo, -number + radius, mask))
                    {
                        Debug.DrawRay(temp, transform.right * (number - radius), Color.blue, float.PositiveInfinity);
                        //temp.x = temp.x - number + hitinfo.point.x;
                        temp.x = hitinfo.point.x - (temp.x - radius);
                        return temp.x;
                    }
                }
                return number;
                break;
            //z
            case 1:
                if (number > 0)
                {

                    if (Physics.Raycast(temp, transform.forward, out hitinfo, number + radius, mask))
                    {
                        Debug.DrawRay(temp, transform.forward * (number + radius), Color.blue, float.PositiveInfinity);
                        //temp.x = temp.x + number - hitinfo.point.x;
                        temp.z = hitinfo.point.z - (temp.z + radius);
                        return temp.z;
                    }
                }
                else if (number < 0)
                {

                    if (Physics.Raycast(temp, -(transform.forward), out hitinfo, -number + radius, mask))
                    {
                        Debug.DrawRay(temp, transform.forward * (number - radius), Color.blue, float.PositiveInfinity);
                        //temp.x = temp.x - number + hitinfo.point.x;
                        temp.z = hitinfo.point.z - (temp.z - radius);
                        return temp.z;
                    }
                }
                return number;
                break;
        }
        return 0;
    }
    public void Move()
    {
        Vector3 moveAfterpt = new Vector3(0, 0, 0);
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
       // {
            moveAfterpt.x = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;

       // }
       // if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        //{
            moveAfterpt.z = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
            if (moveAfterpt.z != 0)
                moveAfterpt.z = RayCast(1, moveAfterpt.z);
        //}

        //PlayerPos.x += moveAfterpt.x;
        //PlayerPos.z += moveAfterpt.z;
        if (moveAfterpt.x != 0)
            moveAfterpt.x = RayCast(0, moveAfterpt.x);
        if (moveAfterpt.z != 0)
            moveAfterpt.z = RayCast(1, moveAfterpt.z);
        if (moveAfterpt.x != 0 || moveAfterpt.z != 0)
        {
            PlayerPos += transform.forward * moveAfterpt.z + transform.right * moveAfterpt.x;
            //if (moveAfterpt.z != 0)
            //    PlayerPos.z += transform.position.z;
            //if (moveAfterpt.x != 0)
            //    PlayerPos.x += transform.position.x;
        }//this.transform.position = PlayerPos;
        PlayerPos.y = this.transform.position.y;
        this.transform.position = PlayerPos;
    }
    void Start()
    {
        PlayerPos = this.transform.position;
        Speed = 3f;
        Scale = this.transform.localScale.x;
    }
    void Update()
    {
       Move();
    }
}
