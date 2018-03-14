using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace ElanVital.DynamicAnimation
{
    public class DynamicAnimation
    {
        public Transform Target;
        private int currentIndex = 0;
        private List<Vector3> animationPath;
        public bool IsFinished { get; set; } = false;
        public event EventHandler AnimationFinished;
        public float Speed;

        public DynamicAnimation(Transform target, List<Vector3> path)
        {
            Target = target;
            animationPath = path;
        }

        public void Update()
        {
            if (!IsFinished)
            {
                // move towards the given position from the target's position at a given speed
                Target.position = Vector3.MoveTowards(Target.position, animationPath[currentIndex], Speed);

                if (Vector3.Distance(Target.position, animationPath[currentIndex]) < 0.1f)
                {
                    currentIndex++;
                }
                // if we've gotten to the end of our animation path, what do we do?
                if (currentIndex == animationPath.Count)
                {
                    IsFinished = true;
                    AnimationFinished?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void Reset()
        {
            IsFinished = false;
            currentIndex = 0;
        }
    }
}
