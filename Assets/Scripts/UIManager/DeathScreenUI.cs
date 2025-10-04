using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Button respawnButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private WarningTextSpawner warningTextSpawnerScript;

    private void Start()
    {
        deathPanel.SetActive(false);
        respawnButton.onClick.AddListener(Respawn);
        exitButton.onClick.AddListener(ExitGame);
    }

    public void ShowDeathScreen()
    {
        warningTextSpawnerScript.ClearAllTexts();
        deathPanel.SetActive(true);
        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Respawn()
    {
        Debug.Log("Нажата кнопка респавн");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        deathPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Нажата кнопка респавн");
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
