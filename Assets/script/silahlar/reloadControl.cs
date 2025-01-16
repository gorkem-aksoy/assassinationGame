using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reloadControl : StateMachineBehaviour
{


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state

    // animasyon bittiðinde reload true olacak ve matematiksel iþlemlerin çalýþmasýný baþlatýcak
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("reload", true);
    }

  
}
