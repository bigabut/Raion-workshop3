using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class Dialogue : MonoBehaviour, EffectScripts
{
    public DialogueSystem dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text DialogueText;
    public TMP_Text nameText;
    public Image portraitImage;

    public int priority = 0;
    public int Priority => priority;

    private int dialogueIndex;
    private bool isTyping;
    private bool isDialogueActive;


    public IEnumerator playEffects()
    {
        

        if (isDialogueActive)
        {
            Debug.Log("NEXT LINE");
            nextLine();
        }
        else if (dialoguePanel != null && !isDialogueActive)
        {
            Debug.Log("START DIALOGUE");
            startDialogue();
        }

        while (isDialogueActive)
            yield return null;
    }

    void startDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.name);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);

        //Type line

        StartCoroutine(typeline());

    }
    void nextLine()
      {
            if (isTyping)
            {
                StopAllCoroutines();
                DialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
                isTyping = false;
            }

            else if(dialogueIndex+1 < dialogueData.dialogueLines.Length)
            {
                dialogueIndex++;
                StartCoroutine(typeline());
            }

             else 
             {
                 endDialogue();
             }
        }

    void endDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        DialogueText.SetText("");
        dialoguePanel.SetActive(false);
        //pause
    }


    IEnumerator typeline()
    {
        isTyping = true;
        DialogueText.SetText("");
        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if(dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            
            // next line
            nextLine();
        }
    }

    
}
