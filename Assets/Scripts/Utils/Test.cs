﻿using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Trigger();
    }

    private void Trigger()
    {
        Attribute damage = new Attribute(AttributeType.DAMAGE, 10);
        Ability ability = new DestroyShieldAbility(2, transform);
        ability.Trigger();
    }
}