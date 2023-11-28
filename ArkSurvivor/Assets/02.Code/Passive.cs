using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive : MonoBehaviour
{
    public ItemDatas.ItemType type;
    public float val;                   //�нú� Ÿ��, ��ġ ����

    public void Init(ItemDatas data)    //�нú� �߰�
    {
        //�⺻ ����
        name = "Passive " + data.itemId;
        transform.parent = Game_Mgr.Inst.player.transform;
        transform.localPosition = Vector3.zero;

        //���� ����
        type = data.itemType;
        val = data.damages[0];
        ApplyPassive();
    }

    public void LevelUp(float a_val)    //������
    {
        val = a_val;
        ApplyPassive();
    }

    void ApplyPassive()//�нú� ����(�нú� �߰�, ������ �� ����)
    {
        switch (type)
        {
            case ItemDatas.ItemType.Glove:
                RateUp();
                break;

            case ItemDatas.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    void RateUp()   //�ٰŸ� ȸ���ӵ�, ���Ÿ� ����ӵ�
    {
        Active[] weapons = transform.parent.GetComponentsInChildren<Active>();

        foreach (Active weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:                             //�ٰŸ�
                    float a_Sp = 150 * Characters.RotSp;
                    weapon.sp = a_Sp + (a_Sp * val);
                    break;

                case 1:                            //���Ÿ�(Ÿ��)
                    a_Sp = (float)(0.7f * Characters.FireCool);
                    weapon.sp = a_Sp * (1f - val);
                    break;

                case 5:                            //���Ÿ�(����)
                    a_Sp = (float)(0.3f * Characters.FireCool);
                    weapon.sp = a_Sp * (1f - val);
                    break;

                case 6:                            //����
                    a_Sp = (float)(7f * Characters.FireCool);
                    weapon.sp = a_Sp * (1f - val);
                    break;

                case 7:                            //���Ÿ�(����Ʈ)
                    a_Sp = (float)(2.5f * Characters.FireCool);
                    weapon.sp = a_Sp * (1f - val);
                    break;

                case 8:                            //���Ÿ�(����)
                    a_Sp = (float)(3.5f * Characters.FireCool);
                    weapon.sp = a_Sp * (1f - val);
                    break;

                case 9:                            //���Ÿ�?(����)
                    a_Sp = (float)(80f * Characters.FireCool);
                    weapon.sp = a_Sp * (1f - val);
                    break;

                case 10:                           //���Ÿ�(����)
                    a_Sp = (float)(7f * Characters.FireCool);
                    weapon.sp = a_Sp * (1f - val);
                    break;

            }
        }
    }

    void SpeedUp()  //�̼�
    {
        float sp = 3 * Characters.MoveSp;
        Game_Mgr.Inst.player.sp = sp + sp * val;
    }
}