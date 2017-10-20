using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public Vector2 velocity;
    public Rigidbody2D rb2D;
    public int[] PlayerPos = new int[2]; //[x,y]
    MapGenerator Mapref;
    public GameObject BG;
    public GameObject self;
    int yd = 15;
    public int[] StartingPos = new int[2]; //[x,y]
    public int[] CurrentPos = new int[2]; //[x,y]

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        System.Random random = new System.Random();
        Mapref = BG.GetComponent<MapGenerator>();
        PlayerPos[0] = random.Next(0, 15);
        PlayerPos[1] = random.Next(0, 15);
        while (Mapref.map[yd - PlayerPos[1], PlayerPos[0]] == 1)
        {
            PlayerPos[0] = random.Next(0, 15);
            PlayerPos[1] = random.Next(0, 15);
        }

        self.transform.Translate(- (velocity * (8 - PlayerPos[0])));//9 to 0, -6 to 15

        velocity.Set(velocity.x, -velocity.y);
        self.transform.Translate(- velocity* (PlayerPos[1]-8));
        velocity.Set(velocity.x, -velocity.y);
        StartingPos[0] = PlayerPos[0];
        StartingPos[1] = PlayerPos[1];
        CurrentPos[0] = PlayerPos[0];
        CurrentPos[1] = 15- PlayerPos[1];
    }


    // Update is called once per frame
    void Update () {
        Mapref = BG.GetComponent<MapGenerator>();
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && PlayerPos[0] > 0 && Mapref.map[yd - PlayerPos[1], PlayerPos[0] - 1] != 1)
        {
            PlayerPos[0] = PlayerPos[0] - 1; 
			rb2D.MovePosition(rb2D.position - velocity);
            
            //rb2D.position.Set(rb2D.position.x-17, rb2D.position.y-18);
        }
		else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && PlayerPos[0] < 15 && Mapref.map[yd- PlayerPos[1], PlayerPos[0] + 1] != 1)
        {
            PlayerPos[0] = PlayerPos[0] + 1;
            rb2D.MovePosition(rb2D.position + velocity);
            //rb2D.position.Set(rb2D.position.x + 17, rb2D.position.y + 18);
        }
		else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && PlayerPos[1] < 15 && Mapref.map[yd - PlayerPos[1]-1, PlayerPos[0]] != 1)
        {
            PlayerPos[1] = PlayerPos[1] + 1;
            velocity.Set(velocity.x, -velocity.y);
            rb2D.MovePosition(rb2D.position - velocity);
            velocity.Set(velocity.x, -velocity.y);
            //rb2D.position.Set(rb2D.position.x + 17, rb2D.position.y - 18);
        }
		else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && PlayerPos[1] > 0 && Mapref.map[yd - PlayerPos[1]+1, PlayerPos[0]] != 1)
        {
            PlayerPos[1] = PlayerPos[1] - 1;
            velocity.Set(-velocity.x, velocity.y);
            rb2D.MovePosition(rb2D.position - velocity);
            velocity.Set(-velocity.x, velocity.y);
            //rb2D.position.Set(rb2D.position.x - 17, rb2D.position.y + 18);
        }
        else {}
	}
}
