using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RSG;

namespace Unitoolz.TWeener
{
    public static class TweenUtils 
    {
        private static TweenRunner tweenRunner = null;

        private static TweenRunner GetOrCacheTweenRunner()
        {
            if (tweenRunner == null) 
            {
                var runnerGameObject = new GameObject ("TweenRunner");
                runnerGameObject.hideFlags = HideFlags.HideAndDontSave;
                UnityEngine.Object.DontDestroyOnLoad (runnerGameObject);
                tweenRunner = runnerGameObject.AddComponent<TweenRunner> ();
            }
            return tweenRunner;
        }

        public static float Pyramid(float t)
        {
            return Reverse(Mathf.Abs((t * 2 - 1)));
        }

        public static float Reverse(float t)
        {
            return 1 - t;
        }

        public static IEnumerator WaitForPromise(IPromise promise)
        {
            bool isComplete = false;
            promise.Done(() => isComplete = true);
            while (!isComplete)
            {
                yield return null;
            }
        }

        public static Coroutine RunCoroutine(IEnumerator enumerable)
        {
            var owner = GetOrCacheTweenRunner ();
            return owner.StartCoroutine(enumerable);
        }

        public static ITweenPromise Ease(float duration, Func<float, float> easing, MonoBehaviour owner = null)
        {
            var promise = new TweenPromise ();
            if (owner == null) {
                owner = GetOrCacheTweenRunner ();
            }
            owner.StartCoroutine (TweenCoroutine (duration, easing, promise));
            return promise;
        }

        public static ITweenPromise Timer(float duration, MonoBehaviour owner = null)
        {
            return Ease (duration, x => x, owner);
        }

        private static IEnumerator TweenCoroutine(float duration, Func<float, float> easing, TweenPromise tweenPromise)
        {
            float elapsed = 0;
            do
            {
                yield return null;
                var progress = Mathf.Min (1, elapsed / duration);
                var tweenValue = easing(progress);
                tweenPromise.ReportUpdate (tweenValue);
                elapsed += Time.deltaTime;
            } while (tweenPromise.CurState == RSG.PromiseState.Pending && elapsed < duration && !tweenPromise.StopRequested);
            if (tweenPromise.CurState == RSG.PromiseState.Pending)
            {
                tweenPromise.Resolve ();
            }
        }
    }
}