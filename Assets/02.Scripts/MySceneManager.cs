using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MySceneManager : Singleton<MySceneManager>
{
    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode) {}

    public void LoadSceneWithCallback<TargetType>(string sceneName, Action<TargetType> onSceneLoad) where TargetType : MonoBehaviour
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.completed += (operation) =>
        {
            TargetType script = FindAnyObjectByType<TargetType>();

            if (script != null)
            {
                onSceneLoad?.Invoke(script);
            }
            else
            {
                Debug.LogWarning($"{sceneName} 씬에서 {typeof(TargetType).Name} 를 찾지 못함");
            }
        };
    }

}
