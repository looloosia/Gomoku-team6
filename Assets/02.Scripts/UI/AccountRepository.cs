using UnityEngine;

// 데이터 저장/조회 담당하는 로직
public class AccountRepository : MonoBehaviour
{
    // 유저마다 고유키를 만들기 위한 클래스 
    public string GetKey(string id)
    {
        return "USER_" + id;
    }
    
    // 해당 키 존재 여부 확인
    public bool Exists(string id)
    {
        return PlayerPrefs.HasKey(GetKey(id));
    }

    // 아이디를 키로 저장
    public void Save(UserData user)
    {
        string json = JsonUtility.ToJson(user);
        PlayerPrefs.SetString(GetKey(user.id), json);
        PlayerPrefs.Save();
    }

    public UserData Load(string id)
    {
        if (!Exists(id))
            return null;

        string json = PlayerPrefs.GetString(GetKey(id));
        return JsonUtility.FromJson<UserData>(json);
    }
}
