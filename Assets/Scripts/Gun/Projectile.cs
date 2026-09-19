using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 25f;
    public float lifespan = 3f;
    private Rigidbody rb;

    private bool isPlayerBullet = false, isEnemyBullet = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (gameObject.CompareTag("PlayerBullet")) isPlayerBullet = true;
        else if (gameObject.CompareTag("EnemyBullet")) isEnemyBullet = true;
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

        if (isPlayerBullet)
        {
            if (destructibleTarget != null)
            {
                destructibleTarget.Break();
            }

            if (enemy != null)
            {
                enemy.TakeDamage();
            }
            
        }

        if (isEnemyBullet)
        {
            if (other.CompareTag("Player"))
            {            
                Debug.Log("Bullet hit player");
            }
        }

        Destroy(gameObject);
    }
}
