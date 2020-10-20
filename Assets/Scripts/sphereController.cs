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
    Renderer renderer;


    void Start()
    {
        retryButton.SetActive(false);
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
        //if(isStarted && isMovable) GetComponent<Rigidbody>().MovePosition(new Vector3(newX, newY, 0f));
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
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Renderer wallRenderer = collision.gameObject.GetComponent<Renderer>();
        if(isStarted && isMovable) wallRenderer.material.color = new Color(255, 255, 255);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
