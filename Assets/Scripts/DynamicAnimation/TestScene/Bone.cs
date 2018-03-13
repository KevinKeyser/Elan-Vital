using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour {

    public float Length;

    public Transform Target;

    private List<Bone> childBones;

    public List<Bone> ChildBones
    {
        get
        {
            if(childBones == null)
            {
                childBones = new List<Bone>();
                for (int index = 0; index < transform.childCount; index++)
                {
                    Bone childBone = transform.GetChild(index).GetComponent<Bone>();
                    childBone.Target = Target;
                    if(childBone != null)
                    {
                        childBones.Add(childBone);
                        childBones.AddRange(childBone.childBones);
                        break;
                    }
                }
            }

            return childBones;    
        }
    }


	// Use this for initialization
	void Start () 
    {
        if(ChildBones.Count > 0)
        {
            Length = Vector3.Distance(transform.position, ChildBones[0].transform.position);
        }
        else
        {
            Length = 0;
        }
	}
}
