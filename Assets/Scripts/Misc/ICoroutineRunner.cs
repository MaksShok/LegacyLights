using System.Collections;
using UnityEngine;

namespace Misc
{
        public interface ICoroutineRunner
        {
                Coroutine StartCoroutine(IEnumerator enumerator);

                void StopCoroutine(IEnumerator enumerator);
                void StopCoroutine(Coroutine coroutine);
        }
}