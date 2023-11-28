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
        foreach (Achieve achive in achives)  //��� ���� Ȯ��
        {
            AchiveCheck(achive);
        }
    }

    void AchiveCheck(Achieve achieve)
    {
        bool isAchieve = false;

        switch (achieve)
        {
            case Achieve.UnlockSkadi:   //30���� óġ��
                isAchieve = Game_Mgr.Inst.kill >= 30;   
                break;

            case Achieve.UnlockOther:   //�� ��Ƴ��� �Ϸ��
                isAchieve = Game_Mgr.Inst.gameTime == Game_Mgr.Inst.maxGameTime;
                break;
        }
        
        if(isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)    //ó�� ���� �޼���
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);  //���� �޼���

            for(int idx = 0; idx < uiNotification.transform.childCount; idx++)
            {
                bool isActive = idx == (int)achieve;    //�˸� â �ڽ� ������Ʈ ��ȸ, ���� ������ Ȱ��ȭ
                uiNotification.transform.GetChild(idx).gameObject.SetActive(isActive);
            }

            StartCoroutine(NotifiRoutine());
        }
            
    }

    IEnumerator NotifiRoutine()
    {
        uiNotification.SetActive(true);

        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.LevelUp);

        yield return wait;

        uiNotification.SetActive(false);
    }
}
