using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Generation : MonoBehaviour
{
    enum Room_type { Regular, Golden, Boss, Big };
    public struct Room
    {
        public Vector2 position;
        public GameObject obj;
        public string type;
    }
    public GameObject example;
    public int number_of_rooms = 50;
    public List<Vector2> possible_points = new List<Vector2>();
    public List<Vector2> points = new List<Vector2>();
    public List<Room> rooms = new List<Room>();
    public Vector2 trash;
    // Use this for initialization
    void Start()
    {
        
        trash.x = 0;
        trash.y = 0;
        possible_points.Add(trash);
        int sum_of_weight = 0;
        System.Random rnd = new System.Random();
        int golden = rnd.Next(2, number_of_rooms - 1);
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

            int counter = -1;
            while (a > 0)
            {
                counter++;
                a -= Weight(possible_points[counter]);
            }

            trash = possible_points[counter];
            points.Add(trash);

            Room trash_room;
            trash_room.type = "Regular";
            GameObject instance = this.gameObject; //crutch
            if (number_of_rooms == golden)
            {
                trash_room.type = "Golden";
                instance = Instantiate(Resources.Load("Room_Golden_template", typeof(GameObject))) as GameObject;
            }
            if (number_of_rooms == 1)
            {
                trash_room.type = "Boss";
                instance = Instantiate(Resources.Load("Room_Boss_template", typeof(GameObject))) as GameObject;
            }
            if (trash_room.type == "Regular")
            {
                instance = Instantiate(Resources.Load("Room_template", typeof(GameObject))) as GameObject;
            }            
            instance.transform.position = new Vector3(trash.x * 20, 100, trash.y * 20);
            trash_room.position = trash;
            trash_room.obj = instance;


            rooms.Add(trash_room);

            List<Vector2> n = new List<Vector2>() { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1) };

            foreach (Vector2 vec in n)
            {
                Vector2 bt = trash + vec;
                if ((possible_points.IndexOf(bt) == -1) && (points.IndexOf(bt) == -1) && (trash_room.type == "Regular"))
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
                    }
                }
                else
                {
                    if (possible_points.IndexOf(bt) > -1)
                    {
                        possible_points.Remove(bt);
                    }
                }
            }
            possible_points.Remove(trash);
            number_of_rooms--;
        }
        Wall_generation();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Wall_generation()
    {
        foreach (Room room in rooms)
        {
            foreach (Transform child in room.obj.transform)
            if (child.tag == "Marker")
            {
                Vector2 vec;
                GameObject instance_wall;
                switch (child.name)
                {
                    case "Wall_Position_Z+":
                        vec.x = 0;
                        vec.y = 1;
                        if (points.IndexOf(room.position + vec) == -1)
                        {
                            instance_wall = Instantiate(Resources.Load("Wall_template", typeof(GameObject))) as GameObject;
                        }
                        else
                        {
                            instance_wall = Instantiate(Resources.Load("Wall_Door_template", typeof(GameObject))) as GameObject;
                        }
                        instance_wall.transform.position = child.transform.position;
                        instance_wall.transform.rotation = child.transform.rotation;
                        instance_wall.transform.SetParent(child.transform);
                        break;
                    case "Wall_Position_Z-":
                        vec.x = 0;
                        vec.y = -1;
                        if (points.IndexOf(room.position + vec) == -1)
                        {
                            instance_wall = Instantiate(Resources.Load("Wall_template", typeof(GameObject))) as GameObject;
                        }
                        else
                        {
                            instance_wall = Instantiate(Resources.Load("Wall_Door_template", typeof(GameObject))) as GameObject;
                        }
                        instance_wall.transform.position = child.transform.position;
                        instance_wall.transform.rotation = child.transform.rotation;
                        instance_wall.transform.SetParent(child.transform);
                        break;
                    case "Wall_Position_X+":
                        vec.x = 1;
                        vec.y = 0;
                        if (points.IndexOf(room.position + vec) == -1)
                        {
                            instance_wall = Instantiate(Resources.Load("Wall_template", typeof(GameObject))) as GameObject;
                        }
                        else
                        {
                            instance_wall = Instantiate(Resources.Load("Wall_Door_template", typeof(GameObject))) as GameObject;
                        }
                        instance_wall.transform.position = child.transform.position;
                        instance_wall.transform.rotation = child.transform.rotation;
                        instance_wall.transform.SetParent(child.transform);
                        break;
                    case "Wall_Position_X-":
                        vec.x = -1;
                        vec.y = 0;
                        if (points.IndexOf(room.position + vec) == -1)
                        {
                            instance_wall = Instantiate(Resources.Load("Wall_template", typeof(GameObject))) as GameObject;
                        }
                        else
                        {
                            instance_wall = Instantiate(Resources.Load("Wall_Door_template", typeof(GameObject))) as GameObject;
                        }
                        instance_wall.transform.position = child.transform.position;
                        instance_wall.transform.rotation = child.transform.rotation;
                        instance_wall.transform.SetParent(child.transform);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    
    int Weight(Vector2 a)
    {
        return (int)(1000 * number_of_rooms / (Mathf.Sqrt(a.x * a.x + a.y * a.y) + 1));
    }
}
