using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavKeypad
{
    public class KeypadInteractionFPV : MonoBehaviour
    {
        private Camera cam;
        private void Awake() => cam = Camera.main;
        private void Update()
        {
           
            var ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider.TryGetComponent(out KeypadButton keypadButton))
                    {
                        Debug.Log("BUTTON FOUND!");
                        keypadButton.PressButton();
                    }
                    else
                    {
                        Debug.Log("Hit something, but NOT a KeypadButton");
                    }
                }
                else
                {
                    Debug.Log("RAYCAST HIT NOTHING");
                }
            }
        }
    }
}