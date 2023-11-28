using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Transform[] Points;
    public SpawnData[] spawnData;
    [HideInInspector] public float spawnTime; //��ȯ ���� ���� ����

    int level;              //��ȯ ����
    [HideInInspector] public float timer;     //���� Ÿ�̸�

    //���� ���̺�
    public float waveTime;  //���̺� ��ȯ ���� 
    public float waveTimer; //���̺� ���� Ÿ�̸�
    public int waveLength;

    void Awake()
    {
        Points = GetComponentsInChildren<Transform>();
        spawnTime = Game_Mgr.Inst.maxGameTime / spawnData.Length;
        //���� �ð� = �ִ� ���� �ð� / ���� Ÿ�� ��

        waveTime = Game_Mgr.Inst.maxGameTime / waveLength;
        //���̺� �ֱ� = �ִ� ���� �ð� / ��
    }

    void Update()
    {
        if (!Game_Mgr.Inst.isLive)
            return;

        timer += Time.deltaTime;
        waveTimer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(Game_Mgr.Inst.gameTime / spawnTime), spawnData.Length - 1);  //�ε��� ���� ��ħ

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
            //Debug.Log("��");
            spawnData[level].c_spawnTime = 0.7f;
        }

        if (spawnData[level].c_spriteType == 1)
        {
            //Debug.Log("��");
            spawnData[level].c_spawnTime = 0.5f;
        }

        if (spawnData[level].c_spriteType == 2)
        {
            //Debug.Log("��");
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

[System.Serializable]
public class SpawnData
{
    public float c_spawnTime; //���� �ð�
    public int c_spriteType;  //���� Ÿ��
    public int c_hp;          //ü��
    public float c_sp;        //�ӵ�
    public int c_damage;      //������
}
