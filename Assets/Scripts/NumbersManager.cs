using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NumbersManager : MonoBehaviour
{
    public Transform NumberPrefab;
    public int numberInLine;
    int numbersCount;
    float scaleNumber;
    public float maxDistance = 280;
    public Vector3 missingPole;
    AudioSource audioSource;
    public AudioClip FalseClip;
    public AudioClip TrueClip;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        numberInLine = LvlManager.numberInLine;
        Lvl();
    }
    void Lvl()
    {
        numbersCount = numberInLine * numberInLine - 1; //количество плиток с пропуском одной
        scaleNumber = maxDistance / numberInLine; //размер одной плитки
        missingPole = NumberInVector(numbersCount);
        for (int i = 0; i < numbersCount; i++)
        {
            Transform newNumber = Instantiate(NumberPrefab);
            newNumber.SetParent(transform);
            newNumber.localScale = new Vector3(scaleNumber, scaleNumber, 1);
            newNumber.GetComponent<TextMesh>().text = (i + 1).ToString(); //порядковый номер плитки
        }
        RandPos();
    }
    void RandPos() //раздаём каждой плитке случайное место
    {
        List<int> l = new List<int>();
        List<int> p = new List<int>();
        for (int i = 0; i < numbersCount; i++)
        {
            l.Add(i);
            p.Add(i);
        }
        for (int i = 0; i < numbersCount; i++)
        {
            int r = Random.Range(0, l.Count);
            transform.GetChild(i).localPosition = NumberInVector(l[r]);
            p[l[r]] = i;
            l.RemoveAt(r);
        }
        int missingCount = numberInLine; //проверка на собираемость
        for(int i = 0; i < numbersCount; i++)
        {
            for(int j = i; j < numbersCount; j++)
            {
                if (p[i] > p[j])
                    missingCount++;
            }
        }
        if (missingCount % 2 != numberInLine % 2) //если собрать невозможно, что меняем местами соседние плитки
        {
            Vector3 _position = transform.GetChild(0).localPosition;
            transform.GetChild(0).localPosition = transform.GetChild(1).localPosition;
            transform.GetChild(1).localPosition = _position;
        }
        if (Win()) //если в результате генерации сформировалась выиграшаная 
        {
            RandPos();
        }
    }
    bool Win()
    {
        for(int i = 0; i < numbersCount; i++)
        {
            if (transform.GetChild(i).localPosition != NumberInVector(i))
                return false;
        }
        return true;
    }
    public void NewTarget(Transform number) //перемещение плитки
    {
        Vector3 _x = number.localPosition;
        Vector3 naprav = missingPole - _x;
        if ((naprav.x == 0 || naprav.y == 0)&& naprav.z == 0)
        {
            Vector3 izmen = (naprav).normalized * scaleNumber;
            if (naprav.magnitude > scaleNumber)
            {
                number.GetComponent<Collider2D>().enabled = false;
                NewTarget(Physics2D.Raycast(number.position , naprav).transform);
                number.GetComponent<Collider2D>().enabled = true;
            }
            else
            {
                audioSource.Play();
            }
            number.localPosition += izmen;
            missingPole = _x;
        }
        if (Win())
        {
            LvlManager.numberInLine++;
            SceneManager.LoadScene(0);
        }
    }
    Vector3 NumberInVector(int n) //Перевод порядкового номера ячейки в локальные координаты
    {
        return new Vector3((scaleNumber - maxDistance) * 0.5f + scaleNumber * (n % numberInLine), ((scaleNumber - maxDistance) * 0.5f + scaleNumber * (n / numberInLine)) * -1, 0);
    }
}
