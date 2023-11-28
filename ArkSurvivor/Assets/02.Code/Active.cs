using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    public int id;          //����
    public int prefabId;    //������
    public float damage;    //������
    public int count;       //����
    public float sp;        //�ӵ�(�ٰŸ� - ȸ��, ���Ÿ� - ����ӵ�)

    PlayerCtrl player;     //�÷��̾� ����
    SpriteRenderer sprite; //�÷��̾� ��������Ʈ

    float timer;    //��Ÿ��

    [HideInInspector] public bool isShield = false;

    public static Active Inst;

    void Awake()
    {
        Inst = this;
        player = Game_Mgr.Inst.player;    //�θ� ������Ʈ ������
        sprite = PlayerCtrl.Inst.spriter;
    }

    void Update()
    {
        if (!Game_Mgr.Inst.isLive)
            return;

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * sp * Time.deltaTime);
                break;

            case 1:
                timer += Time.deltaTime;
                if (timer > sp)
                {
                    Fire();
                    timer = 0;
                }
                break;

            case 5:
                timer += Time.deltaTime;
                if (timer > sp)
                {
                    RandomFire();
                    timer = 0;
                }
                break;

            case 6:
                transform.Rotate(Vector3.zero);
                if (GameObject.Find("Active 6").transform.Find("Shield(Clone)").gameObject.activeSelf == false)
                {
                    //Debug.Log("��Ȱ��");
                    timer += Time.deltaTime;
                    if (timer > sp)
                    {
                        //Debug.Log("Ȱ��");
                        Shield();
                        timer = 0;
                    }
                }
                break;

            case 7:
                timer += Time.deltaTime;
                if (timer > sp)
                {
                    FrontFire();
                    timer = 0;
                }
                break;

            case 8:
                timer += Time.deltaTime;
                if (timer > sp)
                {
                    Spike();
                    timer = 0;
                }
                break;

            case 9:
                timer += Time.deltaTime;
                if (timer > sp)
                {
                    Explosion();
                    timer = 0;
                }
                break;

            case 10:
                timer += Time.deltaTime;
                if (timer > sp)
                {
                    Lightning();
                    timer = 0;
                }
                break;
        }
    }

    public void Init(ItemDatas data)
    {
        #region //������Ʈ ����
        //�⺻ ����
        name = "Active " + data.itemId;
        transform.parent = player.transform;    //�θ� = �÷��̾�
        transform.localPosition = Vector3.zero; //���� ��ġ ����

        //���� ����
        id = data.itemId;
        damage = data.baseDamage * Characters.Damage;
        count = data.baseCount + Characters.Count;

        //��ũ��Ʈ ������Ʈ ������(�δ콺 �ƴ� ���������� ������)
        for (int idx = 0; idx < Game_Mgr.Inst.pool.prefabs.Length; idx++)
        {
            if (data.projectile == Game_Mgr.Inst.pool.prefabs[idx])
            {
                prefabId = idx;
                break;
            }
        }
        #endregion
        switch (id)
        {
            case 0:
                sp = 150 * Characters.RotSp;
                Place();
                break;

            case 1:
                sp = 0.7f * Characters.FireCool;
                break;

            case 5:
                sp = 0.3f * Characters.FireCool;
                break;

            case 6:
                sp = 9 * Characters.FireCool;
                Shield();
                break;

            case 7:
                sp = 2.5f * Characters.FireCool;
                break;

            case 8:
                sp = 3.5f * Characters.FireCool;
                break;

            case 9:
                sp = 80f;
                break;

            case 10:
                sp = 7f * Characters.FireCool;
                break;
        }

        //Ư�� �Լ� ȣ�� �ڽ� ������Ʈ�� �˸�
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    #region //���� ���� 
    void Place()//��
    {
        for (int idx = 0; idx < count; idx++)
        {
            Transform bullet;

            if (idx < transform.childCount)
            {
                bullet = transform.GetChild(idx);
            }
            else //���� ������Ʈ ���� Ȱ�� ��, Ǯ������ ����(�߰�)����
            {
                bullet = Game_Mgr.Inst.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * idx / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullets>().Init(damage, -100); // -100 �����         
        }
    }

    void Fire()//Ÿ�ٸ���
    {
        if (!player.scanner.nearestTarget)  //��ǥ ������ ����
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;//Ÿ��
        Vector3 dir = targetPos - transform.position;             //����
        dir = dir.normalized;   //���� ���� ����, ũ�� 1

        Transform bullet = Game_Mgr.Inst.pool.Get(prefabId).transform;
        bullet.position = transform.position;

        //���� �� �߽����� ��ǥ ���� ȸ��
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullets>().Init(damage, count, dir);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Range);
    }

    void RandomFire()//��������
    {
        Vector3 dir = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), 0);  //����
        dir = dir.normalized;   //���� ���� ����, ũ�� 1

        Transform bullet = Game_Mgr.Inst.pool.Get(prefabId).transform;
        bullet.position = transform.position;

        bullet.GetComponent<Bullets>().Init(damage, count, dir);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Range);
    }

    void FrontFire()//����Ʈ ����
    {
        Vector3 dir = Vector3.right;

        if (sprite.flipX == true)
        {
            dir = Vector3.left;
        }

        dir = dir.normalized;   //���� ���� ����, ũ�� 1

        Transform bullet = Game_Mgr.Inst.pool.Get(prefabId).transform;
        bullet.position = transform.position;

        if (sprite.flipX == true)
        {
            dir = Vector3.left;
            bullet.localScale = new Vector3(-3, 3, 3);
        }
        else
        {
            dir = Vector3.right;
            bullet.localScale = new Vector3(3, 3, 3);
        }

        bullet.GetComponent<Bullets>().Init(damage, count, dir);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Front);
    }
    void Lightning()//����
    {
        Transform bullet = Game_Mgr.Inst.pool.Get(prefabId).transform;
        bullet.position = player.transform.position + (Random.insideUnitSphere * 7f);
        bullet.GetComponent<Bullets>().Init(damage, count);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Lightning);
    }
    #endregion

    #region //���� ����
    void Spike()//������ũ
    {
        Transform bullet = Game_Mgr.Inst.pool.Get(prefabId).transform;
        bullet.position = player.transform.position + (Random.insideUnitSphere * 5f);
        bullet.GetComponent<Bullets>().Init(damage, count);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Spike);
    }
    #endregion

    #region//����
    void Shield()//����
    {
        Transform shleid;

        shleid = Game_Mgr.Inst.pool.Get(prefabId).transform;
        shleid.parent = transform;
        shleid.position = transform.position;

        shleid.GetComponent<Bullets>().Init(-100);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Shield);
    }

    void Explosion()//����
    {
        Transform bullet = Game_Mgr.Inst.pool.Get(prefabId).transform;
        bullet.position = transform.position;

        bullet.GetComponent<Bullets>().Init(damage, count);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Explosion);
    }
    #endregion

    public void LevelUp(float a_damage, int a_count)
    {
        damage = a_damage * Characters.Damage;
        count += a_count;

        if (id == 0)
            Place();

        //Ư�� �Լ� ȣ�� �ڽ� ������Ʈ�� �˸�
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
}
