using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BingoMenu : MonoBehaviour
{
    private void Start() {
        if (PlayerPrefs.HasKey("HasBingo")) {
            gameObject.SetActive(true);
        }
        else {
            gameObject.SetActive(false);
        }
    }
}
