using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMgr : MonoBehaviour
{
    //������ ���� ����
    public GameObject[] prefabs;

    //Ǯ ��� ����Ʈ
    List<GameObject>[] pools;
    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int idx = 0; idx < pools.Length; idx++)
        {   //Ǯ ����Ʈ �ʱ�ȭ
            pools[idx] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject sel = null;

        //������ Ǯ�� ��Ȱ��ȭ ������Ʈ�� ����
        foreach (GameObject item in pools[index])
        {   //�߽߰� sel������ �Ҵ�
            if (!item.activeSelf)
            {
                sel = item;
                sel.SetActive(true);
                break;
            }
        }
        //�� ã����
        if (!sel)
        {   //���Ӱ� ����, sel������ �Ҵ�
            sel = Instantiate(prefabs[index], transform);
            pools[index].Add(sel);
        }

        return sel;
    }
}
