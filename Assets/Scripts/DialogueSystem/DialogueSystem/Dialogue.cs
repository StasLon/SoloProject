using UnityEngine;
using System.Collections;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    [TextArea(3, 10)] public string[] lines;
    public float textSpeed;

    private int index;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private void Start()
    {
        textComponent.text = string.Empty;
        gameObject.SetActive(false); // чтобы панель не была видна с самого начала
    }

    private void Update()
    {
        if (!isDialogueActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping && typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                textComponent.text = lines[index];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void StartDialogue()
    {
        index = 0;
        isDialogueActive = true;
        gameObject.SetActive(true);
        typingCoroutine = StartCoroutine(TypeLine());
        LockPlayerControls();
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = "";
        Debug.Log("Начали печатать строку: " + lines[index]);
        foreach (char c in lines[index])
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        Debug.Log("Закончили печатать строку: " + lines[index]);
        isTyping = false;
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = "";
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        gameObject.SetActive(false);
        textComponent.text = "";
        UnlockPlayerControls();
    }

    private void LockPlayerControls()
    {
        // Пример: отключить движение
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }

    private void UnlockPlayerControls()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
