using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAction : Actions {

    public BattleSystem battle;

    public RunAction() {
        actionName = "tried to run!";
    }

    public void SetBattleSystem(BattleSystem sys) {
        battle = sys;
    }

    public override void PerformAction() {
        battle.Run();
    }

}
