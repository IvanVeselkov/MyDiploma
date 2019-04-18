using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager01 : MonoBehaviour
{
    [Header("игрок")]
    public GameObject _person;//персонаж в озере
    [Header("Орк")]
    public GameObject _enemy;//орк
    [Header("Радиус окружности")]
    public float _R;//радиус окружности
    [Header("Оптимальная точка входа")]
    public float _phi;//угол между персонажем и центром окружности
    public float _phig;
    [Header("Точка маркер")]
    public GameObject _movePoint;//Точка входа в озеро
    [Header("Для угла альфа")]
    public GameObject _S_Point;//точка для нахожения угла альфа
    [Header("Точка касания")]
    public GameObject _T_Point;
    [Header("Beta Point")]
    public GameObject _B_Point;
    [Header("OrcAnglePoint")]
    public GameObject _O_Point;
    float _speedGround;//скорость гоблина по земле
    float _speedLake;//скоровсть гоблина по озеру
    [Header("угол касательной орка")]
    public float _angleT; // угол для определения диапазона 
    public float _angleTg;
    
    //float _phiSearch;//угол для поиска минимума(возможна замена)
    [Header("угол альфа точки пересечения прямой Игрок-Орк и озера")]
    public float _alpha; //угол между точкой входа и началом координат
    public float _alphag;
    [Header("угол между игроком и начаом координат")]
    public float _beta; //угол между игроком и началом координат
    public float _betag;

    [Header("Угол орка")]
    public float _orcAngle;
    public float _orcAngleg;
    // Start is called before the first frame update
    void Start()
    {
        _speedGround = 8f;
        _speedLake = 3f;
        _phi = 0f;
        _angleT = 0f;
        _alpha = 0f;
        _beta = 0f;
        _phig = 0f;
        _angleTg = 0f;
        _alphag = 0f;
        _betag = 0f;
        _orcAngle =0f;
        _orcAngleg =0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Вычисление точки S
        CalcS_PointDestination(_person.transform.position.x,_person.transform.position.z,_enemy.transform.position.x,_enemy.transform.position.z);
        //Вычисление угла альфа
        CalcAngleAlpha(_S_Point.transform.position.x,_S_Point.transform.position.z);
        //Вычисление угла бета
        CalcAngleBeta(_person.transform.position.x,_person.transform.position.z);
        //Вычисление угла орка
        CalcOrcAngle(_enemy.transform.position.x,_enemy.transform.position.z);
        //Вычисление точки Т
        CalcT_PointDestination(_enemy.transform.position.x,_enemy.transform.position.z);
        //Вычисление угла тета
        CalcAngleTet(_T_Point.transform.position.x,_T_Point.transform.position.z);
        
        CalcAngPhi(_alpha,_beta,_angleT);
        


        Vector3 _pos = new Vector3(_R * Mathf.Cos(_phi), 0.5f, _R * Mathf.Sin(_phi));

        _movePoint.transform.SetPositionAndRotation(_pos, _movePoint.transform.localRotation);


    }

    void CalcAngPhi(float _alpha,float _beta,float _teta)
    {
        float _toOrcDistance;
        float _toPersonDistance;
        float _minTime = float.MaxValue;
        //float chec_beta = _beta;
        //float chec_teta = _teta;
        
        if(_beta<=_teta )
        {
            while(_alpha<=_teta)
            {
                _toOrcDistance = Mathf.Sqrt(Mathf.Pow(_enemy.transform.position.x - _R * Mathf.Cos(_alpha),2)+
                                    Mathf.Pow(_enemy.transform.position.z - _R * Mathf.Sin(_alpha),2));
                _toPersonDistance = Mathf.Sqrt(Mathf.Pow(_person.transform.position.x - _R * Mathf.Cos(_alpha),2)+
                                    Mathf.Pow(_person.transform.position.z -  _R * Mathf.Sin(_alpha),2));
                if(_minTime>(_toOrcDistance/8+_toPersonDistance/3))
                {
                    _minTime = _toOrcDistance/8+_toPersonDistance/3;
                    _phi=_alpha;
                }
                _alpha+=Mathf.PI/180;
            }
        }
        else
        {
            while(_teta<=_beta)
            {
                _toOrcDistance = Mathf.Sqrt(Mathf.Pow(_enemy.transform.position.x - _R * Mathf.Cos(_teta),2)+
                                    Mathf.Pow(_enemy.transform.position.z - _R * Mathf.Sin(_teta),2));
                _toPersonDistance = Mathf.Sqrt(Mathf.Pow(_person.transform.position.x - _R * Mathf.Cos(_teta),2)+
                                    Mathf.Pow(_person.transform.position.z -  _R * Mathf.Sin(_teta),2));
                if(_minTime>(_toOrcDistance/8+_toPersonDistance/3))
                {
                    _minTime = _toOrcDistance/8+_toPersonDistance/3;
                    _phi=_teta;
                }
                _teta+=5*Mathf.PI/180;
            }
        }
    }


    void CalcT_PointDestination(float ox,float oy)
    {
        float Tx = ox,Ty=oy;
        if(_R < Mathf.Sqrt(Mathf.Pow(ox,2)+Mathf.Pow(oy,2)))
        {
            float C_1 = Mathf.Pow(_R,4)/Mathf.Pow(ox,2) - Mathf.Pow(_R,2);
            float C_2 = 2*oy*Mathf.Pow(_R,2)/Mathf.Pow(ox,2);
            float C_3 = Mathf.Pow(oy,2)/Mathf.Pow(ox,2) + 1;
            float D = Mathf.Pow(C_2,2) - 4*C_1*C_3;
            if(D==0)
            {
                Ty = C_2/2*C_3;
                Tx = (Mathf.Pow(_R,2)-oy*Ty)/ox;
            }else
            {
                if(D>0)//буду брать всегда первый корень
                {
                    if(_beta>_orcAngle)
                    {
                        Ty = (C_2 - Mathf.Sqrt(D))/2*C_3;
                        Tx = (Mathf.Pow(_R,2)-oy*Ty)/ox;
                    }
                    else
                    {
                        Ty = (C_2 + Mathf.Sqrt(D))/2*C_3;
                        Tx = (Mathf.Pow(_R,2)-oy*Ty)/ox;
                    }
                }
            }
        }
        Vector3 _pos = new Vector3(Tx,0.5f,Ty);
        _T_Point.transform.SetPositionAndRotation(_pos, _T_Point.transform.localRotation);
    }
    void CalcAngleTet(float Tx,float Ty)//угол касательной для точки прохода по окружности
    {
        if (Tx > 0)
        {
            if (Ty >= 0)
            {
                _angleT = Mathf.Atan2(Ty, Tx);
            }
            else
            {
                _angleT= Mathf.Atan2(Ty, Tx) + 2 * Mathf.PI;
            }
        }
        else
        {
            if (Tx < 0)
            {
                if(Ty>0)
                {
                    _beta  = Mathf.Atan2(Ty, Tx);
                }
                else
                {
                    _beta =2* Mathf.PI+    Mathf.Atan2(Ty, Tx);
                }
            }
            if (Tx == 0)
            {
                if (Ty > 0)
                {
                    _angleT = Mathf.PI / 2;
                }
                else
                {
                    _angleT= 3 * Mathf.PI / 2;
                }
            }
        }
        Vector3 _pos = new Vector3(_R * Mathf.Cos(_angleT), 0.5f, _R * Mathf.Sin(_angleT));
        _T_Point.transform.SetPositionAndRotation(_pos, _T_Point.transform.localRotation);
        _angleTg = _angleT*180/Mathf.PI;
        
    }

    void CalcAngleBeta(float x,float y)//находится из координат игрока
    {
        if (x > 0)
        {
            if (y >= 0)
            {
                _beta = Mathf.Atan2(y, x);
            }
            else
            {
                _beta  = Mathf.Atan2(y, x) + 2 * Mathf.PI;
            }
        }
        else
        {
            if (x < 0)
            {
                if(y>0)
                {
                    _beta  = Mathf.Atan2(y, x);
                }
                else
                {
                    _beta =2* Mathf.PI+    Mathf.Atan2(y, x);
                }
            }
            if (x == 0)
            {
                if (y > 0)
                {
                    _beta  = Mathf.PI / 2;
                }
                else
                {
                    _beta  = 3 * Mathf.PI / 2;
                }
            }
        }
        Vector3 _pos = new Vector3(_R * Mathf.Cos(_beta), 0.5f, _R * Mathf.Sin(_beta));
        _B_Point.transform.SetPositionAndRotation(_pos, _S_Point.transform.localRotation);
        _betag=_beta*180/Mathf.PI;
    }
    void CalcS_PointDestination(float px,float py,float ox,float oy)
    {
        float Sx = 0f;
        float Sy = 0f;
        float C_1 = Mathf.Pow(px - ox,2)+Mathf.Pow(py-oy,2);
        float C_2 = (2*ox*(px-ox)+2*oy*(py-oy));
        float C_3 = Mathf.Pow(ox,2)+Mathf.Pow(oy,2)-Mathf.Pow(_R,2);
        float D = Mathf.Pow(C_2,2)-4*C_1*C_3;
        float t1 = (-C_2 - Mathf.Sqrt(D))/(2*C_1);
        float t2 = (-C_2 + Mathf.Sqrt(D))/(2*C_1);
        float t = 0;
        if(t1 >= 0 && t1 < 1 )
        {
            t = t1;
        }
        if(t2 >= 0 && t2 < 1)
        {
            t = t2;
        }
        Sx = (ox + t*(px-ox));
        Sy = (oy + t*(py-oy));
        Vector3 _pos = new Vector3(Sx,0.5f,Sy);
        _S_Point.transform.SetPositionAndRotation(_pos, _S_Point.transform.localRotation);
    }
//угор альфа вычисляется в градусах(пока что)
    void CalcAngleAlpha(float Sx,float Sy)//вычисляется с помощью точки S , а угол находится так же как и в бете
    {
        
        if (Sx > 0)
        {
            if (Sy >= 0)
            {
                _alpha = Mathf.Atan2(Sy, Sx);
            }
            else
            {
                _alpha = Mathf.Atan2(Sy, Sx) + 2 * Mathf.PI;
            }
        }
        else
        {
            if (Sx < 0)
            {
                if(Sy>0)
                {
                    _beta  = Mathf.Atan2(Sy, Sx);
                }
                else
                {
                    _beta =2* Mathf.PI+    Mathf.Atan2(Sy, Sx);
                }
            }
            if (Sx == 0)
            {
                if (Sy > 0)
                {
                    _alpha = Mathf.PI / 2;
                }
                else
                {
                    _alpha = 3 * Mathf.PI / 2;
                }
            }
        }
        _alphag = _alpha*180/Mathf.PI;
    }

    void CalcOrcAngle(float ox,float oy)
    {
         if (ox > 0)
        {
            if (oy >= 0)
            {
                _orcAngle = Mathf.Atan2(oy, ox);
            }
            else
            {
                _orcAngle = Mathf.Atan2(oy, ox) + 2 * Mathf.PI;
            }
        }
        else
        {
            if (ox < 0)
            {
                if(oy>0)
                {
                    _beta  = Mathf.Atan2(oy, ox);
                }
                else
                {
                    _beta =2* Mathf.PI+    Mathf.Atan2(oy, ox);
                }
            }
            if (ox == 0)
            {
                if (oy > 0)
                {
                    _orcAngle  = Mathf.PI / 2;
                }
                else
                {
                    _orcAngle  = 3 * Mathf.PI / 2;
                }
            }
        }
        Vector3 _pos = new Vector3(_R * Mathf.Cos(_orcAngle), 0.5f, _R * Mathf.Sin(_orcAngle));
        _O_Point.transform.SetPositionAndRotation(_pos, _S_Point.transform.localRotation);
        _orcAngleg=_orcAngle*180/Mathf.PI;
    }
}
