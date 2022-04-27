using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

using Penwyn.Tools;
namespace Penwyn.Game
{
    public class CursorManager : MonoBehaviour
    {
        public Texture2D NormalCursor;
        public Texture2D TargettingCursor;

        protected bool _isPermanentlyHide;

        public virtual void ShowMouse()
        {
            ChangeMouseMode(true);
        }

        public virtual void HideMouse()
        {
            if (_isPermanentlyHide)
                ChangeMouseMode(false);
        }

        protected virtual void ChangeMouseMode(bool isVisible, CursorLockMode lockMode = CursorLockMode.Confined)
        {
            Cursor.visible = isVisible;
            Cursor.lockState = lockMode;
        }

        public static Vector3 GetMousePosition()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0;
            return mousePos;
        }

        public GameObject GetObjectUnderMouse()
        {
            Vector3 mousePos = GetMousePosition();
            Vector2 ray = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit2D = Physics2D.Raycast(ray, ray);
            if (hit2D.collider != null)
            {
                return hit2D.collider.gameObject;
            }
            return null;
        }

        public void ResetCursor()
        {
            ChangeCursorSprite(NormalCursor);
        }

        public void ChangeCursorSprite(Texture2D cursorSprite)
        {
            Cursor.SetCursor(cursorSprite, Vector3.zero * cursorSprite.height / 2f, CursorMode.Auto);
        }


        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == SceneManager.Instance.MatchSceenName)
            {
                _isPermanentlyHide = true;
                HideMouse();
            }
            else
            {
                _isPermanentlyHide = false;
                ShowMouse();
            }
        }

        public virtual void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            InputReader.Instance.ChangeMouseVisibilityPressed += ShowMouse;
            InputReader.Instance.ChangeMouseVisibilityReleased += HideMouse;
        }

        public virtual void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
            InputReader.Instance.ChangeMouseVisibilityPressed -= ShowMouse;
            InputReader.Instance.ChangeMouseVisibilityReleased -= HideMouse;
        }
    }
}

