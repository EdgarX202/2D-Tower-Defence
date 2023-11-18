using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PoisonSplash : MonoBehaviour
{
    public int Damage { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().TakeDamage(Damage, Element.POISON);
            Destroy(gameObject);
        }
    }
}
