using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Game game;

    private void Awake()
    {
        game = FindObjectOfType<Game>();
    }

    // If player collides with enemy collider (which is anywhere but the top of the enemy) then call the Hurt function
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            Hurt();
    }

    // Check which trigger player is colliding with
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                StartCoroutine(HurtEnemy(collision.gameObject.GetComponent<Enemy>()));
                // add points to game based on enemy point value
                break;
            case "Gem":
                game.AddLife();
                Destroy(collision.gameObject);
                break;
            case "Coin":
                game.AddPoints(100);
                Destroy(collision.gameObject);
                break;
            default:
                break;
        }

    }

    // Destroy the enemy then wait until the end of frame to continue
    private IEnumerator HurtEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
        yield return new WaitForEndOfFrame();
        game.AddPoints(enemy.pointValue);
    }

    // Destroy the player
    private void Hurt()
    {
        game.LoseLife();
        Destroy(this.gameObject);
    }
}
