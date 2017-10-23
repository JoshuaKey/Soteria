using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chapter3 : MonoBehaviour {

    [SerializeField]
    private RawImage background;
    [SerializeField]
    private Texture originalBackgroundPicture;
    [SerializeField]
    private Texture newBackgroundPicture;
    [SerializeField]
    private Texture campBackgroundPicture;
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
    public SpriteRenderer enemy2;
    [SerializeField]
    public Transform burglar2Trans;
    public Vector3 burglar2Pos;

    [SerializeField]
    private Sprite burglarPic;
    [SerializeField]
    private Sprite homelessPic;

    [SerializeField]
    public SpriteRenderer enemy3;

    [SerializeField]
    public Sprite burglarBody;
    [SerializeField]
    public Sprite homelessBody;

    private bool displayingText = false;

    public delegate void Battle();
    public event Battle battle;

    public Vector3 sceneEscape;
    public Vector3 sceneMidPoint;
    public Vector3 sceneEnter;

    public Vector3 campSceneEnter;
    public Vector3 campCenter;


    // Use this for initialization
    void Start() {

        dialogueUI.endOfDialogue += Continue;
    }

    public IEnumerator Begin() {
        enemy1.enabled = true;
        enemy2.enabled = true;
        enemy3.enabled = false;

        background.texture = originalBackgroundPicture;

        playerTrans.position = playerPos;
        allyTrans.position = allyPos;
        burglar1Trans.position = burglar1Pos;
        burglar2Trans.position = burglar2Pos;

        if (story.justKilled) {
            dialogueUI.QueueDialogue(new Dialogue(allyPic, "!!! There's blood!"));
            dialogueUI.QueueDialogue(new Dialogue(playerPic, "*sigh*, Come on."));

            displayingText = true;
            while (displayingText) {
                yield return null;
            }

            StartCoroutine(Movement(playerTrans, 5.0f, sceneEscape + new Vector3(.5f, 1.0f, .0f)));
            StartCoroutine(Movement(allyTrans, 5.0f, sceneEscape));

        } else {
            StartCoroutine(Movement(playerTrans, 15.0f, sceneEscape + new Vector3(.5f, 1.0f, .0f)) );
            StartCoroutine(Movement(allyTrans, 15.0f, sceneEscape));

            dialogueUI.QueueDialogue(new Dialogue(burglarPic, "Where ya goin? The party just started!"));
        }

        yield return StartCoroutine(Fader());
        
        StartCoroutine(Movement(playerTrans, 1.0f, sceneMidPoint));
        StartCoroutine(Movement(allyTrans, 1.0f, sceneMidPoint));

        if (story.kill >= 2) {
            dialogueUI.QueueDialogue(new Dialogue(allyPic, "Why did you hurt all those people?"));
            dialogueUI.QueueDialogue(new Dialogue(playerPic, "Because they were going to hurt you."));
            dialogueUI.QueueDialogue(new Dialogue(allyPic, "..."));

            displayingText = true;
            while (displayingText) {
                yield return null;
            }
        } else if(story.run >= 3) {
            dialogueUI.QueueDialogue(new Dialogue(allyPic, "Why are all those men chasing after me?"));
            dialogueUI.QueueDialogue(new Dialogue(playerPic, "I don't know, did you do something?"));
            dialogueUI.QueueDialogue(new Dialogue(allyPic, "No."));
            dialogueUI.QueueDialogue(new Dialogue(playerPic, "Are you sure?"));
            dialogueUI.QueueDialogue(new Dialogue(allyPic, "I didn't do anything!"));
            dialogueUI.QueueDialogue(new Dialogue(playerPic, "Ok..."));

            displayingText = true;
            while (displayingText) {
                yield return null;
            }
        } else {
            dialogueUI.QueueDialogue(new Dialogue(allyPic, "I'm sorry."));
            dialogueUI.QueueDialogue(new Dialogue(playerPic, "Don't worry about it."));

            displayingText = true;
            while (displayingText) {
                yield return null;
            }

            yield return new WaitForSeconds(1.5f);

            dialogueUI.QueueDialogue(new Dialogue(playerPic, "I'm going to protect you... Ok?"));

            displayingText = true;
            while (displayingText) {
                yield return null;
            }
        }

        StartCoroutine(Movement(playerTrans, 3.0f, sceneEscape));
        yield return StartCoroutine(Movement(allyTrans, 4.0f, sceneEscape));

        yield return StartCoroutine(FaderToCamp());

        yield return StartCoroutine(Movement(playerTrans, 5.0f, campCenter));

        dialogueUI.QueueDialogue(new Dialogue(playerPic, "This looks like a good place to stay for now."));
        displayingText = true;
        while (displayingText) {
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        yield return StartCoroutine(Movement(playerTrans, 3.0f, playerTrans.position + new Vector3(-5.0f, 2.0f)));
        yield return StartCoroutine(Movement(playerTrans, 5.0f, playerTrans.position + new Vector3(6.0f, 3.0f)));

        dialogueUI.QueueDialogue(new Dialogue(playerPic, "Hmm, an old knife."));

        displayingText = true;
        while (displayingText) {
            yield return null;
        }

        yield return StartCoroutine(Movement(playerTrans, 3.0f, playerTrans.position + new Vector3(1.0f, -5.0f)));

        StartCoroutine(Movement(playerTrans, 3.0f, campCenter));

        dialogueUI.QueueDialogue(new Dialogue(playerPic, "So you don't remember where you live?"));
        dialogueUI.QueueDialogue(new Dialogue(allyPic, "*Shakes head*"));

        if(story.kill >= 2) {
            dialogueUI.QueueDialogue(new Dialogue(playerPic, "Nothing? Not even your last name, address, phone number?"));
            dialogueUI.QueueDialogue(new Dialogue(allyPic, "*Shakes head*"));
        }

        dialogueUI.QueueDialogue(new Dialogue(playerPic, "Why were you in that alley?"));
        dialogueUI.QueueDialogue(new Dialogue(allyPic, "I was just walking and that guy grabbed me"));

        displayingText = true;
        while (displayingText) {
            yield return null;
        }

        yield return StartCoroutine(Movement(playerTrans, 3.0f, allyTrans.position));
        if(story.kill < 3 && story.run < 3) {
            dialogueUI.QueueDialogue(new Dialogue(playerPic, "*Ruffles her hair* It'll be fine."));

            displayingText = true;
            while (displayingText) {
                yield return null;
            }
        }

        yield return new WaitForSeconds(.5f);

        dialogueUI.QueueDialogue(new Dialogue(playerPic, "Go to sleep"));

        displayingText = true;
        while (displayingText) {
            yield return null;
        }

        StartCoroutine(Movement(playerTrans, 3.0f, campCenter));

        yield return StartCoroutine(FaderToBattle());

        dialogueUI.QueueDialogue(new Dialogue(null, "Do you think we should kill him?"));

        dialogueUI.QueueDialogue(new Dialogue(homelessPic, "..."));
        displayingText = true;
        while (displayingText) {
            yield return null;
        }

        battle();
        dialogueUI.endOfDialogue -= Continue;
    }

    public IEnumerator Movement(Transform obj, float speed, Vector3 dest) {

        // Optimize
        Vector3 mvmt = Vector3.Normalize(dest - obj.position) * speed; // Direction Vector with Speed
        while (Vector3.Distance(obj.position, dest) > .2f) {

            Vector3 move = mvmt * Time.deltaTime; // How much we'll move.
            Vector3 dist = dest - obj.position; // Vector pointing from Obj to Dest

            // WE get the lenght squared of both, aka how far we'll move
            // If our distance moved is greater than our actual distance
            // Then break and set our position
            if(Vector3.Dot(move, move) > Vector3.Dot(dist, dist)) {
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

            backgroundFader.color = guiColor;
            yield return null;
        }

        dialogueUI.Next();
        StopCoroutine(Movement(playerTrans, 0, new Vector3(0, 0, 0)));
        StopCoroutine(Movement(allyTrans, 0, new Vector3(0, 0, 0)));
        background.texture = newBackgroundPicture;
        
        playerTrans.position = sceneEnter + new Vector3(.5f, 1.0f, .0f);
        allyTrans.position = sceneEnter;

        burglar1Trans.position = new Vector3(100, 100, 100);
        burglar2Trans.position = new Vector3(100, 100, 100);

        while (guiColor.a > .01f) {
            guiColor = backgroundFader.color;

            guiColor.a -= .5f * Time.deltaTime;
            //print(guiColor.a);

            backgroundFader.color = guiColor;
            yield return null;
        }

    }

    public IEnumerator FaderToCamp() {
        Color guiColor = backgroundFader.color;

        while (guiColor.a < .99f) {
            guiColor = backgroundFader.color;

            guiColor.a += .5f * Time.deltaTime;
            //print(guiColor.a);

            backgroundFader.color = guiColor;
            yield return null;
        }

        dialogueUI.Next();
        StopCoroutine(Movement(null, 0, new Vector3(0, 0, 0)));
        background.texture = campBackgroundPicture;

        playerTrans.position = campSceneEnter + new Vector3(.5f, 1.0f, .0f);
        allyTrans.position = campSceneEnter;

        while (guiColor.a > .01f) {
            guiColor = backgroundFader.color;

            guiColor.a -= .5f * Time.deltaTime;
            //print(guiColor.a);

            backgroundFader.color = guiColor;
            yield return null;
        }

    }

    public IEnumerator FaderToBattle() {
        Color guiColor = backgroundFader.color;

        while (guiColor.a < .99f) {
            guiColor = backgroundFader.color;

            guiColor.a += .5f * Time.deltaTime;
            //print(guiColor.a);

            backgroundFader.color = guiColor;
            yield return null;
        }

        dialogueUI.Next();
        StopCoroutine(Movement(null, 0, new Vector3(0, 0, 0)));
        allyTrans.position = new Vector3(100, 100, 100);

        burglar1Trans.position = playerTrans.position + new Vector3(3, 0, 0);
        burglar2Trans.position = playerTrans.position - new Vector3(3, 0, 0);

        enemy1.sprite = homelessBody;
        enemy2.sprite = homelessBody;

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
