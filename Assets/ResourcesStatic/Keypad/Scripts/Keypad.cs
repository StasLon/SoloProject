using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Animations;



    public class Keypad : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent onAccessGranted;
        [SerializeField] private UnityEvent onAccessDenied;
        [Header("Combination Code (9 Numbers Max)")]
        [SerializeField] private int keypadCombo = 270902;

        public UnityEvent OnAccessGranted => onAccessGranted;
        public UnityEvent OnAccessDenied => onAccessDenied;

        [Header("Settings")]
        [SerializeField] private string accessGrantedText = "Granted";
        [SerializeField] private string accessDeniedText = "Denied";

        [Header("Visuals")]
        [SerializeField] private float displayResultTime = 1f;
        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f;
        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f); //orangy
        [SerializeField] private Color screenDeniedColor = new Color(1f, 0f, 0f, 1f); //red
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f); //greenish
        [Header("SoundFx")]
        [SerializeField] private AudioClip buttonClickedSfx;
        [SerializeField] private AudioClip accessDeniedSfx;
        [SerializeField] private AudioClip accessGrantedSfx;
        [Header("Component References")]
        [SerializeField] private Renderer panelMesh;
        [SerializeField] private TMP_Text keypadDisplayText;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private RenderTexture blurTexture;
        [SerializeField] private GameObject blurScreen;
        [SerializeField] private Animator doorAnimator;
        [SerializeField] private GameObject interactElement1;
        [SerializeField] private GameObject interactElement2;



    [Header("Camera & Controls")]
        [SerializeField] private Camera mainCamera; // твоя основная камера
        [SerializeField] private Transform keypadCamPoint; // позиция камеры у keypad
        [SerializeField] private Transform playerCamPoint; // исходная позиция камеры
        [SerializeField] private FirstPersonController playerMovement; // скрипт передвижения
        [SerializeField] private float cameraMoveSpeed = 5f;

        [Header("Light")]
        [SerializeField] private GameObject lamp;
        [SerializeField] private float lampDelay = 3f;

        private bool isUsingKeypad = false;

        private RenderTexture previousRT;
        private string currentInput;
        private bool displayingResult = false;
        private bool accessWasGranted = false;

        private void Awake()
        {
            ClearInput();
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
        }

        private void Update()
        {
            // ======== ADD: CAMERA FOLLOW WHILE USING ========
            if (isUsingKeypad)
            {
                mainCamera.transform.position = Vector3.Lerp(
                    mainCamera.transform.position,
                    keypadCamPoint.position,
                    Time.deltaTime * cameraMoveSpeed
                );

                mainCamera.transform.rotation = Quaternion.Lerp(
                    mainCamera.transform.rotation,
                    keypadCamPoint.rotation,
                    Time.deltaTime * cameraMoveSpeed
                );

                // выход по ESC
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ExitKeypad();
                }
            }
            // ==================================================
        }


        //Gets value from pressedbutton
        public void AddInput(string input)
        {
            audioSource.PlayOneShot(buttonClickedSfx);
            if (displayingResult || accessWasGranted) return;
            switch (input)
            {
                case "enter":
                    CheckCombo();
                    break;
                default:
                    if (currentInput != null && currentInput.Length == 9) // 9 max passcode size 
                    {
                        return;
                    }
                    currentInput += input;
                    keypadDisplayText.text = currentInput;
                    break;
            }

        }
        public void CheckCombo()
        {
            if (int.TryParse(currentInput, out var currentKombo))
            {
                bool granted = currentKombo == keypadCombo;
                if (!displayingResult)
                {
                    StartCoroutine(DisplayResultRoutine(granted));
                }
            }
            else
            {
                Debug.LogWarning("Couldn't process input for some reason..");
            }

        }

        //mainly for animations 
        private IEnumerator DisplayResultRoutine(bool granted)
        {
            displayingResult = true;

            if (granted) AccessGranted();
            else AccessDenied();

            yield return new WaitForSeconds(displayResultTime);
            displayingResult = false;
            if (granted) yield break;
            ClearInput();
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);

        }

        private void AccessDenied()
        {
            keypadDisplayText.text = accessDeniedText;
            onAccessDenied?.Invoke();
            panelMesh.material.SetVector("_EmissionColor", screenDeniedColor * screenIntensity);
            audioSource.PlayOneShot(accessDeniedSfx);
            
        }

        private void ClearInput()
        {
            currentInput = "";
            keypadDisplayText.text = currentInput;
        }

        private void AccessGranted()
        {
            accessWasGranted = true;
            keypadDisplayText.text = accessGrantedText;
            onAccessGranted?.Invoke();
            panelMesh.material.SetVector("_EmissionColor", screenGrantedColor * screenIntensity);
            audioSource.PlayOneShot(accessGrantedSfx);
            doorAnimator.SetTrigger("OpenDoor");
            StartCoroutine(TurnOnLampAfterDelay());
    }

        public void EnterKeypad()
        {
            isUsingKeypad = true;

            interactElement1.SetActive(false);
            interactElement2.SetActive(false);

            // отключаем игрока
            playerMovement.enabled = false;
           
            // курсор
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            //блюр
            blurScreen.SetActive(false);
            previousRT = mainCamera.targetTexture;
            mainCamera.targetTexture = null;
        }

        private void ExitKeypad()
        {
            isUsingKeypad = false;

            interactElement1.SetActive(true);
            interactElement2.SetActive(true);

            // возвращаем камеру
            mainCamera.transform.position = playerCamPoint.position;
            mainCamera.transform.rotation = playerCamPoint.rotation;

            // возвращаем рендер текстуру
            mainCamera.targetTexture = previousRT;

            // Чтобы RenderTexture не был "замёрзшим":
            mainCamera.Render(); // <<< ВАЖНО: форс перерисовка

            // Включаем отображение RT
            blurScreen.SetActive(true);

            // включаем игрока обратно
            playerMovement.enabled = true;
            
            // скрываем курсор
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

           
        }

        private IEnumerator TurnOnLampAfterDelay()
        {
            yield return new WaitForSeconds(lampDelay);
            lamp.SetActive(true);
        }



}
