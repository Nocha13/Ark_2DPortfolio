using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    public static float MoveSp      //이동속도
    {
        get { return Game_Mgr.Inst.playerId == 0 ? 1.1f : 1f; }  //ID : 0 - 첸 선택시 이속 10% 증가
    }

    public static float RotSp      //회전 속도(근거리)
    {
        get { return Game_Mgr.Inst.playerId == 1 ? 1.1f : 1f; }  //ID : 1 - 아미야 선택시 발사속도 10%증가
    }

    public static float FireCool    //발사 속도 - 발사 쿨타임(원거리)
    {
        get { return Game_Mgr.Inst.playerId == 1 ? 0.9f : 1f; }  //ID : 1 - 아미야 선택시 발사쿨타임 10%감소
    }

    public static float Damage      //데미지 증가
    {
        get { return Game_Mgr.Inst.playerId == 3 ? 1.1f : 1f; }  //ID : 2 - 스카디 선택시 데미지 10%증가
    }

    public static int Count         //무기 수 증가
    {
        get { return Game_Mgr.Inst.playerId == 2 ? 1 : 0; }  //ID : 3 - 텍사스 선택시 무기 수 1 증가
    }
}
