using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleHandlerUi : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject retry;
    public GameObject PlayAgain;
    public GameObject monster;


    public void ShowWin()
    {
        Debug.Log("SHOW WIN UI");

        monster.SetActive(false);

       new WaitForSeconds(0.5f);

        winScreen.SetActive(true);
        new WaitForSeconds(2f);
        PlayAgain.SetActive(true);
    }

    public void ShowLose()
    {
        Debug.Log("SHOW LOSE UI");
        loseScreen.SetActive(true);
        new WaitForSeconds(2f);
        retry.SetActive(true);
        
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }


}
