using UnityEngine;
using System.Collections;

public class AnimaTaskController : MonoBehaviour
{
    [Header("Quest State")]
    [SerializeField] private bool hasTalkedToAnima;
    [SerializeField] private bool letterCollected;
    [SerializeField] private int dialogueCounter = 0;
    private bool storyContinued;

    [Header("Anima Movement")]
    [SerializeField] private Transform anima;          
    [SerializeField] private Transform animaTarget;    
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Animator animaAnimator;

    private bool isMoving = false;

    [Header("Dialogue Control")]
    private bool canTalk = true; 

    private void Start()
    {
        if (anima == null)
            anima = this.transform;
    }

    
    public void TalkedToAnima()
    {
        if (hasTalkedToAnima) return;
        hasTalkedToAnima = true;
        Debug.Log("Игрок поговорил с Анимой и получил задание.");
    }

    
    public void CollectLetter()
    {
        if (letterCollected) return;
        letterCollected = true;
        Debug.Log("Игрок подобрал письмо.");
        CheckStoryProgress();
    }

    
    private void CheckStoryProgress()
    {
        if (storyContinued) return;
        if (!hasTalkedToAnima) return;

        if (letterCollected)
        {
            ContinueStory();
        }
    }

    
    private void ContinueStory()
    {
        storyContinued = true;
        Debug.Log("Письмо найдено. Сюжет идёт дальше.");
        
    }

   
    public void StartAnimaMove()
    {
        if (isMoving) return;

        isMoving = true;
        canTalk = false; 
        animaAnimator.SetBool("isWalking", true);
        StartCoroutine(MoveAnimaToPoint());
    }

    private IEnumerator MoveAnimaToPoint()
    {
        while (Vector3.Distance(anima.position, animaTarget.position) > 0.1f)
        {
            
            Vector3 direction = (animaTarget.position - anima.position).normalized;
            direction.y = 0f; 

            
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                anima.rotation = Quaternion.Slerp(
                    anima.rotation,
                    lookRotation,
                    5f * Time.deltaTime
                );
            }

            
            anima.position = Vector3.MoveTowards(
                anima.position,
                animaTarget.position,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        
        anima.position = animaTarget.position;
        animaAnimator.SetBool("isWalking", false);
        isMoving = false;

        
        anima.rotation = Quaternion.Euler(0f, -177.566f, 0f);

        canTalk = true; 
        Debug.Log("Анима дошла до точки и готова к диалогу.");
    }

    
    public void AddFromAnima()
    {
        if (dialogueCounter == 0)
        {
            dialogueCounter = 1;
            Debug.Log("Анима: переход на диалог 1");
            TalkedToAnima();
            
        }
    }

    public void StartMovingAfterDialogue()
    {
        if (dialogueCounter == 1 && !isMoving)
        {
            StartAnimaMove();
        }
    }

    public void AddFromLetter()
    {
        if (dialogueCounter == 1)
        {
            dialogueCounter = 2;
            Debug.Log("Письмо найдено: переход на финальный диалог");
            CollectLetter();
        }
        else
        {
            Debug.Log("Письмо нельзя использовать сейчас");
        }
    }

    public bool HasTalkedToAnima()
    {
        return hasTalkedToAnima;
    }

    public int GetDialogueStep() => dialogueCounter;
    public bool IsAnimaMoving() => isMoving;
    public bool CanTalk() => canTalk;
}
