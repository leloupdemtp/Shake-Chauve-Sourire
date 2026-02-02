using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Win Condition")]
    public int cocktailsToWin = 10;
    private int completedCocktails = 0;
    
    [Header("Timer")]
    public float totalTime = 120f; // 2 minutes par défaut
    public float timeRemaining;
    
    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI cocktailCountText;
    
    [Header("References")]
    public UI_Manager_Improved uiManager;
    public CocktailManager cocktailManager;
    
    [Header("Events")]
    public UnityEvent onGameStart;
    public UnityEvent onGameWin;
    public UnityEvent onGameLose;
    
    private bool gameActive = false;
    private bool gameEnded = false;
    
    private void Start()
    {
        timeRemaining = totalTime;
        UpdateUI();
        StartGame();
    }
    
    private void Update()
    {
        if (!gameActive || gameEnded)
            return;
            
        // Décompte du temps
        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(0, timeRemaining);
        
        UpdateUI();
        
        // Vérifier la défaite par temps écoulé
        if (timeRemaining <= 0 && !gameEnded)
        {
            CheckEndConditions();
        }
    }
    
    public void StartGame()
    {
        gameActive = true;
        gameEnded = false;
        completedCocktails = 0;
        timeRemaining = totalTime;
        
        UpdateUI();
        onGameStart.Invoke();
    }
    
    public void AddCompletedCocktail()
    {
        completedCocktails++;
        UpdateUI();
        
        // Vérifier la victoire
        if (completedCocktails >= cocktailsToWin)
        {
            WinGame();
        }
    }
    
    private void CheckEndConditions()
    {
        if (gameEnded)
            return;
            
        if (completedCocktails >= cocktailsToWin)
        {
            WinGame();
        }
        else
        {
            LoseGame();
        }
    }
    
    private void WinGame()
    {
        if (gameEnded)
            return;
            
        gameEnded = true;
        gameActive = false;
        
        // Désactiver la création de nouveaux cocktails
        if (cocktailManager != null)
            cocktailManager.EnableCocktailCreation(false);
        
        onGameWin.Invoke();
        
        if (uiManager != null)
            uiManager.Wing();
    }
    
    private void LoseGame()
    {
        if (gameEnded)
            return;
            
        gameEnded = true;
        gameActive = false;
        
        // Désactiver la création de nouveaux cocktails
        if (cocktailManager != null)
            cocktailManager.EnableCocktailCreation(false);
        
        onGameLose.Invoke();
        
        if (uiManager != null)
            uiManager.Lose();
    }
    
    private void UpdateUI()
    {
        // Mise à jour du timer
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        
        // Mise à jour du compteur de cocktails
        if (cocktailCountText != null)
        {
            cocktailCountText.text = $"Cocktails: {completedCocktails}/{cocktailsToWin}";
        }
    }
    
    public int GetCompletedCocktails()
    {
        return completedCocktails;
    }
    
    public bool IsGameActive()
    {
        return gameActive && !gameEnded;
    }
}
