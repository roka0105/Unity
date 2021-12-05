using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region 데이터 구조체 모음 *나중에 해당하는 클래스 위에 배치하기*
public struct StageData
{
	public BlockData m_BlockData;
	public MapData m_MapData;
	public PlayerData m_PlayerData;
	public EnemyData m_EnemyData;
}
public struct GunData
{
	public int type;//enum으로 나중에 바꾸기 임시로 int
	public int m_TotalBullet;
	public float m_AtkRange;//사거리
	public float m_Atk;
	public float m_AtkSpeed;
	public float m_CoolTime;//장전 쿨타임.
}
public struct EnemyData
{
	public int m_TotalEnemy;
	public int type;//이것도 enum으로 나중에 몬스터 종류별로 나눌때 사용.
	public float m_Range;//인식 사거리
	public float m_AtkRange;
	public float m_Hp;
	public float m_Speed;
	public float m_Atk;
	public float m_AtkSpeed;
	public SkillData m_Skill;
}
public struct PlayerData
{
	public float m_Hp;
	public float m_Speed;
	public float m_AtkRange;//기본공격이라 고정값 1로 함.
	public float m_Atk;
	public float m_AtkSpeed;
	public SkillData m_Skill;
}
public struct MapData
{
	public Vector3 m_Startpos;
	public Vector3 m_Endpos;
	public List<Vector3> m_CantRespawnPos;
}
public struct BlockData
{
	public float m_Hp;//부셔지는 블럭
	public float m_Speed;//움직이는 블럭
}
public struct SkillData
{
	public float m_SkillCoolTime;
	public float m_SkillDmg;
	public float m_SkillRange;
	public float m_MaxChargeSkill;
	public float m_SkillDelay;//연속사용되는 스킬 딜레이.
}
#endregion
public class GameManager : Singleton<GameManager>
{
	MapManager M_Map => MapManager.Instance;
	PlayerManager M_Player => PlayerManager.Instance;
	#region 맵 매니저에 줄 정보
	[SerializeField]
	Vector3 m_Startpos;
	[SerializeField]
	Vector3 m_Endpos;
	[SerializeField]
	Vector3 m_RespawnPos;

	public bool testflag;
	int nowstage;

