using UnityEngine;
using System.Collections;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    [Header("Quest")]
    [SerializeField] private AnimaTaskController task;

    [Header("Dialogue Sets")]
    [TextArea(5, 15)]
    [SerializeField] private string[] dialogue0;

    [TextArea(5, 15)]
    [SerializeField] private string[] dialogue1;

    [TextArea(5, 15)]
    [SerializeField] private string[] dialogue2;

    [SerializeField] private AnimaTaskController animaTaskController;

    [TextArea(5, 15)] public string[] lines;
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
        if (animaTaskController != null && animaTaskController.IsAnimaMoving())
            return;

        if (task.GetDialogueStep() == 0);
        if (task.GetDialogueStep() == 1);
        if (task.GetDialogueStep() == 2); 

        if (player == null) return;

        characterAnimator.SetBool("Talking", true);
        playerMovement.enabled = false;
        interactElement1.SetActive(false);
        interactElement2.SetActive(false);

        Transform model = characterAnimator.transform; 
        Vector3 lookPos = player.position;
        lookPos.y = model.position.y;
        model.LookAt(lookPos);

        if (animaTaskController != null)
        {
            int step = animaTaskController.GetDialogueStep();

            if (step == 0)
                lines = dialogue0;
            else if (step == 1)
                lines = dialogue1;
            else if (step >= 2)
                lines = dialogue2;
        }

        index = 0;
        isDialogueActive = true;
        gameObject.SetActive(true);

        textComponent.text = "";

        TrySetTalking(true);
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
        if (animaTaskController != null)
        {
            animaTaskController.StartMovingAfterDialogue();
        }

        characterAnimator.SetBool("Talking", false);
        playerMovement.enabled = true;
        interactElement1.SetActive(true);
        interactElement2.SetActive(true);

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
