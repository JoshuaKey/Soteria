using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterEntity : Entity {

    [SerializeField]
    private BattleUI ui;
    [SerializeField]
    private BattleSystem battle;

    private void Start() {
        battle.killed += Promote;
    }

    private void Promote() {
        attack += 5;
        maxHealth += 20;
    }

    // Tell entity that we are waiting for Action
    public override void WaitForAction() {
        ui.enabled = true;
        ui.inputDetected += RecieveInput;
    }

    private void RecieveInput(Actions action) {
        
        SendAction(action);
        ui.inputDetected -= RecieveInput;


    }

}
