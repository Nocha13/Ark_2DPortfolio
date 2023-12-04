using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    public int id;          //무기
    public int prefabId;    //프리팹
    public float damage;    //데미지
    public int count;       //갯수
    public float sp;        //속도(근거리 - 회전, 원거리 - 연사속도)

    PlayerCtrl player;     //플레이어 변수
    SpriteRenderer sprite; //플레이어 스프라이트

    float timer;    //쿨타임

    [HideInInspector] public bool isShield = false;

    public static Active Inst;

    void Awake()
    {
        Inst = this;
        player = Game_Mgr.Inst.player;    //부모 컴포넌트 가져옴
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
                    //Debug.Log("비활성");
                    timer += Time.deltaTime;
                    if (timer > sp)
                    {
                        //Debug.Log("활성");
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
        #region //오브젝트 설정
        //기본 설정
        name = "Active " + data.itemId;
        transform.parent = player.transform;    //부모 = 플레이어
        transform.localPosition = Vector3.zero; //로컬 위치 원점

        //변경 설정
        id = data.itemId;
        damage = data.baseDamage * Characters.Damage;
        count = data.baseCount + Characters.Count;

        //스크립트 오브젝트 독립성(인댁스 아닌 프리펩으로 설정함)
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

        //특정 함수 호출 자식 오브젝트에 알림
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // ORDER : 05#) 스킬 능력 구현
    #region //메인 공격 
    void Place()//검
    {
        for (int idx = 0; idx < count; idx++)
        {
            Transform bullet;

            if (idx < transform.childCount)
            {
                bullet = transform.GetChild(idx);
            }
            else //기존 오브젝트 먼저 활용 후, 풀링에서 가져(추가)오기
            {
                bullet = Game_Mgr.Inst.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * idx / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullets>().Init(damage, -100); // -100 관통력         
        }
    }

    void Fire()//타겟매직
    {
        if (!player.scanner.nearestTarget)  //목표 없으면 리턴
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;//타겟
        Vector3 dir = targetPos - transform.position;             //방향
        dir = dir.normalized;   //벡터 방향 유지, 크기 1

        Transform bullet = Game_Mgr.Inst.pool.Get(prefabId).transform;
        bullet.position = transform.position;

        //지정 축 중심으로 목표 향해 회전
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullets>().Init(damage, count, dir);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Range);
    }

    void RandomFire()//랜덤매직
    {
        Vector3 dir = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), 0);  //방향
        dir = dir.normalized;   //벡터 방향 유지, 크기 1

        Transform bullet = Game_Mgr.Inst.pool.Get(prefabId).transform;
        bullet.position = transform.position;

        bullet.GetComponent<Bullets>().Init(damage, count, dir);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Range);
    }

    void FrontFire()//프론트 매직
    {
        Vector3 dir = Vector3.right;

        if (sprite.flipX == true)
        {
            dir = Vector3.left;
        }

        dir = dir.normalized;   //벡터 방향 유지, 크기 1

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
    #endregion

    #region //보조 공격
    void Lightning()//벼락
    {
        Transform bullet = Game_Mgr.Inst.pool.Get(prefabId).transform;
        bullet.position = player.transform.position + (Random.insideUnitSphere * 7f); //플레이어 반지름 원 기준 7
        bullet.GetComponent<Bullets>().Init(damage, count);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Lightning);
    }
    
    void Spike()//스파이크
    {
        Transform bullet = Game_Mgr.Inst.pool.Get(prefabId).transform;
        bullet.position = player.transform.position + (Random.insideUnitSphere * 5f);  //플레이어 반지름 원 기준 5
        bullet.GetComponent<Bullets>().Init(damage, count);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Spike);
    }
    #endregion

    #region//보조
    void Shield()//쉴드
    {
        Transform shleid;

        shleid = Game_Mgr.Inst.pool.Get(prefabId).transform;
        shleid.parent = transform;
        shleid.position = transform.position;

        shleid.GetComponent<Bullets>().Init(-100);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Shield);
    }

    void Explosion()//전멸
    {
        Transform bullet = Game_Mgr.Inst.pool.Get(prefabId).transform;
        bullet.position = transform.position;

        bullet.GetComponent<Bullets>().Init(damage, count);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Explosion);
    }
    #endregion

    public void LevelUp(float a_damage, int a_count) //스킬 레벨 업
    {    
        damage = a_damage * Characters.Damage;    //데미지
        count += a_count;                         //근거리 - 무기 숫자 증가, 원거리 - 관통 수 증가

        if (id == 0)
            Place();

        //특정 함수 호출 자식 오브젝트에 알림
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
}
