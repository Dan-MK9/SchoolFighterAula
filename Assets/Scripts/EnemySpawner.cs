using Assets.Scripts;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyArray;

    public int numberOfEnemies;
    private int currentEnemies;

    public float spawnTime;

    public string nextSection;

    void Update()
    {
        // Caso atinja o numero maximo de inimigos spawnados
        if (currentEnemies >= numberOfEnemies)
        {
            //Contar a quatidade de inimigos ativos na cena
            int enemies = FindObjectsByType<EnemyMaleeController>(FindObjectsSortMode.None).Length;

            if (enemies <= 0)
            {
                // Avança de seção
                LevelManager.ChangeSection(nextSection);

                // Desabilitar o Spawner
                this.gameObject.SetActive(false);
            }
        }
    }

    void SpawnEnemy()
    {
        // Posição de spawn do inimigo 
        Vector2 spawnPosition;

        // Limites de Y
        // -0,23
        // -0,93

        spawnPosition.y = Random.Range(-0.23f, -0.93f);

        // Posição X maximo (direita) confiner da camera + 1 de distancia
        // Pegar o RightBound (limite direito) da Section (Confiner) como base

        float rightSectionBound = LevelManager.currentConfiner.BoundingShape2D.bounds.max.x;

        // Define o x do spawnPosition igual ao ponto da direita do confiner
        spawnPosition.x = rightSectionBound;

        // Istancia ("Spawna") os inimigos  
        // Pega um inimigo aleatorio da lista de inimigos
        // Spawna na posição SpawnerPosition
        // Quaternion é uma classe utilizada para trabalhar com rotações
        Instantiate(enemyArray[Random.Range(0, enemyArray.Length)], spawnPosition, Quaternion.identity).SetActive(true);

        //Incrementa o contador de inimigos do Spawner
        currentEnemies++;

        // Se o numero de inimigos atualmente da cena for menor que o numero maximo de inimigos, Invoca novamente a função spawn
        if (currentEnemies < numberOfEnemies)
        {
            Invoke("SpawnEnemy", spawnTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player)
        {
            // Desativa o colisor para inicia o Spawning apenas uma vez
            // ATENÇÃO: Desabilita o collider, mas o objeto Spawner continua ativo
            this.GetComponent<BoxCollider2D>().enabled = false;

            // Invoca pela primeira vez a função SpawnEnemy
            SpawnEnemy();
        }
    }
}
