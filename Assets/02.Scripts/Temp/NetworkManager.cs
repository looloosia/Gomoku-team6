using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkManager : Singleton<NetworkManager>
{
    [Header("서버 주소 설정")]
    private string server = "https://pricilla-multibranched-photochemically.ngrok-free.dev/";

    public void Res(string type, UnityAction<string> onSuccess, UnityAction<string> onError = null)
    {
        StartCoroutine(GetCoroutine(type, onSuccess, onError));
    }

    private IEnumerator GetCoroutine(string type, UnityAction<string> onSuccess, UnityAction<string> onError = null)
    {
        string url = server + type;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"통신 실패: {request.error}");
                onError?.Invoke(request.error);
            }
            else
            {
                string responseText = request.downloadHandler.text;
                Debug.Log($"응답 성공 가져온 데이터: {responseText}");
                onSuccess?.Invoke(responseText);
            }
        }
    }

    public void Req(string type, string jsonBody, UnityAction<string> onSuccess, UnityAction<string> onError = null)
    {
        StartCoroutine(PostCoroutine(type, jsonBody, onSuccess, onError));
    }
    private IEnumerator PostCoroutine(string type, string json, UnityAction<string> success, UnityAction<string> error = null)
    {
        string url = server + type;

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"통신 실패: {request.error}");
                error?.Invoke(request.error); 
            }
            else
            {
                string responseText = request.downloadHandler.text;
                Debug.Log($"응답 성공: {responseText}");
                success?.Invoke(responseText);
            }
        }
    }

    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode) { }
}
