using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public class InputManager : MonoBehaviour
    {
        public InputReader inputReader;
        public bool ShouldHideCursor = false;

        protected virtual void Start()
        {
            Initialization();
        }

        public virtual void Initialization()
        {
            HideCursor();
        }

        public virtual void HideCursor()
        {
            Cursor.visible = !ShouldHideCursor;
        }
    }
}
