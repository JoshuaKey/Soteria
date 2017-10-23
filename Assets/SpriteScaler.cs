using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScaler : MonoBehaviour {

    public int defaultWidth = 1024;
    public int defaultHeight = 768;

    void Start() {
        transform.localScale = new Vector3(defaultWidth / (float)Screen.width, defaultHeight / (float)Screen.height, 1f);
    }
}
