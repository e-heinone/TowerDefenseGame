using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Enemies enemyEnum = Enemies.skeleton1;
    // reference to an array of waypoints that enemies use to walk "on a path"
    // aka enemies walk to waypoints in the order of their index in the array
    public Transform[] waypoints = null;
    private int waypointIndex = 0;

    // values which could be changed in inpector
    public int health = 3;
    public float speed = 2f;
    public int reward = 10;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // get access to gamemanager
        gameManager = GameObject.Find("GameManager").gameObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Null check: error will be printed in scene manager
        if (waypoints != null)
        {
            Move();
        }

    }

    // Waypoints would be set for each enemy in scene manager using this public function
    public void SetWaypoints(Transform[] newTransforms)
    {
        waypoints = newTransforms;
    }

    private void Move()
    {
        // if there's another waypoint the enemy can move towards, move it towards the next waypoint
        if (waypointIndex <= waypoints.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, speed * Time.deltaTime);

            // once enemy reaches the waypoint, go up one number in the waypoint array
            if (transform.position.x < waypoints[waypointIndex].transform.position.x + 0.1 && transform.position.x > waypoints[waypointIndex].transform.position.x - 0.1)
            {
                if (transform.position.y < waypoints[waypointIndex].transform.position.y + 0.1 && transform.position.y > waypoints[waypointIndex].transform.position.y - 0.1)
                {
                    waypointIndex++;
                }
            }
        }

        // if enemy reaches the end remove 1 health point from the player
        if (waypointIndex == waypoints.Length)
        {
            gameManager.playerHealth--;
            Destroy(gameObject);
        }
    }

    // enemy units take damage if gameobjects with the tag "Projectile" or "MagicProjectil" hit them
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            // removes health from enemy and checks if the enemy then has below 0 health
            health--;
            if (health <= 0)
            {
                KillEnemy();
            }
        }
        else if (collision.gameObject.tag == "MagicProjectile")
        {
            health = health - 3;
            if (health <= 0)
            {
                KillEnemy();
            }
        }
    }

    // gives player gold and destroys the enemy
    private void KillEnemy()
    {
        gameManager.gold += reward;
        Destroy(gameObject);
    }
}