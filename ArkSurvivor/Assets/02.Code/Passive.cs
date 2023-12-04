using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive : MonoBehaviour
{
    public ItemDatas.ItemType type;
    public float val;                   //패시브 타입, 수치 변경

    public void Init(ItemDatas data)    //패시브 추가
    {
        //기본 설정
        name = "Passive " + data.itemId;
        transform.parent = Game_Mgr.Inst.player.transform;
        transform.localPosition = Vector3.zero;

        //변경 설정
        type = data.itemType;
        val = data.damages[0];
        ApplyPassive();
    }

    public void LevelUp(float a_val)    //레벨업
    {
        val = a_val;
        ApplyPassive();
    }

    void ApplyPassive()//패시브 관리(패시브 추가, 레벨업 시 적용)
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

    // ORDER : #06) 패시브 능력 구현
    void RateUp()   //근거리 회전속도, 원거리 연사속도
    {
        Active[] weapons = transform.parent.GetComponentsInChildren<Active>();

        foreach (Active weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:                             //근거리
                    float a_Sp = 150 * Characters.RotSp;
                    weapon.sp = a_Sp + (a_Sp * val);
                    break;

                case 1:                            //원거리(타겟)
                    a_Sp = (float)(0.7f * Characters.FireCool);
                    weapon.sp = a_Sp * (1f - val);
                    break;

                case 5:                            //원거리(랜덤)
                    a_Sp = (float)(0.3f * Characters.FireCool);
                    weapon.sp = a_Sp * (1f - val);
                    break;

                case 6:                            //쉴드
                    a_Sp = (float)(7f * Characters.FireCool);
                    weapon.sp = a_Sp * (1f - val);
                    break;

                case 7:                            //원거리(프론트)
                    a_Sp = (float)(2.5f * Characters.FireCool);
                    weapon.sp = a_Sp * (1f - val);
                    break;

                case 8:                            //원거리(지뢰)
                    a_Sp = (float)(3.5f * Characters.FireCool);
                    weapon.sp = a_Sp * (1f - val);
                    break;

                case 9:                            //원거리?(전멸)
                    a_Sp = (float)(80f * Characters.FireCool);
                    weapon.sp = a_Sp * (1f - val);
                    break;

                case 10:                           //원거리(낙뢰)
                    a_Sp = (float)(7f * Characters.FireCool);
                    weapon.sp = a_Sp * (1f - val);
                    break;

            }
        }
    }

    void SpeedUp()  //이속
    {
        float sp = 3 * Characters.MoveSp;
        Game_Mgr.Inst.player.sp = sp + sp * val;
    }
}
