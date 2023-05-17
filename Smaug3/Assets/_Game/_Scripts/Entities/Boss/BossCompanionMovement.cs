using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCompanionMovement : MonoBehaviour
{
    [Header("Movement:")] 
    [SerializeField] 
    private float orbitSpeed;

    private void Update()
    {
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
        transform.RotateAround(transform.parent.position, Vector3.forward, orbitSpeed * Time.deltaTime);
    }
}
