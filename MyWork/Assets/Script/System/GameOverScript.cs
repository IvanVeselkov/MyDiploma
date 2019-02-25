using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public GameObject _player;
    public GameObject _enemy;
    public Text _MesText;
    public GameObject _point;

    public float _timer;
    public bool _over;
    // Start is called before the first frame update
    void Start()
    {
        _MesText.text = "";
        _timer = -1f;
        _over = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_over)
        {
            _timer -= Time.deltaTime;
            if(_timer<0)
            {
                SceneManager.LoadScene("SampleScene");
                _over = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!_over)
        {
            if (other.tag == _enemy.tag)
            {
                _MesText.text = "You lose!";
                _player.GetComponent<Rigidbody>().freezeRotation = true;
                _timer = 5f;
                _over = true;
            }
            if (other.tag == _point.tag)
            {
                _MesText.text = "You win!";
                _player.GetComponent<Rigidbody>().freezeRotation = true;
                _timer = 5f;
                _over = true;
            }
        }
    }
}
