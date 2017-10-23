using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyEntity : Entity {

    public BattleSystem battleSystem;
    public StorySystem story;

    private void Start() {
        battleSystem.ran += Promote;
    }

    private void Promote() {
        attack += 9;
        maxHealth += 10;
    }

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
            default:
                SendAction(null);
                break;
        }
    }

    private void Battle1Action() {
        OtherAction action = new OtherAction();
        action.SetActionText("is too scared!");
        SendAction(action);
    }

    private void Battle2Action() {
        if(Random.Range(0, 2) == 0) {
            OtherAction action = new OtherAction();
            action.SetActionText("is too scared!");
            SendAction(action);
        } else {
            AttackAction action = new AttackAction();
            action.attacker = battleSystem.Ally;
            action.target = battleSystem.enemyEntities[Random.Range(0, battleSystem.enemyEntities.Length)];
            SendAction(action);
        }
    }

}
