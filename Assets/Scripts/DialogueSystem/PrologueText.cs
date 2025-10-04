using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrologueText : MonoBehaviour
{
    public TMP_Text dialogueText;
    [TextArea] public string fullText1;
    [TextArea] public string fullText2;
    [TextArea] public string fullText3;
    public float delay = 0.05f;


    public void PlayText()
    {
        StopAllCoroutines();
        StartCoroutine(TypeText());
    }


    IEnumerator TypeText()
    {
        dialogueText.text = "";
        foreach (char c in fullText1)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(1.2f);

        dialogueText.text = "";
        foreach (char c in fullText2)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(1.2f);

        dialogueText.text = "";
        foreach (char c in fullText3)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
    
    public void HideText()
    {
        dialogueText.text = "";
    }








}
