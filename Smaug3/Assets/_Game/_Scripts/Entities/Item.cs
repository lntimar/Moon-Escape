using System.Collections;
using System.Collections.Generic;
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

    // Vertical Direction
    private float curVerticalDir = 1f;

    private void Start()
    {
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
            if (col.gameObject.TryGetComponent<PlayerShoot>(out PlayerShoot playerShoot))
                playerShoot.ChangeCurrentEnergy(Energy);

            if (col.gameObject.TryGetComponent<PlayerCollision>(out PlayerCollision playerCol))
                playerCol.ChangeCurrentHealth(Life);
            else
                col.gameObject.GetComponent<InitialPlayerCollision>().ChangeCurrentHealth(Life);

            Destroy(gameObject);
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
