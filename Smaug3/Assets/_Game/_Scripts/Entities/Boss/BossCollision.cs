using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BossCollision : MonoBehaviour
{
    [Header("Life:")]
    [SerializeField] private int currentHealth;
    
    [Header("Effects:")]
    [SerializeField] private GameObject[] effects;
    
    [Header("End Boss Fight Timer:")]
    [SerializeField] private float endBossFightTime;

    [Header("Collision Layers:")]
    [SerializeField] CollisionLayers collisionLayers;

    [Header("Player Banana Icon:")]
    [SerializeField] private GameObject nextBanana;
    [SerializeField] private GameObject curBanana;

    // References
    private RectTransform _lifebarTransform;
    private GameObject _player;

    // Components
    private Boss _boss;
    private BlinkSpriteVFX _blink;
    private Animator _anim;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _hitbox;
    
    public MonoBehaviour BehaviourScript;

    private void Start()
    {
        _boss = GetComponent<Boss>();
        _blink = GetComponent<BlinkSpriteVFX>();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _hitbox = GetComponent<CapsuleCollider2D>();
        _lifebarTransform = GameObject.FindGameObjectWithTag("BossLifeBar").GetComponent<RectTransform>();
        _player = GameObject.FindGameObjectWithTag("Player");

        GetBehaviour();

        ChangeCurrentHealth(_boss.MaxHealth);
    }

    public void ChangeCurrentHealth(int points)
    {
        currentHealth = Mathf.Clamp(currentHealth + points, 0, _boss.MaxHealth);
        _lifebarTransform.sizeDelta = new Vector2(currentHealth * 750f / _boss.MaxHealth, _lifebarTransform.sizeDelta.y);

        if (currentHealth == 0)
        {
            // Boss Morre
            _anim.Play("Death Animation");

            if (curBanana != null)
            {
                curBanana.SetActive(false);
                nextBanana.SetActive(true);

                _player.GetComponent<PlayerShoot>().ResetBananaIcon();
            }

            if (_boss.Name == "Gorila Eletric")
            {
                GetComponent<GorilaEletricBehaviour>().DisableEletricTraps();
            }

            BehaviourScript.enabled = false;
            _rb.isKinematic = true;
            Destroy(_hitbox);

            StartCoroutine(EndBossFight(endBossFightTime));
            Destroy(_lifebarTransform.gameObject.transform.parent.gameObject);
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
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
                _blink.SetBlink();
                break;

            case BananaType.Types.Eletric:
                effects[2].GetComponent<BossEletricEffect>().Eletrify();
                break;

            default:
                _blink.SetBlink();
                break;
        }
    }

    private IEnumerator EndBossFight(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Drop o Item da BossFight
        DropItem();
    }

    private void DropItem()
    {
        switch (_boss.Name)
        {
            case "MiniBoss":
                // Dash
                PlayerMovement.HasDash = true;
                break;

            case "Gorila Ice":
                // Banana de Gelo
                PlayerShoot.hasBananas = PlayerShoot.ProgressBanana.HasIce;
                _player.GetComponent<PlayerShoot>().AddBananaType(_boss.BananaType);
                break;

            case "Gorila Bomb":
                // Banana Explosiva
                PlayerShoot.hasBananas = PlayerShoot.ProgressBanana.HasBomb;
                _player.GetComponent<PlayerShoot>().AddBananaType(_boss.BananaType);
                break;

            case "Gorila Eletric":
                // Banana Elétrica
                PlayerShoot.hasBananas = PlayerShoot.ProgressBanana.HasEletric;
                _player.GetComponent<PlayerShoot>().AddBananaType(_boss.BananaType);
                break;
        }
    }

    private void GetBehaviour()
    {
        switch (_boss.Name)
        {
            case "MiniBoss":
                BehaviourScript = GetComponent<JetpackAlienBehaviour>();
                break;
            
            case "Gorila Ice":
                BehaviourScript = GetComponent<GorilaIceBehaviour>();
                break;

            case "Gorila Bomb":
                BehaviourScript = GetComponent<GorilaBombBehaviour>();
                break;

            case "Gorila Eletric":
                BehaviourScript = GetComponent<GorilaEletricBehaviour>();
                break;

            case "FinalBoss":
                BehaviourScript = GetComponent<FinalBossBehaviour>();
                break;
        }
    }
}
