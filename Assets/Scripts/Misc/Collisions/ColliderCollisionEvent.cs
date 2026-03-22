using System;
using UnityEngine;

namespace Misc.Collisions
{
    public class ColliderCollisionEvent : MonoBehaviour
    {
        public event Action<Collision2D> CollisionEnter;
        public event Action<Collision2D> CollisionExit;
        public event Action<Collision2D> CollisionStay;

        private void OnCollisionEnter2D(Collision2D other)
        {
            CollisionEnter?.Invoke(other);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            CollisionExit?.Invoke(other);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            CollisionStay?.Invoke(other);
        }
    }
}