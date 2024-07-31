using UnityEngine;
using TMPro;

public class Obstacle : MonoBehaviour
{
    public int MaxHealth;
    public int Health;

    public TextMeshPro HealthText;

    private void Start()
    {
        Health = MaxHealth;
        HealthText.text = Health.ToString();
    }
    public void GetDamage(int _amount)
    {
        Health -= _amount;
        HealthText.text = Health.ToString();
        if (Health <= 0)
        {
            for (int i = 0; i < MaxHealth; i++)
                PlayerController.Player.AddSomeone();
            Destroy(gameObject);
        }
    }
}
