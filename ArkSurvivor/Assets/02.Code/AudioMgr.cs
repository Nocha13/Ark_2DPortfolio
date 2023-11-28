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

    [Header("Title")] //Ÿ��Ʋ�� ����
    public AudioClip titleClip;
    AudioSource titlePlayer = null;
    public float titleVol = 1;

    [Header("BGM")] //����� ����
    public AudioClip bgmClip;
    AudioSource bgmPlayer = null;
    public float bgmVol = 1;
    AudioLowPassFilter highPass;   //�������� ���

    [Header("SFX")] //ȿ���� ����
    public AudioClip[] sfxClip;
    AudioSource[] sfxPlayer = null;
    public float sfxVol = 1;

    public int channels;    //ä�� ��
    int chIdx;              //ä�� ��ȣ

    void Awake()
    {
        Inst = this;
        Init();
    }

    void Start()
    {
        if (BGMToggle != null)
            BGMToggle.onValueChanged.AddListener(BGMOnOff);

        if (SFXToggle != null)
            SFXToggle.onValueChanged.AddListener(SFXOnOff);

        if (BGMSlider != null)
            BGMSlider.onValueChanged.AddListener(BSliderChange);

        if (SFXSlider != null)
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

    public void BGMCtrl()
    {
        float sound = BGMSlider.value;

        if (sound == -40f)
            masterMixer.SetFloat("BGM", -80);
        else
            masterMixer.SetFloat("BGM", sound);
    }

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

    void Init() //�÷��̾� �ʱ�ȭ
    {
        //Title
        GameObject titleObj = new GameObject("TitlePlayer");
        titleObj.transform.parent = transform;                      //Ÿ��Ʋ�� �ڽ� ������Ʈ ����
        titlePlayer = titleObj.AddComponent<AudioSource>();         //������Ʈ ������ҽ� ���� 
        titlePlayer.playOnAwake = false;                            //���۽� ��� OFF
        titlePlayer.loop = true;                                    //Ÿ��Ʋ�� �ݺ� ON
        titlePlayer.volume = titleVol;                              //����
        titlePlayer.clip = titleClip;                               //���� ����
        titlePlayer.outputAudioMixerGroup = masterMixer.FindMatchingGroups("BGM")[0];

        //BGM 
        GameObject bgmObj = new GameObject("BGMPlayer");
        bgmObj.transform.parent = transform;                        //����� �ڽ� ������Ʈ ����
        bgmPlayer = bgmObj.AddComponent<AudioSource>();             //������Ʈ ������ҽ� ����     
        bgmPlayer.playOnAwake = false;                              //���۽� ��� OFF
        bgmPlayer.loop = true;                                      //����� �ݺ� ON
        bgmPlayer.volume = bgmVol;                                  //����
        bgmPlayer.clip = bgmClip;                                   //���� ����
        highPass = Camera.main.GetComponent<AudioLowPassFilter>();
        bgmPlayer.outputAudioMixerGroup = masterMixer.FindMatchingGroups("BGM")[0];

        //SFX 
        GameObject sfxObj = new GameObject("SFXPlayer");
        sfxObj.transform.parent = transform;                        //����� �ڽ� ������Ʈ ����
        sfxPlayer = new AudioSource[channels];                      //ä�� �� �̿�, ������ҽ� �迭 �ʱ�ȭ

        for (int idx = 0; idx < sfxPlayer.Length; idx++)
        {
            sfxPlayer[idx] = sfxObj.AddComponent<AudioSource>();    //��� ȿ���� ������ҽ� ����, ���� �ݺ�
            sfxPlayer[idx].playOnAwake = false;                     //���۽� ��� OFF
            sfxPlayer[idx].bypassListenerEffects = true;            //AudioLowPassFilter ���� ���� �ʵ��� ON
            sfxPlayer[idx].volume = sfxVol;                         //����
            sfxPlayer[idx].outputAudioMixerGroup = masterMixer.FindMatchingGroups("SFX")[0];
        }
    }

    public void TitleBgm(bool isPlay)
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

    public void PlayBgm(bool isPlay)
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
            //ä�� ����ŭ ��ȸ
            int loopIdx = (idx + chIdx) % sfxPlayer.Length;

            if (sfxPlayer[loopIdx].isPlaying)
                continue;    //�ݺ��� [���� ����]�� �ǳʶ�

            int ranIdx = 0; //���� �̸� ȿ���� ���� ���
            if (sfx == SFX.Hit || sfx == SFX.Melee)
            {
                ranIdx = Random.Range(0, 2);
            }

            chIdx = loopIdx;
            sfxPlayer[loopIdx].clip = sfxClip[(int)sfx + ranIdx];
            sfxPlayer[loopIdx].Play();
            break;          //ȿ���� ��� �� �ݺ��� ����
        }
    }

    public void HighPass(bool isPlay)
    {
        highPass.enabled = isPlay;
    }
}
