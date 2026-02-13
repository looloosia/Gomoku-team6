# 오목 6팀
Notion: https://www.notion.so/304f829a649e80cdb211f7182398ded0?v=304f829a649e809eb57f000c219740e4&source=copy_link

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
- 이름/구현기능: 작업용

## 작업 방법
1. 'develop'에서 브랜치 생성: '이름/구현기능'
2. (선택) base: develop 으로 Draft PR 생성 
3. 기능이 최소 동작/테스트 되면
  - Draft PR 생성했을 경우  
     > Ready for review 클릭  
   - Draft PR 생성 안 했을 경우  
     > PR 생성
4. 리뷰 후 merge (에러 등 확인)
5. 최종 제출 시 develop -> main 으로 PR

## 규칙
- 같은 씬을 동시에 수정X (hierarchy, 오브젝트 배치, 속성/컴포넌트 변경)
- 버그, 기능 제안은 Issues로 등록 (github 상단 Issues 탭)
