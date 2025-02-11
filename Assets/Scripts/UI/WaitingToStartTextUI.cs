using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitingToStartUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waitingToStartText;

    private void Start() {
        Show();
    }

    private void Update()
    {
        if(KitchenGameManager.Instance.waitingToStartText()){
            Show();
        }else{
            Hide();
        }
    }

    private void Show(){
        gameObject.SetActive(true);
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

}
