using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 25f;
    public float lifespan = 3f;
    public float damage = 5f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = transform.forward * speed;

        Destroy(gameObject, lifespan);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Destructible destructibleTarget = other.GetComponentInParent<Destructible>();

        AIManager enemy = other.GetComponentInParent<AIManager>();

        if (destructibleTarget != null)
        {
            destructibleTarget.Break();
        }

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
