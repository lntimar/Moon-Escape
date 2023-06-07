using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shoot:")]
    [SerializeField] private float shootInterval;
    [SerializeField] private bool canShoot = true;

    [Header("Types:")] 
    [SerializeField] private int defaultBananaIndex;
    private static int currentBananaIndex;
    public int maxEnergy;
    [SerializeField] private int inspectorCurrentEnergy;
    public static int currentEnergy;
    [SerializeField] private BananaType[] bananas = new BananaType[4]; // Lista dos tipos de bananas que possuo no momento
    [SerializeField] private BananaType[] setBananas; 

    // References
    private PlayerEnergyBar _playerEnergyBar;
    private PlayerBananaIcon _playerBananaIcon;
    private AudioManager _audioManager;

    // Components
    private Animator _anim;
    private SpriteRenderer _spr;

    // Direção do Disparo
    private Vector2 _direction;
    private string _shootAnim;

    private void Awake() => currentEnergy = 0;

    public static ProgressBanana hasBananas = ProgressBanana.HasNothing;

    public enum ProgressBanana
    {
        HasNothing,
        HasIce,
        HasBomb,
        HasEletric
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _spr = GetComponent<SpriteRenderer>();
        _playerEnergyBar = GameObject.FindGameObjectWithTag("Energy Bar").GetComponent<PlayerEnergyBar>();
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        // Colocando os Valores Iniciais
        currentBananaIndex = defaultBananaIndex;

        if (currentEnergy == 0)
        {
            ChangeCurrentEnergy(maxEnergy);
        }

        SetBananas();

        _playerBananaIcon = GameObject.FindGameObjectWithTag("Banana Icon").GetComponent<PlayerBananaIcon>();
        
        if (_playerBananaIcon != null) _playerBananaIcon.SetCurrentIcon(currentBananaIndex);
    }

    private void Update()
    {
        // Ignore a detecção de input, caso estiver morto ou tomado dano
        if (PlayerStateMachine.StateManager.IsNotFine()) return; 

        ShootInput();
        ChangeInput();

        inspectorCurrentEnergy = currentEnergy;
    }

    // Disparo
    private void ShootInput()
    {
        if (canShoot && currentEnergy - bananas[currentBananaIndex].GetEnergyCost() >= 0)
        {
            if (Input.GetButton("Shoot Mouse"))
            {
                canShoot = false;
                _audioManager.PlaySFX("macaquinho caindo");
                SpawnBanana(bananas[currentBananaIndex].GetComponent<BananaMovement>());
                ChangeCurrentEnergy(-bananas[currentBananaIndex].GetEnergyCost());
                StartCoroutine(SetShootInterval(shootInterval));
                PlayShootAnimation();
            }
            else if (Input.GetButton("Shoot Key"))
            {
                canShoot = false;
                _audioManager.PlaySFX("macaquinho caindo");
                SpawnBanana(bananas[currentBananaIndex].GetComponent<BananaMovement>());
                ChangeCurrentEnergy(-bananas[currentBananaIndex].GetEnergyCost());
                StartCoroutine(SetShootInterval(shootInterval));
                PlayShootAnimation();
            }
        }

        /*
        if (Input.GetButtonUp("Shoot Mouse") || Input.GetButtonUp("Shoot Key"))
        {
            canShoot = true;
            StopAllCoroutines(); // Pare todas as coroutines SetShootInterval
        }
        */
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

    public void ChangeCurrentEnergy(int increment)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + increment, 0, maxEnergy);
        _playerEnergyBar.SetEnergyBar(currentEnergy);
    }

    public int GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public int GetMaxEnergy()
    {
        return maxEnergy;
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
            else
            {
                currentBananaIndex = defaultBananaIndex;
            }
        }
        else if (Input.GetButtonDown("Change Default Banana")) // Troca para a default
        {
            currentBananaIndex = defaultBananaIndex;
        }

        if (_playerBananaIcon != null) _playerBananaIcon.SetCurrentIcon(currentBananaIndex);
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

    private void SetBananas()
    {
        switch (hasBananas)
        {
            case ProgressBanana.HasIce:
                AddBananaType(setBananas[0]);
                break;

            case ProgressBanana.HasBomb:
                print("Test");
                AddBananaType(setBananas[0]);
                AddBananaType(setBananas[1]);
                break;

            case ProgressBanana.HasEletric:
                AddBananaType(setBananas[0]);
                AddBananaType(setBananas[1]);
                AddBananaType(setBananas[2]);
                break;
        }
    }

    public void ResetBananaIcon()
    {
        _playerBananaIcon = GameObject.FindGameObjectWithTag("Banana Icon").GetComponent<PlayerBananaIcon>();

       _playerBananaIcon.SetCurrentIcon(currentBananaIndex);
    }
}
