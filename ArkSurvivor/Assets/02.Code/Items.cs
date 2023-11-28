using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public ItemDatas data;  //������
    public int level;       //����
    public Active active;   //��Ƽ��
    public Passive passive; //�нú�

    Image icon;             //������
    Text LevelText;         //����
    Text NameText;          //�̸�
    Text DescText;          //����

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1]; //�ڱ��ڽ�
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
            case ItemDatas.ItemType.Sword:      //��
                if (level == 0)
                {
                    DescText.text = "�ڽ��� �ֺ��� ȸ���ϴ� ���� �����Ѵ�";
                }

                else
                {
                    DescText.text = string.Format("������ {0}% ����/ȸ��ü {1} ����", data.damages[level] * 100, data.counts[level]); //% ����(�����)
                }
                break;

            case ItemDatas.ItemType.Target:      //Ÿ��
                if (level == 0)
                {
                    DescText.text = "�ֺ��� ���� ���� ����ü�� �߻��Ѵ�";
                }
                else if (level == 1 || level == 3)
                {
                    DescText.text = string.Format("����� {0} ����", data.counts[level]);
                }
                else
                {
                    DescText.text = string.Format("������ {0}% ����", data.damages[level] * 100);
                }
                break;

            case ItemDatas.ItemType.Random:      //����
                if (level == 0)
                {
                    DescText.text = "������ �������� ����ü�� �߻��Ѵ�";
                }
                else if (level == 2 || level == 4)
                {
                    DescText.text = string.Format("����� {0} ����", data.counts[level]);
                }
                else
                {
                    DescText.text = string.Format("������ {0}% ����", data.damages[level] * 100);
                }
                break;

            case ItemDatas.ItemType.Front:      //����Ʈ
                if (level == 0)
                {
                    DescText.text = "�ڽ��� �߽����� �� ���⿡ ����ü�� �߻��Ѵ�";
                }
                else
                {
                    DescText.text = string.Format("����� {0} ����/������ {1}% ����", data.counts[level], data.damages[level] * 100);
                }
                break;

            case ItemDatas.ItemType.Lightnig:      //����
                if (level == 0)
                {
                    DescText.text = "������ ��ġ�� ���ڸ� ����Ʈ����";
                }
                else if (level == 1 || level == 3)
                {
                    DescText.text = string.Format("����� {0} ����", data.counts[level]);
                }
                else
                {
                    DescText.text = string.Format("����� {0} ����/������ {1}% ����", data.counts[level], data.damages[level] * 100);
                }
                break;

            case ItemDatas.ItemType.Spike:      //������ũ
                if (level == 0)
                {
                    DescText.text = "������ ��ġ�� ������ũ�� �����Ѵ�";
                }
                else if (level == 1 || level == 3)
                {
                    DescText.text = string.Format("������ {0}% ����", data.damages[level] * 100);
                }
                else
                {
                    DescText.text = string.Format("����� {0} ����/������ {1}% ����", data.counts[level], data.damages[level] * 100);
                }
                break;

            case ItemDatas.ItemType.Explosion:
            case ItemDatas.ItemType.Shield:
                DescText.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;

            case ItemDatas.ItemType.Glove:
            case ItemDatas.ItemType.Shoe:
                DescText.text = string.Format(data.itemDesc, data.damages[level] * 100); //% ����(�����)
                break;

            default:    //��
                DescText.text = string.Format(data.itemDesc);
                break;
        }


    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemDatas.ItemType.Sword:   //��
            case ItemDatas.ItemType.Target:  //Ÿ��
            case ItemDatas.ItemType.Random:  //����
            case ItemDatas.ItemType.Front:   //����Ʈ
            case ItemDatas.ItemType.Lightnig://����
            case ItemDatas.ItemType.Spike:   //������ũ
            case ItemDatas.ItemType.Shield:     //����
            case ItemDatas.ItemType.Explosion:  //����
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

                    nextDamage += data.baseDamage * data.damages[level]; //n%����
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

            case ItemDatas.ItemType.Heal:       //��(��ȸ�� - �������� ����)  
                {
                    Game_Mgr.Inst.hp = Game_Mgr.Inst.maxHp;
                    AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Heal);
                }             
                break;
        }

        if (level == data.damages.Length)
        {//�ִ뷹�� ���޽� ���� ������ ���� �ѱ��� �ʰ� ��
            GetComponent<Button>().interactable = false;
        }
    }
}
