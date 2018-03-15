using System.Collections;
using System.Collections.Generic;
using ElanVital.Combat;
using ElanVital.DynamicAnimation;
using UnityEngine;

public class BossLimbHealth : MonoBehaviour, IHealth
{
    private Bone bone;
	// Use this for initialization
	void Start ()
	{
	    bone = transform.parent.GetComponent<Bone>();
	}

    public bool IsDead => Health <= 0;
    [SerializeField]
    private int health;
    public int Health => health;

    public void ModifyHealth(int value)
    {
        health = Mathf.Max(0, health + value);
        if (health <= 0)
        {
            if (bone.ChildBones.Count > 0)
            {
                bone.ChildBones[0].transform.parent = null;
                for (int i = 0; i < bone.ChildBones.Count; i++)
                {
                    bone.ChildBones[i].rigidbody.isKinematic = false;
                }
                Destroy(this.gameObject);
            }
        }
    }
}
