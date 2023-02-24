using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaCollision : MonoBehaviour
{
    // Components
    private Banana _banana;
    private BananaType _bananaType;

    private void Start()
    {
        _banana = GetComponent<Banana>();
        _bananaType = GetComponent<BananaType>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // TODO: Colisão com inimigos, obstáculos, puzzles, etc...
        Destroy(gameObject);
    }
}
