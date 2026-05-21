using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform playerPos;
    public float speed = 3f;

    void Start()
    {
        playerPos = GameObject.Find("Player 1").transform;    
    }

    void Update()
    {
        ChasePlayer();    
    }

    private void ChasePlayer()
    {
        Vector2 target = playerPos.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
