using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistent : MonoBehaviour {

    //private static Persistent instance = null;
    //public static Persistent Instance {
    //    get { return instance; }
    //}
    //void Awake() {
    //    if (instance != null && instance != this) {
    //        Destroy(this.gameObject);
    //        return;
    //    } else {
    //        instance = this;
    //    }
    //    DontDestroyOnLoad(this.gameObject);
    //}

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
	}
	

}
