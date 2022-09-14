using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private string characterName = "Commander";


    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }


    }

    public void SetCharacter(string characterName)
    {
        this.characterName = characterName;
    }

    public string GetCharacter()
    {
        return characterName;
    }

    public void StartTheGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
