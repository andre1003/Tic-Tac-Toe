using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridCheck : MonoBehaviour {
    public GameObject gameOverCanvas;
    public Color player1Color;
    public Color player2Color;
    public Color gameOverColor;

    public AudioClip ambient;
    public AudioClip win;
    public AudioClip lose;


    private PlaceObjectOnGrid placeObjectOnGrid;
    private Node[,] nodes;

    private AudioSource audioSource;

    private int height;
    private int width;

    private int LineCount;
    private int ColumnCount;

    private int DiagonalCount;
    private int CounterDiagonalCount;

    private GameObject[] player1Pieces;
    private GameObject[] player2Pieces;

    private bool isGameOver = false;


    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = ambient;
        audioSource.Play();
    }

    // Start is called before the first frame update
    void Start() {
        placeObjectOnGrid = FindObjectOfType<PlaceObjectOnGrid>();
        nodes = placeObjectOnGrid.nodes;

        height = placeObjectOnGrid.GetHeight();
        width = placeObjectOnGrid.GetWidth();

        LineCount = 0;
        ColumnCount = 0;

        DiagonalCount = 0;
        CounterDiagonalCount = 0;

        gameOverCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (!isGameOver) {
            nodes = placeObjectOnGrid.nodes;
            player1Pieces = GetPlayerPieces(1);
            player2Pieces = GetPlayerPieces(2);
            CheckGrid();
        }
    }

    private void CheckGrid() {
        LineCount = 0;
        ColumnCount = 0;
        DiagonalCount = 0;
        CounterDiagonalCount = 0;

        int count = 0;
        bool end = false;

        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                if (!nodes[i, j].isPlaceable) {
                    LineCount++;
                    count++;
                }
                if(!nodes[j, i].isPlaceable) {
                    ColumnCount++;
                }
            }

            if(!nodes[i, i].isPlaceable) {
                DiagonalCount++;
            }

            if (!nodes[i, (height - i - 1)].isPlaceable) {
                CounterDiagonalCount++;
            }


            // Check if line, column, diagonal or counter diagonal is full
            if (LineCount == height) {
                end = CheckWinner(0, i, height);
                if (end)
                    break;
            }

            if (ColumnCount == height) {
                end = CheckWinner(1, i, height);
                if (end)
                    break;
            }
            
            if (DiagonalCount == height) {
                end = CheckWinner(2, i, height);
                if (end)
                    break;
            }

            if (CounterDiagonalCount == height) {
                end = CheckWinner(3, i, height);
                if (end)
                    break;
            }

            LineCount = 0;
            ColumnCount = 0;
        }

        if (count == (height * width) && !end) {
            audioSource.clip = lose;
            audioSource.loop = false;
            audioSource.Play();
            GameOver("Game Over", gameOverColor);
        }
    }

    private GameObject[] GetPlayerPieces(int playerNumber) {
        return GameObject.FindGameObjectsWithTag("Player" + playerNumber);
    }

    private bool CheckWinner(int op, int lineOrColumn, int length) {
        int player1 = 0;
        int player2 = 0;

        Vector3[] compareVector = new [] {new Vector3(), new Vector3(), new Vector3()};

        switch (op) {
            case 0: // Line
                for(int i = 0; i < length; i++)
                    compareVector[i] = new Vector3((float)lineOrColumn, 0, i);

                break;


            case 1: // Column
                for(int i = 0; i < length; i++)
                    compareVector[i] = new Vector3(i, 0, (float)lineOrColumn);

                break;


            case 2: // Diagonal
                for (int i = 0; i < length; i++)
                    compareVector[i] = new Vector3(i, 0, i);

                break;

            case 3: // Counter diagonal
                for (int i = 0; i < length; i++)
                    compareVector[i] = new Vector3(i, 0, (length - i - 1));

                break;
        }

        try {
            // Player 1
            for (int i = 0; i < player1Pieces.Length; i++) {
                for (int j = 0; j < length; j++) {
                    if (player1Pieces[i].transform.position == compareVector[j]) {
                        player1++;
                    }
                }

            }

            if (player1 == length) {
                audioSource.clip = win;
                audioSource.loop = false;
                audioSource.Play();
                GameOver("Player 1\nWINS!", player1Color);
                return true;
            }
            else
                throw new System.Exception();
        }
        catch {
            // Player 2
            try {
                for (int i = 0; i < player2Pieces.Length; i++) {
                    for (int j = 0; j < length; j++) {
                        if (player2Pieces[i].transform.position == compareVector[j]) {
                            player2++;
                        }
                    }
                }

                if (player2 == length) {
                    audioSource.clip = win;
                    audioSource.loop = false;
                    audioSource.Play();
                    GameOver("Player 2\nWINS!", player2Color);
                    return true;
                }
            }
            catch { }
        }

        return false;
    }

    private void GameOver(string msg, Color msgColor) {
        isGameOver = true;
         
        if(gameOverCanvas != null) {
            gameOverCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = msg;
            gameOverCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = msgColor;
            gameOverCanvas.SetActive(true);
        }
        
        placeObjectOnGrid.enabled = false;
    }

    public void ResetGrid() {
        // Game Over
        if (gameOverCanvas != null) {
            gameOverCanvas.SetActive(false);
        }
        

        // Destroy Player 1 pieces
        int length = player1Pieces.Length;

        for (int i = 0; i < length; i++)
            Destroy(player1Pieces[i]);

        // Destroy Player 2 pieces
        length = player2Pieces.Length;

        for (int i = 0; i < length; i++)
            Destroy(player2Pieces[i]);

        player1Pieces = null;
        player2Pieces = null;

        isGameOver = false;

        // Enable PlaceObjectOnGrid script
        placeObjectOnGrid.enabled = true;
        placeObjectOnGrid.DestroyGrid();
        placeObjectOnGrid.CreateGrid();

        audioSource.clip = ambient;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void SetIsGameOver(bool isGameOver) {
        this.isGameOver = isGameOver;
    }
}
