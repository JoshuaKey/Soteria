using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleUI : MonoBehaviour {

    public AudioSource failMusic;
    public Text failText;
    public Image fadeBackground;

    public RawImage background;

    public Camera mainCamera;
    public BattleSystem battle;
    public Transform entities;

    public Image border;
    public Text attackText;
    public Text runText;
    public Image selectedActionImage;
    public Image selectedTargetImage;

    public Text actionText;

    public Transform attackTextPos;
    public Transform runTextPos;
    public Transform selectedAction;
    public Transform selectedTarget;

    public List<Transform> enemies = new List<Transform>();
    
    public delegate void InputDetected(Actions action);
    public event InputDetected inputDetected;

    private int currIndex = 0;
    private int actionIndex = -1;
    private int targetIndex = -1;

    private void Start() {

        attackTextPos = attackText.transform;
        runTextPos = runText.transform;

        battle.battleHasEnded += OnBattleEnd;
    }

    private void Update() {
        if(actionIndex == 1) {
            var run = new RunAction();
            run.SetBattleSystem(battle);
            inputDetected(run);
            this.enabled = false;
        } else if(actionIndex == 0) {
            ChooseTarget();
            if(targetIndex != -1) {
                AttackAction action = new AttackAction {
                    attacker = battle.MainChar,
                    target = battle.GetEntity(targetIndex)
                };
                inputDetected(action);
                this.enabled = false;
            }
        } else {
            ChooseAction();
        }
    }

    private void ChooseTarget() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            selectedActionImage.enabled = true;
            selectedTargetImage.enabled = false;
            actionIndex = -1;
            return;
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            print(enemies.Count);
            currIndex -= 1;
            if(currIndex < 0) {
                currIndex = battle.enemyEntities.Length - 1;
            }
            selectedTarget.position = mainCamera.WorldToScreenPoint(battle.enemyEntities[currIndex].ImageTrans.position);
            //selectedTarget.position = mainCamera.WorldToScreenPoint(enemies[currIndex].position);

        } else if (Input.GetKeyDown(KeyCode.S)) {
            currIndex += 1;
            if (currIndex >= battle.enemyEntities.Length) {
                currIndex = 0;
            }
            selectedTarget.position = mainCamera.WorldToScreenPoint(battle.enemyEntities[currIndex].ImageTrans.position);
            //selectedTarget.position = mainCamera.WorldToScreenPoint(enemies[currIndex].position);
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            targetIndex = currIndex;
            currIndex = 0;
            selectedTargetImage.enabled = false;
        }
    }

    private void ChooseAction() {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)) {
            if (currIndex == 0) {
                currIndex = 1;
                selectedAction.position = runTextPos.position;
            } else if (currIndex == 1) {
                currIndex = 0;
                selectedAction.position = attackTextPos.position;
            }

        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            actionIndex = currIndex;
            currIndex = 0;
            selectedActionImage.enabled = false;
            selectedTargetImage.enabled = true;
        }
    }

    public IEnumerator SetActionText(string text) {
        if (!text.Equals("")) {
            actionText.text = text;
        }
        
        yield return new WaitForSeconds(2.0f);
        actionText.text = "";
    }

    private void OnEnable() {
        border.enabled = true;
        attackText.enabled = true;
        runText.enabled = true;
        selectedActionImage.enabled = true;

        actionText.enabled = true;

        Color temp = background.color;
        temp.a = .5f;
        background.color = temp;

        currIndex = 0;
        actionIndex = -1;
        targetIndex = -1;
        selectedAction.position = attackTextPos.position;
        
        Transform[] childTrans = entities.GetComponentsInChildren<Transform>();
        for(int i = 0; i < childTrans.Length; i++) {
            if(childTrans[i].gameObject.tag == "Enemy") {
                enemies.Add(childTrans[i]);
            }
        }
        //mainCamera.WorldToViewportPoint(enemies[currIndex].position)

        //selectedTarget.position = mainCamera.WorldToScreenPoint(enemies[currIndex].position);
        //selectedTarget.position = enemies[currIndex].position;
        selectedTarget.position = mainCamera.WorldToScreenPoint(battle.enemyEntities[currIndex].ImageTrans.position);
    }

    private void OnDisable() {
        
        attackText.enabled = false;
        runText.enabled = false;
        selectedActionImage.enabled = false;
        selectedTargetImage.enabled = false;

        currIndex = 0;
        actionIndex = -1;
        targetIndex = -1;

        enemies.Clear();
    }

    private void OnBattleEnd() {
        actionText.enabled = false;
        border.enabled = false;

        Color temp = background.color;
        temp.a = 1.0f;
        background.color = temp;
    }

    public void Fail() {
        StartCoroutine(failMusic.FadeIn(.1f, .1f));
        StartCoroutine(Fade());
    }

    private IEnumerator Fade() {
        Color guiColor = fadeBackground.color;
        Color textColor = failText.color;

        while (guiColor.a < .99f) {
            guiColor = fadeBackground.color;

            guiColor.a += .2f * Time.deltaTime;
            textColor.a = guiColor.a;

            fadeBackground.color = guiColor;
            failText.color = textColor;
            yield return null;
        }

        yield return StartCoroutine(failMusic.FadeOut(.01f, .1f));

        while(!Input.GetKeyDown(KeyCode.Return)) { yield return null; }

        yield return StartCoroutine(failMusic.FadeOut());
        SceneManager.LoadScene("IntroScreen", LoadSceneMode.Single);
    }
}
