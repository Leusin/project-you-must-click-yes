using UnityEngine;

namespace Leusin.Tools
{
    
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _isShuttingDown = false;
        private static readonly object _lockObj = new object();

        protected virtual void OnApplicationQuit()
        {
            _isShuttingDown = true;
        }

        public static T Instance
        {
            get
            {
                if (_isShuttingDown)
                {
                    Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed. Returning null.");
                    return null;
                }

                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        _instance = FindFirstObjectByType<T>();

                        if (_instance == null)
                        {
                            GameObject singletonObj = new GameObject(typeof(T).Name);
                            _instance = singletonObj.AddComponent<T>();
                            DontDestroyOnLoad(singletonObj);
                        }
                    }
                }

                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<T>();
                    obj.name = typeof(T).ToString();
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            // 중복 인스턴스 제거
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }

            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}