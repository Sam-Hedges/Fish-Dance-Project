using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LiquidBar), (typeof(Button)))]
public class Ability : MonoBehaviour
{

    private LiquidBar barUI;
    private Button button;
    [SerializeField] private int rechargeTimeInSeconds = 60;
    [SerializeField] private bool pauseButton = false;
    [SerializeField] PlayerManager playerManager;
    
    private void Awake() 
    {
        barUI = GetComponent<LiquidBar>();
        button = GetComponent<Button>();
    }

    public void StartAbility() 
    {
        playerManager.ExecuteAbility(this);
    }
    
    private void Start()
    {
        Coroutine recharge = StartCoroutine(Recharge());    
    }

    private IEnumerator Recharge() {
        while (true) 
        {
            while (barUI.targetFillAmount < 1) 
            {
                button.interactable = false;
                barUI.targetFillAmount += 1f / rechargeTimeInSeconds;
                yield return new WaitForSeconds(1);
            }

            barUI.targetFillAmount = 1;
            button.interactable = true;
            
            yield return null;
        }
    }
}
