using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAndBowScript : MonoBehaviour
{

    private Rigidbody myBody;

    public float speed = 30f;

    public float deactivate_Timer = 3f;

    public float damage = 50f;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }


    void Start()
    {

        Invoke("DeactivateGameObject", deactivate_Timer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DeactivateGameObject()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tags.ENEMY_TAG)
        {
            target.GetComponent<HealthScript>().ApplyDamage(damage);
            gameObject.SetActive(false);

        }
    }


    public void Launch(Camera mainCamera)
    {
        myBody.velocity = mainCamera.transform.forward * speed;

        transform.LookAt(transform.position + myBody.velocity);
    }


}
