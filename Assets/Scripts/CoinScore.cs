using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinScore : MonoBehaviour
{
    [SerializeField] Text _score;
    private static PlayerController _player;
    private int _coinCount;

    private void Start()
    {
        _coinCount = 0;
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        _player.onCoinTake += ChangeScore;
        _score.text = _coinCount.ToString();
     }

    public void ChangeScore()
    {
        _coinCount++;
        _score.text = _coinCount.ToString(); 
    }
}


