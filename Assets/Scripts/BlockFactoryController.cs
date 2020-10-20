using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;



public class BlockFactoryController : MonoBehaviour{

    public GameObject blockPrefab, goalPrefab;
    public int height, width;

    bool[,] generateMaze(int height, int width) {
        if (height % 2 == 0) height++;
        if (width % 2 == 0) width++;
        bool[,] maze = new bool[height, width];
        for (int i = 0; i < height; ++i) for (int j = 0; j < width; ++j) {
            if (i == 0 || j == 0 || i == height - 1 || j == width - 1) maze[i, j] = true;
            else maze[i, j] = false;
        }

        System.Random randomSource = new System.Random();
        for (int i = 2; i < height - 1; i += 2) for (int j = 2; j < width - 1; j += 2) {
            maze[i, j] = true;
            while (true) {
                int R = 3;
                if (i == 2) R++;
                int direction = randomSource.Next(0, R);
                int nx = i, ny = j;
                if (direction == 0) ny++;
                if (direction == 1) nx++;
                if (direction == 2) ny--;
                if (direction == 3) nx--;
                if (!maze[nx, ny]) {
                        maze[nx, ny] = true;
                        break;
                }
            }
        }
        return maze;
    }

    // Start is called before the first frame update
    void Start(){
        bool[,] field = generateMaze(height, width);
        for (int i = 0; i < height; ++i) {
            for (int j = 0; j < width; ++j) {
                if((i!=1 || j!=0) && field[i, j]) {
                    Vector3 placePosition = new Vector3(j - 0.5f - width/2, i - 0.5f - height/2, -0.4f);
                    Instantiate(blockPrefab, placePosition, Quaternion.identity);

                }
            }
        }
        Vector3 goalPosition = new Vector3(width/2 - 1.5f, height/2 - 1.5f, 0.2f);
        Instantiate(goalPrefab, goalPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update(){
        
    }
}
