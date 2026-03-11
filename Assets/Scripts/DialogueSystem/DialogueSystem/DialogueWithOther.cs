using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueWithOther : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    [TextArea(3, 10)] public string[] lines;
    public float textSpeed;

    [Header("Sound")]
    [SerializeField] private AudioSource typingAudio;

    private Transform player;

    [SerializeField] private FirstPersonController playerMovement;
    [SerializeField] private GameObject interactElement1;
    [SerializeField] private GameObject interactElement2;

    private int index;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private void Start()
    {
        textComponent.text = string.Empty;
        gameObject.SetActive(false);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
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

                if (typingAudio != null)
                    typingAudio.Stop();
            }
            else
            {
                NextLine();
            }
        }
    }

    public void StartDialogue()
    {
        if (player == null) return;

        // ==== Блокируем игрока и интеракты ====
        playerMovement.enabled = false;
        interactElement1.SetActive(false);
        interactElement2.SetActive(false);
        // =====================================

        index = 0;
        isDialogueActive = true;
        gameObject.SetActive(true);

        textComponent.text = "";

        typingCoroutine = StartCoroutine(TypeLine());

        LockPlayerControls();
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = "";

        if (typingAudio != null)
            typingAudio.Play();

        foreach (char c in lines[index])
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;

        if (typingAudio != null)
            typingAudio.Stop();
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
        // ==== Возвращаем игрока и интеракты ====
        playerMovement.enabled = true;
        interactElement1.SetActive(true);
        interactElement2.SetActive(true);
        // =====================================

        isDialogueActive = false;
        gameObject.SetActive(false);
        textComponent.text = "";

        UnlockPlayerControls();

        if (typingAudio != null)
            typingAudio.Stop();
    }

    private void LockPlayerControls()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void UnlockPlayerControls()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
