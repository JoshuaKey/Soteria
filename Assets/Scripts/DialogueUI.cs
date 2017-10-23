using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour {

    [SerializeField]
    private Image charImage;
    [SerializeField]
    private Text charText;
    [SerializeField]
    private Image border;

    public delegate void EndOfDialogue();
    public event EndOfDialogue endOfDialogue;

    private Queue<Dialogue> dialogue = new Queue<Dialogue>();

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Return)) { Next(); }
    }

    public void Next() {
        if(dialogue.Count > 0) {
            Dialogue d = dialogue.Dequeue();
            DisplayCharacterText(d);
        } else {
            this.enabled = false;
            endOfDialogue();
        }
        
    }

    public void QueueDialogue(Dialogue d) {
        dialogue.Enqueue(d);
        if(!this.enabled) {
            this.enabled = true;
            Next();
        }
    }

    private void DisplayCharacterText(Dialogue d) {
        charImage.sprite = d.sprite;
        charText.text = d.text;
    }

    private void OnEnable() {
        charImage.enabled = true;
        charText.enabled = true;
        border.enabled = true;
    }

    private void OnDisable() {
        charImage.enabled = false;
        charText.enabled = false;
        border.enabled = false;
    }
}
