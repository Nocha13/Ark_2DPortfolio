using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public float sp;            //속도
    public int damage;          //데미지
    public float hp;            //현재 체력
    public float maxHp;         //최대 체력
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;  //타겟 = 플레이어

    public GameObject shadow;

    bool isLive;         //생사여부

    public static EnemyCtrl Inst;

    //컴포넌트
    Rigidbody2D rigid;
    Collider2D coll2D;    //표시
    SpriteRenderer spriter;
    Animator anim;
    WaitForFixedUpdate wait;

    void Awake()
    {
        Inst = this;

        rigid = GetComponent<Rigidbody2D>();
        coll2D = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
    }
    // ORDER : #적 플레이어 자동 추적)
    void FixedUpdate()
    {
        if (!Game_Mgr.Inst.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * sp * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!isLive || !Game_Mgr.Inst.isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        target = Game_Mgr.Inst.player.GetComponent<Rigidbody2D>();
        shadow.gameObject.SetActive(true);
        isLive = true;
        coll2D.enabled = true;      //컴포넌트 비활성
        rigid.simulated = true;     //RigidBody 비활성
        spriter.sortingOrder = 2;   //스프라이트 Order in Layer
        anim.SetBool("Dead", false); 
        hp = maxHp;
    }

    public void Init(SpawnData data)//스폰 데이터 받는 함수
    {
        anim.runtimeAnimatorController = animCon[data.c_spriteType];
        sp = data.c_sp;
        damage = data.c_damage;
        maxHp = data.c_hp;
        hp = data.c_hp;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.CompareTag("Bullet") || !isLive) //태그가 Bullet 아니면 리턴
            return;

        hp -= coll.GetComponent<Bullets>().damage;  //체력 = 체력 - 데미지
        StartCoroutine(KnockBack());                //넉백 실행

        if (hp > 0) //생존 중
        {
            anim.SetTrigger("Hit");
            AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Hit);
        }
        else        //죽음
        {
            shadow.gameObject.SetActive(false);
            isLive = false;
            coll2D.enabled = false;     //컴포넌트 비활성
            rigid.simulated = false;    //RigidBody 비활성
            spriter.sortingOrder = 1;   //스프라이트 Order in Layer
            anim.SetBool("Dead", true);
            Game_Mgr.Inst.kill++;
            Game_Mgr.Inst.GetExp();

            if(Game_Mgr.Inst.isLive)    // 게임 종료가 아닐때만 효과음 재생(종료시 모든 적 죽기때문)
                AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Dead);
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; // 다음 하나의 물리 프레임 딜레이
        Vector3 playerPos = Game_Mgr.Inst.player.transform.position;//플레이어 위치
        Vector3 dirVec = transform.position - playerPos;            //현재 위치 - 플레이어 위치
        //즉발적 힘
        rigid.AddForce(dirVec.normalized * 7, ForceMode2D.Impulse);
    }

    void Dead()//죽음
    {
        gameObject.SetActive(false);
    }
}
