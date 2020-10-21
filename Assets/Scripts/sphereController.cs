using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Media;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sphereController : MonoBehaviour
{
    public GameObject marker, canvas, retryButton, rankingForm, rankingTextBox, textBox, goToRankingButton;
    int hitCount = 0, timeCount = 0, lastScore = 0;
    bool isMovable = true, isStarted;
    UnityEngine.UI.Text msgtxt, rankingText;
    Renderer renderer;



    void Start()
    {
        retryButton.SetActive(false);
        rankingForm.SetActive(false);
        goToRankingButton.SetActive(false);
        rankingText = rankingTextBox.GetComponent<UnityEngine.UI.Text>();
        msgtxt = canvas.GetComponent<UnityEngine.UI.Text>();
        msgtxt.text = "keep the ball\nout of the maze";
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float x = marker.transform.position.x;
        float y = marker.transform.position.y;
        float z = marker.transform.position.z;
        float nowX = transform.position.x;
        float nowY = transform.position.y;
        float newX = x + x / (-20 - z) * z;
        float newY = y + y / (-20 - z) * z;

        if (!isStarted)
        {
            if (newX < -6.5 || 5 < newX || newY < -6.5 || 5 < newY)
            {
                if (++timeCount > 400)
                {
                    isStarted = true;
                    timeCount = 0;
                    msgtxt.fontSize = 50;
                    msgtxt.text = "Start !";
                }
            }
            else timeCount = 0;
        }
        else
        {
            if (timeCount > 100 && isMovable)
            {
                msgtxt.text = "";
                textBox.SetActive(false);
            }
            if (isMovable && timeCount < 2e7) timeCount++;
        }
        if(!isStarted &&isMovable) transform.position = new Vector3(newX, newY, 0);
        if (isStarted && isMovable) GetComponent<Rigidbody>().velocity = new Vector3(newX - nowX, newY - nowY, 0f) * 12;
    }



    void OnCollisionEnter(Collision collision)
    {
        Renderer wallRenderer = collision.gameObject.GetComponent<Renderer>();
        if(isStarted && isMovable) wallRenderer.material.color = new Color(255, 0, 0);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "MazeWall")
        {
            UnityEngine.Debug.Log("Hit!");
            if (isStarted && isMovable && hitCount < 2e7)
            {
                hitCount++;
                renderer.material.color = new Color(0, 0, hitCount / 30);
            }
        }
        else if (isStarted)
        {
            UnityEngine.Debug.Log("Goal!");
            isMovable = false;
            int score = (int)2e7 / (timeCount / 5 + hitCount);
            string s = "GOAL !!!\nScore: " + score.ToString();
            textBox.SetActive(true);
            msgtxt.text = s;
            retryButton.SetActive(true);
            OnGoal(score);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Renderer wallRenderer = collision.gameObject.GetComponent<Renderer>();
        if(isStarted && isMovable) wallRenderer.material.color = new Color(255, 255, 255);
    }

    void OnGoal(int score)
    {
        bool rankin = (gameManager.ranking.Count<20);
        for(int i=0; i<20 && i< gameManager.ranking.Count; i++)
        {
            if (gameManager.ranking[i].score < score) rankin = true;
        }
        if (!rankin) return;
        rankingText.text = "Rank in !!!";
        rankingForm.SetActive(true);
        goToRankingButton.SetActive(true);
        lastScore = score;
    }

    public void GoToRankingWithRankIn()
    {
        player newPlayer;
        newPlayer.name = rankingForm.GetComponent<UnityEngine.UI.InputField>().text;
        newPlayer.score = lastScore;
        gameManager.ranking.Add(newPlayer);
        GoToRanking();

    }

    public void GoToRanking()
    {
        SceneManager.LoadScene("Ranking");
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
