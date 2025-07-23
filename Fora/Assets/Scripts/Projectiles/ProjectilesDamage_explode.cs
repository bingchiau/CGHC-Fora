using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesDamage_explode : ProjectilesDamage
{
    public override void DestroyThis()
    {
        //...
    }

    public void DestoyBullet()
    {
        Destroy(transform.parent.gameObject);
    }
}
