using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
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
        Time.timeScale = 0;
        StartCoroutine(Restart());
    }
    
    public void Lose()
    {
        lose.SetActive(true);
        Time.timeScale = 0;
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
