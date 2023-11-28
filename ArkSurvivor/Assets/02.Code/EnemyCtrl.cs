using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public float sp;            //�ӵ�
    public int damage;          //������
    public float hp;            //���� ü��
    public float maxHp;         //�ִ� ü��
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;  //Ÿ�� = �÷��̾�

    public GameObject shadow;

    bool isLive;         //���翩��

    public static EnemyCtrl Inst;

    //������Ʈ
    Rigidbody2D rigid;
    Collider2D coll2D;    //ǥ��
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
        coll2D.enabled = true;      //������Ʈ ��Ȱ��
        rigid.simulated = true;     //RigidBody ��Ȱ��
        spriter.sortingOrder = 2;   //��������Ʈ Order in Layer
        anim.SetBool("Dead", false); 
        hp = maxHp;
    }

    public void Init(SpawnData data)//���� ������ �޴� �Լ�
    {
        anim.runtimeAnimatorController = animCon[data.c_spriteType];
        sp = data.c_sp;
        damage = data.c_damage;
        maxHp = data.c_hp;
        hp = data.c_hp;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.CompareTag("Bullet") || !isLive) //�±װ� Bullet �ƴϸ� ����
            return;

        hp -= coll.GetComponent<Bullets>().damage;  //ü�� = ü�� - ������
        StartCoroutine(KnockBack());                //�˹� ����

        if (hp > 0) //���� ��
        {
            anim.SetTrigger("Hit");
            AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Hit);
        }
        else        //����
        {
            shadow.gameObject.SetActive(false);
            isLive = false;
            coll2D.enabled = false;     //������Ʈ ��Ȱ��
            rigid.simulated = false;    //RigidBody ��Ȱ��
            spriter.sortingOrder = 1;   //��������Ʈ Order in Layer
            anim.SetBool("Dead", true);
            Game_Mgr.Inst.kill++;
            Game_Mgr.Inst.GetExp();

            if(Game_Mgr.Inst.isLive)    // ���� ���ᰡ �ƴҶ��� ȿ���� ���(����� ��� �� �ױ⶧��)
                AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Dead);
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; // ���� �ϳ��� ���� ������ ������
        Vector3 playerPos = Game_Mgr.Inst.player.transform.position;//�÷��̾� ��ġ
        Vector3 dirVec = transform.position - playerPos;            //���� ��ġ - �÷��̾� ��ġ
        //����� ��
        rigid.AddForce(dirVec.normalized * 7, ForceMode2D.Impulse);
    }

    void Dead()//����
    {
        gameObject.SetActive(false);
    }
}
