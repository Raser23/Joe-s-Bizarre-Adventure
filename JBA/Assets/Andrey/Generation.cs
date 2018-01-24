using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Generation : MonoBehaviour
{
    public GameObject example;
    public int number_of_rooms = 100;
    public List<Vector2> possible_points = new List<Vector2>();
    public List<Vector2> points = new List<Vector2>();
    public Vector2 trash;
    // Use this for initialization
    void Start()
    {
        trash.x = 0;
        trash.y = 0;
        possible_points.Add(trash);
        int sum_of_weight = 0;
        System.Random rnd = new System.Random();
        /*     
             Random rnd = new Random();

             trash.x = 0;
             trash.y = 1;
             possible_points.Add(trash);

             trash.x = 1;
             trash.y = 0;
             possible_points.Add(trash);

             trash.x = 0;
             trash.y = -1;
             possible_points.Add(trash);

             trash.x = -1;
             trash.y = 0;
             possible_points.Add(trash);
             */
        sum_of_weight = Weight(Vector2.zero);
        while (number_of_rooms > 0)
        {
            int jopa_govna = 0;
            foreach (Vector2 ad in possible_points)
            {
                jopa_govna += Weight(ad);
            }
            sum_of_weight = jopa_govna;
            int a = rnd.Next(1, sum_of_weight);
            //print(a.ToString() + ' ' + sum_of_weight.ToString() + ' ' + number_of_rooms.ToString());
            int counter = -1;
            while (a > 0)
            {
                counter++;
                a -= Weight(possible_points[counter]);
            }
            points.Add(possible_points[counter]);
            GameObject instance = Instantiate(Resources.Load("Room_template", typeof(GameObject))) as GameObject;
            trash = possible_points[counter];
            instance.transform.position = new Vector3(trash.x*15, 100, trash.y*15);
            
            List<Vector2> n = new List<Vector2>() { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1) };

            foreach (Vector2 vec in n)
            {
                Vector2 bt = trash + vec;
                if ((possible_points.IndexOf(bt) == -1) && (points.IndexOf(bt) == -1))
                {
                    int near = 0;
                    foreach (Vector2 vec1 in n)
                    {
                        if (points.IndexOf(bt + vec1) != -1)
                        {
                            near++;
                        }
                    }
                    if (near <= 1)
                    {
                        possible_points.Add(bt);
                     //   sum_of_weight += Weight(bt);
                    }
                }
                else
                {
                    if (possible_points.IndexOf(bt) > -1)
                    {
                     //   sum_of_weight -= Weight(bt);
                        possible_points.Remove(bt);
                    }
                }
            }
         //   sum_of_weight -= Weight(possible_points[counter]);
            possible_points.Remove(trash);
            number_of_rooms--;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    int Weight(Vector2 a)
    {
        return (int)(1000*number_of_rooms/(Mathf.Sqrt(a.x*a.x + a.y*a.y)+1));
    }
}
