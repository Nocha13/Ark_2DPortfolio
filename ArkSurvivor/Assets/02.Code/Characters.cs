using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    public static float MoveSp      //�̵��ӵ�
    {
        get { return Game_Mgr.Inst.playerId == 0 ? 1.1f : 1f; }  //ID : 0 - þ ���ý� �̼� 10% ����
    }

    public static float RotSp      //ȸ�� �ӵ�(�ٰŸ�)
    {
        get { return Game_Mgr.Inst.playerId == 1 ? 1.1f : 1f; }  //ID : 1 - �ƹ̾� ���ý� �߻�ӵ� 10%����
    }

    public static float FireCool    //�߻� �ӵ� - �߻� ��Ÿ��(���Ÿ�)
    {
        get { return Game_Mgr.Inst.playerId == 1 ? 0.9f : 1f; }  //ID : 1 - �ƹ̾� ���ý� �߻���Ÿ�� 10%����
    }

    public static float Damage      //������ ����
    {
        get { return Game_Mgr.Inst.playerId == 3 ? 1.1f : 1f; }  //ID : 2 - ��ī�� ���ý� ������ 10%����
    }

    public static int Count         //���� �� ����
    {
        get { return Game_Mgr.Inst.playerId == 2 ? 1 : 0; }  //ID : 3 - �ػ罺 ���ý� ���� �� 1 ����
    }
}
