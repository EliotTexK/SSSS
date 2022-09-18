using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMessage : MonoBehaviour
{
    //singleton
    public static GameMessage Instance { get; private set; }
    private void Awake()
    {
        // singleton
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }
    public Text messageText;

    void Start() {
        messageText = GetComponent<Text>();
    }
}
