using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour {
    [SerializeField]
    private BattleUI ui;
    [SerializeField]
    private AudioSource battleMusic;

    [SerializeField]
    private Transform pos1;
    [SerializeField]
    private Transform pos2;
    [SerializeField]
    private Transform pos3;
    [SerializeField]
    private Transform pos4;
    [SerializeField]
    private Transform pos5;

    public Entity MainChar;
    public Entity Ally;

    public Entity[] enemyEntities;

    private Entity currEntityTurn;
    private bool hasRan = false;

    public delegate void BattleHasEnded();
    public event BattleHasEnded battleHasEnded;

    public delegate void Ran();
    public event Ran ran;

    public delegate void Killed();
    public event Killed killed;

    [HideInInspector]
    public Vector3 mainCharPrevPos;
    [HideInInspector]
    public Quaternion mainCharPrevRot;

    [HideInInspector]
    public Vector3 allyPrevPos;
    [HideInInspector]
    public Quaternion allyPrevRot;

    [HideInInspector]
    public Vector3[] enemyPrevPos;
    [HideInInspector]
    public Quaternion[] enemyPrevRot;

    public void SetEnemies(Entity[] enemies) {
        enemyEntities = enemies;

        enemyPrevPos = new Vector3[enemies.Length];
        enemyPrevRot = new Quaternion[enemies.Length];

        for(int i = 0; i < enemies.Length; i++) {
            enemyPrevPos[i] = enemies[i].ImageTrans.position;
            enemyPrevRot[i] = enemies[i].ImageTrans.rotation;
        }
    }

    public Entity GetEntity(int index) {
        return enemyEntities[index];
    }

    public void BattleStart() {
        currEntityTurn = MainChar;

        mainCharPrevPos = MainChar.ImageTrans.position;
        mainCharPrevRot = MainChar.ImageTrans.rotation;

        MainChar.ImageTrans.position = pos1.position;
        MainChar.ImageTrans.rotation = Quaternion.Euler(0, 0, 0);
        if (Ally.enabled) {
            allyPrevPos = Ally.ImageTrans.position;
            allyPrevRot = Ally.ImageTrans.rotation;

            Ally.ImageTrans.position = pos2.position;
            Ally.ImageTrans.rotation = Quaternion.Euler(0, 0, 0);
        }
        

        enemyEntities[0].ImageTrans.position = pos3.position;
        enemyEntities[0].ImageTrans.rotation = Quaternion.Euler(0, 180, 0);
        if (enemyEntities.Length > 1) {
            enemyEntities[1].ImageTrans.position = pos4.position;
            enemyEntities[1].ImageTrans.rotation = Quaternion.Euler(0, 180, 0);
        }
        if(enemyEntities.Length > 2) {
            enemyEntities[2].ImageTrans.position = pos5.position;
            enemyEntities[2].ImageTrans.rotation = Quaternion.Euler(0, 180, 0);
        }

        MainChar.Recover();
        Ally.Recover();
        for(int i = 0; i < enemyEntities.Length; i++) {
            enemyEntities[i].Recover();
        }

        //battleMusic.FadeIn();
        StartCoroutine(battleMusic.FadeIn());

        BeginTurn();
    }

    public IEnumerator BattleEnd(string endText = "") {

        StartCoroutine(battleMusic.FadeOut());

        yield return StartCoroutine(ui.SetActionText(endText));

        MainChar.ImageTrans.position = mainCharPrevPos;
        MainChar.ImageTrans.rotation = mainCharPrevRot;

        if (Ally.enabled) {
            Ally.ImageTrans.position = allyPrevPos;
            Ally.ImageTrans.rotation = allyPrevRot;
        }
        

        for(int i = 0; i < enemyEntities.Length; i++) {
            enemyEntities[i].ImageTrans.position = enemyPrevPos[i];
            enemyEntities[i].ImageTrans.rotation = enemyPrevRot[i];
        }

        hasRan = false;
        enemyEntities = null;

        battleHasEnded();
    }

    private void BeginTurn() {
        // Animate current Player
        Transform t = currEntityTurn.transform;
        t.Translate(new Vector3(3.0f, .0f, .0f));

        // Subscribe and wait
        currEntityTurn.getAction += GetAction;
        currEntityTurn.WaitForAction();
    }

    public void GetAction(Actions action) {
        action.PerformAction();
        StartCoroutine(ui.SetActionText(currEntityTurn.Name + " " + action.ActionName));
        print(currEntityTurn.Name + " " + action.ActionName);

        EndTurn();
    }

    public void Run() {
        int length = enemyEntities.Length; // Multiplier
        // Exponential
        int healthAvg = 0;
        int healthTotal = 0;

        for(int i = 0; i < length; i++) {
            healthAvg += enemyEntities[i].Health;
            healthTotal += enemyEntities[i].MaxHealth;
        }
        healthTotal += MainChar.Health;
        if (Ally.enabled) {
            healthTotal += Ally.Health;
        }

        float percent = Mathf.Pow((healthTotal - healthAvg) / (float)healthTotal, length);
        float result = Random.Range(0, 1.0f);
        hasRan = result <= percent;
        print("Computed Run value: " + percent + ", " + result);

        // Check if run was successful
    }

    // Entity
    // Name
    // Health
    // Attack
    // GetAttack();
    // Sprite

    // Action
    // Type / NAme, action
    // Effect

    public void EndTurn() {
        // Animate player back...
        Transform t = currEntityTurn.transform;
        t.Translate(new Vector3(-3.0f, .0f, .0f));

        currEntityTurn.getAction -= GetAction;

        if (MainChar.Health <= 0) {
            // Lose
            StartCoroutine(battleMusic.FadeOut());
            ui.failText.text = "You lose...";
            ui.Fail();
            print("MC has died");
            return;
        }
        if(Ally.enabled && Ally.Health <= 0) {
            // Lose
            // Load last save
            StartCoroutine(battleMusic.FadeOut());
            ui.failText.text = "You lose...";
            ui.Fail();
            print("Ally has died");
            return;
        }

        if(hasRan) {
            // Update Child character  

            for(int i = 0; i < enemyEntities.Length; i++) {
                ran();
            }
            StartCoroutine(BattleEnd(currEntityTurn.Name + " ran away."));
            //StartCoroutine(ui.SetActionText(currEntityTurn.Name + " ran away."));
            return;
            
        }

        for (int i = 0; i < enemyEntities.Length; i++) {
            if (enemyEntities[i].Health <= 0) {
                Entity[] temp = new Entity[enemyEntities.Length - 1];
                int amo = 0;
                for(int y = 0; y < enemyEntities.Length; y++) {
                    if(y == i) { continue; }
                    temp[amo++] = enemyEntities[y];
                }

                StartCoroutine(ui.SetActionText((currEntityTurn.Name + " killed " + enemyEntities[i].Name)));

                i--;
                killed();
                enemyEntities = temp;

            }
        }
        if(enemyEntities.Length == 0) {
            StartCoroutine(BattleEnd("All enemies are dead!"));
            return;
        }

        //End Turn
        if (currEntityTurn == MainChar) {
            if (Ally.enabled) {
                currEntityTurn = Ally;
                BeginTurn();
            } else {
                currEntityTurn = enemyEntities[0];
                BeginTurn();
            }
            
        } else if (currEntityTurn == Ally) {
            currEntityTurn = enemyEntities[0];
            BeginTurn();
        } else {
            for (int i = 0; i < enemyEntities.Length; i++) {
                if (currEntityTurn == enemyEntities[i]) {
                    if(i == enemyEntities.Length - 1) {
                        currEntityTurn = MainChar;
                        BeginTurn();
                    } else {
                        bool nextEnemyAlive = false;
                        for(int y = i + 1; y < enemyEntities.Length; y++) {
                            if(enemyEntities[y].Health > 0) {
                                currEntityTurn = enemyEntities[y];
                                nextEnemyAlive = true;
                                BeginTurn();
                                break;
                            }
                        }
                        if (!nextEnemyAlive) {
                            currEntityTurn = MainChar;
                            BeginTurn();
                        }
                    }
                    break;
                }
            }
        }
    }
}
