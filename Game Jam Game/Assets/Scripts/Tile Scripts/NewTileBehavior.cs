using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewTileBehavior : MonoBehaviour {
    
    //Control tile image
    [SerializeField] private Image tileImage;
    [SerializeField] private Sprite tileFace;
    [SerializeField] private Sprite tileBack;
    [SerializeField] private Sprite tileCorrupted;

    //Control tile text
    [SerializeField] private TMP_Text locationNameText;
    public string locationName;

    //Tile Button
    [SerializeField] Button tileButton;
    [SerializeField] private Image shadeImage;

    //Keep track of what face is showing
    [SerializeField] private bool isFaceUp;
    //Keep track of corruption leve
    [SerializeField] private bool isCorrupted;

    void Start() {
        //Start tile face down
        tileImage.sprite = tileBack;
        locationNameText.text = "";
        //Set button to uninteractable
        tileButton.interactable = false;
        //Deactivate shade image
        UnshadeTile();
    }

    //Initiate a flip
    public void Flip() {
        StartCoroutine(FlipEnum());
    }

    IEnumerator FlipEnum() {
        int timer = 0;
        for(int i = 0; i < 180; i++) {
            yield return new WaitForSeconds(0.0001f);
            transform.Rotate(new Vector3(0, 1, 0));
            timer++;

            if (timer == 90) {
                //Flip card's x scale so that the image and text are not backwards
                transform.localScale = new Vector3(transform.localScale.x*-1, 1, 1);
                //Select the correct image sprite
                if (isFaceUp) {
                    //If tile is face up, switch to face down
                    tileImage.sprite = tileBack;
                    locationNameText.text = "";
                    //set boolean variable
                    isFaceUp = false;
                }
                else {
                    //If tile is not face up, switch to face up
                    tileImage.sprite = tileFace;
                    locationNameText.text = locationName;
                    //set boolean variable
                    isFaceUp = true;
                }
            }
        }
        timer = 0;
    }

    public void Corrupt() {
        if (isCorrupted) {
            //If the tile is corrupted, remove it
            gameObject.SetActive(false);
            isCorrupted = false;
        }
        else {
            //If the tile is not corrupted, make it corrupted
            tileImage.sprite = tileCorrupted;
            isCorrupted = true;
        }
    }

    public void Cleanse() {
        //tileImage.sprite = tileFace;
        isCorrupted = false;
    }

    public void ActivateButton() {
        tileButton.interactable = true;
    }

    public void DeactivateButton() {
        tileButton.interactable = false;
    }

    public void ShadeTile() {
        //Activate shade image
        shadeImage.enabled = true;
    }

    public void UnshadeTile() {
        //Deactivate shade image
        shadeImage.enabled = false;
    }

    public void MoveHere() {
        Debug.Log($"Move to {this.name}");
        NewBoardManager board = GetComponentInParent<NewBoardManager>();
        board.UpdatePlayerIndex(gameObject);
        board.CompleteMove();
    }

}
