using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class InitialPlayerStateMachine : MonoBehaviour
{
    // Referência global da state machine do Player Inicial
    public static InitialPlayerStateMachine StateManager;

    [SerializeField] private InitialPlayerStates defaultState;
    [SerializeField] private InitialPlayerStates currentState;

    // Possibilidades de States
    public enum InitialPlayerStates
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

    public InitialPlayerStates GetState()
    {
        return StateManager.currentState;
    }

    public void SetState(InitialPlayerStates state)
    {
        StateManager.currentState = state;
    }

    public bool CompareState(InitialPlayerStates state)
    {
        return StateManager.currentState == state;
    }

    public bool IsNotFine()
    {
        // Verifico se o player inicial está morto ou tomou dano
        var isDeadorDamaged = InitialPlayerStateMachine.StateManager.CompareState(InitialPlayerStateMachine.InitialPlayerStates.Dead) || InitialPlayerStateMachine.StateManager.CompareState(InitialPlayerStateMachine.InitialPlayerStates.Damaged);
        // Retorno true ou false com base na verificação anterior
        return isDeadorDamaged;
    }
}
