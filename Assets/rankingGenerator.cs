using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rankingGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameManager.ranking.Sort((a, b) => {
            return b.score - a.score;
        });

        string s = "";
        for (int i = 0; i < gameManager.ranking.Count; i++)
        {
            s += gameManager.ranking[i].name + " : " + gameManager.ranking[i].score.ToString() + "\n";
        }
        GetComponent<UnityEngine.UI.Text>().text = s;
        UnityEngine.Debug.Log(s);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
