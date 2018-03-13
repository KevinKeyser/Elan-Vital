using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElanVital.DynamicAnimation
{
    public class Bone : MonoBehaviour
    {

        public float Length;
        [SerializeField] private float maxLength;

        public float MaxLength
        {
            get
            {
                maxLength = Length;
                if (ChildBones.Count > 0)
                {
                    maxLength += ChildBones[0].MaxLength;
                }

                return maxLength;
            }
        }

        public Transform Target;

        private Rigidbody rb;

        [SerializeField] private List<Bone> childBones;

        public List<Bone> ChildBones
        {
            get
            {
                if (childBones == null)
                {
                    childBones = new List<Bone>();
                    for (int index = 0; index < transform.childCount; index++)
                    {
                        Bone childBone = transform.GetChild(index).GetComponent<Bone>();
                        if (childBone != null)
                        {
                            //childBone.Target = Target;
                            childBones.Add(childBone);
                            childBones.AddRange(childBone.ChildBones);
                            break;
                        }
                    }
                }

                return childBones;
            }
        }


        // Use this for initialization
        void Start()
        {
            if (ChildBones.Count > 0)
            {
                Length = Vector3.Distance(transform.position, ChildBones[0].transform.position);
            }
            else
            {
                Length = 0;
            }
        }
    }
}