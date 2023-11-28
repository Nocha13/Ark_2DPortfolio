using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public enum InfoType
    {
        Hp,     //체력
        Time,   //시간
        Kill,   //처치 수
        Exp,    //경험치
        Level   //레벻
    }
    public InfoType type;

    Text text;
    Slider slider;

    void Awake()
    {
        text = GetComponent<Text>();
        slider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch(type)
        {
            case InfoType.Hp:
                float curHp = Game_Mgr.Inst.hp;
                float maxHp = Game_Mgr.Inst.maxHp;
                slider.value = curHp / maxHp;
                break;

            case InfoType.Time:
                float reTime = Game_Mgr.Inst.maxGameTime - Game_Mgr.Inst.gameTime;
                int min = Mathf.FloorToInt(reTime / 60);
                int sec = Mathf.FloorToInt(reTime % 60);
                text.text = string.Format("{0:D2} : {1:D2}", min, sec);
                break;

            case InfoType.Kill:
                text.text = string.Format("{0:F0}", Game_Mgr.Inst.kill);
                break;
                
            case InfoType.Exp:
                float curExp = Game_Mgr.Inst.exp;
                float nextExp = Game_Mgr.Inst.nextExp[Mathf.Min(Game_Mgr.Inst.level, Game_Mgr.Inst.nextExp.Length - 1)];
                slider.value = curExp / nextExp;
                break;

            case InfoType.Level:
                text.text = string.Format("Lv.{0}", Game_Mgr.Inst.level);
                break;
        }
    }
}
