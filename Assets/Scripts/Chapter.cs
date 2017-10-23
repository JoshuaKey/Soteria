using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chapter : MonoBehaviour {

    [SerializeField]
    private RawImage background;
    [SerializeField]
    private Texture backgroundPicture;

    [SerializeField]
    private DialogueUI dialogueUI;

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
    public Transform burglarTrans;
    public Vector3 burglarPos;
    [SerializeField]
    private Sprite burglarPic;

    public Vector3 sidewalkMovement;
    public Vector3 helpMovement;
    public Vector3 engageMovement;

    private bool displayingText = false;

    public delegate void Battle();
    public event Battle battle;

    // Use this for initialization
    void Start() {
        background.texture = backgroundPicture;
        // Load other Entities and assets.

        // Diable Text by default
        //dialogueUI.enabled = false;
        dialogueUI.endOfDialogue += Continue;


    }

    public IEnumerator Begin() {
        // yield return new WaitForSeconds(1.5f); //??????????????????????????????????????????????????????????????????/
        //// Why does this break it???????????????????????????????????????????????????????????
        //print(mainCharacterTrans);

        StartCoroutine(Movement(playerTrans, 2.0f, sidewalkMovement));
        dialogueUI.QueueDialogue(new Dialogue(playerPic, "..."));
        dialogueUI.QueueDialogue(new Dialogue(allyPic, "HEEEEEEEEEELP!"));
        dialogueUI.QueueDialogue(new Dialogue(playerPic, "Hmm?"));

        displayingText = true;
        while (displayingText) {
            yield return null;
        }

        yield return StartCoroutine(Movement(playerTrans, 15.0f, helpMovement));

        dialogueUI.QueueDialogue(new Dialogue(null, "*Smack*"));
        dialogueUI.QueueDialogue(new Dialogue(allyPic, "..."));
        dialogueUI.QueueDialogue(new Dialogue(burglarPic, "I told you to be quiet"));
        dialogueUI.QueueDialogue(new Dialogue(playerPic, "Hey!"));
        dialogueUI.QueueDialogue(new Dialogue(burglarPic, "Huh!"));
        dialogueUI.QueueDialogue(new Dialogue(playerPic, "Let her go!"));

        displayingText = true;
        while (displayingText) {
            yield return null;
        }

        yield return StartCoroutine(Movement(playerTrans, 15.0f, engageMovement));
        
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

    private void Continue() {
        displayingText = false;
    }

    // Create a custom animation
    // Add an Animation Event
    // When StorySystem  is ready (Animation / Event/ etc.) Call Chapter Begin()
    // Starts an animation
    // When animation hits a certain point, make event
    // Keep going for additional animation
    // When using Text... Make a queue? Add everything and with input skip/ speed up?
    // Then can have a text finished delegate for doing the next animation...
    // USe animation events / delegates for Battles, should be used on Story System, not Chapter

}
