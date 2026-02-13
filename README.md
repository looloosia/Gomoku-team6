# 오목 6팀

## 환경
- Unity: 6000.3.7f1

## 프로젝트 구조
- Assets/01.Scenes
- Assets/02.Scripts
- Assets/03.Prefabs
- Assets/04.Sprites
- Assets/05.Materials
- Assets/1_UI_Scenes
- Assets/99.Resources

## 브랜치 전략
- main: 제출용(직접 push 금지됨)
- develop: 병합용
- 이름: 작업용

## 작업 방법
1. 'develop'에서 브랜치 생성
2. 해당 브랜치에서 commit, push
3. base: develop으로 PR 생성
4. console 에러 없으면 merge
5. 제출 시 develop -> main 으로 PR

## 규칙
- 같은 씬을 동시에 수정X (hierarchy, 오브젝트 배치, 속성/컴포넌트 변경)
