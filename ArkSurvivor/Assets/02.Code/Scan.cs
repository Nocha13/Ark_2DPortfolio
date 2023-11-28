using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scan : MonoBehaviour
{
    public float scanRan;         //����
    public LayerMask targetLayer;   //���̾�
    public RaycastHit2D[] targets;  //��ĵ ��� �迭
    public Transform nearestTarget; //���� ����� Ÿ��

    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRan, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;   //�Ÿ� 100

        foreach (RaycastHit2D target in targets)    //ĳ���� ��� ������Ʈ ����
        {
            Vector3 myPos = transform.position;                 //�÷��̾� ��ġ
            Vector3 targetPos = target.transform.position;      //Ÿ�� ��ġ
            float curDiff = Vector3.Distance(myPos, targetPos); //������Ʈ ���� �Ÿ�

            if (curDiff < diff) //���� �Ÿ�(diff)���� ������ Ÿ�� ��ü
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
