using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float forwardSpeed = 2f;
    public Transform platform; // Reference to the platform
    public float platformWidth = 10f; // Adjust based on your platform size
    public float platformLength = 10f; // Adjust based on your platform size

    private List<Transform> followers = new List<Transform>();
    public Transform Holder;
    public static PlayerController Player;
    public int ballCount;
    public float ballFireTimer;
    public float fireDelay = 2;

    private void Awake()
    {
        if (ballCount <= 0)
            ballCount = 1;
        
        Player = this;
    }

    void Update()
    {
        // Basic player movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * speed * Time.deltaTime);

        // Update the positions of the followers
        UpdateFollowers();

        if (ballFireTimer < Time.time)
        {
            Fire(ballCount);
            ballFireTimer = Time.time + fireDelay;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            AddSomeone();
            other.GetComponent<Collider>().enabled = false;
            other.gameObject.SetActive(false);
        }
    }

    public void AddSomeone()
    {
        GameObject teammate = Instantiate(Resources.Load<GameObject>("Teammate"), Vector3.zero, Quaternion.identity, Holder);
        teammate.transform.localPosition = Vector3.zero;
        followers.Add(teammate.transform);
    }

    void UpdateFollowers()
    {
        float spacing = 0.5f; // Adjust this value to change the spacing between followers
        int rowSize = 5; // Initial number of NPCs in the first row

        int npcIndex = 0;

        for (int row = 0; npcIndex < followers.Count; row++)
        {
            int numNpcsInRow = rowSize + (row * 2); // Calculate the number of NPCs in the current row

            for (int col = 0; col < numNpcsInRow; col++)
            {
                if (npcIndex >= followers.Count) break;

                // Calculate xOffset to place NPCs starting from the center, alternating left and right
                float xOffset;
                if (col % 2 == 0)
                {
                    xOffset = (col / 2) * spacing; // Right side
                }
                else
                {
                    xOffset = -((col / 2) + 1) * spacing; // Left side
                }

                float zOffset = -(row + 1) * spacing - 0.5f; // Adjusted to set the first row nearer to the player

                Vector3 targetPosition = transform.position + new Vector3(xOffset, 0, zOffset);
                followers[npcIndex].position = Vector3.Lerp(followers[npcIndex].position, targetPosition, speed * Time.deltaTime);

                // Check if the follower is out of the platform bounds
                if (IsOutOfBounds(followers[npcIndex]))
                {
                    followers[npcIndex].GetComponent<Animator>().SetBool("Fall", true);
                    followers[npcIndex].SetParent(null);
                    Destroy(followers[npcIndex].gameObject,5);
                    RemoveFollower(followers[npcIndex]);
                    npcIndex--; // Decrement index to account for the removed follower
                }

                npcIndex++;
            }
        }
    }

    bool IsOutOfBounds(Transform follower)
    {
        Vector3 followerPosition = follower.position;
        Vector3 platformPosition = platform.position;

        // Check if the follower is out of the platform boundaries
        return (followerPosition.x < platformPosition.x - platformWidth / 2 ||
                followerPosition.x > platformPosition.x + platformWidth / 2 ||
                followerPosition.z < platformPosition.z - platformLength / 2 ||
                followerPosition.z > platformPosition.z + platformLength / 2);
    }

    void RemoveFollower(Transform follower)
    {
        // Enable gravity to make the follower fall
        Rigidbody rb = follower.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
        }

        // Remove the follower from the list
        followers.Remove(follower);
    }

    //Top atisi

    void Fire(int count)
    {
        GameObject handler = Instantiate(Resources.Load<GameObject>("BallParent"), transform.position + transform.forward, Quaternion.identity);
        BallHandler ballHandler = handler.GetComponent<BallHandler>();
        ballHandler.Initialize(count);
    }
}
