using UnityEngine;

[DefaultExecutionOrder(-10)]
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;
    private static bool _applicationIsQuitting = false;

    [Header("Dont Destroy On Load?")] [SerializeField]
    private bool dontDestroy;

    public static T Instance
    {
        get
        {
            if (!_applicationIsQuitting) return _instance;
            Debug.LogWarning("[MonoSingleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
            return null;
        }
    }

    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    private void InitializeSingleton()
    {
        _applicationIsQuitting = false;
        if (_instance == null)
        {
            _instance = this as T;
            if (dontDestroy) DontDestroyOnLoad(this.gameObject);
//      Debug.Log("INIT");
        }
        else if (_instance != this)
        {
            HandleDuplicateInstance();
        }
    }

    private void HandleDuplicateInstance()
    {
        Debug.LogWarning("[MonoSingleton] Duplicate instance of " + typeof(T) + " found. Destroying the new one.");
        Destroy(gameObject);
    }
    /*private void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _applicationIsQuitting = true;
        }
    }*/
}