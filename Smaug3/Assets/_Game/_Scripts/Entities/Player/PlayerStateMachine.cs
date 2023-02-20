using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    // Referência global da state machine do Player
    public static PlayerStateMachine StateManager;

    [SerializeField, ReadOnly] private PlayerStates currentState = PlayerStates.Playing;

    // Possibilidades de States
    public enum PlayerStates
    {
        NotPlaying,
        Playing,
        Moving,
        Damaged
    };

    private void Awake()
    {
        StateManager = this;
    }

    public PlayerStates GetState() 
    {
        return StateManager.currentState;
    }

    public void SetState(PlayerStates state)
    {
        StateManager.currentState = state;
    }

    public bool CompareState(PlayerStates state) 
    {
        return StateManager.currentState == state;
    }
}
