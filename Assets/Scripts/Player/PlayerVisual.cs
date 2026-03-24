using System;
using UnityEngine;

namespace Player
{
    public class PlayerVisual : MonoBehaviour
    {
        [SerializeField] 
        private Transform[] _requiredFlipObjects;
        
        private void RotateToMouse()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mouseWorldPos - transform.position;

            float rotationAngle;
            if (direction.x < 0)
            {
                rotationAngle = 180f;
            }
            else
            {
                rotationAngle = 0f;
            }

            transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
            foreach (var trn in _requiredFlipObjects)
            {
                trn.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
            }
        }

        private void Update()
        {
            RotateToMouse();
        }
    }
}