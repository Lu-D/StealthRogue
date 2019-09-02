using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class chestControl : MonoBehaviour
    {
        public GameObject[] drops;

        void Awake()
        {

        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                dropItem();
            }
        }

        void dropItem()
        {
            System.Random rnd = new System.Random();
            spawnItem(drops[UnityEngine.Random.Range(0, 2)]);
        }

        void spawnItem(GameObject type)
        {
            GameObject dropped = (GameObject)Instantiate(type, transform.position, transform.rotation);
            dropped.transform.position = dropped.transform.position + new Vector3(0, (float)-0.2, 0);
            dropped.GetComponent<Collider2D>().enabled = true;
            Destroy(this.gameObject);
        }
    }
}
