using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaCollision : MonoBehaviour
{
    // Components
    private Banana _banana;
    private BananaType _bananaType;
    private BananaMovement _bananaMovement;

    [Header("Collision Layers:")]
    [SerializeField] CollisionLayers collisionLayers;

    private void Start()
    {
        _banana = GetComponent<Banana>();
        _bananaType = GetComponent<BananaType>();
        _bananaMovement = GetComponent<BananaMovement>();

        Destroy(gameObject, _banana.LifeTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent<EnemyCollision>(out EnemyCollision enemyCol))
        {
            enemyCol.ChangeCurrentHealth(-_banana.Damage);
            enemyCol.ApplyEffect(_bananaType.GetType());

            if (_bananaType.GetType() == BananaType.Types.Default || _bananaType.GetType() == BananaType.Types.Bomb)
            {
                enemyCol.ApplyKnockback(_bananaMovement.GetDirection());
            }
        }
        else if (col.gameObject.TryGetComponent<BossCollision>(out BossCollision bossCol))
        {
            bossCol.ChangeCurrentHealth(-_banana.Damage);
            bossCol.ApplyEffect(_bananaType.GetType());
        }
        Destroy(gameObject);
    }
}
