using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private bool isDontDestroy;

    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            instance = (T)FindObjectOfType(typeof(T));

            if (instance == null)
            {
                Debug.LogError($"SingletonMonoBehaviour エラー");
            }

            return instance;
        }
    }

    private void Awake()
    {
        OnAwake();

        if (!isDontDestroy)
        {
            return;
        }

        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    protected virtual void OnAwake() { }
}