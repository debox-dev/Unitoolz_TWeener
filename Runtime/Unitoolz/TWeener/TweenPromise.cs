using System;
using System.Collections.Generic;

using RSG;

namespace Unitoolz.TWeener
{

    public interface ITweenPromise : IPromise
    {
        ITweenPromise Update(Action<float> onUpdate);
        ITweenPromise Update(Func<float, bool> onUpdate);

        void Stop();
    }


    public class TweenPromise : Promise, ITweenPromise
    {
        private readonly IList<Action<float>> onUpdateGenericActions = new List<Action<float>>();
        private readonly IList<Func<float, bool>> onUpdateGenericValidators = new List<Func<float, bool>>(); 

        public bool StopRequested { get; protected set; }

        public TweenPromise () : base()
        {
            SetupCleanUpCallbacks ();
        }


        public TweenPromise (Action<Action, Action<Exception>> resolver) : base (resolver)
        {
            SetupCleanUpCallbacks ();
        }

        public ITweenPromise Update (Action<float> onUpdate)
        {
            onUpdateGenericActions.Add (onUpdate);
            return this;
        }

        public ITweenPromise Update (Func<float, bool> onUpdate)
        {
            onUpdateGenericValidators.Add (onUpdate);
            return this;
        }

        public virtual void ReportUpdate(float progress01)
        {
            for (int i = 0; i < onUpdateGenericValidators.Count; i++)
            {
                var updateValidator = onUpdateGenericValidators [i];
                var shouldContinue = updateValidator (progress01);
                if (!shouldContinue)
                {
                    Resolve ();
                }
            }
            for (int i = 0; i < onUpdateGenericActions.Count; i++)
            {
                var updateAction = onUpdateGenericActions [i];
                updateAction (progress01);
            }
        }

        public void Stop ()
        {
            StopRequested = true;
        }

        private void SetupCleanUpCallbacks()
        {
            Then (() => CleanUp ())
                .Catch (e => CleanUp ());
        }

        private void CleanUp()
        {
            onUpdateGenericActions.Clear ();
            onUpdateGenericValidators.Clear ();
        }
    }

}