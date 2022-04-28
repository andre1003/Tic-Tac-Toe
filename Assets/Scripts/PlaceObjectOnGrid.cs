using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjectOnGrid : MonoBehaviour {
    public Transform gridCellPrefab;

    public Transform player1;
    public Transform player2;

    public Node[,] nodes;

    public Transform onMousePrefab;
    public Vector3 smoothMousePosition;

    public AudioSource placeAudioSource;
    public AudioClip place;

    [SerializeField] private int height;
    [SerializeField] private int width;
    
    
    private Plane plane;
    private Vector3 mousePosition;

    // Start is called before the first frame update
    void Awake() {
        CreateGrid();
        plane = new Plane(Vector3.up, transform.position);

        CameraReposition();
    }

    // Update is called once per frame
    void Update() {
        GetMousePostitionOnGrid();
    }


    public void OnMouseClickOnUI() {
        if (onMousePrefab == null && enabled) {
            onMousePrefab = Instantiate(player1, mousePosition, Quaternion.identity);
        }
    }

    public void OnMouseClickOnUI2() {
        if (onMousePrefab == null && enabled) {
            onMousePrefab = Instantiate(player2, mousePosition, Quaternion.identity);
            onMousePrefab.Rotate(new Vector3(0, 90f, 0));
        }
    }


    private void GetMousePostitionOnGrid() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(plane.Raycast(ray, out var enter)) {
            mousePosition = ray.GetPoint(enter);
            smoothMousePosition = mousePosition;
            mousePosition = Vector3Int.RoundToInt(mousePosition);

            foreach (var node in nodes) {
                if (node.cellPosition == mousePosition && node.isPlaceable) { 
                    if(Input.GetMouseButtonUp(0) && onMousePrefab != null) {
                        placeAudioSource.clip = place;
                        placeAudioSource.Play();

                        node.isPlaceable = false;
                        onMousePrefab.GetComponent<FollowMouse>().isOnGrid = true;
                        onMousePrefab.position = node.cellPosition;
                        onMousePrefab = null;
                    }
                }
            }
        }
    }


    public void CreateGrid() {
        nodes = new Node[width, height];
        int name = 0;

        for(int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                Vector3 worldPosition = new Vector3(i, 0, j);
                Transform obj = Instantiate(gridCellPrefab, worldPosition, Quaternion.identity);
                obj.name = "Cell_" + name;
                nodes[i, j] = new Node(true, worldPosition, obj);
                name++;
            }
        }
    }

    public void DestroyGrid() {
        GameObject[] cells = GameObject.FindGameObjectsWithTag("Cell");
        int length = cells.Length;

        for (int i = 0; i < length; i++)
            Destroy(cells[i]);

        nodes = null;
    }

    private void CameraReposition() {
        Camera.main.transform.position = Vector3Int.RoundToInt(new Vector3((width / 2), 8, (height / 2)));
    }

    public int GetHeight() {
        return height;
    }

    public int GetWidth() {
        return width;
    }
}


public class Node {
    public bool isPlaceable;
    public Vector3 cellPosition;
    public Transform obj;

    public Node(bool isPlaceable, Vector3 cellPosition, Transform obj) {
        this.isPlaceable = isPlaceable;
        this.cellPosition = cellPosition;
        this.obj = obj;
    }
}
