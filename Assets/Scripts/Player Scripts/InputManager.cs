using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class InputManager : MonoBehaviour
{
    float horizontal, vertical;
    PlayerMove playerMove;
    Combat playerCombat;
    Animator animator;
    bool primaryBuffered;
    bool secondaryBuffered;
    public float bufferTime;

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
            StartCoroutine(BufferPrimary());
        }
        if (Input.GetMouseButton(1)){
            //UseOffHand(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            StartCoroutine(BufferSecondary());
        }
        if (Math.Abs(Input.mouseScrollDelta.y) * .05f > 0)
        {
            SwitchMainHand();
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt)){
            print("Switching scenes!");
            if (Input.GetKey(KeyCode.A)) {
                SceneManager.LoadScene("Ambush_Showcase");
            }
            if (Input.GetKey(KeyCode.B)) {
                SceneManager.LoadScene("GrandPianoBoss");
            }
            if (Input.GetKey(KeyCode.T)) {
                SceneManager.LoadScene("Tutorial");
            }
            if (Input.GetKey(KeyCode.Alpha1)) {
                SceneManager.LoadScene("Level 1");
            }
            if (Input.GetKey(KeyCode.Alpha2)) {
                SceneManager.LoadScene("Level 2");
            }
            if (Input.GetKey(KeyCode.Alpha3)) {
                SceneManager.LoadScene("Level 3");
            }

        }
        UseMainHand(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        UseMainHandSecondary(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    void FixedUpdate()
    {
        playerMove.Move(horizontal, vertical);
    }

    private IEnumerator BufferPrimary()
    {
        //Debug.Log("primary buffered!");
        if (!primaryBuffered)
        {
            primaryBuffered = true;
            yield return new WaitForSeconds(bufferTime);
            primaryBuffered = false;
        }
        //Debug.Log("primary no longer buffered");
    }

    private IEnumerator BufferSecondary()
    {
        //Debug.Log("secondary buffered!");
        if (!secondaryBuffered)
        {
            secondaryBuffered = true;
            yield return new WaitForSeconds(bufferTime);
            secondaryBuffered = false;
        }
        //Debug.Log("secondary no longer buffered");
    }

    private void UseMainHand(Vector3 mousePos) {
        if (primaryBuffered){
            playerCombat.UseMainHand(mousePos);
        }
    }

    private void UseMainHandSecondary(Vector3 mousePos) {
        if (secondaryBuffered){
            playerCombat.UseMainHandSecondary(mousePos);
        }
    }

    private void UseOffHand(Vector3 mousePos) {
        playerCombat.UseOffHand(mousePos);
    }

    private void SwitchMainHand()
    {
        if (Inventory.instance.weaponItems.Count > 0)
        {
            Inventory.instance.SetMainHand(Inventory.instance.weaponItems[0]);
            playerCombat.setMainHand();
        }
    }
}

