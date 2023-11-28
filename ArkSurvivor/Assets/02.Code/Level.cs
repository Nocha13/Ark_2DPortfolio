using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    RectTransform rect;
    Items[] items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Items>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        Game_Mgr.Inst.Pause();
        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.LevelUp);
        AudioMgr.Inst.HighPass(true);
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        Game_Mgr.Inst.Resume();
        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Select);
        AudioMgr.Inst.HighPass(false);
    }

    public void Sel(int idx)
    {
        items[idx].OnClick();
    }

    void Next()
    {
        //모든 아이템 비활성
        foreach (Items item in items)
        {
            item.gameObject.SetActive(false);
        }
        //그 중 랜덤으로 3개 활성
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && 
                ran[0] != ran[2] && 
                ran[1] != ran[2])
                break;
        }

        for(int idx = 0; idx < ran.Length; idx++)
        {
            Items ranItem = items[ran[idx]];

            //만렙 아이템 소비아이템으로 대체
            if(ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }

            else
                ranItem.gameObject.SetActive(true);
        }
    }
}
