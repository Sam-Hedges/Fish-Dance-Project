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
                animator.SetTrigger("AB" + (i + 1));
            }
        }
    }

}
