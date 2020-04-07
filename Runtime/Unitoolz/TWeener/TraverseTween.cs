using UnityEngine;
using System;
using System.Collections;


namespace DeBox.Unitoolz.TWeener
{
    public class TraverseTween : TWeener
    {        
        public ITraversable Traversable;
        public Target Target;        

        protected override void PrePlay()
        {
            base.PrePlay();
        }
          
        public override void Tween(float d)
        {
            if (Traversable == null)
            {
                return;
            }
            Position = Traversable.GetPoint(d);
        }

        private Vector3 Position
        {
            get
            {
                switch (Target)
                {
                    case (Target.Position):
                        {
                            return transform.position;
                        }
                    case (Target.LocalPosition):
                        {
                            return transform.localPosition;
                        }
                    case (Target.EulerAngles):
                        {
                            return transform.eulerAngles;
                        }
                    case (Target.LocalScale):
                        {
                            return transform.localScale;
                        }
                    default:
                        {
                            throw new Exception("Invalid space");
                        }
                }
            }
            set
            {
                switch (Target)
                {

                    case (Target.Position):
                        {
                            transform.position = value;
                            break;
                        }
                    case (Target.LocalPosition):
                        {
                            transform.localPosition = value;
                            break;
                        }
                    case (Target.EulerAngles):
                        {
                            transform.eulerAngles = value;
                            break;
                        }
                    case (Target.LocalScale):
                        {
                            transform.localScale = value;
                            break;
                        }
                    default:
                        {
                            throw new Exception("Invalid space");
                        }
                }
            }
        }

    }

}