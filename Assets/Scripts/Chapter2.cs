using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chapter2 : MonoBehaviour {

    [SerializeField]
    private RawImage background;
    [SerializeField]
    private Texture originalBackgroundPicture;
    [SerializeField]
    private Texture newBackgroundPicture;
    [SerializeField]
    private Image backgroundFader;

    [SerializeField]
    private DialogueUI dialogueUI;
    [SerializeField]
    private StorySystem story;

    [SerializeField]
    private Transform playerTrans;
    public Vector3 playerPos;
    [SerializeField]
    private Sprite playerPic;


    [SerializeField]
    public Transform allyTrans;
    public Vector3 allyPos;
    [SerializeField]
    private Sprite allyPic;

    [SerializeField]
    public SpriteRenderer enemy1;
    [SerializeField]
    public Transform burglar1Trans;
    public Vector3 burglar1Pos;
    [SerializeField]
    private Sprite burglar1Pic;

    [SerializeField]
    public SpriteRenderer enemy2;
    [SerializeField]
    public Transform burglar2Trans;
    [SerializeField]
    private Sprite burglar2Pic;

    [SerializeField]
    public SpriteRenderer enemy3;

    private bool displayingText = false;

    public delegate void Battle();
    public event Battle battle;

    public Vector3 turnPointPlayer;
    public Vector3 turnPointAlly;

    public Vector3 chapterEscape;

    public Vector3 chapter2Arrive;

    public Vector3 waitingPoint;
    public Vector3 burglarHidingPoint;

    // Use this for initialization
    void Start () {

        dialogueUI.endOfDialogue += Continue;
    }

    public IEnumerator Begin() {
        enemy1.enabled = true;
        enemy2.enabled = false;
        enemy3.enabled = false;

        playerTrans.position = playerPos;
        allyTrans.position = allyPos;
        burglar1Trans.position = burglar1Pos;

        if (story.justKilled) {
            dialogueUI.QueueDialogue(new Dialogue(allyPic, "You just killed him!"));
            dialogueUI.QueueDialogue(new Dialogue(playerPic, "I had no choice."));
            displayingText = true;
            while (displayingText) {
                yield return null;
            }
        }
        yield return new WaitForSeconds(.5f);
        dialogueUI.QueueDialogue(new Dialogue(playerPic, "Come on, lets get out of here."));
        displayingText = true;
        while (displayingText) {
            yield return null;
        }

        StartCoroutine(Movement(playerTrans, 7.0f, turnPointPlayer));
        yield return StartCoroutine(Movement(allyTrans, 6.0f, turnPointAlly));

        StartCoroutine(Movement(playerTrans, 7.0f, chapterEscape));
        yield return StartCoroutine(Movement(allyTrans, 6.0f, chapterEscape));

        yield return StartCoroutine(Fader());

        StartCoroutine(Movement(playerTrans, 1.5f, waitingPoint + new Vector3(.5f, .5f, .0f)));
        StartCoroutine(Movement(allyTrans, 1.5f, waitingPoint));

        dialogueUI.QueueDialogue(new Dialogue(playerPic, "Are you ok?"));
        dialogueUI.QueueDialogue(new Dialogue(allyPic, "Yeah."));

        dialogueUI.QueueDialogue(new Dialogue(playerPic, "What's your name?"));
        dialogueUI.QueueDialogue(new Dialogue(allyPic, "Remi"));
        dialogueUI.QueueDialogue(new Dialogue(playerPic, "Mat"));

        displayingText = true;
        while (displayingText) {
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        dialogueUI.QueueDialogue(new Dialogue(playerPic, "Do you know where you live? I can take you..."));

        displayingText = true;
        while (displayingText) {
            yield return null;
        }

        StopAllCoroutines();

        StartCoroutine(Movement(burglar1Trans, 20.0f, playerTrans.position + new Vector3(3.0f, .0f, .0f)));
        yield return StartCoroutine(Movement(burglar2Trans, 20.0f, playerTrans.position - new Vector3(3.0f, .0f, .0f)));

        dialogueUI.QueueDialogue(new Dialogue(burglar2Pic, "Well, look what we have here."));
        if (story.justKilled) {
            dialogueUI.QueueDialogue(new Dialogue(playerPic, "Stay behind me."));
        } else {
            dialogueUI.QueueDialogue(new Dialogue(playerPic, "What do you want?"));
        }

        displayingText = true;
        while (displayingText) {
            yield return null;
        }

        battle();
        dialogueUI.endOfDialogue -= Continue;
    }

    public IEnumerator Movement(Transform obj, float speed, Vector3 dest) {

        Vector3 mvmt = Vector3.Normalize(dest - obj.position) * speed; // Direction Vector with Speed
        while (Vector3.Distance(obj.position, dest) > .2f) {

            Vector3 move = mvmt * Time.deltaTime; // How much we'll move.
            Vector3 dist = dest - obj.position; // Vector pointing from Obj to Dest

            // WE get the lenght squared of both, aka how far we'll move
            // If our distance moved is greater than our actual distance
            // Then break and set our position
            if (Vector3.Dot(move, move) > Vector3.Dot(dist, dist)) {
                //obj.position += dist;
                print("Too Close");
                break;
            }
            // We calculate the relative direction of our mvmt vector with our dist vecotr
            // If less than 0, then we moved too far, set our position
            else if (Vector3.Dot(mvmt, dist) < 0) {
                //mvmt = Vector3.Normalize(dest - obj.position) * speed;
                print("Too Far");
                break;
            }
            // Otherwise, move in direction
            else {
                obj.position += move;
            }

            yield return null;
        }
        obj.position = dest;
        yield return null;
    }

    public IEnumerator Fader() {
        Color guiColor = backgroundFader.color;

        while (guiColor.a < .99f) {
            guiColor = backgroundFader.color;

            guiColor.a += .5f * Time.deltaTime;
            //print(guiColor.a);

            backgroundFader.color = guiColor;
            yield return null;
        }

        background.texture = newBackgroundPicture;
        //background.texture = null;
        playerTrans.position = chapter2Arrive + new Vector3(.5f, .5f, .0f);
        allyTrans.position = chapter2Arrive;

        enemy2.enabled = true;

        burglar1Trans.position = burglarHidingPoint;
        Vector3 temp = burglarHidingPoint;
        temp.x = -temp.x;
        burglar2Trans.position = temp;

        while (guiColor.a > .01f) {
            guiColor = backgroundFader.color;

            guiColor.a -= .5f * Time.deltaTime;
            //print(guiColor.a);

            backgroundFader.color = guiColor;
            yield return null;
        }

    }


    private void Continue() {
        displayingText = false;
    }
}
