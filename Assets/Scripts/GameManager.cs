using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Importer le namespace TextMeshPro
using UnityEngine.SceneManagement;  // Importer pour gérer les scènes
using UnityEngine.UI;  // Importer pour gérer les boutons

public class GameManager : MonoBehaviour
{
    // Liste des prefabs à instancier
    public GameObject[] targetPrefabs;

    // Temps entre les instantiations
    public float spawnInterval = 2f;

    // Limites pour la position de spawn aléatoire
    public float xRange = 5f;
    public float zRange = 3f;
    public float spawnYPosition = 0f;

    // Gestion du score
    public TextMeshProUGUI scoreText;  // Variable publique pour afficher le score
    private int score;  // Variable pour stocker le score

    // Chronomètre
    public TextMeshProUGUI timerText;  // Pour afficher le chronomètre
    private float gameTimer;  // Timer pour la durée du jeu

    // Game Over Panel
    public GameObject gameOverPanel;  // Référence au panneau de Game Over
    public Button restartButton;  // Bouton pour redémarrer le jeu

    // Gestion du Game Over
    public bool isGameActive = true;  // Indique si le jeu est actif

    void Start()
    {
        // Initialiser le score à une valeur positive pour éviter le GameOver immédiat
        score = 10;  // Par exemple, commence avec 10 points
        gameTimer = 0f;
        isGameActive = true;

        // Mettre à jour l'affichage du score et du chronomètre
        UpdateScore();
        UpdateTimer();

        // Désactiver le panneau de Game Over et le bouton Restart
        gameOverPanel.SetActive(false);
        restartButton.gameObject.SetActive(false);

        // Lancer la coroutine qui spawn des fruits si le jeu est actif
        StartCoroutine(SpawnTargets());
    }

    void Update()
    {
        // Mettre à jour le chronomètre uniquement si le jeu est actif
        if (isGameActive)
        {
            gameTimer += Time.deltaTime;  // Incrémenter le timer
            UpdateTimer();  // Mettre à jour l'affichage du timer
        }

        // Arrêter le jeu si le score tombe à 0
        if (score <= 0 && isGameActive)
        {
            GameOver();
        }
    }

    // Coroutine pour instancier des objets de manière régulière si le jeu est actif
    IEnumerator SpawnTargets()
    {
        // Boucle infinie pour continuer à lancer des fruits
        while (isGameActive)
        {
            // Choisir un prefab aléatoire parmi la liste
            int randomIndex = Random.Range(0, targetPrefabs.Length);
            GameObject selectedPrefab = targetPrefabs[randomIndex];

            // Définir une position de spawn aléatoire
            Vector3 spawnPosition = new Vector3(
                Random.Range(-xRange, xRange),   // Position X aléatoire
                spawnYPosition,                  // Position Y fixe
                Random.Range(-zRange, zRange)    // Position Z aléatoire
            );

            // Instancier le prefab à la position de spawn si le jeu est actif
            if (isGameActive)
            {
                Instantiate(selectedPrefab, spawnPosition, selectedPrefab.transform.rotation);
            }

            // Attendre avant de lancer le prochain objet
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Méthode pour mettre à jour l'affichage du score
    void UpdateScore()
    {
        scoreText.text = "Score : " + score.ToString();  // Mettre à jour le texte du score
    }

    // Méthode pour ajouter des points au score
    public void AddScore(int points)
    {
        score += points;  // Ajouter les points au score
        UpdateScore();  // Mettre à jour l'affichage
    }

    // Méthode pour mettre à jour l'affichage du chronomètre
    void UpdateTimer()
    {
        timerText.text = "Chrono: " + Mathf.FloorToInt(gameTimer).ToString();  // Mettre à jour le texte du timer
    }

    // Méthode pour gérer le Game Over
    void GameOver()
    {
        isGameActive = false;  // Arrêter le jeu
        gameOverPanel.SetActive(true);  // Activer le panneau de Game Over
        restartButton.gameObject.SetActive(true);  // Afficher le bouton Restart
    }

    // Méthode pour redémarrer le jeu
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Recharger la scène actuelle
    }
}
