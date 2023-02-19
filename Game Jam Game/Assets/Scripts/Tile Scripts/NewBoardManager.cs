using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NewBoardManager : MonoBehaviour {

    //List of tiles in board
    [SerializeField] private List<GameObject> tiles = new List<GameObject>();
    //Control layout group controlling layout of tiles
    [SerializeField] private GridLayoutGroup layoutGroup;
    [SerializeField] private  GameController gameController;
    //The Object representing where the player (and the birds they have saved) is on the board
    [SerializeField] private GameObject birdGroup;
    //When resetting the board, do not Flip first if it is the first set up
    private bool firstBoardSetUp = true;

    //Game event to message game controller when board is set up
    [SerializeField] private UnityEvent onBoardSetUpFinished;

    void Start() {
        //SetUpRound();
    }

    public void SetUpRound() {
        StartCoroutine(SetUpRoundEnum());
    }

    IEnumerator SetUpRoundEnum() {
        ResetBoardCorruption();
        if (!firstBoardSetUp) {
            StartCoroutine(FlipBoardEnum());
            yield return new WaitForSeconds(2.5f);
        }
        firstBoardSetUp = false;
        RandomizeBoard();

        //Set player index to Phoenix Cage tile
        gameController.playerIndex = tiles.FindIndex(tile => tile.name == "Phoenix Cage");
        ////Set birdGroup as child of player index Tile once board is flipped
        Invoke("SetBirdGroupParent", 3f);
        StartCoroutine(FlipBoardEnum());

        //Start initial corruption card draw process in game controller
        //gameController.CorruptionBegins();
    }

    IEnumerator FlipBoardEnum() {
        foreach (Transform tile in transform) {
            yield return new WaitForSeconds(0.1f);
            tile.GetComponent<NewTileBehavior>().Flip();
        }
        yield return new WaitForSeconds(1f);
        onBoardSetUpFinished.Invoke();
    }

    private void SetBirdGroupParent() {
        birdGroup.transform.SetParent(transform.GetChild(gameController.playerIndex));
        birdGroup.transform.position = birdGroup.transform.parent.position;
    }

    private void RandomizeBoard() {
        layoutGroup.enabled = true;
        foreach (GameObject tile in tiles) {
            tile.transform.SetSiblingIndex(Random.Range(0, tiles.Count));
        }
        //Reset Tile list to match new order
        tiles = new List<GameObject>();
        foreach (Transform tile in transform) {
            tiles.Add(tile.gameObject);
        }
        Invoke("DisableLayoutGroup", 0.5f);
    }

    private void DisableLayoutGroup() {
        layoutGroup.enabled = false;
    }

    public void ResetBoardCorruption() {
        foreach (Transform tile in transform) {
            tile.GetComponent<NewTileBehavior>().Cleanse();
        }
    }

    public void Move() {
        foreach (Transform tile in transform) {
            tile.GetComponent<NewTileBehavior>().ShadeTile();
        }
        tiles[gameController.playerIndex].GetComponent<NewTileBehavior>().UnshadeTile();
        switch (gameController.playerIndex) {
            case 0:
                //Top left corner of grid, activate tiles to right and below
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 1:
            case 2:
                //Two top edge tiles, activate tile to right, left, and below
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 3:
                //top right corner of grid, activate tiles to left and below
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 4:
            case 8:
                //Left two edge tiles, activate above, below, and right
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 5:
            case 6:
            case 9:
            case 10:
                //Four center tiles, activate all adjacent tiles
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 7:
            case 11:
                //Right two edge tiles, activate above, below, and left
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 12:
                //Bottom left corner, activate above and right
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 13:
            case 14:
                //Bottom two edge tiles, activate left, right, and above
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 15:
                //Bottom right corner, activate above and left
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
        }
    }

    public void CompleteMove() {
        foreach (Transform tile in transform) {
            tile.GetComponent<NewTileBehavior>().DeactivateButton();
            tile.GetComponent<NewTileBehavior>().UnshadeTile();
        }
    }

    public void UpdatePlayerIndex(GameObject newTile) {
        Debug.Log("UpdatePlayerIndex");
        //Set player index
        gameController.playerIndex = tiles.FindIndex(tile => tile == newTile);
        //Set birdGroup as child of player index Tile
        SetBirdGroupParent();
    }

}
