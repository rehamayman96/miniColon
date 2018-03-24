using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDest : MonoBehaviour {

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
