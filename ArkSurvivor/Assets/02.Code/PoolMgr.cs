using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMgr : MonoBehaviour
{
    //프리팹 보관 변수
    public GameObject[] prefabs;

    //풀 담당 리스트
    List<GameObject>[] pools;
    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int idx = 0; idx < pools.Length; idx++)
        {   //풀 리스트 초기화
            pools[idx] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject sel = null;

        //선택한 풀의 비활성화 오브젝트에 접근
        foreach (GameObject item in pools[index])
        {   //발견시 sel변수에 할당
            if (!item.activeSelf)
            {
                sel = item;
                sel.SetActive(true);
                break;
            }
        }
        //못 찾을시
        if (!sel)
        {   //새롭게 생성, sel변수에 할당
            sel = Instantiate(prefabs[index], transform);
            pools[index].Add(sel);
        }

        return sel;
    }
}
