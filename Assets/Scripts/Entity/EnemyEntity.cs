using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : Entity {

    public BattleSystem battleSystem;
    public StorySystem story;

    public override void WaitForAction() {
        StartCoroutine(DetermineAction());
    }

    private IEnumerator DetermineAction() {
        // Animation
        yield return new WaitForSeconds(2.0f);

        switch (story.currBattle) {
            case 1:
                Battle1Action();
                break;
            case 2:
                Battle2Action();
                break;
            case 3:
                Battle3Action();
                break;
            default:
                SendAction(null);
                break;
        }
   
    }

    private void Battle1Action() {
        AttackAction action = new AttackAction();
        action.attacker = this;
        action.target = battleSystem.MainChar;
        SendAction(action);
    }

    private void Battle2Action() {
        AttackAction action = new AttackAction();
        action.attacker = this;
        action.target = (Random.Range(0, 2) == 0 ? battleSystem.MainChar : battleSystem.Ally);
        SendAction(action);
        
    }

    private void Battle3Action() {
        if(Random.Range(0, 10) < 3) {
            OtherAction action = new OtherAction();
            action.SetActionText("missed");
            SendAction(action);
        } else {
            AttackAction action = new AttackAction();
            action.attacker = this;
            action.target = battleSystem.MainChar;
            SendAction(action);
        }
    }
}
