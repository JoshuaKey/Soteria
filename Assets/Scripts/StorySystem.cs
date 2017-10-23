using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorySystem : MonoBehaviour {

    public enum GameState { STORY, BATTLE };

    [SerializeField]
    private DialogueUI ui;
    [SerializeField]
    private DebugController debugController;
    [SerializeField]
    private BattleSystem battle;
    [SerializeField]
    private BattleUI battleUI;

    [SerializeField]
    private AudioSource backgroundMusic;

    [SerializeField]
    private MainCharacterEntity player;
    [SerializeField]
    private AllyEntity ally;

    private GameState currState;
    public GameState State { get { return currState; } }

    [SerializeField]
    private Chapter chapter1;
    [SerializeField]
    private Chapter2 chapter2;
    [SerializeField]
    private Chapter3 chapter3;
    [SerializeField]
    private Chapter4 chapter4;

    [SerializeField]
    private Entity[] battle1Enemies;
    [SerializeField]
    private Entity[] battle2Enemies;
    [SerializeField]
    private Entity[] battle3Enemies;

    public int currBattle = 0;
    public int run = 0;
    public int kill = 0;
    public bool justKilled = false;

    private void Start() {
        currState = GameState.STORY;

        //ui.inputDetected += Progress;
        // Register all delegates to things...

        battle.killed += Kill;
        battle.ran += Run;

        BeginChapter1();
    }

    private void BeginChapter1() {
        StartCoroutine(backgroundMusic.FadeIn());

        chapter1.battle += Battle1;
        StartCoroutine(chapter1.Begin());
    }

    private void Battle1() {
        chapter1.battle -= Battle1;
        StartCoroutine(backgroundMusic.FadeOut());

        battle.battleHasEnded += BeginChapter2;

        currBattle = 1;
        battle.SetEnemies(battle1Enemies);
        battle.BattleStart();
    }

    private void BeginChapter2() {
        battle.battleHasEnded -= BeginChapter2;
        StartCoroutine(backgroundMusic.FadeIn());

        player.IncreaseHealth(3);
        ally.IncreaseHealth(3);
        player.IncreaseDamage(1);
        ally.IncreaseDamage(1);

        chapter2.battle += Battle2;
        StartCoroutine(chapter2.Begin());
    }

    private void Battle2() {
        chapter2.battle -= Battle2;
        StartCoroutine(backgroundMusic.FadeOut());

        battle.battleHasEnded += BeginChapter3;
        justKilled = false;
        currBattle = 2;

        battle2Enemies[0].maxHealth = 30;
        battle2Enemies[0].attack = 5;
        battle2Enemies[0].name = "Burglar 1";

        battle2Enemies[1].maxHealth = 30;
        battle2Enemies[1].attack = 5;
        battle2Enemies[1].name = "Burglar 2";

        battle.SetEnemies(battle2Enemies);
        battle.BattleStart();
    }

    private void BeginChapter3() {
        battle.battleHasEnded -= BeginChapter3;
        StartCoroutine(backgroundMusic.FadeIn());

        player.IncreaseHealth(3);
        ally.IncreaseHealth(3);
        player.IncreaseDamage(1);
        ally.IncreaseDamage(1);


        chapter3.battle += Battle3;
        StartCoroutine(chapter3.Begin());
    }

    private void Battle3() {
        chapter3.battle -= Battle3;
        StartCoroutine(backgroundMusic.FadeOut());

        player.IncreaseDamage(7); // Dagger
        ally.enabled = false;

        battle.battleHasEnded += BeginChapter4;
        justKilled = false;
        currBattle = 3;

        battle3Enemies[0].maxHealth = 30;
        battle3Enemies[0].attack = 5;
        battle3Enemies[0].name = "Homeless 1";

        battle3Enemies[1].maxHealth = 30;
        battle3Enemies[1].attack = 5;
        battle3Enemies[1].name = "Homeless 2";

        battle.SetEnemies(battle3Enemies);
        battle.BattleStart();
    }

    private void BeginChapter4() {
        battle.battleHasEnded -= BeginChapter4;
        StartCoroutine(backgroundMusic.FadeIn());

        player.IncreaseHealth(3);
        ally.IncreaseHealth(3);
        player.IncreaseDamage(1);
        ally.IncreaseDamage(1);

        chapter4.end += End;
        StartCoroutine(chapter4.Begin());
    }

    private void End() {
        StartCoroutine(backgroundMusic.FadeOut());

        // Set Text;
        battleUI.failText.text = "To be Continued?";
        battleUI.Fail();
    }


    private void Kill() { kill++; justKilled = true; }
    private void Run() { run++; }


}

// Can use delegates in area
// Story subscribes and when an action happens
// Call delegate