	List<StageData> m_StageData;
	public Vector3 GetStartPos
	{
		get { return m_Startpos; }
	}
	public Vector3 GetEndPos
	{
		get { return m_Endpos; }
	}
	#endregion
	#region 블럭 매니저에 줄 정보
	[SerializeField]
	private float InitBlockHP;
	[SerializeField]
	private int InitBlockROW;
	[SerializeField]
	private int InitBlockCOL;
	[SerializeField]
	private Block BlockPrefeb;
	#region 프로퍼티
	public int Stageindex
	{
		get { return nowstage; }
		set { nowstage = value; }
	}
	public Block BlockOriginal
	{
		get => BlockPrefeb;
	}
	public float GetBlockHP
	{
		get => InitBlockHP;
	}
	public int GetBlockRow
	{
		get => InitBlockROW;
	}
	public int GetBlockCol
	{
		get => InitBlockROW;
	}
	#endregion
	#endregion
	//스테이지 리스트에서 스테이지 관련 정보 읽어와서 현재 게임 상태로 적용하기.
	void LoadStageData()
	{
		LoadMapdata();
	}
	void LoadMapdata()
	{
		if (!testflag)
		{
			m_Startpos = m_StageData[nowstage].m_MapData.m_Startpos;
			m_Endpos = m_StageData[nowstage].m_MapData.m_Endpos;
		}
	}
	[ContextMenu("맵 생성")]
	void CreateMap()
	{
		if (m_StageData.Count <= 0)
		{
			Debug.Log("stagedata list null");
			return;
		}
		LoadStageData();
		M_Map.CreateFixBlock();
		while(true)
		{
			RandomPlayerPos();
			GameObject player = M_Player.GetObjPlayer;
			if (!RaycastSpawn(player))
			{
				break;
			}
		}
	}
	[ContextMenu("맵 삭제")]
	void DeleteMap()
	{
		//blocklist type 0:ground 1:move 2:crush 3:wall
		for (int i = 0; i < M_Map.BlocklistTypeCount; ++i)
		{
			M_Map.DeleteBlocklist(i);
		}
		M_Player.SetActivePlayer(false);
	}
	//벽속에서 스폰됐는지 체크 참이면 다시 위치 받기.
	bool RaycastSpawn(GameObject obj)
	{
		int mask = 1 << 7;
		float radius = obj.transform.localScale.x/2;
		RaycastHit hitinfo;

		if(Physics.Raycast(obj.transform.position,transform.forward,out hitinfo,radius,mask))
		{
			return true;
		}
        if(Physics.Raycast(obj.transform.position, -transform.forward, out hitinfo, radius, mask))
		{
			return true;
		}
		if(Physics.Raycast(obj.transform.position, transform.right, out hitinfo, radius, mask))
		{
			return true;
		}
		if(Physics.Raycast(obj.transform.position, -transform.right, out hitinfo, radius, mask))
		{
			return true;
		}
		return false;
	}
	void RandomPlayerPos()
	{
		float x;
		float z;
		float y = 0;
		//while (true)
		//{
		x = Random.Range(m_Startpos.x + 20, m_Endpos.x + 20);
		y = m_RespawnPos.y;
		z = Random.Range(m_Startpos.z - 5, m_Endpos.z + 5);

		M_Player.SetPos(x, y, z);

		M_Player.SetActivePlayer(true);
		//코루틴+이벤트로 플레이어가 떨어진 곳의 y 값이 2 이상인지 체크 이상이면 벽 위로 스폰된것으로 다시 위치 셋팅받기.

		//break;


		//}

	}
	public void AddCantRespawnList(Vector3 pos)
	{
		m_StageData[nowstage].m_MapData.m_CantRespawnPos.Add(pos);
	}
	private void Start()
	{
		m_RespawnPos.y = -20;
		m_StageData = new List<StageData>();
		StageData temp1 = new StageData();
		//StageData temp2 = new StageData();
		//StageData temp3=new StageData();
		#region 맵 정보 초기화

		temp1.m_MapData.m_Startpos.x = -33;
		temp1.m_MapData.m_Startpos.z = 5;
		temp1.m_MapData.m_Endpos.x = 57;
		temp1.m_MapData.m_Endpos.z = -41;
		temp1.m_MapData.m_CantRespawnPos = new List<Vector3>();

		#endregion

		#region 스테이지 블럭 정보 초기화
		temp1.m_BlockData.m_Hp = 20;
		#endregion

		#region 플레이어 정보 초기화
		temp1.m_PlayerData.m_Atk = 5;
		temp1.m_PlayerData.m_AtkSpeed = 1.2f;
		temp1.m_PlayerData.m_AtkRange = 1f;
		temp1.m_PlayerData.m_Hp = 100;
		temp1.m_PlayerData.m_Speed = 3f;
		//temp1.m_PlayerData.m_SkillCoolTime = 5f;
		//temp1.m_PlayerData.m_SkillRange = 15f;
		#endregion
		#region 적 정보 초기화
		//* 임시로 한가지 타입의 적을 사용하려고 여기서 초기화 하는데 원래는 적 타입별로 pool생성할때 기본값 설정해야함.
		//스테이지 정보에서 가져와야할건 스테이지 1에서는 어떤 타입의 몬스터가 출현할지에 대한것.
		temp1.m_EnemyData.m_TotalEnemy = 5;
		temp1.m_EnemyData.m_Speed = 2.5f;
		temp1.m_EnemyData.m_Atk = 10;
		temp1.m_EnemyData.m_AtkSpeed = 2f;
		temp1.m_EnemyData.m_Hp = 15;
		temp1.m_EnemyData.m_AtkRange = 4f;
		temp1.m_EnemyData.m_Range = 10f;
		temp1.m_EnemyData.m_Skill.m_SkillCoolTime = 5f;
		temp1.m_EnemyData.m_Skill.m_SkillRange = 10f;
		#endregion
		m_StageData.Add(temp1);
	}
}
