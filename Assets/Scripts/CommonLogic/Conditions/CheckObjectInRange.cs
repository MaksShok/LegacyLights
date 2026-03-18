using UnityEngine;

namespace CommonLogic.Conditions
{
    public class CheckObjectInRange
    {
        public bool IsInRange
        {
            get
            {
                float distanceToTarget = Vector2.Distance(_obj1.position, _obj2.position);
                return distanceToTarget >= _minDistance && distanceToTarget <= _maxDistance;
            }
        }

        public bool IsTooClose => Distance < _minDistance;
        public bool IsTooFar => Distance > _maxDistance;
        public float Distance => Vector2.Distance(_obj1.position, _obj2.position);

        private readonly Transform _obj1;
        private readonly Transform _obj2;
        private readonly float _minDistance;
        private readonly float _maxDistance;

        public CheckObjectInRange(Transform obj1, Transform obj2, float minDistance, float maxDistance)
        {
            _obj1 = obj1;
            _obj2 = obj2;
            _minDistance = Mathf.Max(0, minDistance);
            _maxDistance = Mathf.Max(minDistance, maxDistance);
        }
    }
}