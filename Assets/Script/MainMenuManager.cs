using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void gotomainmenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void gotogame()
    {
        SceneManager.LoadScene("Gameplay");
    }
    public void quittinggame()
    {
        Application.Quit();
        Debug.Log("quit");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quittinggame();
        }
        
    }
}
