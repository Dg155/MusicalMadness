using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    float horizontal, vertical;
    PlayerMove playerMove;
    Combat playerCombat;
    Animator animator;
    void Start()
    {
        animator = this.GetComponent<Animator>();
        playerMove = this.GetComponent<PlayerMove>();
        playerCombat = this.GetComponent<Combat>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0){
            animator.SetFloat("Speed", 1);
        }
        else{
            animator.SetFloat("Speed", 0);
        }
        if (Input.GetMouseButton(0)){
            UseMainHand(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if (Input.GetMouseButton(1)){
            //UseOffHand(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            UseMainHandSecondary(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    void FixedUpdate()
    {
        playerMove.Move(horizontal, vertical);
    }

    private void UseMainHand(Vector3 mousePos) {
        playerCombat.UseMainHand(mousePos);
    }

    private void UseMainHandSecondary(Vector3 mousePos) {
        playerCombat.UseMainHandSecondary(mousePos);
    }

    private void UseOffHand(Vector3 mousePos) {
        playerCombat.UseOffHand(mousePos);
    }
}

