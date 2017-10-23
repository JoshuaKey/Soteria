using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutineRun : MonoBehaviour {

    public static RoutineRun Instance;

    private void Awake() {
        print("here");
        Instance = this;
    }
}

public static class ExtensionMethods  {

    //public static IEnumerator FadingIn(AudioSource source, float speed, float maxVolume) {
    //    source.volume = 0;

    //    float currVol = source.volume;
    //    while(currVol < maxVolume) {
    //        currVol += speed * Time.deltaTime;

    //        source.volume = currVol;
    //        yield return null;
    //    }
    //}

    //public static IEnumerator FadingOut(AudioSource source, float speed, float minValue) {

    //    float currVol = source.volume;
    //    while (currVol > 0) {
    //        currVol -= speed * Time.deltaTime;

    //        source.volume = currVol;
    //        yield return null;
    //    }
    //}

    public static IEnumerator FadeIn(this AudioSource source, float speed = 1, float maxVolume = 1) {
        //RoutineRun.Instance.StartCoroutine(FadingIn(source, speed, maxVolume));
        
        source.volume = 0;
        source.Play();

        float currVol = source.volume;
        while (currVol < maxVolume) {
            currVol += speed * Time.deltaTime;

            source.volume = currVol;
            yield return null;
        }

        yield return null;
    }

    public static IEnumerator FadeOut(this AudioSource source, float speed = 1, float minValue = 0) {
        //RoutineRun.Instance.StartCoroutine(FadingOut(source, speed));

        float currVol = source.volume;
        while (currVol > minValue) {
            currVol -= speed * Time.deltaTime;

            source.volume = currVol;
            yield return null;
        }

        if(minValue == 0) {
            source.Stop();
        }

        yield return null;
    }
}
