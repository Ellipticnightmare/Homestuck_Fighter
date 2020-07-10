using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    public CharacterController cont;
    private bool canMove;
    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(cont.transform.position - transform.position, transform.TransformDirection(Vector3.up));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        if (canMove)
            transform.Translate(transform.forward * 2);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == cont.gameObject)
        {
            cont.takeDamage(20);
            Destroy(this.gameObject);
        }
    }
    public void StartMovement()
    {
        this.GetComponent<BoxCollider2D>().enabled = true;
        canMove = true;
    }
    public void EndMovement()
    {
        Destroy(this.gameObject);
    }
}