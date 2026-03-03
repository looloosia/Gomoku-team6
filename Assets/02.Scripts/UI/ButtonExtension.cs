using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class ButtonExtension
{
    public static void BindEventWithSound(this Button btn, UnityAction action)
    {
        // 기존에 코드로 연결된 이벤트 싹 비우기 (중복 클릭 버그 방지)
        // 스크립트의 BindButtons()에서 코드로 연결하는 방식에 가장 최적화)
        btn.onClick.RemoveAllListeners();

        // 2. 효과음 재생 + 원래 함수 실행을 하나의 세트로 묶어서 버튼에 달아줍니다.
        btn.onClick.AddListener(() =>
        {
            // 팀원이 만든 사운드 매니저를 호출하여 메뉴 클릭 소리 재생
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX(SFX.MenuSelectionClick);
            }
            
            // 원래 버튼이 해야 할 일(action)을 마저 실행
            action?.Invoke();
        });
    }
}
