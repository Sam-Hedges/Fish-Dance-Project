using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] private Ability[] abilities;
    [SerializeField] private Animator animator;

    public void ExecuteAbility(Ability _ability) 
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i] == _ability)
            {
                _ability.EnableButton();
                animator.SetTrigger("AB" + (i + 1));
                continue;
            }
            
            // Length of current animation clip rounded up to the nearest second
            abilities[i].DisableButton();
        }
    }
    
    public AnimatorStateInfo GetAnimationState()
    {
        return animator.GetCurrentAnimatorStateInfo(0);
    }
    
}
