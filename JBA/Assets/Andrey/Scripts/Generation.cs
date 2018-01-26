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
    public int chance_of_big = 10;
    public int max_number_of_big;
    public int number_of_rooms = 50;
    public int number_of_rooms_start;
    public List<Vector2> possible_points = new List<Vector2>();
    public List<Vector2> points = new List<Vector2>();
    public List<Room> rooms = new List<Room>();
    public List<Vector2> connection = new List<Vector2>();//better use pair, but idk how
    public Vector2 trash;
    // Use this for initialization
    void Start()
    {
        number_of_rooms_start = number_of_rooms;
        max_number_of_big = 3;
        trash.x = 0;
        trash.y = 0;
        possible_points.Add(trash);
        int sum_of_weight = 0;
        System.Random rnd = new System.Random();
        int golden = rnd.Next(2, number_of_rooms - 1);
        sum_of_weight = Weight(Vector2.zero);
        while (number_of_rooms > 0)
        {
            sum_of_weight = 0;
            foreach (Vector2 ad in possible_points)
            {
                sum_of_weight += Weight(ad);
            }

            int a = rnd.Next(1, sum_of_weight);

            int counter = -1;
            while (a > 0)
            {
                counter++;
                a -= Weight(possible_points[counter]);
            }

            trash = possible_points[counter];
            points.Add(trash);//fishka 1
            int total_trash = 0; //adding hall

            List<Vector2> nn = new List<Vector2>() { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1) };

            foreach (Vector2 vec in nn)
            {
                if (points.IndexOf(trash + vec) > -1)
                {
                    total_trash++;
                }
            }
            if (total_trash == 1)
            {
                foreach (Vector2 vec in nn)
                {
                    if (points.IndexOf(trash + vec) > -1)
                    {
                        GameObject instance_hall = Instantiate(Resources.Load("Hall_doors", typeof(GameObject))) as GameObject;
                        instance_hall.transform.position = place(trash + vec * 0.5f);//new Vector3(trash.x * 20 + vec.x * 10, 100, trash.y * 20 + vec.y * 10);
                        connection.Add(new Vector2(points.IndexOf(trash + vec), points.IndexOf(trash)));
                        connection.Add(new Vector2(points.IndexOf(trash), points.IndexOf(trash + vec)));
                        if (vec.x == 0)
                        {
                            instance_hall.transform.Rotate(Vector3.up, 90, Space.World);
                        }
                    }
                }
            }
            if (total_trash >= 2)
            {
                foreach (Vector2 vec in nn)
                {
                    if ((points.IndexOf(trash + vec) > -1) && (rooms[points.IndexOf(trash + vec)].type == "Big"))
                    {
                        GameObject instance_hall = Instantiate(Resources.Load("Hall_doors", typeof(GameObject))) as GameObject;
                        instance_hall.transform.position = place(trash + vec * 0.5f);//new Vector3(trash.x * 20 + vec.x * 10, 100, trash.y * 20 + vec.y * 10);
                        connection.Add(new Vector2(points.IndexOf(trash + vec), points.IndexOf(trash)));
                        connection.Add(new Vector2(points.IndexOf(trash), points.IndexOf(trash + vec)));
                        if (vec.x == 0)
                        {
                            instance_hall.transform.Rotate(Vector3.up, 90, Space.World);
                        }
                        break;
                    }
                }
            }//done adding hall
            points.Remove(trash);//fishka 2

            bool Add_Big = false;
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
                int b = rnd.Next(1, chance_of_big + 1);
                if ((number_of_rooms < number_of_rooms_start - 5) && (check_big_possibility(trash) != Vector2.zero) && (b <= max_number_of_big))
                {
                    max_number_of_big--;
                    chance_of_big += 10;
                    Room trash_room_0_1, trash_room_1_0, trash_room_1_1, trash_room_0_0;
                    Add_Big = true;
                    Vector2 big_vec = check_big_possibility(trash);
                    GameObject instance_0_0 = Instantiate(Resources.Load((-big_vec.x).ToString() + "_" + (-big_vec.y).ToString(), typeof(GameObject))) as GameObject;
                    GameObject instance_0_1 = Instantiate(Resources.Load((-big_vec.x).ToString() + "_" + (big_vec.y).ToString(), typeof(GameObject))) as GameObject;
                    GameObject instance_1_0 = Instantiate(Resources.Load((big_vec.x).ToString() + "_" + (-big_vec.y).ToString(), typeof(GameObject))) as GameObject;
                    GameObject instance_1_1 = Instantiate(Resources.Load((big_vec.x).ToString() + "_" + (big_vec.y).ToString(), typeof(GameObject))) as GameObject;

                    GameObject instance_other = Instantiate(Resources.Load("Big_Other", typeof(GameObject))) as GameObject;
                    GameObject instance_Big_Room = Instantiate(Resources.Load("Room_Big_Empty", typeof(GameObject))) as GameObject;

                    instance_0_0.transform.position = place(trash); new Vector3(trash.x * 20, 100, trash.y * 20);
                    instance_1_0.transform.position = place(trash + new Vector2(big_vec.x, 0));//new Vector3(trash.x * 20 + big_vec.x * 20, 100, trash.y * 20);
                    instance_0_1.transform.position = place(trash + new Vector2(0, big_vec.y));//new Vector3(trash.x * 20, 100, trash.y * 20 + big_vec.y * 20);
                    instance_1_1.transform.position = place(trash + new Vector2(big_vec.x, big_vec.y));//new Vector3(trash.x * 20 + big_vec.x * 20, 100, trash.y * 20 + big_vec.y * 20);

                    instance_other.transform.position = place(trash + big_vec*0.5f);
                    instance_Big_Room.transform.position = place(trash + big_vec * 0.5f);//new Vector3(trash.x * 20 + big_vec.x * 10, 100, trash.y * 20 + big_vec.y * 10);
                    instance_0_0.transform.SetParent(instance_Big_Room.transform);
                    instance_0_1.transform.SetParent(instance_Big_Room.transform);
                    instance_1_0.transform.SetParent(instance_Big_Room.transform);
                    instance_1_1.transform.SetParent(instance_Big_Room.transform);
                    instance_other.transform.SetParent(instance_Big_Room.transform);

                    trash_room_0_0.obj = instance_0_0;
                    trash_room_0_1.obj = instance_0_1;
                    trash_room_1_0.obj = instance_1_0;
                    trash_room_1_1.obj = instance_1_1;
                    trash_room_0_0.position = trash;
                    trash_room_0_1.position = trash + new Vector2(0, big_vec.y);
                    trash_room_1_0.position = trash + new Vector2(big_vec.x, 0);
                    trash_room_1_1.position = trash + new Vector2(big_vec.x, big_vec.y);
                    trash_room_0_0.type = "Big";
                    trash_room_0_1.type = "Big";
                    trash_room_1_0.type = "Big";
                    trash_room_1_1.type = "Big";
                    List<Room> Big_4 = new List<Room>() { trash_room_0_0, trash_room_0_1, trash_room_1_0, trash_room_1_1 };
                    foreach (Room r in Big_4)
                    {
                        rooms.Add(r);
                        points.Add(r.position);
                        possible_points.Remove(r.position);
                        Add_possible(r.position);
                    }
                }
                else
                {
                    instance = Instantiate(Resources.Load("Room_template", typeof(GameObject))) as GameObject;
                    chance_of_big -= 3;
                    if (chance_of_big <= 1)
                    {
                        chance_of_big = 2;
                    }
                }
            }
            if (!Add_Big)
            {
                instance.transform.position = place(trash);//new Vector3(trash.x * 20, 100, trash.y * 20);
                trash_room.position = trash;
                trash_room.obj = instance;

                points.Add(trash);
                rooms.Add(trash_room);

                List<Vector2> n = new List<Vector2>() { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1) };
                if (trash_room.type == "Regular")
                {
                    Add_possible(trash);
                }
                else
                {
                    foreach (Vector2 vec in n)
                    {
                        Vector2 bt = trash + vec;
                        if (possible_points.IndexOf(bt) > -1)
                        {
                            if (!check_possibility(bt, 0))
                            {
                                possible_points.Remove(bt);
                            }
                        }
                    }
                }
                possible_points.Remove(trash);
            }
            number_of_rooms--;
        }
        Wall_generation();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Add_possible(Vector2 a)
    {
        List<Vector2> n = new List<Vector2>() { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1) };

        foreach (Vector2 vec in n)
        {
            Vector2 bt = a + vec;
            if ((possible_points.IndexOf(bt) == -1) && (points.IndexOf(bt) == -1))
            {

                if (check_possibility(bt, 1))
                {
                    possible_points.Add(bt);
                }
            }
            else
            {
                if (possible_points.IndexOf(bt) > -1)
                {
                    if (!check_possibility(bt, 1))
                    {
                        possible_points.Remove(bt);
                    }
                }
            }
        }
    }

    bool check_possibility(Vector2 bt, int max_close) {

        List<Vector2> n = new List<Vector2>() { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1) };

        if (points.IndexOf(bt) == -1)
        {
            int near = 0;
            foreach (Vector2 vec1 in n)
            {
                if (points.IndexOf(bt + vec1) != -1)
                {
                    if (rooms[points.IndexOf(bt + vec1)].type != "Big")
                    {
                        near++;
                    }
                }
            }
            if (near <= max_close)
            {
                return true;
            }
        }

        return false;

    }


    Vector2 check_big_possibility(Vector2 a)
    {
        List<Vector2> n = new List<Vector2>() { new Vector2(1, 1), new Vector2(-1, 1), new Vector2(-1, -1), new Vector2(1, -1) };

        foreach (Vector2 vec in n)
        {
            Vector2 trash = a;
            trash.x += vec.x;
            if (check_possibility(trash, 0))
            {
                trash.y += vec.y;
                if (check_possibility(trash, 0))
                {
                    trash.x -= vec.x;
                    if (check_possibility(trash, 0))
                    {
                        return vec;
                    }

                }

            }
        }

        return Vector2.zero;

    }


    void Wall_generation()
    {
        foreach (Room room in rooms)
        {
            string rt = "";
            switch (room.type)
            {
                case "Regular":
                    break;
                case "Golden":
                    rt = "_Golden";
                    break;
                case "Boss":
                    rt = "_Boss";
                    break;
                default:
                    break;
            }
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
                        if ((points.IndexOf(room.position + vec) > -1) && (connection.IndexOf(new Vector2(points.IndexOf(room.position + vec), points.IndexOf(room.position))) > -1))
                        {
                            instance_wall = Instantiate(Resources.Load("Wall_Door" + rt + "_template", typeof(GameObject))) as GameObject;
                        }
                        else
                        {
                            instance_wall = Instantiate(Resources.Load("Wall" + rt + "_template", typeof(GameObject))) as GameObject;
                        }
                        instance_wall.transform.position = child.transform.position;
                        instance_wall.transform.rotation = child.transform.rotation;
                        instance_wall.transform.SetParent(child.transform);
                        break;
                    case "Wall_Position_Z-":
                        vec.x = 0;
                        vec.y = -1;
                            if ((points.IndexOf(room.position + vec) > -1) && (connection.IndexOf(new Vector2(points.IndexOf(room.position + vec), points.IndexOf(room.position))) > -1))
                            {
                                instance_wall = Instantiate(Resources.Load("Wall_Door" + rt + "_template", typeof(GameObject))) as GameObject;
                            }
                            else
                            {
                                instance_wall = Instantiate(Resources.Load("Wall" + rt + "_template", typeof(GameObject))) as GameObject;
                            }
                            instance_wall.transform.position = child.transform.position;
                        instance_wall.transform.rotation = child.transform.rotation;
                        instance_wall.transform.SetParent(child.transform);
                        break;
                    case "Wall_Position_X+":
                        vec.x = 1;
                        vec.y = 0;
                            if ((points.IndexOf(room.position + vec) > -1) && (connection.IndexOf(new Vector2(points.IndexOf(room.position + vec), points.IndexOf(room.position))) > -1))
                            {
                                instance_wall = Instantiate(Resources.Load("Wall_Door" + rt + "_template", typeof(GameObject))) as GameObject;
                            }
                            else
                            {
                                instance_wall = Instantiate(Resources.Load("Wall" + rt + "_template", typeof(GameObject))) as GameObject;
                            }
                            instance_wall.transform.position = child.transform.position;
                        instance_wall.transform.rotation = child.transform.rotation;
                        instance_wall.transform.SetParent(child.transform);
                        break;
                    case "Wall_Position_X-":
                        vec.x = -1;
                        vec.y = 0;
                            if ((points.IndexOf(room.position + vec) > -1) && (connection.IndexOf(new Vector2(points.IndexOf(room.position + vec), points.IndexOf(room.position))) > -1))
                            {
                                instance_wall = Instantiate(Resources.Load("Wall_Door" + rt + "_template", typeof(GameObject))) as GameObject;
                            }
                            else
                            {
                                instance_wall = Instantiate(Resources.Load("Wall" + rt + "_template", typeof(GameObject))) as GameObject;
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
    
    Vector3 place (Vector2 a)
    {
        return new Vector3(a.x * 20, 0, a.y * 20);
    }

    int Weight(Vector2 a)
    {
        return (int)(1000 * number_of_rooms / (Mathf.Sqrt(a.x * a.x + a.y * a.y) + 1));
    }
}
