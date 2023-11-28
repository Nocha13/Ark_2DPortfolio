using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scan : MonoBehaviour
{
    public float scanRan;         //범위
    public LayerMask targetLayer;   //레이어
    public RaycastHit2D[] targets;  //스캔 결과 배열
    public Transform nearestTarget; //가장 가까운 타겟

    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRan, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;   //거리 100

        foreach (RaycastHit2D target in targets)    //캐스팅 결과 오브젝트 접근
        {
            Vector3 myPos = transform.position;                 //플레이어 위치
            Vector3 targetPos = target.transform.position;      //타겟 위치
            float curDiff = Vector3.Distance(myPos, targetPos); //오브젝트 간의 거리

            if (curDiff < diff) //저장 거리(diff)보다 작으면 타겟 교체
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
