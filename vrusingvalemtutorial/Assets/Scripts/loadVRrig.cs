using UnityEngine;

public class loadVRrig : MonoBehaviour
{
    private static GameObject original = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (original == null)
            original = gameObject;
        if (gameObject != original)
            Destroy(gameObject);


    }
    void Start()
    {
        DontDestroyOnLoad(this);
    }
}
