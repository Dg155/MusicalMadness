using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownBarScript : MonoBehaviour
{
    private Transform bar;
    void Start()
    {
        bar = transform.Find("Bar");
    }

    public void setCooldownBar(float normalizedSize)
    {
        if (bar == null) { bar = transform.Find("Bar"); }
        bar.localScale = new Vector3(normalizedSize, 1f);
    }
}
