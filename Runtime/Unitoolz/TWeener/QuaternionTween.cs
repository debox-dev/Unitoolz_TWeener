using UnityEngine;
using System;
using System.Collections;


namespace Smackware.TWeener
{



    public class QuaternionTween : TWeener
    {
        public Quaternion From;
        public Quaternion To;
        public Space Space;


        protected override void PrePlay()
        {
            base.PrePlay();
        }

        public override void Tween(float d)
        {
            Rotation = Quaternion.Lerp(From, To, d);
        }

        private Quaternion Rotation
        {
            get
            {
                switch (Space)
                {
                    case (Space.Self):
                        {
                            return transform.localRotation;
                        }
                    case (Space.World):
                        {
                            return transform.rotation;
                        }
                    default:
                        {
                            throw new Exception("Invalid space");
                        }
                }
            }
            set
            {
                switch (Space)
                {

                    case (Space.Self):
                        {
                            transform.localRotation = value;
                            break;
                        }
                    case (Space.World):
                        {
                            transform.rotation = value;
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