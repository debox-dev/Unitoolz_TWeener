using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RSG;

namespace DeBox.Unitoolz.TWeener
{
    [System.Serializable]
    public enum LoopType
    {
        None,
        Loop,
        ReturnAndLoop,
        ReturnOnce
    }

    [System.Serializable]
    public enum EasingType
    {
        None,
        Linear,
        QuadIn,
        QuadOut,
        QuadInOut,

        CircInOut,
        CircIn,
        CircOut,
        BounceIn,
        BounceOut,
        ElasticIn,
        QubicIn,
        QubicOut,
        QubicInOut,
        SineIn,
        SineOut,
        SineInOut,
        QuintOut,
        ExpoOut,
    }

    public abstract class TWeener : MonoBehaviour
    {
        #region Public Properties
        public bool PlayOnStart;
        public bool PlayOnEnable;
        public float Duration;
        public EasingType EasingType;
        public LoopType Loop;
        public bool Reverse;
        public string Name;
        public bool Test;
        public float CurrentValue;
        #endregion

        #region Public Events
        public event Action<TWeener, float> OnTween;
        #endregion

        private static readonly Dictionary<EasingType, Func<float, float>> _easingDict = new Dictionary<EasingType, Func<float, float>>()
        {
            {EasingType.Linear, Easing.linear},
            {EasingType.QuadIn, Easing.quadIn},
            {EasingType.QuadOut, Easing.quadOut},
            {EasingType.QuadInOut, Easing.quadInOut},
            {EasingType.BounceIn, Easing.bounceIn},
            {EasingType.BounceOut, Easing.bounceOut},
            {EasingType.CircIn, Easing.circIn},
            {EasingType.CircOut, Easing.circOut},
            {EasingType.CircInOut, Easing.circInOut},
            {EasingType.ElasticIn, Easing.elasticIn},
            {EasingType.QubicIn, Easing.cubicIn},
            {EasingType.QubicOut, Easing.cubicOut},
            {EasingType.QubicInOut, Easing.cubicInOut},
            {EasingType.SineIn, Easing.sineIn},
            {EasingType.SineOut, Easing.sineOut},
            {EasingType.SineInOut, Easing.sineInOut},
            {EasingType.ExpoOut, Easing.expoOut},
            {EasingType.QuintOut, Easing.quintOut},
        };

        private float _startTime;
        private bool _isPlaying;
        private Action _onNextStop;
        private Promise _lastPromise;
        private Coroutine _lastCoroutine;
        public bool UseCurve;
        public AnimationCurve Curve;

        public bool IsPlaying { get { return _isPlaying;  } }


        void OnEnable()
        {
            if (PlayOnEnable)
            {
                Play();
            }
        }


        public Promise Play(float duration = -1, EasingType easingType = EasingType.None, Action<float> onTween = null)
        {
            PrePlay();
            if (_isPlaying == true)
            {
                Stop();
            }
            if (duration > 0)
            {
                Duration = duration;
            }
            if (easingType != EasingType.None)
            {
                EasingType = easingType;
            }
            if (EasingType == EasingType.None)
            {
                EasingType = EasingType.Linear;
            }
            // Reject any existing promise and clean it up

            _lastPromise = new Promise();
            _lastCoroutine = StartCoroutine(TweenCoro(onTween));
            return _lastPromise;
        }

		public Coroutine PlayCoro(float duration = -1, EasingType easingType = EasingType.None, Action<float> onTween = null)
		{
			Play(duration, easingType, onTween);
			return _lastCoroutine;
		}

        public void Stop()
        {
            _isPlaying = false;
            if (_lastPromise != null)
            {
                _lastPromise.Reject(null);
                _lastPromise = null;
            }
            if (_lastCoroutine != null)
            {
                StopCoroutine(_lastCoroutine);
                _lastCoroutine = null;
            }
        }

        private IEnumerator TweenCoro(Action<float> onTween = null)
        {            
            var startTime = Time.time;
            float elapsedTime = 0;
            _isPlaying = true;
            float realT;
            bool keepPlaying = true;
            do
            {
                if (!_isPlaying)
                {
                    yield break;
                }
                if (Loop == LoopType.Loop || Loop == LoopType.ReturnAndLoop)
                {
                    elapsedTime = (Time.time - startTime) % Duration;
                }
                else
                {
                    elapsedTime = Mathf.Min(Time.time - startTime, Duration);
                }
                if (Loop == LoopType.ReturnAndLoop || Loop == LoopType.ReturnOnce)
                {
                    realT = Mathf.Min(elapsedTime, Duration) / Duration;
                    CurrentValue = 1 - Mathf.Abs((realT * 2) - 1);
                }
                else
                {                    
                    CurrentValue = elapsedTime / Duration;
                    realT = CurrentValue;
                }                
                Tween(GetDelta(CurrentValue));
                OnTween?.Invoke(this, CurrentValue);
                onTween?.Invoke(CurrentValue);

                yield return new WaitForEndOfFrame();
                bool timeOver = realT >= 1;
                if (Loop == LoopType.Loop || Loop == LoopType.ReturnAndLoop)
                {
                    keepPlaying = _isPlaying;
                }
                else
                {
                    keepPlaying = !timeOver && _isPlaying;
                }
            }
            while (keepPlaying);

            var promise = _lastPromise;
            _isPlaying = false;            
            _lastPromise = null;
            _lastCoroutine = null;
            promise.Resolve();           
        }

        private float GetDelta(float t)
        {            
            if (Reverse)
            {
                t = 1 - t;
            }
            if (UseCurve)
            {                
                return Curve.Evaluate(t);
            }
            Func<float, float> easing = _easingDict[EasingType];            
            return easing(t);
        }

        public virtual void Tween(float d)
        {

        }

        private void Start()
        {
            if (PlayOnStart)
            {
                Play();
            }
        }

        protected virtual void PrePlay()
        {

        }

    }

}