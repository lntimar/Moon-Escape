using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    [SerializeField] private bool isMecha = false;
    [SerializeField] private bool canFollow = true;

    [SerializeField] private Vector3 offset;

    [Header("Min/Max Positions:")]
    [SerializeField] private float xMin;
    [SerializeField] private float xMax;
    [SerializeField] private float yMin;
    [SerializeField] private float yMax;

    // References
    private Transform _playerTransf;
    //private float _speed;

    private void Start()
    {
        if (isMecha) // Macaco com Armadura
        {
            var monkeyArmored = GameObject.FindObjectOfType<PlayerMovement>();
            //_speed = monkeyArmored.MoveSpeed; 
            _playerTransf = monkeyArmored.transform;
        }
        else // Macaco sem Armadura
        {
            var monkey = GameObject.FindObjectOfType<InitialPlayerMovement>();
            //_speed = monkey.MoveSpeed;
            _playerTransf = monkey.transform;
        }
    }

    /*
    private void Update()
    {
        if (canFollow)
        {
            //Pega a posição do Player. Vector Z no -10f é a posição padrão da câmera 2D
            //Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
            Vector3 newPos = new Vector3(_playerTransf.position.x, _playerTransf.position.y, -10f);

            //Velocidade que segue o Player
            transform.position = Vector3.Slerp(transform.position, newPos, _speed);
            //transform.position = Vector3.Slerp(transform.position, newPos, playerMov.moveSpeed*Time.deltaTime);
        }
    }
    */

    private void LateUpdate()
    {
        if (canFollow)
        {
            float xClamp = Mathf.Clamp(_playerTransf.position.x, xMin, xMax);
            float yClamp = Mathf.Clamp(_playerTransf.position.y, yMin, yMax);

            Vector3 targetpos = _playerTransf.transform.position + offset;
            Vector3 clampedpos = new Vector3(Mathf.Clamp(targetpos.x, xMin, xMax), Mathf.Clamp(targetpos.y, yMin, yMax), 0);

            SetNewPosition(clampedpos);
        }
    }

    public void SetNewPosition(Vector2 pos)
    {
        Vector3 newPos = (Vector3)pos + new Vector3(0, 0, -10f);
        transform.position = newPos;
    }
}
