using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSensor : MonoBehaviour
{
    private PlayerLife2 playerLife2;
    private PlayerLife1 playerLife1;

    private Spawn spawn;

    [SerializeField]
    private MeshRenderer renderer1, renderer2, renderer3;

    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    void OnTriggerEnter(Collider collision)
    {
        playerLife1 = GameObject.FindObjectOfType<PlayerLife1>();
        playerLife2 = GameObject.FindObjectOfType<PlayerLife2>();

        if (collision.gameObject.name == "Player1")
        {  
            audioManager.Play("Heal");
            if(playerLife1.currentHealth > 0){
                StartCoroutine(playerLife1.fillHealth());
            }
            renderer1.enabled = false;
            renderer2.enabled = false;
            renderer3.enabled = false;
            Destroy(gameObject, 1);
        }

        if (collision.gameObject.name == "Player2")
        {
            audioManager.Play("Heal");
            if(playerLife2.currentHealth > 0){
                StartCoroutine(playerLife2.fillHealth());
            }
            renderer1.enabled = false;
            renderer2.enabled = false;
            renderer3.enabled = false;
            Destroy(gameObject, 1);
        }
    }
}
