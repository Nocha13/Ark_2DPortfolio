using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public GameObject FadePannel;
    public static Fade Inst;

    void Awake()
    {
        Inst = this;
    }

    //페이드 아웃
    public IEnumerator FadeInStart()
    {
        FadePannel.SetActive(true);
        for (float f = 1f; f > 0; f -= 0.02f)
        {
            Color c = FadePannel.GetComponent<Image>().color;
            c.a = f;
            FadePannel.GetComponent<Image>().color = c;
            yield return null;
        }
        yield return new WaitForSeconds(2.5f);
        FadePannel.SetActive(false);
        Game_Mgr.Inst.isStart = true;
        Game_Mgr.Inst.isLive = true;
    }

    //페이드 인
    public IEnumerator FadeOutStart()
    {
        FadePannel.SetActive(true);
        for (float f = 0f; f < 1; f += 0.02f)
        {
            Color c = FadePannel.GetComponent<Image>().color;
            c.a = f;
            FadePannel.GetComponent<Image>().color = c;

            yield return null;
        }
    }


    //public bool IsFadeOut = false;  //나갈 때 연출할 건지의 여부
    //public bool IsFadeIn = false;  //들어올 때 연출할 건지의 여부

    ////--- Fade In Out 관련 변수들...
    //Image m_FadeImg = null;
    //float AniDuring = 3f;     //페이드아웃 연출 시간 설정
    //public bool m_StartFade = false;
    //float m_CacTime = 0.0f;
    //float m_AddTimer = 0.0f;
    //Color m_Color;

    //float m_StVal = 1.0f;
    //float m_EndVal = 0.0f;

    ////--- Fade In Out 관련 변수들...

    //public static Fade Inst = null;

    //void Awake()
    //{
    //    Inst = this;
    //}

    //void Update()
    //{
    //    if (m_FadeImg == null)
    //        return;

    //    FadeUpdate();
    //}

    //void FadeUpdate()
    //{
    //    if (m_StartFade == false)
    //        return;

    //    if (m_CacTime < 1.0f)
    //    {
    //        m_AddTimer += Time.deltaTime;
    //        m_CacTime = m_AddTimer / AniDuring;
    //        m_Color = m_FadeImg.color;
    //        m_Color.a = Mathf.Lerp(m_StVal, m_EndVal, m_CacTime);
    //        m_FadeImg.color = m_Color;

    //        if (1.0f <= m_CacTime)
    //        {
    //            if (m_StVal == 1.0f && m_EndVal == 0.0f) //들어올 때 
    //            {
    //                m_Color.a = 0.0f;
    //                m_FadeImg.color = m_Color;
    //                m_FadeImg.gameObject.SetActive(false);
    //                m_StartFade = false;
    //            }
    //        }//if(1.0f < m_CacTime)
    //    }//if(m_CacTime < 1.0f)
    //}//void FadeUpdate()

    //public void FindFade()
    //{
    //    GameObject a_Canvas = GameObject.Find("Canvas");
    //    if (a_Canvas != null)
    //    {
    //        //매개변수 true의 의미는 Active가 꺼져 있는 게임오브젝트도 모두 가져오라는 뜻
    //        Image[] a_ImgList = a_Canvas.transform.GetComponentsInChildren<Image>(true);
    //        for (int ii = 0; ii < a_ImgList.Length; ii++)
    //        {
    //            if (a_ImgList[ii].gameObject.name == "Fade")
    //            {
    //                m_FadeImg = a_ImgList[ii];
    //                break;
    //            }
    //        }
    //    }//if(a_Canvas != null)

    //    //---- Fade In 초기화
    //    if (m_FadeImg != null && IsFadeIn == true)
    //    {
    //        m_StVal = 1.0f;
    //        m_EndVal = 0.0f;
    //        m_FadeImg.color = new Color32(0, 0, 0, 255);
    //        m_FadeImg.gameObject.SetActive(true);
    //        m_StartFade = true;
    //    }
    //}
}
