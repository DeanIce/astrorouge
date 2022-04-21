using UnityEngine;

namespace Enemy.Ingenalvus
{
    public class IngenalvusStateMachine : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            Debug.Log("animation begun");
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            Debug.Log("animation done");
            GameObject ingenalvus = animator.gameObject.transform.root.gameObject;
            var ing = ingenalvus.GetComponent<Ingenalvus>();
            ing.EndWeakPoints();
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }
    }
}