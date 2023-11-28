using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    public Vector2 inputVec;    //�Է�
    public float sp;            //�ӵ�

    public Scan scanner;
    public GameObject shadow;
    public RuntimeAnimatorController[] animCtrl;

    //������Ʈ
    Rigidbody2D rigid;
    [HideInInspector] public SpriteRenderer spriter;
    Animator anim;
    Transform player;

    public static PlayerCtrl Inst;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scan>();
        Inst = this;
        player = GetComponent<Transform>();
    }

    void OnEnable()
    {
        sp *= Characters.MoveSp;
        anim.runtimeAnimatorController = animCtrl[Game_Mgr.Inst.playerId];
    }

    void Update()
    {
        if (!Game_Mgr.Inst.isLive)
            return;
    }

    void FixedUpdate()
    {
        if (!Game_Mgr.Inst.isLive)
            return;

        Vector2 nextVec = inputVec.normalized * sp * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        if (!Game_Mgr.Inst.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(player.position, Vector2.one * 20);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, 9);
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (!Game_Mgr.Inst.isLive)
            return;

        Active[] a_Atv = GameObject.FindObjectsOfType<Active>();
        for (int ii = 0; ii < a_Atv.Length; ii++)
        {
            if (a_Atv[ii] == null)
                continue;

            if (a_Atv[ii].id == 6)
            {
                if (GameObject.Find("Active 6").transform.Find("Shield(Clone)").gameObject.activeSelf == true)
                {
                    Debug.Log("��ġ��");
                    return;
                }
            }
        }

        Game_Mgr.Inst.hp -= Time.deltaTime * EnemyCtrl.Inst.damage;

        if (Game_Mgr.Inst.hp < 0)
        {
            for (int idx = 2; idx < transform.childCount; idx++)
            {
                transform.GetChild(idx).gameObject.SetActive(false);
            }
            shadow.gameObject.SetActive(false);
            GameObject.Find("InfoUI").gameObject.SetActive(false);
            anim.SetTrigger("Dead");
            Game_Mgr.Inst.GameOver();
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}
