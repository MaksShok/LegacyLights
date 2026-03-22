using System;
using UnityEngine;

namespace Misc.Collisions
{
    public class CollisionColliderEvent : MonoBehaviour
    {
        public event Action<Collision> CollisionEnter;
        public event Action<Collision> CollisionExit;
        public event Action<Collision> CollisionStay;

        private void OnCollisionEnter(Collision other)
        {
            CollisionEnter?.Invoke(other);
        }

        private void OnCollisionStay(Collision other)
        {
            CollisionExit?.Invoke(other);
        }

        private void OnCollisionExit(Collision other)
        {
            CollisionStay?.Invoke(other);
        }
    }
}