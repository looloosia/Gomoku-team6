using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class DemoSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {

        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoad;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected abstract void OnSceneLoad(Scene scene, LoadSceneMode mode);//씬이 바뀌었을 때 호출될 함수

    protected virtual void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
}