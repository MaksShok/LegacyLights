using _main.ServiceLoc;
using UnityEngine;

namespace InputControl
{
    public class InputController : IService
    {
        public Vector2 MoveInput()
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
            return moveInput;
        }

        public bool CheckAttackInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }

            return false;
        }
    }
}