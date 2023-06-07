using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Bonus Values:")]
    public int Energy;
    public int Life;

    [Header("Floating Movement:")]
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float changeDirTime;

    [Header("Collision Layers:")] 
    [SerializeField] private CollisionLayers collisionLayers;

    // References
    private AudioManager _audioManager;

    // Vertical Direction
    private float curVerticalDir = 1f;

    private void Start()
    {
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        StartCoroutine(ChangeVerticalDirection(changeDirTime));
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.up * curVerticalDir * verticalSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == collisionLayers.PlayerLayer)
        {
            // Energia
            if (Life == 0)
            {
                if (col.gameObject.TryGetComponent<PlayerShoot>(out PlayerShoot playerShoot))
                {
                    if (playerShoot.GetCurrentEnergy() != playerShoot.GetMaxEnergy())
                    {
                        playerShoot.ChangeCurrentEnergy(Energy);
                        _audioManager.PlaySFX("vida energia");
                        Destroy(gameObject);
                    }
                }
            }
            else // Vida
            {
                // Mecha
                if (col.gameObject.TryGetComponent<PlayerCollision>(out PlayerCollision playerCol))
                {
                    if (playerCol.GetCurrentHealth() != playerCol.GetMaxHealth())
                    {
                        playerCol.ChangeCurrentHealth(Life);
                        _audioManager.PlaySFX("vida energia");
                        Destroy(gameObject);
                    }
                }
                else // Macaquinho
                {
                    var initialPlayerCol = col.gameObject.GetComponent<InitialPlayerCollision>();
                    if (initialPlayerCol.GetCurrentHealth() != initialPlayerCol.GetMaxHealth())
                    {
                        initialPlayerCol.ChangeCurrentHealth(Life);
                        _audioManager.PlaySFX("vida energia");
                        Destroy(gameObject);
                    }
                }
            }
        }

        if (col.gameObject.layer == collisionLayers.GroundLayer)
            GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private IEnumerator ChangeVerticalDirection(float t)
    {
        yield return new WaitForSeconds(t);
        curVerticalDir *= -1f;

        StartCoroutine(ChangeVerticalDirection(changeDirTime));
    }
}
