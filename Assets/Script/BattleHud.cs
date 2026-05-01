using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleHud : MonoBehaviour
{
    public TMP_Text levelText;
    public Slider hpSlider;

    [Header("Damage Text")]
    public TMP_Text damageText;

    public void setHud(Unit unit)
    {
        levelText.text = unit.unitName + " Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHealth;
        hpSlider.value = unit.currentHealth;
    }

    public void setHp(int hp)
    {
        hpSlider.value = hp;
    }

    // 💥 SHOW DAMAGE
    public void ShowDamage(int damage)
    {
        StopAllCoroutines();
        StartCoroutine(ShowDamageRoutine(damage));
    }

    IEnumerator ShowDamageRoutine(int damage)
    {
        damageText.text = damage.ToString();
        damageText.gameObject.SetActive(true);

        // efek naik dikit (optional)
        Vector3 startPos = damageText.transform.localPosition;

        float t = 0f;
        while (t < 0.5f)
        {
            damageText.transform.localPosition = startPos + Vector3.up * (t * 30f);
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        damageText.gameObject.SetActive(false);
        damageText.transform.localPosition = startPos;
    }
}