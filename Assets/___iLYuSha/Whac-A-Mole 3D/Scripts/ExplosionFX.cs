using UnityEngine;
using System.Collections;

public class ExplosionFX : ObjectPoolRecoverySystem
{
    public float lifeTime;
    protected float timeRecovery;
    private AudioSource audioSound;

    private float timer;
    public AudioClip[] clip;

    void Awake()
    {
        audioSound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        timeRecovery = Time.time + lifeTime;
    }

    void Update()
    {
        if (Time.time > timeRecovery)
            RecoverGameObject(gameObject);
    }

    public IEnumerator Audio(int clipNum)
    {
        yield return new WaitForSeconds(0.01f);
        if (clipNum == 0)
        {
            audioSound.volume = 1.0f;
            audioSound.priority = 128;
        }
        else if (clipNum == 1)
        {
            audioSound.volume = 0.7f;
            audioSound.priority = 128;
        }
        else if (clipNum == 10)
        {
            audioSound.volume = 0.9f;
            audioSound.priority = 220;
        }
        else if (clipNum == 11)
        {
            audioSound.volume = 1.0f;
            audioSound.priority = 100;
        }
        else
        {
            audioSound.volume = 1.0f;
            audioSound.priority = 128;
        }
        yield return new WaitForSeconds(0.03f);
        audioSound.PlayOneShot(clip[clipNum]);
    }
}
