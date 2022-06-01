using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntVisAudFX : MonoBehaviour
{
    //particle game object may have sounds attached
    public GameObject deathParticle;
    public GameObject collisionParticle;
    public AudioClip collisionSound;
    public AudioClip deathSound;

    public SpriteRenderer sprite;
    public Material flashWhite;
    public Color flashColor;
    private Color actualColor;
    private Material actualMaterial;
    bool flashing = false;

    public void Awake(){
        sprite = this.GetComponent<SpriteRenderer>();
        actualMaterial = sprite.material;
        actualColor = sprite.color;
        if (flashColor.a == 0){
            flashColor = Color.white;
        }
    }

    public void DeathEffect(){
        if (deathParticle == null){
            return;
        }
        instantiateEffect(deathParticle, this.transform.position);
        FindObjectOfType<SoundEffectPlayer>().PlaySound(deathSound);
    }

    public void CollisionEffect(Vector3 pos){
        if (collisionParticle == null){
            return;
        }
        instantiateEffect(collisionParticle, pos);
        FindObjectOfType<SoundEffectPlayer>().PlaySound(collisionSound);
    }
    void instantiateEffect(GameObject effect, Vector3 pos){
        Instantiate(effect, pos, Quaternion.identity);
    }
    public void Flash(){
        if (!flashing){
            StartCoroutine(flashSprite());
        }
    }

    IEnumerator flashSprite(){
        flashing = true;
        int i = 0;
        while (i < 3){
            sprite.material = flashWhite;
            sprite.color = flashColor;
            yield return new WaitForSeconds(.2f);
            sprite.material = actualMaterial;
            sprite.color = actualColor;
            yield return new WaitForSeconds(.05f);
            i++;
        }
        flashing = false;
    }
}
