using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public int damage;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        // ao colidir com o player
        if (player)
        {
            // O player toma dano
            player.TakeDamage(damage);

            // o projetil é destruido
            Destroy(this.gameObject);
        }

        // Destrui o projetil ao colidir com os limites da fase (left/right)
        if (collision.CompareTag("Wall"))
        {
            // O projetil é destruido
            Destroy(this.gameObject);
        }
    }
}
