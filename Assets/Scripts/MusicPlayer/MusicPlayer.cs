using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    static bool loaded = false;
    // Use this for initialization
    void Awake() {
        if (!loaded) {
            loaded = true;
            GetComponent<AudioSource>().Play();
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}
