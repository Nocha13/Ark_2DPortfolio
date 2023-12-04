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
* #01)([스크립트](https://github.com/Nocha13/Ark_2DPortfolio/blob/main/ArkSurvivor/Assets/02.Code/Repositions.cs)) [맵 재배치 로직 구현]
  
<details>
<summary>예시 코드</summary>
  
```csharp
public override IEnumerable<BattleUnit> SetTarget(BattleUnit actionUnit, List<GridPosition> targetUnits)
{
    // 적군에서 가장 체력 낮은 적을 타겟으로 잡음
    return targetUnits.GetEnemyTarget(actionUnit, this).OrderLowHealth().GetTargetNum(this).SelectBattleUnit();
}
```

</details>
<!---
Nocha13/Nocha13 is a ✨ special ✨ repository because its `README.md` (this file) appears on your GitHub profile.
You can click the Preview link to take a look at your changes.
--->
