using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEletricEffect : MonoBehaviour
{
    [Header("Boss References:")] 
    [SerializeField] private Boss bossScript;
    [SerializeField] private BossCollision bossColScript;
    [SerializeField] private Rigidbody2D bossRb;
    [SerializeField] private Animator bossAnim;

    [Header("Eletric Banana:")]
    [SerializeField] private Banana eletricBanana;

    [Header("Collision Layers:")]
    [SerializeField] private CollisionLayers collisionLayers;

    [Header("Stop Eletrification:")]
    [SerializeField] private float eletrificationTime;
    [SerializeField] private float cooldownTime;

    // References
    private MonoBehaviour _bossBehaviourScript;

    // Components
    private SpriteRenderer _spr;
    private Animator _anim;

    // Stop Animator
    private float _bossAnimDefaultSpeed;

    private void Start()
    {
        _spr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();

        _bossAnimDefaultSpeed = bossAnim.speed;
    }

    public void Eletrify()
    {
        _bossBehaviourScript = transform.parent.GetComponent<BossCollision>().BehaviourScript;
        bossColScript.gameObject.layer = collisionLayers.IgnoreEletricLayer;
        _bossBehaviourScript.enabled = false;
        bossAnim.speed = 0f;
        bossRb.velocity = Vector2.zero;

        // Se for o finalBoss, eletrifique seus ajudantes
        if (bossScript.Name == "FinalBoss")
        {
            var companions = transform.parent.gameObject.GetComponent<FinalBossBehaviour>().Companions;
            for (int i = 0; i < companions.Length; i++)
            {
                companions[i].GetComponent<BossCompanionMovement>().enabled = false;
                companions[i].GetComponent<BossCompanionShoot>().enabled = false;
                companions[i].transform.Find("Eletric Effect").GetComponent<SpriteRenderer>().enabled = true;
                companions[i].transform.Find("Eletric Effect").GetComponent<Animator>().SetBool("eletrify", true);
            }
        }

        _spr.enabled = true;

        _anim.SetBool("eletrify", true);

        StartCoroutine(StopEletrification(eletrificationTime));
    }

    private IEnumerator StopEletrification(float t)
    {
        yield return new WaitForSeconds(t);
        _spr.enabled = false;
        _anim.SetBool("eletrify", false);
        _bossBehaviourScript.enabled = true;
        bossAnim.speed = _bossAnimDefaultSpeed;

        if (bossScript.Name == "FinalBoss")
        {
            transform.parent.gameObject.GetComponent<FinalBossBehaviour>().EnableBoss();

            // Pare de eletrocutar os companions
            var companions = transform.parent.gameObject.GetComponent<FinalBossBehaviour>().Companions;
            for (int i = 0; i < companions.Length; i++)
            {
                companions[i].GetComponent<BossCompanionMovement>().enabled = true;
                companions[i].GetComponent<BossCompanionShoot>().enabled = true;
                companions[i].GetComponent<BossCompanionShoot>().StartSpawnOrbInterval();
                companions[i].transform.Find("Eletric Effect").GetComponent<SpriteRenderer>().enabled = false;
                companions[i].transform.Find("Eletric Effect").GetComponent<Animator>().SetBool("eletrify", false);
            }
        }

        StartCoroutine(SetCooldown(cooldownTime));
    }

    private IEnumerator SetCooldown(float t)
    {
        yield return new WaitForSeconds(t);
        bossColScript.gameObject.layer = collisionLayers.BossLayer;
    }
}
