using UnityEngine;

public class TeammateController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            Debug.Log("Takim arkadasim birini aldi.");
            PlayerController.Player.AddSomeone();
            other.GetComponent<Collider>().enabled = false;
            other.gameObject.SetActive(false);
        }
    }
}
