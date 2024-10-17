using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float minForce = 10f;
    public float maxForce = 15f;
    public float xRange = 5f;
    public float yRange = 1f;
    public float zRange = 3f;

    private Rigidbody rb;

    // Variables pour gérer les points
    public int pointsValueOnClick = 5;  // Points gagnés ou perdus au clic
    public int pointsValueOnDestroyNormal = 50;  // Points perdus à la destruction d'un fruit normal
    public int pointsValueOnDestroyMalus = 5;  // Points gagnés à la destruction d'un malus
    public bool isMalus = false;  // Indique si l'objet est un malus

    // Variable pour le système de particules
    public ParticleSystem explosionParticle;  // Référence au prefab de particules d'explosion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Launch();  // Lancer l'objet dès le début
    }

    // Lancer l'objet vers le haut à partir d'une position aléatoire
    void Launch()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-xRange, xRange),
            Random.Range(-zRange, zRange)
        );

        transform.position = randomPosition;
        float randomForce = Random.Range(minForce, maxForce);
        rb.AddForce(Vector3.up * randomForce, ForceMode.Impulse);  // Appliquer une force vers le haut
    }

    // Méthode appelée lorsqu'on clique sur l'objet
    void OnMouseDown()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();  // Trouver le GameManager
        if (gameManager != null)
        {
            if (isMalus)
            {
                gameManager.AddScore(-pointsValueOnClick);  // Retirer des points si c'est un malus
            }
            else
            {
                gameManager.AddScore(pointsValueOnClick);  // Ajouter des points si c'est un bonus
            }
        }

        // Instancier les particules d'explosion
        Instantiate(explosionParticle, transform.position, transform.rotation);

        Destroy(gameObject);  // Détruire l'objet après le clic
    }

    // Méthode appelée lorsqu'un objet entre en collision avec un trigger (comme "Sensor")
    void OnTriggerEnter(Collider other)
    {
        // Vérifier si l'objet qui entre en collision est le "Sensor"
        if (other.gameObject.CompareTag("Sensor"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                if (isMalus)
                {
                    gameManager.AddScore(pointsValueOnDestroyMalus);  // Ajouter 5 points à la destruction d'un malus
                }
                else
                {
                    gameManager.AddScore(-pointsValueOnDestroyNormal);  // Retirer 50 points à la destruction d'un fruit normal
                }
            }

            // Instancier les particules d'explosion
            Instantiate(explosionParticle, transform.position, transform.rotation);

            Destroy(gameObject);  // Détruire l'objet après la collision avec le "Sensor"
        }
    }
}
