using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{

    public static KitchenGameManager Instance {get; private set;}

    public event EventHandler OnStateChanged;
    private enum State{
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        Gameover,
    }

    private State state;
    private float waitingToStartTimer = 5f;
    private float countDownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 90f;
    

    private void Awake() {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Update() {
        switch (state){
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if(waitingToStartTimer < 0f){
                    state = State.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                countDownToStartTimer -= Time.deltaTime;
                if(countDownToStartTimer < 0f){
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if(gamePlayingTimer < 0f){
                    state = State.Gameover;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Gameover:
                break;
        }

        Debug.Log(state);
    }

    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsCountDownToStartActive(){
        return state == State.CountdownToStart;
    }

    public bool waitingToStartText(){
        return state == State.WaitingToStart;
    }

    public float GetCountDownToStartTimer(){
        return countDownToStartTimer;
    }

    public bool IsGameOver(){
        return state == State.Gameover;
    }

    public float GetGamePlayingTimerNormalized(){
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }
}
