using UnityEngine;

public class DonotDestoryOnLoad : MonoBehaviour
{
    void Start()
    {
		DontDestroyOnLoad(gameObject);
    }
}
