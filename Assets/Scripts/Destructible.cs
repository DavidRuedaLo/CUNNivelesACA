using UnityEngine;

public class Destructible : MonoBehaviour
{
    public void Break()
    {
        Destroy(gameObject);
    }
}
