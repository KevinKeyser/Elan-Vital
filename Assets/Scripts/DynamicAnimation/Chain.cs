using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElanVital.DynamicAnimation
{
    public class Chain : Bone
    {
        public float Tolerance = 0.01f;

        public void ForwardKinematics()
        {
            // this sets the root at the initial position
            // after setting the root at the initial position,
            // this finds the difference between the current node's new position
            // and the next node's current position
            ChildBones[0].transform.position = transform.position;
            for (int index = 0; index < ChildBones.Count - 1; index++)
            {
                var difference = (ChildBones[index + 1].transform.position - ChildBones[index].transform.position);
                var proportion = ChildBones[index].Length / difference.magnitude;

                var position = (1 - proportion) * ChildBones[index].transform.position +
                               (proportion * ChildBones[index + 1].transform.position);
                ChildBones[index + 1].transform.position = position;
            }
        }

        public void ReverseKinematics()
        {
            ChildBones[ChildBones.Count - 1].Target = Target;
            for (int index = ChildBones.Count - 1; index >= 0; index--)
            {
                if (ChildBones[index].Target)
                {
                    var differenceBetweenTargetAndLastBone =
                        (ChildBones[index].Target.position - ChildBones[index].transform.position);
                    var proportion = ChildBones[index].Length / differenceBetweenTargetAndLastBone.magnitude;

                    var position = (1 - proportion) * ChildBones[index].Target.position +
                                   (proportion * ChildBones[index].transform.position);
                    ChildBones[index].transform.position = position;
                }
                else
                {
                    var differenceBetweenBones =
                        (ChildBones[index + 1].transform.position - ChildBones[index].transform.position);
                    var proportion = ChildBones[index].Length / differenceBetweenBones.magnitude;

                    var position = (1 - proportion) * ChildBones[index + 1].transform.position +
                                   (proportion * ChildBones[index].transform.position);
                    ChildBones[index].transform.position = position;
                }
            }
        }

        public void Solve()
        {
            float distanceBetweenStartAndTarget = Vector3.Distance(transform.position, Target.position);
            if (distanceBetweenStartAndTarget > MaxLength)
            {
                ChildBones[0].transform.position = transform.position;
                for (int index = 0; index < ChildBones.Count - 1; index++)
                {
                    var differenceBetweenTargetAndBone =
                        (Target.position - ChildBones[index].transform.position).magnitude;
                    var proportion = ChildBones[index].Length / differenceBetweenTargetAndBone;

                    ChildBones[index + 1].transform.position =
                        (1 - proportion) * ChildBones[index].transform.position + proportion * Target.position;
                }
            }
            else
            {
                int approximationCount = 0;
                var differenceBetweenLastBoneAndTarget =
                    (ChildBones[ChildBones.Count - 1].transform.position - Target.position).magnitude;
                while (differenceBetweenLastBoneAndTarget > Tolerance)
                {
                    // start with the reverse, then do forward 
                    ReverseKinematics();
                    ForwardKinematics();
                    differenceBetweenLastBoneAndTarget =
                        (ChildBones[ChildBones.Count - 1].transform.position - Target.position).magnitude;
                    approximationCount += 1;
                    // 10 approximatinos ought to suffice
                    if (approximationCount > 10)
                    {
                        break;
                    }
                }
            }

            Vector3[] finalPositions = new Vector3[ChildBones.Count];
            for (int i = 0; i < ChildBones.Count; i++)
            {
                finalPositions[i] = ChildBones[i].transform.position;
            }

            for (int index = 0; index < ChildBones.Count; index++)
            {
                if (index == ChildBones.Count - 1)
                {
                    ChildBones[index].transform.LookAt(Target.position);
                }
                else
                {
                    ChildBones[index].transform.LookAt(finalPositions[index + 1]);
                }
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            Solve();
        }


        public void OnDrawGizmos()
        {
            for (int i = 0; i < ChildBones.Count - 1; i++)
            {
                Gizmos.DrawLine(ChildBones[i].transform.position, ChildBones[i + 1].transform.position);
            }
        }
    }
}