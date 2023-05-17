using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedBanana : MonoBehaviour
{
    public void SwitchAnimation(BananaType.Types type)
    {
        var anim = GetComponent<Animator>();
        switch (type)
        {
            case BananaType.Types.Default:
                anim.Play("Destroy Banana Default Animation");
                break;

            case BananaType.Types.Ice:
                anim.Play("Destroy Banana Ice Animation");
                break;

            case BananaType.Types.Bomb:
                anim.Play("Destroy Banana Bomb Animation");
                break;

            case BananaType.Types.Eletric:
                anim.Play("Destroy Banana Eletric Animation");
                break;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
