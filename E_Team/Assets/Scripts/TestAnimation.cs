using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour {

    private Animation anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayAttackAnimation();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayDamageAnimation();
        }
	}

    public void PlayAttackAnimation() {
        anim.Play("Attack");
    }

    public void PlayDamageAnimation() {
        anim.Play("TakeDamage");
    }
}
