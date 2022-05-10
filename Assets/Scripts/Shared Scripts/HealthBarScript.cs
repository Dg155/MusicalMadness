using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    private Transform bar;
    void Start()
    {
        bar = transform.Find("Bar");
    }

    public void setHealthBar(float normalizedSize)
    {
        bar.localScale = new Vector3(normalizedSize, 1f);
    }
}
