using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveMgr : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotification;

    WaitForSecondsRealtime wait;

    enum Achieve { UnlockSkadi, UnlockOther }
    Achieve[] achives;

    void Awake()
    {
        achives = (Achieve[])System.Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(5);

        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achieve achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for (int index = 0; index < lockCharacter.Length; index++)
        {
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharacter[index].SetActive(!isUnlock);
            unlockCharacter[index].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        foreach (Achieve achive in achives)  //모든 업적 확인
        {
            AchiveCheck(achive);
        }
    }

    void AchiveCheck(Achieve achieve)
    {
        bool isAchieve = false;

        switch (achieve)
        {
            case Achieve.UnlockSkadi:   //30마리 처치시
                isAchieve = Game_Mgr.Inst.kill >= 30;   
                break;

            case Achieve.UnlockOther:   //1번 살아남기 완료시
                isAchieve = Game_Mgr.Inst.gameTime == Game_Mgr.Inst.maxGameTime;
                break;
        }
        
        if(isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)    //처음 업적 달성시
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);  //업적 달성시

            for(int idx = 0; idx < uiNotification.transform.childCount; idx++)
            {
                bool isActive = idx == (int)achieve;    //알림 창 자식 오브젝트 순회, 순번 맞으면 활성화
                uiNotification.transform.GetChild(idx).gameObject.SetActive(isActive);
            }

            StartCoroutine(NotifiRoutine());
        }
            
    }

    IEnumerator NotifiRoutine()    //알림창
    {
        uiNotification.SetActive(true);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.LevelUp);

        yield return wait;

        uiNotification.SetActive(false);
    }
}
