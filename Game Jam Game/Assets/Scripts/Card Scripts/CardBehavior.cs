using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBehavior : MonoBehaviour {
    
    public KeyCardObject cardObject;
    [SerializeField] private Image cardFace;

    void Start() {
        cardFace.sprite = cardObject.cardFace;
    }

}
