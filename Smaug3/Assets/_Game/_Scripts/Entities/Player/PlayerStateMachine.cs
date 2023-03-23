using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    // Referência global da state machine do Player
    public static PlayerStateMachine StateManager;

    [SerializeField] private PlayerStates defaultState;
    [SerializeField] private PlayerStates currentState;

    // Possibilidades de States
    public enum PlayerStates
    {
        Dead,
        Idle,
        Moving,
        Damaged
    };

    private void Awake()
    {
        StateManager = this;
        StateManager.SetState(defaultState);
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

    public bool IsNotFine()
    {
        // Verifico se o player está morto ou tomou dano
        var isDeadorDamaged = PlayerStateMachine.StateManager.CompareState(PlayerStateMachine.PlayerStates.Dead) || PlayerStateMachine.StateManager.CompareState(PlayerStateMachine.PlayerStates.Damaged);
        // Retorno true ou false com base na verificação anterior
        return isDeadorDamaged;
    }
}
