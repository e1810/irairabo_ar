using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sphereController : MonoBehaviour
{
    public GameObject marker, canvas, retryButton, textBox;
    int hitCount = 0, timeCount = 0;
    bool isMovable = true, isStarted;
    UnityEngine.UI.Text msgtxt;

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "MazeWall")
        {
            UnityEngine.Debug.Log("Hit!");
            if(isStarted && isMovable && hitCount<2e7) hitCount++;
        }
        else if(isStarted)
        {
            UnityEngine.Debug.Log("Goal!");
            isMovable = false;
            int score = (int)2e7 / (timeCount / 5 + hitCount);
            string s = "GOAL !!!\nScore: " + score.ToString();
            textBox.SetActive(true);
            msgtxt.text = s;
            retryButton.SetActive(true);
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    void Start()
    {
        retryButton.SetActive(false);
        msgtxt = canvas.GetComponent<UnityEngine.UI.Text>();
        msgtxt.text = "keep the ball\nout of the maze";
        //msgtxt.color = new Color(1f, 0f, 0f, 1f);
    }

    void Update()
    {
        float x = marker.transform.position.x;
        float y = marker.transform.position.y;
        float z = marker.transform.position.z;
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
        if(isMovable) transform.position = new Vector3(newX, newY, 0);
    }
}
