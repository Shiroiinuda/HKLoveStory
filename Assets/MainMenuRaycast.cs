using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MainMenu
{
    public class MainMenuRaycast : MonoBehaviour
    {
        public Camera camera;
        public bool canClick;

        private void Start()
        {
            if (camera != null) return;
            camera = GetComponentInChildren<Camera>();
        }

        public void CanClick(bool yes) => canClick = yes;

        void Update()
        {
            if (!canClick) return;
            Vector2 inputPosition = Vector2.zero;
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                inputPosition = Mouse.current.position.ReadValue();
            }
            // Otherwise, check for a touch on mobile
            else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                inputPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            }
#if UNITY_STANDALONE_WIN || UNITYSTANDALONE_OSX
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                // Get the current mouse position (in screen coordinates)
                Vector2 mousePosition = inputPosition;

                // Convert the screen-space mouse position to world space
                Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition);

                // Cast a ray from the mouse's world position
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

                // If we hit an object with a Collider2D, interact with it
                if (hit.collider != null)
                {
                    Debug.Log("Clicked on object: " + hit.collider.gameObject.name);

                    // Example interaction:
                    hit.collider.GetComponent<IMainMenuInteraction>()?.Interactions();
                }
            }
#endif
#if UNITY_ANDROID || UNITY_IOS
            if(Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                // Get the current mouse position (in screen coordinates)
                Vector2 mousePosition = inputPosition;

                // Convert the screen-space mouse position to world space
                Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition);

                // Cast a ray from the mouse's world position
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

                // If we hit an object with a Collider2D, interact with it
                if (hit.collider != null)
                {
                    Debug.Log("Clicked on object: " + hit.collider.gameObject.name);

                    // Example interaction:
                    hit.collider.GetComponent<IMainMenuInteraction>()?.Interactions();
                }
            }
#endif
        }
    }

    interface IMainMenuInteraction
    {
        void Interactions();
    }
}