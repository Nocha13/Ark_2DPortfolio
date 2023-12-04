using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMgr : MonoBehaviour
{
    public enum BGM
    {
        Title = 0,
        BGM
    }

    public enum SFX
    {
        Dead,
        Hit,
        LevelUp = 3,
        Lose,
        Melee,
        Range = 7,
        Select,
        Win = 9,
        Cancle,
        Join,        
        Heal,
        Lightning,
        Front,
        Explosion,
        Spike,
        Shield
    }

    public static AudioMgr Inst;

    public AudioMixer masterMixer;
    public Slider BGMSlider;
    public Slider SFXSlider;
    public Toggle BGMToggle;
    public Toggle SFXToggle;
    [HideInInspector] public bool isBgmOnOff = true;
    [HideInInspector] public bool isSfxOnOff = true;

    [Header("Title")] //타이틀음 관련
    public AudioClip titleClip;
    AudioSource titlePlayer = null;
    public float titleVol = 1;

    [Header("BGM")] //배경음 관련
    public AudioClip bgmClip;
    AudioSource bgmPlayer = null;
    public float bgmVol = 1;
    AudioLowPassFilter highPass;   //저음역대 통과

    [Header("SFX")] //효과음 관련
    public AudioClip[] sfxClip;
    AudioSource[] sfxPlayer = null;
    public float sfxVol = 1;

    public int channels;    //채널 수
    int chIdx;              //채널 번호

    void Awake()
    {
        Inst = this;
        Init();
    }

    void Start()
    {
        if (BGMToggle != null)    //BGM 토글
            BGMToggle.onValueChanged.AddListener(BGMOnOff);

        if (SFXToggle != null)    //SFX 토글
            SFXToggle.onValueChanged.AddListener(SFXOnOff);
    
        if (BGMSlider != null)    //BGM 슬라이더
            BGMSlider.onValueChanged.AddListener(BSliderChange);

        if (SFXSlider != null)    //SFX 슬라이더
            SFXSlider.onValueChanged.AddListener(SSliderChange);

        int a_bgmOnOff = PlayerPrefs.GetInt("BgmOnOff", 1);
        int a_sfxOnOff = PlayerPrefs.GetInt("SfxOnOff", 1);
        
        if (BGMToggle != null || SFXToggle != null)
        {
            if (a_bgmOnOff == 1)
                BGMToggle.isOn = true;
            else
                BGMToggle.isOn = false;

            if (a_sfxOnOff == 1)
                SFXToggle.isOn = true;
            else
                SFXToggle.isOn = false;
        }

        if (BGMSlider != null)
            BGMSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0);

        else if (SFXSlider != null)
            SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0);
    }

    //슬라이더 볼륨
    public void BGMCtrl()
    {
        float sound = BGMSlider.value;

        if (sound == -40f)
            masterMixer.SetFloat("BGM", -80);
        else
            masterMixer.SetFloat("BGM", sound);
    }
    //슬라이더 볼륨
    public void SFXCtrl()
    {
        float sound = SFXSlider.value;

        if (sound == -40f)
            masterMixer.SetFloat("SFX", -80);
        else
            masterMixer.SetFloat("SFX", sound);
    }

    void BGMOnOff(bool val)
    {
        if (BGMToggle != null)
        {
            if (val == true)
                PlayerPrefs.SetInt("BgmOnOff", 1);
            else
                PlayerPrefs.SetInt("BgmOnOff", 0);

            BgmOnOff(val);
        }
    }

    void BgmOnOff(bool a_bgmOnOff = true)
    {
        bool a_bgmMuteOnOff = !a_bgmOnOff;

        if (bgmPlayer != null || titlePlayer != null)
        {
            bgmPlayer.mute = a_bgmMuteOnOff;
            titlePlayer.mute = a_bgmMuteOnOff;
        }

        isBgmOnOff = a_bgmOnOff;
    }

    void BSliderChange(float val)
    {
        PlayerPrefs.SetFloat("BGMVolume", val);
        BgmVolume(val);
    }

    void BgmVolume(float bVolume)
    {    
        titleVol = bVolume;
        bgmVol = bVolume;
    }

    void SFXOnOff(bool val)
    {
        if (SFXToggle != null)
        {
            if (val == true)
                PlayerPrefs.SetInt("SfxOnOff", 1);
            else
                PlayerPrefs.SetInt("SfxOnOff", 0);

            SfxOnOff(val);
        }
    }

    void SfxOnOff(bool a_sfxOnOff = true)
    {
        bool a_sfxMuteOff = !a_sfxOnOff;

        for (int idx = 0; idx < channels; idx++)
        {
            if (sfxPlayer[idx] != null)
            {
                sfxPlayer[idx].mute = a_sfxMuteOff;
            }
        }

        isSfxOnOff = a_sfxOnOff;
    }

    void SSliderChange(float val)
    {
        PlayerPrefs.SetFloat("SFXVolume", val);
        Sfxvolume(val);
    }

    void Sfxvolume(float sVolume)
    {
        sfxVol = sVolume;
    }

    void Init() //플레이어 초기화
    {
        //Title
        GameObject titleObj = new GameObject("TitlePlayer");
        titleObj.transform.parent = transform;                      //타이틀음 자식 오브젝트 생성
        titlePlayer = titleObj.AddComponent<AudioSource>();         //컴포넌트 오디오소스 생성 
        titlePlayer.playOnAwake = false;                            //시작시 재생 OFF
        titlePlayer.loop = true;                                    //타이틀음 반복 ON
        titlePlayer.volume = titleVol;                              //볼륨
        titlePlayer.clip = titleClip;                               //음원 파일
        titlePlayer.outputAudioMixerGroup = masterMixer.FindMatchingGroups("BGM")[0];

        //BGM 
        GameObject bgmObj = new GameObject("BGMPlayer");
        bgmObj.transform.parent = transform;                        //배경음 자식 오브젝트 생성
        bgmPlayer = bgmObj.AddComponent<AudioSource>();             //컴포넌트 오디오소스 생성     
        bgmPlayer.playOnAwake = false;                              //시작시 재생 OFF
        bgmPlayer.loop = true;                                      //배경음 반복 ON
        bgmPlayer.volume = bgmVol;                                  //볼륨
        bgmPlayer.clip = bgmClip;                                   //음원 파일
        highPass = Camera.main.GetComponent<AudioLowPassFilter>();
        bgmPlayer.outputAudioMixerGroup = masterMixer.FindMatchingGroups("BGM")[0];

        //SFX 
        GameObject sfxObj = new GameObject("SFXPlayer");
        sfxObj.transform.parent = transform;                        //배경음 자식 오브젝트 생성
        sfxPlayer = new AudioSource[channels];                      //채널 값 이용, 오디오소스 배열 초기화

        for (int idx = 0; idx < sfxPlayer.Length; idx++)
        {
            sfxPlayer[idx] = sfxObj.AddComponent<AudioSource>();    //모든 효과음 오디오소스 생성, 저장 반복
            sfxPlayer[idx].playOnAwake = false;                     //시작시 재생 OFF
            sfxPlayer[idx].bypassListenerEffects = true;            //AudioLowPassFilter 영향 받지 않도록 ON
            sfxPlayer[idx].volume = sfxVol;                         //볼륨
            sfxPlayer[idx].outputAudioMixerGroup = masterMixer.FindMatchingGroups("SFX")[0];
        }
    }

    public void TitleBgm(bool isPlay) //타이틀
    {
        if (isPlay)
        {
            titlePlayer.Play();
        }

        else
        {
            titlePlayer.Stop();
        }
    }

    public void PlayBgm(bool isPlay)    //플레이이
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }

        else
        {
            bgmPlayer.Stop();
        }
    }

    public void SelBtn()
    {
        PlaySfx(AudioMgr.SFX.Select);
    }

    public void CanBtn()
    {
        PlaySfx(AudioMgr.SFX.Cancle);
    }

    public void Join()
    {
        PlaySfx(AudioMgr.SFX.Join);
    }


    public void PlaySfx(SFX sfx)
    {
        for (int idx = 0; idx < sfxPlayer.Length; idx++)
        {
            //채널 수만큼 순회
            int loopIdx = (idx + chIdx) % sfxPlayer.Length;

            if (sfxPlayer[loopIdx].isPlaying)
                continue;    //반복문 [다음 루프]로 건너뜀

            int ranIdx = 0; //같은 이름 효과음 랜덤 재생
            if (sfx == SFX.Hit || sfx == SFX.Melee)
            {
                ranIdx = Random.Range(0, 2);
            }

            chIdx = loopIdx;
            sfxPlayer[loopIdx].clip = sfxClip[(int)sfx + ranIdx];
            sfxPlayer[loopIdx].Play();
            break;          //효과음 재생 후 반복문 종료
        }
    }

    public void HighPass(bool isPlay)
    {
        highPass.enabled = isPlay;
    }
}
