using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleCtrl : MonoBehaviour
{
    public static TitleCtrl Inst;
    Animator anim;
    public static bool isAnim = true;

    void Awake()
    {
        Inst = this;
        anim = GetComponent<Animator>();
    }

    public void StartAnim()
    {
        if (isAnim == true)
        {
            anim.SetTrigger("Start");
            //Debug.Log("���ʽ���");
            isAnim = false;
        }
    }

    public void NextAnim()
    {
        anim.SetTrigger("isNext");
        //Debug.Log("isNext ������");
    }

    void OnEnable()
    {
        if (isAnim == false)
        {
            NextAnim();
        }
    }
}
