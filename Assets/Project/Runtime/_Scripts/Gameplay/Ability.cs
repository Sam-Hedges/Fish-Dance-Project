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
    
    public void DisableButton()
    {
        pauseButton = true;

        StartCoroutine(SetButtonStatus(true));
    }
    
    public void EnableButton()
    {
        pauseButton = false;
    }
    
    private void Start()
    {
        StartCoroutine(Recharge());    
    }

    private IEnumerator SetButtonStatus(bool status)
    {
        bool ensureRunsOnce = true;
        
        while (ensureRunsOnce)
        {
            if (!playerManager.GetAnimationState().IsName("Breakdance Ready"))
            {
                do
                {
                    pauseButton = status;
                    
                    if (playerManager.GetAnimationState().IsName("Breakdance Ready"))
                    {
                        ensureRunsOnce = false;
                    }
                    
                    yield return new WaitForSeconds(1);

                } while (!playerManager.GetAnimationState().IsName("Breakdance Ready"));
                
            }
            yield return null;
        }

        pauseButton = !status;
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
            button.interactable = !pauseButton;
            
            yield return null;
        }
    }
}
