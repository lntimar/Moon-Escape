using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFinalBoss : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D[] hitboxes;

    public void EnableHitbox(int index)
    {
        for (int i = 0; i < hitboxes.Length; i++)
        {
            if (i == index) hitboxes[i].enabled = true;
            else hitboxes[i].enabled = false;
        }
    }

    public void DisableLaser()
    {
        gameObject.SetActive(false);
    }
}
