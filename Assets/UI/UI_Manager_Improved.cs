using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager_Improved : MonoBehaviour
{
    [SerializeField]
    private GameObject win;
    [SerializeField]
    private GameObject lose;
    
    [SerializeField]
    private GaugeSystem gauge; 

    private void Start()
    {
        win.SetActive(false);
        lose.SetActive(false);
    }
    
    public void Wing()
    {
        win.SetActive(true);
        StartCoroutine(Restart());
    }
    
    public void Lose()
    {
        lose.SetActive(true);
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSecondsRealtime(5); // Utilise WaitForSecondsRealtime pour ignorer Time.timeScale
        Time.timeScale = 1; // Réinitialise le temps avant de recharger
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
