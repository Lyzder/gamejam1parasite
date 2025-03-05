using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricada : MonoBehaviour
{
    [SerializeField] GameObject burnEffect;
    [SerializeField] Vector3 effectOffset;
    public AudioClip burningSfx;
    public AudioClip destroySfx;
    private bool burning;
    private GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        burning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (burning && effect == null)
        {
            AudioManager.Instance.PlaySFX(destroySfx);
            Destroy(gameObject);
        }
    }

    public void BurnDown()
    {
        effect = Instantiate(burnEffect, transform.position + effectOffset, transform.rotation);
        burning = true;
        AudioManager.Instance.PlaySFX(burningSfx);
    }
}
