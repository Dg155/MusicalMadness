using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    float horizontal, vertical;
    PlayerMove playerMove;
    void Start()
    {
        playerMove = this.GetComponent<PlayerMove>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        playerMove.Move(horizontal, vertical);
    }
}
