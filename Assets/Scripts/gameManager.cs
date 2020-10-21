using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct player
{
    public int score;
    public string name;
};


public class gameManager : MonoBehaviour
{
    public static List<player> ranking = new List<player>();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Retry()
    {
        SceneManager.LoadScene("Playing");
    }
}
