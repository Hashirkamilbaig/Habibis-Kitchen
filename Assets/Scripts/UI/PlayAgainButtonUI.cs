using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayAgainButtonUI : MonoBehaviour
{
    [SerializeField] private Button playAgainButton;

    private void Awake() {
        playAgainButton.onClick.AddListener(()=>{
            SceneManager.LoadScene(0);
        });
    }
}
