using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shoot:")]
    [SerializeField] private float shootInterval;
    [SerializeField, ReadOnly] private bool canShoot = true;

    [Header("Types:")] 
    [SerializeField] private int defaultBananaIndex;
    [SerializeField] private int currentBananaIndex;
    [SerializeField] private int maxEnergy; // Provavelmente vai ser static
    [SerializeField] private int currentEnergy; // Provavelmente vai ser static
    [SerializeField] private BananaType[] bananas = new BananaType[4]; // Lista dos tipos de bananas que possuo no momento

    // Components
    private Animator _anim;
    private SpriteRenderer _spr;

    // Direção do Disparo
    private Vector2 _direction;
    private string _shootAnim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _spr = GetComponent<SpriteRenderer>();

        // Colocando os Valores Iniciais
        currentBananaIndex = defaultBananaIndex;
        ChangeCurrentEnergy(maxEnergy);
    }

    private void Update()
    {
        // Ignore a detecção de input, caso estiver morto ou tomado dano
        if (PlayerStateMachine.StateManager.IsNotFine()) return; 

        ShootInput();
        ChangeInput();
    }

    // Disparo
    private void ShootInput()
    {
        if (canShoot && Input.GetButton("Shoot") && currentEnergy - bananas[currentBananaIndex].GetEnergyCost() >= 0)
        {
            canShoot = false;
            SpawnBanana(bananas[currentBananaIndex].GetComponent<BananaMovement>());
            ChangeCurrentEnergy(bananas[currentBananaIndex].GetEnergyCost());
            StartCoroutine(SetShootInterval(shootInterval));
            PlayShootAnimation();
        }

        
        if (Input.GetButtonUp("Shoot"))
        {
            canShoot = true;
            StopAllCoroutines(); // Pare todas as coroutines SetShootInterval
        }
    }

    private IEnumerator SetShootInterval(float time)
    {
        yield return new WaitForSeconds(time);
        canShoot = true;
    }

    public void SetDirection(Vector2 dir)
    {
        _direction = dir;
    }

    public bool GetCanShoot()
    {
        return canShoot;
    }

    // Banana
    private void SpawnBanana(BananaMovement b)
    {
        //var rotationZ = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        var offset = Vector2.zero;

        switch (_shootAnim)
        {
            case "Diagonal Up":
                if (_spr.flipX) offset = new Vector2(-1.22f, 0.45f);
                else offset = new Vector2(1.22f, 0.45f);
                break;

            case "Diagonal Down":
                if (_spr.flipX) offset = new Vector2(-1.22f, -0.45f);
                else offset = new Vector2(1.22f, -0.45f);
                break;

            case "Vertical Up":
                if (_spr.flipX) offset = new Vector2(-0.8f, 0.45f);
                else offset = new Vector2(0.8f, 0.45f);
                break;

            case "Vertical Down":
                if (_spr.flipX) offset = new Vector2(-0.8f, -0.45f);
                else offset = new Vector2(0.8f, -0.45f);
                break;

            default:
                offset = new Vector2(1f, 2f) * _direction;
                break;
        }

        var banana = Instantiate(b, transform.position + (Vector3) offset, Quaternion.identity);
        banana.SetDirection(_direction);
    }

    // Energia
    public void ChangeMaxEnergy(int increment)
    {
        maxEnergy += increment;
    }

    public void ChangeCurrentEnergy(int increment)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + increment, 0, maxEnergy);
    }

    // Troca de tipo
    private void ChangeInput()
    {
        if (Input.GetButtonDown("Change Banana")) // Troca para a seguinte
        {
            if (currentBananaIndex + 1 >= bananas.Length)
            {
                currentBananaIndex = defaultBananaIndex;
            }
            else if (bananas[currentBananaIndex + 1] != null)
            {
                currentBananaIndex++;
            }
        }
        else if (Input.GetButtonDown("Change Default Banana")) // Troca para a default
        {
            currentBananaIndex = defaultBananaIndex;
        }
    }

    public void AddBananaType(BananaType b)
    {
        for (int i = 0; i < bananas.Length; i++)
        {
            if (bananas[i] == null)
            {
                bananas[i] = b;
                break;
            }
        }
    }

    public void SetShootAnim(string dirName)
    {
        _shootAnim = dirName;
    }

    private void PlayShootAnimation()
    {
        switch (_shootAnim)
        {
            case "Diagonal Up":
                _anim.SetTrigger("shootDiagonalUp");
                break;

            case "Diagonal Down":
                _anim.SetTrigger("shootDiagonalDown");
                break;

            case "Vertical Up":
                _anim.SetTrigger("shootVerticalUp");
                break;

            case "Vertical Down":
                _anim.SetTrigger("shootVerticalDown");
                break;
                
            default:
                _anim.SetTrigger("shootHorizontal");
                break;
        }
    }
}
