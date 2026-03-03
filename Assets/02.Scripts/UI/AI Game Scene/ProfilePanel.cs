using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePanel : MonoBehaviour
{
    [SerializeField] private Image profileImg;
    [SerializeField] private TMP_Text nickname;
    [SerializeField] private TMP_Text rank;
    [SerializeField] private Image markerIcon;

    [SerializeField] private Sprite wmarkerSprite;
    [SerializeField] private Sprite bmarkerSprite;

    // 외부에서 데이터를 던져주면 화면에 그려줌
    public void SetProfileInfo(string _nickname, string _rank, Sprite profileSprite = null)
    {
        nickname.text = _nickname;
        rank.text = _rank;

        if (profileSprite != null)
            profileImg.sprite = profileSprite;
    }

    // 돌 색상 설정
    public void SetMarkerImage(bool isBlack)
    {
        markerIcon.sprite = isBlack ? bmarkerSprite : wmarkerSprite;
        
        // 만약 이전에 stoneIcon.color를 건드렸던 적이 있다면 본래 이미지 색상이 온전히 나오도록 흰색(기본값)으로 초기화
        markerIcon.color = Color.white;
    }
    
}
