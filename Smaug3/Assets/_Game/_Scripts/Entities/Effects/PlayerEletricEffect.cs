using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEletricEffect : MonoBehaviour
{
    [Header("Settings:")] 
    [SerializeField] private float eletrifyTime;

    [Header("Player References:")]
    [SerializeField] private Animator playerAnim;

    // Components
    private SpriteRenderer _spr;
    private Animator _anim;

    // Stop Animator
    private float _playerAnimDefaultSpeed;

    // Activate Eletrify
    private bool _eletrify = false;

    private void Start()
    {
        _spr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();

        _playerAnimDefaultSpeed = playerAnim.speed;
    }

    private void Update()
    {
        if (_eletrify) Eletrify();
    }

    public void ActivateEletrify(bool hasInterval=true)
    {
        // Comece o efeito
        _eletrify = true;

        // Inicie o intervalo
        if (hasInterval) StartCoroutine(EletrifyInterval(eletrifyTime));
    }

    private void Eletrify()
    {
        playerAnim.speed = 0f;
        _spr.enabled = true;
        PlayerStateMachine.StateManager.SetState(PlayerStateMachine.PlayerStates.Damaged);
        _anim.SetBool("eletrify", true);
    }

    private IEnumerator EletrifyInterval(float t)
    {
        yield return new WaitForSeconds(t);
        StopEletrification();
    }

    public void StopEletrification()
    {
        _eletrify = false;
        PlayerStateMachine.StateManager.SetState(PlayerStateMachine.PlayerStates.Idle);
        playerAnim.speed = _playerAnimDefaultSpeed;
        _spr.enabled = false;
        _anim.SetBool("eletrify", false);
    }
}
