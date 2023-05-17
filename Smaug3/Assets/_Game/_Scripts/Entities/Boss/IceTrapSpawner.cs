using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTrapSpawner : MonoBehaviour
{
    [SerializeField] private IceTrap iceTrapPrefab;
    [SerializeField] private Transform spawnIceTrapPosition;

    [HideInInspector] public bool CanDrop = true;

    // Components
    private Animator _anim;
    

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void DropIceTrap()
    {
        if (CanDrop)
        {
            Instantiate(iceTrapPrefab, spawnIceTrapPosition.position, Quaternion.identity);
            _anim.Play("IceTrapSpawner Wait Animation");
        }
    }

    // Chame no último frame da animação de espera
    public void ResetCanDrop()
    {
        CanDrop = true;
    }
}
