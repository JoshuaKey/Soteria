using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    [SerializeField]
    public string name;
    [SerializeField]
    public int attack;
    [SerializeField]
    public int currHealth = 100;
    [SerializeField]
    public int maxHealth = 100;

    public string Name { get { return name; } }
    public int Health { get { return currHealth; } }
    public int MaxHealth { get { return maxHealth; } }
    public int Attack { get { return attack; } }

    [SerializeField]
    protected Transform imageTrans;

    public Transform ImageTrans { get { return imageTrans; } }

    public delegate void GetAction(Actions a);
    public event GetAction getAction;

    // Tell entity that we are waiting for Action
    public virtual void WaitForAction() {
        print("Incorrect");
    }

    protected void SendAction(Actions a) {
        getAction(a);
    }

    public void TakeDamage(int damage) {
        currHealth -= damage;
    }

    public void Recover() {
        currHealth = maxHealth;
    }

    public void IncreaseDamage(int damage) {
        attack += damage;
    }
    
    public void IncreaseHealth(int hp) {
        maxHealth += hp;
    }

}
