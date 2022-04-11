using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    float horizontal, vertical;
    PlayerMove playerMove;
    Combat playerCombat;
    void Start()
    {
        playerMove = this.GetComponent<PlayerMove>();
        playerCombat = this.GetComponent<Combat>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (Input.GetMouseButton(0)){
            UseMainHand();
        }
        if (Input.GetMouseButton(1)){
            UseOffHand();
        }
    }

    void FixedUpdate()
    {
        playerMove.Move(horizontal, vertical);
    }

    private void UseMainHand() {
        playerCombat.UseMainHand();
    }

    private void UseOffHand() {
        playerCombat.UseOffHand();
    }
}
