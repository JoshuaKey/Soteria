using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroSceneManager : MonoBehaviour {

    [SerializeField]
    private Image backgroundFade;
    [SerializeField]
    private AudioSource backgroundMusic;

    private void Start() {
        StartCoroutine(backgroundMusic.FadeIn());
    }
   

    public void Play() {
        StartCoroutine(Fade());
        StartCoroutine(backgroundMusic.FadeOut());
    }

    public void Load() {
        print("Have not implemented Loading and Saving yet.");
    }

    public void Exit() {
        Application.Quit();
    }

    private IEnumerator Fade() {

        Color guiColor = backgroundFade.color;

        while (guiColor.a < .99f) {
            guiColor = backgroundFade.color;

            guiColor.a += .5f * Time.deltaTime;
            //print(guiColor.a);

            backgroundFade.color = guiColor;
            yield return null;
        }

        SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single);
    }
}
