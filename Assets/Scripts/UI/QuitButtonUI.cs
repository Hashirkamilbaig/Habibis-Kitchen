using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButtonUI : MonoBehaviour
{
    [SerializeField] private Button QuitButton;

    private void Awake() {
            QuitButton.onClick.AddListener(()=>{
            Application.Quit();
        });
    }
}
