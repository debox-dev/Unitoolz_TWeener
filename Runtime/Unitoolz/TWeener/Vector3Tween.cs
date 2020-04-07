using UnityEngine;
using System;


namespace DeBox.Unitoolz.TWeener
{
    [System.Serializable]
    public enum Target
    {
        Position,
        EulerAngles,
        LocalEulerAngles,
        LocalPosition,
        LocalScale
    }


    public class Vector3Tween : TWeener {
        public Vector3 From;
        public Vector3 To;
        public bool Relative;
        public Target Target;

        private Vector3 _to;
        private Vector3 _from;

        protected override void PrePlay()
        {
            base.PrePlay();            
            if (Relative)
            {
                _from = Position + From;
                _to = Position + To;
            }
            else
            {
                _from = From;
                _to = To;                
            }            
        }

        public override void Tween(float d)
        {
            Vector3 Delta = _to - _from;
            Position = _from + Delta * d;            
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
                    case (Target.LocalEulerAngles):
                    {
                        return transform.localEulerAngles;
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
                    case (Target.LocalEulerAngles):
                        {
                            transform.localEulerAngles = value;
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