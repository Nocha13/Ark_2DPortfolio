ArkSurvivor
===


게임 장르 : 2D 로그라이크, 핵 앤 슬래시
---

게임 소개 : 
주어진 시간 동안 적으로부터 살아남는 것이 목표입니다.
---


개발 목적 : 좋아하는 게임 + 장르 플레이
---

사용 엔진 : UNITY 2021.3.17f1
---


개발 기간 : 2023.06 ~ 2023.08
---


포트폴리오 영상
---
[유튜브 영상 링크](https://www.youtube.com/watch?v=LQdjr4ddYrE)


빌드 파일
---
[구글 드라이브 다운로드 링크](https://drive.google.com/file/d/10tfnUTy3M5X7IJ6oDWn9iNFqcnlONGa1/view?usp=drive_link)

* 시작 시 음향이 클 수 있으므로 주의 바랍니다.

다른 포트폴리오 보기
---
[Project Merge](https://github.com/Nocha13/Merge_2DPortfolio.git)

주요 활용 기술
---
* #01)([스크립트](https://github.com/Nocha13/Ark_2DPortfolio/blob/main/ArkSurvivor/Assets/02.Code/Repositions.cs#L14)) [맵 재배치 로직 구현]
  
<details>
<summary>예시 코드</summary>
  
```csharp
 void OnTriggerExit2D(Collider2D coll)
    {
        if (!coll.CompareTag("Area"))
            return;

        Vector3 playerPos = Game_Mgr.Inst.player.transform.position;
        Vector3 tilePos = transform.position;                           

        switch (transform.tag)
        {
            case "Ground":

                float diffX = playerPos.x - tilePos.x;
                float diffY = playerPos.y - 현]

<details>
<summary>예시 코드</summary>
  
```csharp
 public class PoolMgr : MonoBehaviour
{
    //프리팹 보관 변수
    public GameObject[] prefabs;

    //풀 담당 리스트
    List<GameObject>[] pools;
    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int idx = 0; idx < pools.Length; idx++)
        {   //풀 리스트 초기화
            pools[idx] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject sel = null;

        //선택한 풀의 비활성화 오브젝트에 접근
        foreach (GameObject item in pools[index])
        {   //발견시 sel변수에 할당
            if (!item.activeSelf)
            {
                sel = item;
                sel.SetActive(true);
                break;
            }
        }
        //못 찾을시
        if (!sel)
        {   //새롭게 생성, sel변수에 할당
            sel = Instantiate(prefabs[index], transform);
            pools[index].Add(sel);
        }

        return sel;
    }
}
```
</details>
<!---
Nocha13/Nocha13 is a ✨ special ✨ repository because its `README.md` (this file) appears on your GitHub profile.
You can click the Preview link to take a look at your changes.
--->
