using UnityEngine;
using System.Collections;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    [TextArea(3, 10)] public string[] lines;
    public float textSpeed;

    [Header("Sound")]
    [SerializeField] private AudioSource typingAudio;

    [Header("Animation")]
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private float maxTalkingDistance = 5f;
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

        // ==== Костыли игрока/интерактов ====
        characterAnimator.SetBool("Talking", true);
        playerMovement.enabled = false;
        interactElement1.SetActive(false);
        interactElement2.SetActive(false);
        // =================================

        // ==== Поворот персонажа к игроку (только модель) ====
        Transform model = characterAnimator.transform; // или отдельная ссылка на модель
        Vector3 lookPos = player.position;
        lookPos.y = model.position.y; // горизонтальный уровень
        model.LookAt(lookPos);
        // ====================================================

        index = 0;
        isDialogueActive = true;
        gameObject.SetActive(true);

        // ==== Сбрасываем текст на TextMeshPro ====
        textComponent.text = "";

        // ==== Анимация и звук ====
        TrySetTalking(true);
        typingCoroutine = StartCoroutine(TypeLine());
        // =======================

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
        // ==== Включаем обратно игрока и интеракты ====
        characterAnimator.SetBool("Talking", false);
        playerMovement.enabled = true;
        interactElement1.SetActive(true);
        interactElement2.SetActive(true);
        // ==========================================

        isDialogueActive = false;
        gameObject.SetActive(false);
        textComponent.text = "";
        UnlockPlayerControls();
        TrySetTalking(false);

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

    private void TrySetTalking(bool state)
    {
        if (characterAnimator == null || player == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= maxTalkingDistance)
        {
            characterAnimator.SetBool("Talking", state);
        }
    }
}
