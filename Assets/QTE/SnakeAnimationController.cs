using UnityEngine;

public class SnakeAnimationController : MonoBehaviour
{
    [Header("Animation")]
    public Animator snakeAnimator;
    
    [Header("Animation Triggers")]
    public string shakeAnimationTrigger = "Shake";
    public string idleAnimationTrigger = "Idle";
    
    [Header("Shake Settings")]
    public float shakeDuration = 1.5f;
    
    private bool isShaking = false;
    
    private void Start()
    {
        if (snakeAnimator == null)
        {
            snakeAnimator = GetComponent<Animator>();
        }
    }
    
    // Appelé quand le QTE est réussi via UnityEvent
    public void PlayShakeAnimation()
    {
        if (isShaking)
            return;
            
        if (snakeAnimator != null)
        {
            isShaking = true;
            snakeAnimator.SetTrigger(shakeAnimationTrigger);
            
            // Retour à l'idle après la durée du shake
            Invoke(nameof(ReturnToIdle), shakeDuration);
        }
    }
    
    private void ReturnToIdle()
    {
        isShaking = false;
        if (snakeAnimator != null)
        {
            snakeAnimator.SetTrigger(idleAnimationTrigger);
        }
    }
    
    // Méthodes supplémentaires pour d'autres animations
    public void PlayIdleAnimation()
    {
        if (snakeAnimator != null && !isShaking)
        {
            snakeAnimator.SetTrigger(idleAnimationTrigger);
        }
    }
    
    public void StopAllAnimations()
    {
        CancelInvoke();
        isShaking = false;
    }
}
