using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Actions {

    public Entity attacker;
    public Entity target;

    public AttackAction() {
        actionName = "attacked!";
    }

    public override void PerformAction() {
        target.TakeDamage(attacker.Attack);
    }
}
