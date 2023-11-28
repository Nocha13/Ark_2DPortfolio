using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public ItemDatas data;  //데이터
    public int level;       //레벨
    public Active active;   //액티브
    public Passive passive; //패시브

    Image icon;             //아이콘
    Text LevelText;         //레벨
    Text NameText;          //이름
    Text DescText;          //설명

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1]; //자기자신
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        LevelText = texts[0];
        NameText = texts[1];
        DescText = texts[2];
        NameText.text = data.itemName;
    }

    private void OnEnable()
    {
        LevelText.text = "Lv." + (level + 1);

        switch (data.itemType)
        {
            case ItemDatas.ItemType.Sword:      //검
                if (level == 0)
                {
                    DescText.text = "자신의 주변을 회전하는 검을 생성한다";
                }

                else
                {
                    DescText.text = string.Format("데미지 {0}% 증가/회전체 {1} 증가", data.damages[level] * 100, data.counts[level]); //% 증가(백분율)
                }
                break;

            case ItemDatas.ItemType.Target:      //타겟
                if (level == 0)
                {
                    DescText.text = "주변의 적을 향해 투사체를 발사한다";
                }
                else if (level == 1 || level == 3)
                {
                    DescText.text = string.Format("관통력 {0} 증가", data.counts[level]);
                }
                else
                {
                    DescText.text = string.Format("데미지 {0}% 증가", data.damages[level] * 100);
                }
                break;

            case ItemDatas.ItemType.Random:      //랜덤
                if (level == 0)
                {
                    DescText.text = "무작위 방향으로 투사체를 발사한다";
                }
                else if (level == 2 || level == 4)
                {
                    DescText.text = string.Format("관통력 {0} 증가", data.counts[level]);
                }
                else
                {
                    DescText.text = string.Format("데미지 {0}% 증가", data.damages[level] * 100);
                }
                break;

            case ItemDatas.ItemType.Front:      //프론트
                if (level == 0)
                {
                    DescText.text = "자신을 중심으로 앞 방향에 투사체를 발사한다";
                }
                else
                {
                    DescText.text = string.Format("관통력 {0} 증가/데미지 {1}% 증가", data.counts[level], data.damages[level] * 100);
                }
                break;

            case ItemDatas.ItemType.Lightnig:      //낙뢰
                if (level == 0)
                {
                    DescText.text = "무작위 위치에 낙뢰를 떨어트린다";
                }
                else if (level == 1 || level == 3)
                {
                    DescText.text = string.Format("관통력 {0} 증가", data.counts[level]);
                }
                else
                {
                    DescText.text = string.Format("관통력 {0} 증가/데미지 {1}% 증가", data.counts[level], data.damages[level] * 100);
                }
                break;

            case ItemDatas.ItemType.Spike:      //스파이크
                if (level == 0)
                {
                    DescText.text = "무작위 위치에 스파이크를 생성한다";
                }
                else if (level == 1 || level == 3)
                {
                    DescText.text = string.Format("데미지 {0}% 증가", data.damages[level] * 100);
                }
                else
                {
                    DescText.text = string.Format("관통력 {0} 증가/데미지 {1}% 증가", data.counts[level], data.damages[level] * 100);
                }
                break;

            case ItemDatas.ItemType.Explosion:
            case ItemDatas.ItemType.Shield:
                DescText.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;

            case ItemDatas.ItemType.Glove:
            case ItemDatas.ItemType.Shoe:
                DescText.text = string.Format(data.itemDesc, data.damages[level] * 100); //% 증가(백분율)
                break;

            default:    //힐
                DescText.text = string.Format(data.itemDesc);
                break;
        }


    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemDatas.ItemType.Sword:   //검
            case ItemDatas.ItemType.Target:  //타겟
            case ItemDatas.ItemType.Random:  //랜덤
            case ItemDatas.ItemType.Front:   //프론트
            case ItemDatas.ItemType.Lightnig://낙뢰
            case ItemDatas.ItemType.Spike:   //스파이크
            case ItemDatas.ItemType.Shield:     //쉴드
            case ItemDatas.ItemType.Explosion:  //전멸
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    active = newWeapon.AddComponent<Active>();
                    active.Init(data);
                }

                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level]; //n%증가
                    nextCount += data.counts[level];

                    active.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;

            case ItemDatas.ItemType.Glove:
            case ItemDatas.ItemType.Shoe:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    passive = newGear.AddComponent<Passive>();
                    passive.Init(data);
                }

                else
                {
                    float nextVal = data.damages[level];
                    passive.LevelUp(nextVal);
                }
                level++;
                break;

            case ItemDatas.ItemType.Heal:       //힐(일회성 - 레벨제한 없음)  
                {
                    Game_Mgr.Inst.hp = Game_Mgr.Inst.maxHp;
                    AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Heal);
                }             
                break;
        }

        if (level == data.damages.Length)
        {//최대레벨 도달시 레벨 데이터 개수 넘기지 않게 함
            GetComponent<Button>().interactable = false;
        }
    }
}
