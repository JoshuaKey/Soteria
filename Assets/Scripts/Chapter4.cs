using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chapter4 : MonoBehaviour {

    [SerializeField]
    private RawImage background;
    [SerializeField]
    private Texture originalBackgroundPicture;
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
    public SpriteRenderer enemy3;

    public delegate void End();
    public event End end;

    private bool displayingText = false;

    public Vector3 sceneEscape;

    public IEnumerator Begin() {
        dialogueUI.endOfDialogue += Continue;

        background.texture = originalBackgroundPicture;

        playerTrans.position = playerPos;
        allyTrans.position = allyPos;
        burglar1Trans.position = burglar1Pos;
        burglar2Trans.position = burglar2Pos;

        if (!story.justKilled) {
            yield return StartCoroutine(Movement(playerTrans, 7.0f, sceneEscape));
        } else {
            dialogueUI.QueueDialogue(new Dialogue(playerPic, "*huff, huff*"));

            yield return StartCoroutine(Movement(playerTrans, 3.0f, sceneEscape));

            dialogueUI.Next();
        }

        yield return StartCoroutine(Fader());

        dialogueUI.QueueDialogue(new Dialogue(playerPic, "Remi? Where are you?"));

        displayingText = true;
        while (displayingText) {
            yield return null;
        }

        end();
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

            backgroundFader.color = guiColor;
            yield return null;
        }

        background.texture = null;
        background.color = Color.black;

        allyTrans.position = new Vector3(100, 100, 100);
        playerTrans.position = new Vector3(100, 100, 100);

        burglar1Trans.position = new Vector3(100, 100, 100);
        burglar2Trans.position = new Vector3(100, 100, 100);

        while (guiColor.a > .01f) {
            guiColor = backgroundFader.color;

            guiColor.a -= .5f * Time.deltaTime;

            backgroundFader.color = guiColor;
            yield return null;
        }

    }


    private void Continue() {
        displayingText = false;
    }
}
