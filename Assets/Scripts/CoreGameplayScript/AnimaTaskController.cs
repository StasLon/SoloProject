using UnityEngine;

public class AnimaTaskController : MonoBehaviour
{
    [Header("Quest State")]
    [SerializeField] private bool hasTalkedToAnima;   // Первый разговор
    [SerializeField] private bool letterCollected;    // Письмо подобрано

    private bool storyContinued;

    // ===== Вызывается из первого диалога =====
    public void TalkedToAnima()
    {
        if (hasTalkedToAnima)
            return;

        hasTalkedToAnima = true;
        Debug.Log("Игрок поговорил с Анимой и получил задание.");
    }

    // ===== Вызывается при нажатии E на письмо =====
    public void CollectLetter()
    {
        if (letterCollected)
            return;

        letterCollected = true;
        Debug.Log("Игрок подобрал письмо.");

        CheckStoryProgress();
    }

    // ===== Проверка условий =====
    private void CheckStoryProgress()
    {
        if (storyContinued)
            return;

        if (!hasTalkedToAnima)
            return;

        if (letterCollected)
        {
            ContinueStory();
        }
    }

    // ===== Продвижение сюжета =====
    private void ContinueStory()
    {
        storyContinued = true;

        Debug.Log("Письмо найдено. Анима меняет позицию и диалог. Сюжет идёт дальше.");

        // Тут позже:
        // - смена позиции Анимы
        // - новый диалог
        // - сигнал в StoryManager
    }

    // ===== Для диалоговой системы =====
    public bool HasTalkedToAnima() => hasTalkedToAnima;
    public bool HasLetter() => letterCollected;
}
