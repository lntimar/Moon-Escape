using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTrapSpawner : MonoBehaviour
{
    [SerializeField] private IceTrap iceTrapPrefab;
    [SerializeField] private Transform spawnIceTrapPosition;

    [HideInInspector] public bool CanDrop = true;

    // References
    private AudioManager _audioManager;

    // Components
    private Animator _anim;
    

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void DropIceTrap()
    {
        if (CanDrop)
        {
            Instantiate(iceTrapPrefab, spawnIceTrapPosition.position, Quaternion.identity);
            _audioManager.PlaySFX("estalactite desgrudando");
            _anim.Play("IceTrapSpawner Wait Animation");
        }
    }

    // Chame no último frame da animação de espera
    public void ResetCanDrop()
    {
        CanDrop = true;
    }
}
