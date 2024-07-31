using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Obstacle>().GetDamage(1);
        Destroy(gameObject);
    }
}
