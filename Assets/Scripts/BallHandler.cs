using System.Collections.Generic;
using UnityEngine;

public class BallHandler : MonoBehaviour
{
    public GameObject ballPrefab;
    public int numberOfBalls = 10; 
    public float distanceBetweenRows = 1.5f; 
    public float moveSpeed = 5f; 
    public float rotationSpeed = 100f; 
    public float circleRadius = 0.5f; 

    private List<GameObject> balls = new List<GameObject>();

    void Start()
    {
        Destroy(gameObject, 6f);
        ArrangeBalls();
    }

    void Update()
    {
        MoveBalls();
        RotateBalls();
    }

    public void Initialize(int count)
    {
        if(count > numberOfBalls)
            count = numberOfBalls;

        InstantiateBalls(count);
    }

    void InstantiateBalls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject ball = Instantiate(ballPrefab, transform);
            balls.Add(ball);
        }
    }

    void ArrangeBalls()
    {
        balls[0].transform.localPosition = new Vector3(0, 0, -2 * distanceBetweenRows);

        int b = balls.Count - 1;
        if (b <= 0)
            return;
        int limit = Mathf.Clamp(b, 1, 3);
        ArrangeCircle(1, limit, new Vector3(0, 0, -distanceBetweenRows), circleRadius);

        if (b <= 3)
            return;
        limit = Mathf.Clamp(b, 4, 6);
        ArrangeCircle(4, limit - 3, Vector3.zero, circleRadius);

        if (b <= 6)
            return;
        limit = Mathf.Clamp(b, 7, 8);
        ArrangeCircle(7, limit - 6, new Vector3(0, 0, distanceBetweenRows), circleRadius / 2);

        if (b <= 8)
            return;
        balls[9].transform.localPosition = new Vector3(0, 0, 2 * distanceBetweenRows);
    }

    void ArrangeCircle(int startIndex, int ballCount, Vector3 center, float radius)
    {
        float angleIncrement = 360f / ballCount;
        if (ballCount == 1)
        {
            balls[startIndex].transform.localPosition = new Vector3(0, 0, center.z);
        }
        else
        {
            for (int i = 0; i < ballCount; i++)
            {
                float angle = i * angleIncrement;
                float radian = angle * Mathf.Deg2Rad;
                balls[startIndex + i].transform.localPosition = new Vector3(radius * Mathf.Cos(radian), radius * Mathf.Sin(radian), center.z);
            }
        }
    }

    void MoveBalls()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    void RotateBalls()
    {
        int b = balls.Count - 1;
        if (b <= 0)
            return;
        int limit = Mathf.Clamp(b, 1, 3);
        RotateCircle(1, limit, new Vector3(0, 0, -distanceBetweenRows));

        if (b <= 3)
            return;
        limit = Mathf.Clamp(b, 4, 6);
        RotateCircle(4, limit - 3, Vector3.zero);

        if (b <= 6)
            return;
        limit = Mathf.Clamp(b, 7, 8);
        RotateCircle(7, limit - 6, new Vector3(0, 0, distanceBetweenRows));
    }

    void RotateCircle(int startIndex, int ballCount, Vector3 center)
    {
        for (int i = 0; i < ballCount; i++)
            balls[startIndex + i].transform.RotateAround(transform.position + center, Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
