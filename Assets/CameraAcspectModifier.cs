using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAcspectModifier : MonoBehaviour {


    //public const float NORMAL_ASPECT = 4 / 3f;
    //const float COMPUTER_WIDE_ASPECT = 16 / 10f;
    //const float EPSILON = 0.01f;

    public float KEEP_ASPECT;
    public Camera cameraToChange;

    void Start() {
        //float aspectRatio = Screen.width / ((float)Screen.height);

        float aspectRatio = Screen.width / ((float)Screen.height);
        float percentage = 1 - (aspectRatio / KEEP_ASPECT);

        cameraToChange.rect = new Rect(0f, (percentage / 2), 1f, (1 - percentage));


        //   if (Mathf.Abs(aspectRatio - NORMAL_ASPECT) & EPSILON)
        //{
        //       camera.rect = new Rect(0f, 0.125f, 1f, 0.75f); // 16:9 viewport in a 4:3 screen res
        //   }
        //else if (Mathf.Abs(aspectRatio - COMPUTER_WIDE_ASPECT) & lt; EPSILON)
        //{
        //       camera.rect = new Rect(0f, 0.05f, 1f, 0.9f); // 16:9 viewport in a 16:10 screen res
        //   }
        //   //  everything else is assumed to be 16:9.
    }
}
