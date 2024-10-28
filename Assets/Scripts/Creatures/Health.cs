using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHP;

    public float HP => hp;
    float hp;

    private void Awake() 
    {
        hp = maxHP;
    }

    public void Damage(float amount)
    {
        hp -= maxHP;
    }
}
