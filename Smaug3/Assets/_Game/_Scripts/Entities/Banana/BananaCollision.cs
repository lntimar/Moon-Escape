using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaCollision : MonoBehaviour
{
    // References
    private AudioManager _audioManager;

    // Components
    private Banana _banana;
    private BananaType _bananaType;
    private BananaMovement _bananaMovement;

    [Header("Destroyed Banana:")] 
    [SerializeField] private DestroyedBanana destroyedBananaPrefab;

    [Header("Collision Layers:")]
    [SerializeField] CollisionLayers collisionLayers;

    private void Start()
    {
        _banana = GetComponent<Banana>();
        _bananaType = GetComponent<BananaType>();
        _bananaMovement = GetComponent<BananaMovement>();

        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        Destroy(gameObject, _banana.LifeTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent<EnemyCollision>(out EnemyCollision enemyCol))
        {
            enemyCol.ChangeCurrentHealth(-_banana.Damage);
            var enemyBehaviour = col.gameObject.GetComponent<EnemyBehaviour>();

            if (enemyBehaviour.shooting == false || enemyBehaviour.chasing == false)
            {
                var sprRender = col.gameObject.GetComponent<SpriteRenderer>();
                sprRender.enabled = true;
                enemyBehaviour.chasing = true;
                sprRender.flipX = !sprRender.flipX;
            }

            if (_bananaType.GetBananaType() == BananaType.Types.Default || _bananaType.GetBananaType() == BananaType.Types.Bomb)
            {
                enemyCol.ApplyKnockback(_bananaMovement.GetDirection(), 1f);
            }

            enemyCol.ApplyEffect(_bananaType.GetBananaType());
        }
        else if (col.gameObject.TryGetComponent<BossCollision>(out BossCollision bossCol))
        {
            bossCol.ChangeCurrentHealth(-_banana.Damage);
            bossCol.ApplyEffect(_bananaType.GetBananaType());
        }


        if (col.gameObject.tag != "Enemy Bullet" && col.gameObject.tag != "Trigger") Destroy(gameObject);
    }

    private void OnDestroy()
    {
        switch (_bananaType.GetBananaType())
        {
            // Normal
            case BananaType.Types.Default:
                _audioManager.PlaySFX("barulho banana neutra");
                break;

            // Gelo
            case BananaType.Types.Ice:
                _audioManager.PlaySFX("barulho banana neutra");
                break;

            // Bomba
            case BananaType.Types.Bomb:
                _audioManager.PlaySFX("barulho banana bomba");
                break;

            // El�trica
            case BananaType.Types.Eletric:
                _audioManager.PlaySFX("barulho banana eletrica");
                break;
        }

        var destroyedBanana = Instantiate(destroyedBananaPrefab, transform.position, Quaternion.identity);
        destroyedBanana.SwitchAnimation(_bananaType.GetBananaType());

        destroyedBanana.gameObject.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;

        float bananaDirY = _bananaMovement.GetDirection().y;
        if (bananaDirY != 0)
        {
            destroyedBanana.transform.rotation = Quaternion.Euler(0f, 0f, 90f * Mathf.Sign(bananaDirY));
        }
    }
}
