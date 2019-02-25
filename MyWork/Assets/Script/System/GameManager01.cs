using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager01 : MonoBehaviour
{
    public GameObject _person;//персонаж в озере
    public float _R;//радиус окружности
    private float _phi;//угол между персонажем и центром окружности
    public GameObject _movePoint;//точка на окружности к которой стремится гоблин
  

    // Start is called before the first frame update
    void Start()
    {
        //_R = 11f;
        _phi = 0f;

    }

    // Update is called once per frame
    void Update()
    {

        CalcAng(_person.transform.position.x, _person.transform.position.z);//вычисление угла

        Vector3 _pos = new Vector3(_R * Mathf.Cos(_phi), 0.5f, _R * Mathf.Sin(_phi));

        _movePoint.transform.SetPositionAndRotation(_pos, _movePoint.transform.localRotation);


    }

    void CalcAng(float x, float y)
    {
        if (x > 0)
        {
            if (y >= 0)
            {
                _phi = Mathf.Atan2(y, x);
            }
            else
            {
                _phi = Mathf.Atan2(y, x) + 2 * Mathf.PI;
            }
        }
        else
        {
            if (x < 0)
            {
                _phi = Mathf.Atan2(y, x);
            }
            if (x == 0)
            {
                if (y > 0)
                {
                    _phi = Mathf.PI / 2;
                }
                else
                {
                    _phi = 3 * Mathf.PI / 2;
                }
            }
        }
    }
}
