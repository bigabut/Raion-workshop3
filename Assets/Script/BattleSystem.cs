using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BattleState { Start, Playerturn, enemiesTurn, won, defeat }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    [Header("Units")]
    public List<Unit> players;
    public Unit enemies;

    [Header("HUD")]
    public List<BattleHud> playerHuds;
    public BattleHud enemiesHud;

    [Header("UI")]
    public GameObject actionPanel;

    [Header("Events")]
    public UnityEvent onBattleWin;
    public UnityEvent onBattleLose;

    private int currentPlayerIndex = 0;
    private bool battleEnded = false;
    public CamerasShakeCM cameraShake;
    

    void Start()
    {
        state = BattleState.Start;

        // reset player HP
        for (int i = 0; i < players.Count; i++)
        {
            players[i].currentHealth = players[i].maxHealth;
            players[i].isGuarding = false;
        }

        enemies.currentHealth = enemies.maxHealth;

        StartCoroutine(setupBattle());
    }

    IEnumerator setupBattle()
    {
        for (int i = 0; i < players.Count; i++)
        {
            playerHuds[i].setHud(players[i]);
        }

        enemiesHud.setHud(enemies);

        yield return new WaitForSeconds(1f);

        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        state = BattleState.Playerturn;

        if (players[currentPlayerIndex].currentHealth <= 0)
        {
            NextPlayer();
            return;
        }

        Debug.Log("Turn: " + players[currentPlayerIndex].unitName);
        actionPanel.SetActive(true);
    }


    public void OnAttackButton()
    {
        if (state != BattleState.Playerturn) return;
        StartCoroutine(PlayerAttack());

    }

    IEnumerator PlayerAttack()
    {
        yield return StartCoroutine(DealDamage(players[currentPlayerIndex].damage));
    }


    public void OnSkillButton()
    {
        if (state != BattleState.Playerturn) return;
        StartCoroutine(PlayerSkill());
    }

    IEnumerator PlayerSkill()
    {
        yield return StartCoroutine(DealDamage(players[currentPlayerIndex].damage * 3));
    }


    public void OnHealButton()
    {
        if (state != BattleState.Playerturn) return;
        StartCoroutine(Heal(players[currentPlayerIndex]));
    }

    IEnumerator Heal(Unit target)
    {
        state = BattleState.enemiesTurn;
        actionPanel.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        int healAmount = 10;

        target.currentHealth += healAmount;
        target.currentHealth = Mathf.Min(target.currentHealth, target.maxHealth);

        int index = players.IndexOf(target);
        playerHuds[index].setHp(target.currentHealth);
        playerHuds[index].damageText.color = Color.green;
        playerHuds[index].ShowDamage(healAmount);

        Debug.Log(target.unitName + " HEAL → " + target.currentHealth);

        yield return new WaitForSeconds(1f);
        playerHuds[index].damageText.color = Color.orange;
        NextPlayer();

    }


    public void OnGuardButton()
    {
        if (state != BattleState.Playerturn) return;
        StartCoroutine(Guard());
    }

    IEnumerator Guard()
    {
        state = BattleState.enemiesTurn;
        actionPanel.SetActive(false);

        Unit currentPlayer = players[currentPlayerIndex];
        currentPlayer.isGuarding = true;

        Debug.Log(currentPlayer.unitName + " is GUARDING 🛡️");

        yield return new WaitForSeconds(0.5f);

        NextPlayer();
    }

  
    IEnumerator DealDamage(int damage)
    {
        state = BattleState.enemiesTurn;
        actionPanel.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        enemies.takeDamage(damage);
        enemiesHud.setHp(enemies.currentHealth);

        enemiesHud.ShowDamage(damage);
        cameraShake.Shake();

        Debug.Log(enemies.unitName + " HP: " + enemies.currentHealth);

        yield return new WaitForSeconds(1f);

        if (enemies.currentHealth <= 0)
        {
            state = BattleState.won;
            EndBattle();
            onBattleWin?.Invoke();
        }
        else
        {
            NextPlayer();
        }
    }

   
    void NextPlayer()
    {
        currentPlayerIndex++;

        if (currentPlayerIndex >= players.Count)
        {
            currentPlayerIndex = 0;
            StartCoroutine(EnemyTurn());
        }
        else
        {
            StartPlayerTurn();
        }
    }

    IEnumerator EnemyTurn()
    {
        state = BattleState.enemiesTurn;

        yield return new WaitForSeconds(1f);

        List<int> alivePlayers = new List<int>();

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].currentHealth > 0)
                alivePlayers.Add(i);
        }

        if (alivePlayers.Count == 0)
        {
            state = BattleState.defeat;
            EndBattle();
            onBattleLose?.Invoke();
            yield break;
        }

        int targetIndex = alivePlayers[Random.Range(0, alivePlayers.Count)];

        players[targetIndex].takeDamage(enemies.damage);
        playerHuds[targetIndex].setHp(players[targetIndex].currentHealth);
        playerHuds[targetIndex].ShowDamage(enemies.damage);
        float amp = Mathf.Clamp(enemies.damage * 0.15f, 0.5f, 3f);
        cameraShake.Shake();

        Debug.Log(players[targetIndex].unitName + " HP: " + players[targetIndex].currentHealth);

        yield return new WaitForSeconds(1f);

        StartPlayerTurn();
    }

   
    void EndBattle()
    {
        if (battleEnded) return;
        battleEnded = true;

        actionPanel.SetActive(false);

        if (state == BattleState.won)
            Debug.Log("MENANG REK AOWKOAWKOAW");
        else if (state == BattleState.defeat)
            Debug.Log("KALAH WOK AAWOKWOKOKAW 💀");
    }
}