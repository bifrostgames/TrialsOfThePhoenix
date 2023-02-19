using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour {

    public static GameController instance;
    
    //Player Stat variables
    public int playerNumStartCards;
    public int playerIndex;
    public int actionsRemaining;
    public int corruptionLevel;
    public int playerActionsPerTurn;
    public int playerHandSize;
    public int playerCardsPerDrawPhase;
    public int cardDrawsRemaining;

    //Control corruption card drawing
    [SerializeField] private CorruptionDeckBehavior corruptionDeck;
    [SerializeField] private KeyDeckBehavior keyDeck;

    //Buttons that control various game functions
    //[SerializeField] private Button corrptionCardDrawButton;
    [SerializeField] private Button keyCardDrawButton;
    [SerializeField] private Button[] actionButtons;

    //Board Manager
    [SerializeField] private NewBoardManager boardManager;

    //Flags to control order of events
    private bool boardSetUpFinished;
    private bool corruptionBeginsFinished;
    private bool dealStartingHandFinished;
    private bool actionPhaseFinished;
    private bool keyCardDrawPhaseFinished;
    private bool turnFinished;

    //Display order of play
    [SerializeField] private TMP_Text currentPhaseText; 
    [SerializeField] private TMP_Text actionsRemainingText; 

    void Awake() {
        instance = this;
    }

    void Start() {
        //TODO change to function
        DisableAllButtons();
        StartCoroutine(StartRound());
    }

    void Update() {
        //Update action remaining text
        actionsRemainingText.text = $"Actions: {actionsRemaining}";

        
    }

    //Order of play
    //Board is set up 
    //Draw 4 corruption cards (one at a time)
    //Draw 2 key cards (make sure no Corruption Spreads cards are drawn)
    //Start first action phase
        //Only buttons active should be action buttons (and any ability cards in hand)
    
    IEnumerator StartRound() {
        //Set up Board
        currentPhaseText.text = "Setting up board";
        boardManager.SetUpRound();
        yield return new WaitUntil(() => boardSetUpFinished == true);
        boardSetUpFinished = false;
        //Draw 4 corruption cards
        currentPhaseText.text = "The Corruption begins";
        CorruptionBegins();
        yield return new WaitUntil(() => corruptionBeginsFinished == true);
        corruptionBeginsFinished = false;
        //Enable keyCardDrawButton
            //Let player draw PLAYERNUMSTARTCARDS key cards (disable keyCardDrawButton after PLAYERNUMSTARTCARDS cards have been drawn)
            //If player's hand has PLAYERHANDSIZE cards in it (disable keyCardDrawButton)
            //Call special function that prevents corruption spreads cards from being drawn
        currentPhaseText.text = "Dealing starting hand";
        DealStartingHand();
        yield return new WaitUntil(() => dealStartingHandFinished == true);
        dealStartingHandFinished = false;
        //Start first turn
        StartCoroutine(StartTurn());
    }

    IEnumerator StartTurn() {
        //Enable action buttons
            //Let player take up to PLAYERACTIONSPERTURN
            //TODO: add an end turn button
            //Once player has taken PLAYERACTIONSPERTURN actions or clicked "End Turn" button
                //disable action buttons
        currentPhaseText.text = $"Action phase! Take up to {playerActionsPerTurn} actions";
        StartActionPhase();
        yield return new WaitUntil(() => actionPhaseFinished == true);
        actionPhaseFinished = false;
        EndActionPhase();
        //Enable keyCardDrawButton
            //Let player draw 2 key cards (disable keyCardDrawButton after 2 cards have been drawn)
            //If player's hand has PLAYERHANDSIZE cards in it (disable keyCardDrawButton)
            //Call function that allows corruption spreads cards to be drawn
        currentPhaseText.text = $"Draw Phase! Draw {playerCardsPerDrawPhase} cards";
        StartKeyCardDrawPhase();
        yield return new WaitUntil(() => keyCardDrawPhaseFinished == true);
        keyCardDrawPhaseFinished = false;
        yield return new WaitForSeconds(2f);
        EndKeyCardDrawPhase();
        //Draw corruption cards according to corruption level
            //Check if player loses game
        currentPhaseText.text = "The Corruption spreads";
        CorruptionSpreads();
        yield return new WaitUntil(() => corruptionBeginsFinished == true);
        corruptionBeginsFinished = false;
        //This is the end of a turn
        //turnFinished = true;
        StartCoroutine(StartTurn());
    }

    public void OnBoardSetUpFinished() {
        boardSetUpFinished = true;
    }

    public void DealStartingHand() {
        StartCoroutine(DealStartingHandEnum());
    }

    IEnumerator DealStartingHandEnum() {
        //yield return new WaitForSeconds(4f);
        Debug.Log("Dealing starting hand");
        for (int i = 0; i < playerNumStartCards; i++) {
            yield return new WaitForSeconds(2f);
            keyDeck.DrawCardStartingHand();
        }
        yield return new WaitForSeconds(2f);
        dealStartingHandFinished = true;
        Debug.Log("Finished Dealing Starting Hand");
    }

    public void CorruptionBegins() {
        StartCoroutine(CorruptionBeginsEnum());
    }

    IEnumerator CorruptionBeginsEnum() {
        //yield return new WaitForSeconds(4f);
        Debug.Log("Corruption Begins");
        for (int i = 0; i < 4; i++) {
            yield return new WaitForSeconds(2f);
            corruptionDeck.DrawCard();
        }
        yield return new WaitForSeconds(2f);
        corruptionBeginsFinished = true;
        Debug.Log("Finished Corruption Begins");
    }

    public void CorruptionSpreads() {
        StartCoroutine(CorruptionSpreadsEnum());
    }

    IEnumerator CorruptionSpreadsEnum() {
        //yield return new WaitForSeconds(4f);
        Debug.Log("Corruption Begins");
        for (int i = 0; i < 4; i++) {
            yield return new WaitForSeconds(2f);
            corruptionDeck.DrawCard();
        }
        yield return new WaitForSeconds(2f);
        corruptionBeginsFinished = true;
        Debug.Log("Finished Corruption Begins");
    }

    private void DisableAllButtons() {
        foreach (Button button in actionButtons) {
            button.interactable = false;
        }
        keyCardDrawButton.interactable = false;
    }

    private void StartKeyCardDrawPhase() {
        cardDrawsRemaining = playerCardsPerDrawPhase;
        Debug.Log("Draw phase begin");
        keyCardDrawButton.interactable = true;
    }

    private void EndKeyCardDrawPhase() {
        Debug.Log("Draw phase ended");
        keyCardDrawButton.interactable = false;
    }

    private void StartActionPhase() {
        actionsRemaining = playerActionsPerTurn;
        Debug.Log("Action phase begin");
        foreach (Button button in actionButtons) {
            button.interactable = true;
        }
    }

    private void EndActionPhase() {
        Debug.Log("Action phase ended");
        foreach (Button button in actionButtons) {
            button.interactable = false;
        }
    }

    public void OnEndTurnEarly() {
        actionsRemaining = 0;
        actionPhaseFinished = true;
    }

    public void OnActionCompleted() {
        actionsRemaining--;
        if (actionsRemaining == 0) {
            actionPhaseFinished = true;
        }
    }

    public void OnCardDrawn() {
        cardDrawsRemaining--;
        if (cardDrawsRemaining == 0) {
            keyCardDrawPhaseFinished = true;
        }
        Debug.Log(keyDeck.numCardsInHand);
        if (keyDeck.numCardsInHand == playerHandSize) {
            keyCardDrawPhaseFinished = true;
        }
    }

}
