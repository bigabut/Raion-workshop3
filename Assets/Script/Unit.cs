using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Game/Unit")]
public class Unit : ScriptableObject
{
    public string unitName;
    public int unitLevel;
    public int damage;
    public int maxHealth;
    public int currentHealth;

    public bool isGuarding = false;

    public void takeDamage(int damage)
    {
        if (isGuarding)
        {
            damage = Mathf.RoundToInt(damage * 0.5f);
            isGuarding = false;
        }

        currentHealth -= damage;
    }


}