using UnityEngine;

// 데이터 저장/조회 담당하는 로직
public class AccountRepository
{
    // 유저마다 고유키를 만들기 위한 클래스 
    public string GetKey(string id)
    {
        return "USER_" + id;
    }

    // 닉네임 중복 검사를 위한 전용 키 생성 (DB의 Unique Key 역할)
    public string GetNicknameKey(string nickname)
    {
        return "NICKNAME_" + nickname;
    }
    
    // 해당 키 존재 여부 확인
    public bool Exists(string id)
    {
        return PlayerPrefs.HasKey(GetKey(id));
    }

    // 닉네임 중복 여부 확인
    public bool IsNicknameExists(string nickname)
    {
        return PlayerPrefs.HasKey(GetNicknameKey(nickname));
    }

    // 데이터 저장 (아이디 등록 + 닉네임 선정)
    public void Save(UserData user)
    {
        string json = JsonUtility.ToJson(user);
        PlayerPrefs.SetString(GetKey(user.id), json);
        PlayerPrefs.SetString(GetNicknameKey(user.nickname), user.id);
        PlayerPrefs.Save();
    }

    public UserData Load(string id)
    {
        if (!Exists(id))
            return null;

        string json = PlayerPrefs.GetString(GetKey(id));
        return JsonUtility.FromJson<UserData>(json);
    }

    public void UpdateNickname(UserData user, string newNickname)
    {
        PlayerPrefs.DeleteKey(GetNicknameKey(user.nickname));
        
        user.nickname = newNickname;

        Save(user);
    }
}
