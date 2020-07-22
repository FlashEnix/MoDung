using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class shootPrefab : MonoBehaviour
{
    public float speed;
    public GameObject explosion;
    public AudioClip explosionSfx;

    private PlayerScript _initiate;
    private PlayerScript _target;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void setTarget(PlayerScript initiate, PlayerScript target)
    {
        _initiate = initiate;
        _target = target;
        Vector3 vel = target.transform.position - transform.position;
        vel.y = 0;
        GetComponent<Rigidbody>().velocity = vel.normalized * speed;
        transform.LookAt(new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z));
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _target.gameObject)
        {
            if (explosionSfx != null) GameHelper.instance.PlaySoundShot(explosionSfx);
            Instantiate(explosion, transform.position,Quaternion.identity);
            _initiate.dealDamage();
            _initiate.endAttack();
            Destroy(this.gameObject);
        }
    }
}
