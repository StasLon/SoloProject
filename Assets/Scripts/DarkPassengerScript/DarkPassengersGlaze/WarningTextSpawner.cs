using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class WarningTextSpawner : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float spawnInterval = 0.3f;
    [SerializeField] private float textLifetime = 1f;
    [SerializeField] private List<string> warningWords = new List<string> {};
    [SerializeField] private List<GameObject> spawnedTexts = new List<GameObject>();

    private bool isSpawning = false;

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            StartCoroutine(SpawnLoop());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        ClearAllTexts();
    }

    private IEnumerator SpawnLoop()
    {
        isSpawning = true;
        while (isSpawning)
        {
            SpawnWarningText();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnWarningText()
    {
        if (warningWords.Count == 0 || textPrefab == null || canvas == null) return;

        string randomWord = warningWords[Random.Range(0, warningWords.Count)];
        GameObject instance = Instantiate(textPrefab, canvas.transform);
        spawnedTexts.Add(instance);

        RectTransform rect = instance.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(
            Random.Range(-Screen.width / 2f, Screen.width / 2f),
            Random.Range(-Screen.height / 2f, Screen.height / 2f)
        );

        TMP_Text text = instance.GetComponent<TMP_Text>();
        if (text != null)
        {
            text.text = randomWord;
            text.alpha = 0f;
            StartCoroutine(FadeInAndDestroy(text, textLifetime));
        }
    }

    private IEnumerator FadeInAndDestroy(TMP_Text text, float lifetime)
    {
        float duration = 0.3f;
        float elapsed = 0f;

        // Fade in
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);
            text.alpha = alpha;
            yield return null;
        }

        yield return new WaitForSeconds(lifetime);

        // Destroy
        if (text != null)
        {
            Destroy(text.gameObject);
        }
        
        
    }

    public void ClearAllTexts()
    {
        foreach (GameObject obj in spawnedTexts)
        {
            if (obj != null)
                Destroy(obj);
        }
        spawnedTexts.Clear();
    }


}
