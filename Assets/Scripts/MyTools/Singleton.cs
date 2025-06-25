using UnityEngine;

namespace Leusin.Tools
{
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    private static readonly object lockObj = new object();

    private static bool isShuttingDown = false;

        protected virtual void OnApplicationQuit()
        {
            isShuttingDown = true;
        }

    public static T Instance
        {
            get
            {
                if (isShuttingDown)
                {
                    Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed. Returning null.");
                    return null;
                }

                if (instance == null)
                {
                    lock (lockObj)
                    {
                        instance = FindFirstObjectByType<T>();

                        if (instance == null)
                        {
                            GameObject singletonObj = new GameObject(typeof(T).Name);
                            instance = singletonObj.AddComponent<T>();
                            DontDestroyOnLoad(singletonObj);
                        }
                    }
                }
                return instance;
            }
        }

    protected virtual void Awake()
    {
        // 중복 인스턴스 제거
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        OnAwake();
    }

    protected virtual void OnAwake() { }
}
}