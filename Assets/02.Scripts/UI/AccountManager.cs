using System.Linq;
using UnityEngine;

public class AccountManager : MonoBehaviour // 싱글톤으로 변경 예정
{
    private AccountRepository repository = new AccountRepository();

     public UserData CurrentUser { get; private set; }

    // 저장소에 아아디(이메일) 중복 확인
    public bool CheckDuplicate(string id)
    {
        return repository.Exists(id);
    }

    public bool Repository(string id, string pw)
    {
        // 1. 아이디 중복 검사
        if (repository.Exists(id))
            return false;

        // 2. 비밀번호 유효성 검사
        if (!IsValidPassword(pw))
            return false;

        // 3. UserData 생성
        UserData newUser = new UserData(id, pw);

        // 4. 저장
        repository.Save(newUser);

        return true;
    }

    public bool Login(string id, string pw)
    {
        var user = repository.Load(id);

        if (user == null)
            return false;

        if (user.pw != pw)
            return false;
        
        CurrentUser = user;
        return true;
    }

    public UserData GetUser(string id)
    {
        return repository.Load(id);
    }

    // 형식 검사 로직 (패스워드)
    public bool IsValidPassword(string pw)
    {
        return pw.Length >= 8 && pw.Any(char.   IsLetter) && pw.Any(char.IsDigit);
    }
}
