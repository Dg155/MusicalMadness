using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class FinalBlock : MonoBehaviour
{

    private bool artifactsCollected;
    public AudioClip errorSFX;
    public AudioClip crumbleSFX;

    // Start is called before the first frame update
    void Start()
    {
        artifactsCollected = false;
    }

    public void setBool(bool state)
    {
        artifactsCollected = state;
    }

    async void crumble()
    {
        GetComponent<Animator>().SetTrigger("ambushBeaten");
        //GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SoundEffectPlayer>().PlaySound(crumbleSFX);
        await Task.Delay(800);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player")
        {
            if (artifactsCollected)
            {
                crumble();
            }
            else
            {
                GameObject.FindObjectOfType<LevelInfo>().popUpInfo();
                GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SoundEffectPlayer>().PlaySound(errorSFX);
            }
        }
    }

}
