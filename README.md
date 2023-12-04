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
                float diffY = playerPos.y - tilePos.y;
     
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
        }
    }
}
```
</details>

---
* #02)([스크립트](https://github.com/Nocha13/Ark_2DPortfolio/blob/main/ArkSurvivor/Assets/02.Code/EnemyCtrl.cs#L37)) [적 플레이어 자동 추적]

<details>
<summary>예시 코드</summary>
  
```csharp
    void FixedUpdate()
    {
        if (!Game_Mgr.Inst.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * sp * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }
```
</details>

---
* #03)([스크립트](https://github.com/Nocha13/Ark_2DPortfolio/blob/main/ArkSurvivor/Assets/02.Code/PoolMgr.cs)) [적 오브젝트 풀링 구현]

---
* #04)([스크립트](https://github.com/Nocha13/Ark_2DPortfolio/blob/main/ArkSurvivor/Assets/02.Code/Spawn.cs#L29)) [시간에 따른 적 데이터]

<details>
<summary>예시 코드</summary>
  
```csharp
 void Update()
    {
        if (!Game_Mgr.Inst.isLive)
            return;

        timer += Time.deltaTime;
        waveTimer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(Game_Mgr.Inst.gameTime / spawnTime), spawnData.Length - 1);  //인덱스 에러 고침

        if (timer > spawnData[level].c_spawnTime)
        {
            timer = 0;
            Spawns();
        }

        if (waveTimer > waveTime)
        {
            StartCoroutine(Wave());

            waveTimer = 0;
        }
    }
```
</details>

---
* #05)([스크립트](https://github.com/Nocha13/Ark_2DPortfolio/edit/main/ArkSurvivor/Assets/02.Code/Active.cs#L175)) [스킬 능력 구현]

---
* #06)([스크립트](https://github.com/Nocha13/Ark_2DPortfolio/blob/main/ArkSurvivor/Assets/02.Code/Passive.cs#L43)) [패시브 능력 구현]

---
* #07)([스크립트](https://github.com/Nocha13/Ark_2DPortfolio/blob/main/ArkSurvivor/Assets/02.Code/ItemDatas.cs)) [아이템 데이터 구현]

<details>
<summary>예시 코드</summary>
  
```csharp
     [Header("# Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;

    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level")]
    public float baseDamage;        //기본 데미지
    public int baseCount;           //기본(근거리 - 갯수, 원거리 - 관통)
    public float[] damages;         //추가 데미지
    public int[] counts;            //추가(근거리 - 갯수, 원거리 - 관통)

    [Header("# Weapon")]
    public GameObject projectile;   //투사체
```
</details>

---
* #08)([스크립트](https://github.com/Nocha13/Ark_2DPortfolio/blob/main/ArkSurvivor/Assets/02.Code/Items.cs)) [능력 업그레이드]

---
* #09)([스크립트01](https://github.com/Nocha13/Ark_2DPortfolio/edit/main/ArkSurvivor/Assets/02.Code/Game_Mgr.cs#L62))([스크립트02](https://github.com/Nocha13/Ark_2DPortfolio/blob/main/ArkSurvivor/Assets/02.Code/PlayerCtrl.cs#L33)) [플레이어 캐릭 선택]

<details>
<summary>예시 코드</summary>
  
```csharp
 
```
</details>
<!---
Nocha13/Nocha13 is a ✨ special ✨ repository because its `README.md` (this file) appears on your GitHub profile.
You can click the Preview link to take a look at your changes.
--->
