using UnityEngine;
using System;

public class InputController : MonoBehaviour
    {
        public event Action<Vector2> OnMoveInput;

        private void CheckMoveInput()
        {
            float horizontal = 0f;
            float vertical = 0f;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                vertical = 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                vertical = -1f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                horizontal = -1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                horizontal = 1f;

            Vector2 moveInput = new Vector2(horizontal, vertical);
            OnMoveInput?.Invoke(moveInput);
        }

        private void Update()
        {
            CheckMoveInput();
        }
    }