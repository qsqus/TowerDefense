using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void DestroyMusicManager()
    {
        if (instance == this)
        {
            instance = null;
            Destroy(gameObject);
        }
    }
}
