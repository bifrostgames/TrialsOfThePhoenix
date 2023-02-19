using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class KeyDeckBehavior : MonoBehaviour {
    
    [SerializeField] List<KeyCardObject> drawPile = new List<KeyCardObject>();
    [SerializeField] List<KeyCardObject> discardPile = new List<KeyCardObject>();
    [SerializeField] List<KeyCardObject> hand = new List<KeyCardObject>();
    [SerializeField] private Image deckImage;
    [SerializeField] private Sprite deckSprite;
    public GameObject cardPrefab;
    public Transform handParent;

    //Control limiting number of cards player draws per draw phase
    [SerializeField] private UnityEvent cardDrawn;
    public int numCardsInHand;

    //void Update() {
    //    //numCardsInHand = hand.Count;
    //}

    public void DrawCard() {
        if (drawPile.Count > 0) {
            KeyCardObject randCard = drawPile[Random.Range(0, drawPile.Count)];
            drawPile.Remove(randCard);
            hand.Add(randCard);
            
            //Invoke onCardDrawn event
            numCardsInHand = hand.Count;
            cardDrawn.Invoke();

            //Set deck sprite to randCard sprite
            deckImage.sprite = randCard.cardFace;

            //Check if card is a Corruption Spreads card before adding it to the deck
            StartCoroutine(AddToHand(randCard));
            
        }
    }

    public void DrawCardStartingHand() {
        //TODO change this to not draw any corruption cards
        if (drawPile.Count > 0) {
            KeyCardObject randCard = drawPile[Random.Range(0, drawPile.Count)];
            drawPile.Remove(randCard);
            hand.Add(randCard);

            //Set deck sprite to randCard sprite
            deckImage.sprite = randCard.cardFace;

            //Check if card is a Corruption Spreads card before adding it to the deck
            StartCoroutine(AddToHand(randCard));
            
        }
    }

    IEnumerator AddToHand(KeyCardObject newCard) {
        yield return new WaitForSeconds(1f);
        deckImage.sprite = deckSprite;
        GameObject newCardObject = Instantiate(cardPrefab, handParent);
        newCardObject.GetComponent<CardBehavior>().cardObject = newCard;

        if (drawPile.Count == 0) {
            ShuffleDiscard();
        }
    }

    public void ShuffleDiscard() {
        foreach (KeyCardObject card in discardPile) {
            drawPile.Add(card);
        }
        discardPile.Clear();
        //Change discard pile sprite to deck placeholder sprite
    }

    public void OnCardDrawn() {
        cardDrawn.Invoke();
    }

}
