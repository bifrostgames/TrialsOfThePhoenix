using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Key Card", menuName = "ScriptableObject/Card/KeyCard")]
public class KeyCardObject : ScriptableObject {

    public Sprite cardFace;
    public string cardName;
    public string description;

}
