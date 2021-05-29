using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPathSystem : MonoBehaviour
{
    // List of ColorCube gameobject
    public GameObject[] colorCubesArr;
    public string correctAnswerFilename = "";
    private PlayerSystem m_playerSystem;
    private const int NUM_CUBE_PER_ROW = 10;
    private const int NUM_CUBE_PER_COL = 10;
    private char[,] m_correctAnswer = new char[NUM_CUBE_PER_ROW, NUM_CUBE_PER_COL]; 

    public bool CheckAnswer()
    {
        bool isCorrect = true;
        for (int row = 0; row < NUM_CUBE_PER_ROW; row++)
        {
            for (int col = 0; col < NUM_CUBE_PER_COL; col++)
            {
                Color color = colorCubesArr[row * 10 + col].GetComponent<Renderer>().material.color;
                if (color == Color.red && m_correctAnswer[row,col] != 'r')
                {
                    isCorrect = false;
                    break;
                }
                else if (color == Color.green && m_correctAnswer[row, col] != 'g')
                {
                    isCorrect = false;
                    break;
                }
                else if (color == Color.blue && m_correctAnswer[row, col] != 'b')
                {
                    isCorrect = false;
                    break;
                }
                else if (color == Color.yellow && m_correctAnswer[row, col] != 'y')
                {
                    isCorrect = false;
                    break;
                }
                else if (color == Color.white)
                {
                    isCorrect = false;
                    break;
                }
            }
            if (!isCorrect)
                break;
        }
        Debug.Log("IsCorrect = " + isCorrect);
        return isCorrect;
    }

    //private string GetPath()
    //{
    //    string fileName = correctAnswerFilename + ".txt";
    //    return System.IO.Path.Combine(Environment.CurrentDirectory, @"Assets\Resources\", fileName);
    //}

    private bool ParseFile(string path)
    {
        string[] lines = Resources.Load<TextAsset>(path).text.Split('\n');
        //string[] lines = System.IO.File.ReadAllLines(path);
        if (lines.Length != 10) return false;
        for (int i = 0; i < NUM_CUBE_PER_ROW; i++)
        {
            string line = lines[i];
            Debug.Log("Line = " + line);
            for (int j = 0; j < NUM_CUBE_PER_COL; j++)
            {
                char c = line[j];
                if (c != 'r' && c != 'g' && c != 'b' && c != 'y')
                {
                    return false;
                }
                m_correctAnswer[i, j] = c;
            }
        }
        return true;
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_playerSystem = GameObject.FindObjectOfType<PlayerSystem>();
        //string path = GetPath();
        bool isValidInput = ParseFile(correctAnswerFilename);
        Debug.Log("Is ColorPath file a valid input = " + isValidInput.ToString());
    }
}
