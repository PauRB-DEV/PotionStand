using System;
using UnityEngine;

[Serializable]
public class CustomerOrder
{
    public ClientTypes clientType;
    public PotionTypes potionType;

    public float totalPatience;
    public float currentPatience;
}
