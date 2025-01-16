using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reloadControl : StateMachineBehaviour
{


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state

    // animasyon bitti�inde reload true olacak ve matematiksel i�lemlerin �al��mas�n� ba�lat�cak
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("reload", true);
    }

  
}
