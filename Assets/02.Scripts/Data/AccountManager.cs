using System.Linq;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System;

public class AccountManager
{
    private static AccountManager instance;
    public static AccountManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AccountManager();
            }
            return instance;
        }
    }

    private AccountRepository repository = new AccountRepository();

    public Action OnUserDataUpdated;

    public UserData CurrentUser { get; private set; }

    public UserData GetUser(string id)
    {
        return repository.Load(id);
    }

    // 저장소에 아아디(이메일) 중복 확인
    public bool CheckDuplicate(string id)
    {
        return repository.Exists(id);
    }

    // 형식 검사 로직 (패스워드)
    public bool IsValidPassword(string pw)
    {
        return pw.Length >= 8 && pw.Any(char.IsLetter) && pw.Any(char.IsDigit);
    }

    // ==========================================
    // 보안: 비밀번호 암호화
    // ==========================================
    private string HashPassword(string pw)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pw));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }

    // ==========================================
    // 회원가입 / 로그인 로직
    // ==========================================
    public bool Signup(string id, string pw)
    {
        // 1. 아이디 중복 검사
        if (repository.Exists(id))
            return false;

        // 2. 비밀번호 유효성 검사
        if (!IsValidPassword(pw))
            return false;

        // 닉네임 자동 생성 및 중복 방지 (실제 서버이선 DB 유니크 키로 처리)
        string generatedNickname = "User_" + UnityEngine.Random.Range(1000, 9999);
        while(repository.IsNicknameExists(generatedNickname))
            generatedNickname = "User_" + UnityEngine.Random.Range(1000, 9999);

        // 비밀번호 암호화 후 저장
        string hashedPw = HashPassword(pw);
        UserData newUser = new UserData(id, hashedPw, generatedNickname);
        repository.Save(newUser);

        return true;
    }

    public bool Login(string id, string pw)
    {
        var user = repository.Load(id);

        if (user == null)
            return false;

        // 입력받은 비번을 암호화해서 저장된 암호화 비번과 비교
        if (user.pwHash != HashPassword(pw))
            return false;
        
        CurrentUser = user;
        return true;
    }

    // ==========================================
    // 닉네임 검사 및 변경 로직
    // ==========================================
    public bool IsValidNickname(string nickname)
    {
        // 정규표현식 해석:
        // ^ : 문자열의 시작
        // [a-zA-Z0-9가-힣] : 영어 대소문자, 숫자, 한글(완성형)만 허용 (특수문자, 공백 자동 차단)
        // {2,10} : 이 조합으로 2글자 이상, 10글자 이하만 허용!
        // $ : 문자열의 끝

        Regex regex = new Regex(@"^[a-zA-Z0-9가-힣]{2,10}$");
        
        // IsMatch가 true면 완벽하게 조건에 맞는 닉네임, false면 조건 위반
        return regex.IsMatch(nickname);
    }

    public bool TryChangeNickname(string newNickname, out string errorMsg)
    {
        // 유효성 검사 (빈칸, 특수문자, 띄어쓰기, 길이 모두 여기서 한 번에 검사)
        if (!IsValidNickname(newNickname))
        {
            errorMsg = "닉네임은 특수문자와 띄어쓰기 없이\n2~10자의 한글/영문/숫자만 가능합니다.";
            return false;
        }

        // 중복 검사
        if (repository.IsNicknameExists(newNickname))
        {
            errorMsg = "이미 존재하는 닉네임입니다.";
            return false;
        }

        // 3. 통과! 데이터 업데이트
        repository.UpdateNickname(CurrentUser, newNickname);

        OnUserDataUpdated?.Invoke();

        errorMsg = "닉네임이 성공적으로 변경되었습니다!";
        return true;
    }

    // ==========================================
    // 급수 기준과 승급 조건 관련 로직
    // ==========================================
    public void ApplyMatchResult(bool isWin)
    {
        if (CurrentUser == null) return;

        // 이기면 +1, 지면 -1
        CurrentUser.rankPoint += isWin ? 1 : -1;

        int requiredPoints = GetRequiredPoints(CurrentUser.rank);

        // 승급 로직
        if (CurrentUser.rankPoint >= requiredPoints)
        {
            if (CurrentUser.rank > 1) // 1급이 최고 급수
            {
                CurrentUser.rank--; 
                CurrentUser.rankPoint = 0; // 포인트 초기화
                Debug.Log($"승급했습니다! 현재 급수: {CurrentUser.rank}급");
            }
            else
                CurrentUser.rankPoint = requiredPoints; // 1급은 깎이지 않는 한 최대치 유지
        }
        // 강등 로직
        else if (CurrentUser.rankPoint <= -requiredPoints)
        {
            if (CurrentUser.rank < 18) // 18급이 최하급수
            {
                CurrentUser.rank++;
                CurrentUser.rankPoint = 0;
                Debug.Log($"강등되었습니다. 현재 급수: {CurrentUser.rank}급");
            }
            else
                CurrentUser.rankPoint = 0; // 18급에서는 더 안 떨어짐
        }

        repository.Save(CurrentUser); // 결과 저장
    }

    private int GetRequiredPoints(int rank)
    {
        if (rank >= 10 && rank <= 18) return 3;
        if (rank >= 5 && rank <= 9) return 5;
        if (rank >= 1 && rank <= 4) return 10;
        return 3;
    }
}
