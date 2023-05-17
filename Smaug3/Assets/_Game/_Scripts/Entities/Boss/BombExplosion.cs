using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    [SerializeField] private CollisionLayers collisionLayers;

    // Components
    private DropItem _dropItem;

    // DropItem
    private bool _colPlayer = false;

    private void Start()
    {
        _dropItem = GetComponent<DropItem>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.PlayerLayer) _colPlayer = true;
    }

    public void SelfDestroy()
    {
        if (!_colPlayer) _dropItem.SpawnBonus(true);
        Destroy(gameObject);
    }
}
