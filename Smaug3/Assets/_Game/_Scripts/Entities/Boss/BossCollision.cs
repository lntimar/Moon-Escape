using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollision : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField] private GameObject[] effects;

    [Header("Collision Layers:")]
    [SerializeField] CollisionLayers collisionLayers;

    // Components
    private Boss _boss;
    private BlinkSpriteVFX _blink;
    private Animator _anim;

    private void Start()
    {
        _boss = GetComponent<Boss>();
        _blink = GetComponent<BlinkSpriteVFX>();
        _anim = GetComponent<Animator>();

        ChangeCurrentHealth(_boss.MaxHealth);
    }

    public void ChangeCurrentHealth(int points)
    {
        currentHealth = Mathf.Clamp(currentHealth + points, 0, _boss.MaxHealth);

        if (currentHealth == 0)
        {
            // Boss Morre
            Destroy(gameObject);
            // TODO: Dar play na animação de morte do Boss
        }
    }

    public void ApplyEffect(BananaType.Types effect)
    {
        switch (effect)
        {
            case BananaType.Types.Ice:
                effects[0].GetComponent<BossIceEffect>().Freeze();
                break;

            case BananaType.Types.Bomb:
                effects[1].GetComponent<BossBombEffect>().Explode();
                break;

            case BananaType.Types.Eletric:
                effects[2].GetComponent<BossEletricEffect>().Eletrify();
                break;
        }

        _blink.SetBlink();
    }
}
