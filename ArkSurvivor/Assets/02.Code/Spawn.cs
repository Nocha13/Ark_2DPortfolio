using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Transform[] Points;
    public SpawnData[] spawnData;
    [HideInInspector] public float spawnTime; //소환 레벨 구간 결정

    int level;              //소환 레벨
    [HideInInspector] public float timer;     //스폰 타이머

    //몬스터 웨이브
    public float waveTime;  //웨이브 소환 구간 
    public float waveTimer; //웨이브 스폰 타이머
    public int waveLength;

    void Awake()
    {
        Points = GetComponentsInChildren<Transform>();
        spawnTime = Game_Mgr.Inst.maxGameTime / spawnData.Length;
        //스폰 시간 = 최대 게임 시간 / 몬스터 타입 수

        waveTime = Game_Mgr.Inst.maxGameTime / waveLength;
        //웨이브 주기 = 최대 게임 시간 / 수
    }

    // ORDER : 04#) 시간에 따른 적 데이터
    void Update()
    {
        if (!Game_Mgr.Inst.isLive)
            return;

        timer += Time.deltaTime;
        waveTimer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(Game_Mgr.Inst.gameTime / spawnTime), spawnData.Length - 1);  //인덱스 에러 고침

        if (timer > spawnData[level].c_spawnTime)
        {
            timer = 0;
            Spawns();
        }

        if (waveTimer > waveTime)
        {
            StartCoroutine(Wave());

            waveTimer = 0;
        }
    }

    IEnumerator Wave()
    {
        spawnData[level].c_spawnTime = 0.03f;

        yield return new WaitForSeconds(1);

        if (spawnData[level].c_spriteType == 0)
        {
            //Debug.Log("웨");
            spawnData[level].c_spawnTime = 0.7f;
        }

        if (spawnData[level].c_spriteType == 1)
        {
            //Debug.Log("이");
            spawnData[level].c_spawnTime = 0.5f;
        }

        if (spawnData[level].c_spriteType == 2)
        {
            //Debug.Log("브");
            spawnData[level].c_spawnTime = 0.3f;
        }

        yield break;
    }

    void Spawns()
    {
        GameObject enemy = Game_Mgr.Inst.pool.Get(0);
        enemy.transform.position = Points[Random.Range(1, Points.Length)].position;
        enemy.GetComponent<EnemyCtrl>().Init(spawnData[level]);
    }
}

[System.Serializable] //데이터 직렬화
public class SpawnData
{
    public float c_spawnTime; //스폰 시간
    public int c_spriteType;  //몬스터 타입
    public int c_hp;          //체력
    public float c_sp;        //속도
    public int c_damage;      //데미지
}
