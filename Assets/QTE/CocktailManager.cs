using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CocktailManager : MonoBehaviour
{
    [Header("References")]
    public QTESystem qteSystem;
    public GaugeSystem gaugeSystem;
    public GameManager gameManager;
    
    [Header("Input")]
    public InputActionReference startCocktailInput;
    
    [Header("Cocktail State")]
    private bool isProcessingCocktail = false;
    private bool canStartNewCocktail = true;
    
    [Header("Events")]
    public UnityEvent onCocktailStarted;
    public UnityEvent onCocktailCompleted;
    public UnityEvent onCocktailFailed;
    
    private void OnEnable()
    {
        if (startCocktailInput != null)
        {
            startCocktailInput.action.Enable();
            startCocktailInput.action.performed += OnStartCocktailInput;
        }
    }
    
    private void OnDisable()
    {
        if (startCocktailInput != null)
        {
            startCocktailInput.action.performed -= OnStartCocktailInput;
            startCocktailInput.action.Disable();
        }
    }
    
    private void OnStartCocktailInput(InputAction.CallbackContext context)
    {
        if (canStartNewCocktail && !isProcessingCocktail)
        {
            StartCocktail();
        }
    }
    
    public void StartCocktail()
    {
        if (isProcessingCocktail || !canStartNewCocktail)
            return;
            
        // Vérifier s'il reste du temps
        if (gameManager != null && gameManager.timeRemaining <= 0)
            return;
            
        isProcessingCocktail = true;
        onCocktailStarted.Invoke();
        
        // Lancer le QTE
        qteSystem.StartQTE();
    }
    
    // Appelé par le QTESystem via UnityEvent quand le QTE est réussi
    public void OnQTESuccess()
    {
        // Le QTE lance déjà la jauge automatiquement
        // On attend juste la fin de la jauge
    }
    
    // Appelé par le QTESystem via UnityEvent quand le QTE échoue
    public void OnQTEFailed()
    {
        isProcessingCocktail = false;
        onCocktailFailed.Invoke();
        
        // Le QTE se relance automatiquement, donc on remet à jour l'état
        // mais on ne compte pas ce cocktail comme terminé
    }
    
    // Appelé par le GaugeSystem via UnityEvent quand la jauge est réussie
    public void OnGaugeSuccess()
    {
        isProcessingCocktail = false;
        
        // Incrémenter le compteur de cocktails réussis
        if (gameManager != null)
        {
            gameManager.AddCompletedCocktail();
        }
        
        onCocktailCompleted.Invoke();
    }
    
    // Appelé par le GaugeSystem via UnityEvent quand la jauge échoue
    public void OnGaugeFailed()
    {
        isProcessingCocktail = false;
        onCocktailFailed.Invoke();
    }
    
    public void EnableCocktailCreation(bool enable)
    {
        canStartNewCocktail = enable;
    }
}
