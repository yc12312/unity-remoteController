using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : StateMachineBehaviour {

    public float timer;

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (timer <= 0)
        {
            animator.SetBool("Hit", false);
        }
        else
        {
            timer -= Time.deltaTime;
        }
	}

}
