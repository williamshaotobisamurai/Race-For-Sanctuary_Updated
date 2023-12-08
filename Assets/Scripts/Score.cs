using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Transform Player;
    public Text scoreText;

    // Update is called once per frame
    void Update() {
        float score = Mathf.Max(0, Player.position.z);
        scoreText.text = score.ToString("0");
    }
}