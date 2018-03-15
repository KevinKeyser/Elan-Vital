using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElanVital.DynamicAnimation
{
    public class Bone : MonoBehaviour
    {
        private new Rigidbody rigidbody;
        public Quaternion OffsetRotation;
        public Vector3 euler;

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

        void Awake()
        {
            childBones = null;
        }

        // Use this for initialization
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            OffsetRotation = transform.rotation;
            if (ChildBones.Count > 0)
            {
                Length = Vector3.Distance(transform.position, ChildBones[0].transform.position);
                //OffsetRotation = transform.rotation * Quaternion.Euler(45, 0, 0);
                //euler = OffsetRotation.eulerAngles;
            }
            else
            {
                Length = 0;
            }
        }

        public void SetPosition(Vector3 position)
        {
            if (rigidbody)
            {
                rigidbody.MovePosition(position);
            }
            else
            {
                transform.position = position;
            }
        }

        public void SetRotation(Quaternion rotation)
        {
            if (rigidbody)
            {
                rigidbody.MoveRotation(rotation);
            }
            else
            {
                transform.rotation = rotation;
            }

        }
    }
}