using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Mgr : MonoBehaviour
{
    public static Game_Mgr Inst;
    [Header("Object")]
    public PlayerCtrl player;
    public PoolMgr pool;
    public Level UILevel;
    public GameResult UIResult;
    public GameObject UI;
    public GameObject AllClear;

    [Header("Player Info")]
    public int playerId;                //캐릭터 ID
    public float hp;                    //체력
    public float maxHp = 100;           //최대체력
    public int level;                   //소환 레벨
    public int kill;                    //처치 수
    public int exp;                     //경험치
    public int[] nextExp = { 3, 5, 10, 50, 130, 190, 230, 290, 370, 500 }; //필요 경험치
    
    [Header("Game Ctrl")]
    public float gameTime;              //게임시간
    public float maxGameTime = 3 * 10f; //게임 최대 시간
    public bool isLive;
    public bool isStart;

    void Awake()
    {
        //PlayerPrefs.DeleteAll();
        //Application.targetFrameRate = 60;
        Inst = this;
        maxHp = 100;
        hp = 100;
        
    }

    void Start()
    {
        Time.timeScale = 1;
        AudioMgr.Inst.TitleBgm(true);
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameWin();
        }
    }

    // ORDER : #09) 플레이어 캐릭터 선택
    public void GameStart(int a_id)
    {
        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Join);
        AudioMgr.Inst.TitleBgm(false);
        
        StartCoroutine(Fade.Inst.FadeInStart());
        StartCoroutine(Fade.Inst.FadeOutStart());

        AudioMgr.Inst.PlayBgm(true);

        playerId = a_id;

        hp = maxHp;
        player.gameObject.SetActive(true);

        UILevel.Sel(playerId % 2); //무기 수
    }

    public void GameReStart()
    {
        SceneManager.LoadScene("InGame");
    }

    public void GameWin()
    {
        StartCoroutine(GameWinRoutine());
    }

    IEnumerator GameWinRoutine()
    {
        AllClear.gameObject.SetActive(true);

        isLive = false;

        yield return new WaitForSeconds(0.5f);

        UIResult.gameObject.SetActive(true);
        UI.gameObject.SetActive(false);
        UIResult.Win();
        Pause();

        AudioMgr.Inst.PlayBgm(false);
        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Win);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(1f);
        UIResult.gameObject.SetActive(true);
        UI.gameObject.SetActive(false);
        UIResult.Lose();
        Pause();

        AudioMgr.Inst.PlayBgm(false);
        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Lose);
    }

    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level++;
            exp = 0;
            UILevel.Show();
        }
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }

    public void Pause()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void GameQuit()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
        //#if UNITY_EDITOR
        //    UnityEditor.EditorApplication.isPlaying = false;
        //#else
        //    Application.Quit(); // 어플리케이션 종료
        //#endif
    }

}
