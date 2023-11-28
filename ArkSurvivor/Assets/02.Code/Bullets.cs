using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Explosion,   //����
    Spike,       //����
    Lightning,   //����
    Shield,      //����
    etc          //��Ÿ
}

public class Bullets : MonoBehaviour
{
    public WeaponType ponType = WeaponType.etc;

    public float damage;    //������
    public int per;         //����

    Rigidbody2D rigid;

    public static Bullets Inst;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void SetAc()    //�ִϸ��̼� �̺�Ʈ(���� ����)
    {
        if (gameObject.activeSelf == true)
        {
            if ((ponType == WeaponType.Spike) || ponType == WeaponType.Lightning || ponType == WeaponType.Explosion)
            {
                StartCoroutine(Actives(0.1f));
            }
        }
    }

    IEnumerator Actives(float time)
    {
        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
    }

    public void Init(int a_per)
    {
        per = a_per;
    }

    public void Init(float a_damage, int a_per)
    {
        damage = a_damage;
        per = a_per;
    }

    public void Init(float a_damage, int a_per, Vector3 dir)
    {
        damage = a_damage;
        per = a_per;

        if (per >= 0)
        {
            rigid.velocity = dir * 15;      // �ӵ� ����
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            if (ponType == WeaponType.Shield)
            {
                gameObject.SetActive(false);
            }
        }

        if (!coll.CompareTag("Enemy") || per == -100)
            return;

        per--;

        if (per < 0)
        {
            if (ponType == WeaponType.etc)
            {
                rigid.velocity = Vector2.zero;  //���� �ӵ� �ʱ�ȭ
                gameObject.SetActive(false);    //��Ȱ��
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (!coll.CompareTag("MainCamera") || per == -100)
            return;

        gameObject.SetActive(false);
    }
}
