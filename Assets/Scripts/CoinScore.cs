using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinScore : MonoBehaviour
{
    [SerializeField] Text _score;
    private static PlayerController _player;
    public int CoinCount;

    private void Start()
    {
        CoinCount = 0;
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        _player.onCoinTake += ChangeScore;
        _score.text = CoinCount.ToString();
     }

    public void ChangeScore()
    {
        CoinCount++;
        _score.text = CoinCount.ToString(); 
    }

    public void SpendCoins(int coins)
    {
        CoinCount -= coins;
        _score.text = CoinCount.ToString();
    }
}


