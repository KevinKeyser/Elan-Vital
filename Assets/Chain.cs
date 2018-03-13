using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour {

    public Transform Target;

    public float Tolerance = 0.01f;

    private List<Bone> bones;

    public List<Bone> Bones
    {
        get
        {
            if(bones == null)
            {
                // this goes through the children and tries to find any bones among them;
                // if such a bone exists, find all of the children of that bone
                bones = new List<Bone>();
                for (int index = 0; index < transform.childCount; index++)
                {
                    Bone childBone = transform.GetChild(index).GetComponent<Bone>();
                    if(childBone != null)
                    {
                        bones.Add(childBone);
                        bones.AddRange(childBone.ChildBones);
                    }
                }
            }
            return bones;
        }
    }

    public float Length
    {
        // this returns the total length of the chain
        // by summing the individual lengths
        get
        {
            float length = 0;
            for (int index = 0; index < Bones.Count; index++)
            {
                length += Bones[index].Length;
            }
            return length;
        }
    }

    public void ForwardKinematics()
    {
        // this sets the root at the initial position
        // after setting the root at the initial position,
        // this finds the difference between the current node's new position
        // and the next node's current position
        Bones[0].transform.position = transform.position;
        for (int index = 0; index < Bones.Count - 1; index++)
        {
            var difference = (Bones[index + 1].transform.position - Bones[index].transform.position);
            var proportion = Bones[index].Length / difference.magnitude;

            var position = (1 - proportion) * Bones[index].transform.position + 
                                                          (proportion * Bones[index - 1].transform.position);
            Bones[index + 1].transform.position = position;
        }
    }

    public void ReverseKinematics()
    {
        Bones[Bones.Count - 1].Target = Target;
        for (int index = Bones.Count - 1; index >= 0; index--)
        {
            if(Bones[index].Target)
            {
                var differenceBetweenTargetAndLastBone = (Bones[index].Target.position - Bones[index].transform.position);
                var proportion = Bones[index].Length / differenceBetweenTargetAndLastBone.magnitude;

                var position = (1 - proportion) * Bones[index].Target.position + (proportion * Bones[index].transform.position);
                Bones[index].transform.position = position;
            }
            else
            {
                var differenceBetweenBones = (Bones[index + 1].transform.position - Bones[index].transform.position);
                var proportion = Bones[index].Length / differenceBetweenBones.magnitude;

                var position = (1 - proportion) * Bones[index + 1].transform.position + (proportion * Bones[index].transform.position);
                Bones[index].transform.position = position;
            }
        }
    }

    public void Solve()
    {
        float distanceBetweenStartAndTarget = Vector3.Distance(transform.position, Target.position);
        if (distanceBetweenStartAndTarget > Length)
        {
            Bones[0].transform.position = transform.position;
            for (int index = 0; index < Bones.Count - 1; index++)
            {
                var differenceBetweenTargetAndBone = (Target.position - Bones[index].transform.position).magnitude;
                var proportion = Bones[index].Length / differenceBetweenTargetAndBone;

                Bones[index + 1].transform.position = (1 - proportion) * Bones[index].transform.position + proportion * Target.position;
            }
        }
        else
        {
            int approximationCount = 0;
            var differenceBetweenLastBoneAndTarget = (Bones[Bones.Count - 1].transform.position - Target.position).magnitude;
            while(differenceBetweenLastBoneAndTarget > Tolerance)
            {
                // start with the reverse, then do forward 
                ReverseKinematics();
                ForwardKinematics();
                differenceBetweenLastBoneAndTarget = (Bones[Bones.Count - 1].transform.position - Target.position).magnitude;
                approximationCount += 1;
                // 10 approximatinos ought to suffice
                if(approximationCount > 10)
                {
                    break;
                }
            }
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
        Solve();
	}
}
