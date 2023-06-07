using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGorila : MonoBehaviour
{
    // References
    private AudioManager _audioManager;

    private bool _canPlayGrito = true;

    private void Start()
    {
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void BaterNoChao()
    {
        _audioManager.PlaySFX("gorila batendo no chao");
    }

    public void Grito()
    {
        if (_canPlayGrito)
        {
            _audioManager.PlaySFX("inicio boss fight");
            _canPlayGrito = false;
        }
    }

    public void ResetGrito()
    {
        _canPlayGrito = true;
    }

    private void OnDestroy()
    {
        _audioManager.PlaySFX("colecionaveis");
    }
}
