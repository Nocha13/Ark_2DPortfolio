using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repositions : MonoBehaviour
{
    Collider2D coll2D;

    void Awake()
    {
        coll2D = GetComponent<Collider2D>();
    }

// ORDER : #1) 맵 재배치 로직 구현
    void OnTriggerExit2D(Collider2D coll)
    {
        if (!coll.CompareTag("Area"))
            return;

        Vector3 playerPos = Game_Mgr.Inst.player.transform.position; //플레이어 위치
        Vector3 tilePos = transform.position;                        //타일 맵 위치
        
        switch (transform.tag)
        {
            case "Ground":

                float diffX = playerPos.x - tilePos.x;    //좌표 X
                float diffY = playerPos.y - tilePos.y;    //좌표 Y

                //3항 연산
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;

                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                if (Mathf.Abs(diffX - diffY) <= 0.1f)
                {
                    transform.Translate(Vector3.right * dirX * 60);
                    transform.Translate(Vector3.up * dirY * 60);
                }

                else if (diffX > diffY) //수평 이동
                {
                    transform.Translate(Vector3.right * dirX * 60);
                }
                else if (diffX < diffY) //수직 이동
                {
                    transform.Translate(Vector3.up * dirY * 60);
                }
                break;

            case "Enemy":
                if (coll2D.enabled)
                {
                    Vector3 dist = playerPos - tilePos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}
