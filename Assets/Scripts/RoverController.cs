using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RoverController : MonoBehaviour
{
    public Text winText;
    public SceneManager scene;
    
    public InputField inputField;
    private Rover1 rover = new Rover1();

    /// <summary>
    /// Unity function. Start before the first frame.
    /// </summary>
    public void Start()
    {
        this.rover.UpdatePosition(new Position(this.scene.NumberOfTile / 2, this.scene.NumberOfTile / 2));
        Vector3 v = this.scene.GetVectorFromPosition(this.rover.GetPosition());
        gameObject.transform.position = v;
    }

    /// <summary>
    /// Update the player position
    /// </summary>
    /// <param name="key">The key which was pressed or passed by timer from command sequences.
    /// it might be rgl or up, left, right.
    /// </param>
    public Position UpdatePlayer(int key)
    {
        Position newPos = new Position();
        Vector3 v;
        Command com = this.GetCommandByKeyCode(key);
        int dir;

        // Based on the key pressed, we will rotate and transform the player.
        switch (com)
        {
            case Command.GO:
                Position oldPos = this.rover.GetPosition();

                newPos = this.rover.GetNextPosition(com);
                if (!this.scene.IsPositionValid(newPos))
                {
                    ////linear behavior on hit boundary else random behavior on hit boundary.
                    if (!this.scene.isOnHitBoundaryPlaceRandom)
                    {
                        if (newPos.Row < 0)
                        {
                            newPos.Row = this.scene.NumberOfTile - 1;
                        }

                        if (newPos.Col < 0)
                        {
                            newPos.Col = this.scene.NumberOfTile - 1;
                        }

                        if (newPos.Row >= this.scene.NumberOfTile)
                        {
                            newPos.Row = 0;
                        }

                        if (newPos.Col >= this.scene.NumberOfTile)
                        {
                            newPos.Col = 0;
                        }
                    }
                    else
                    {
                        System.Random r = new System.Random();
                        int rowOrCol = r.Next(3);
                        if (rowOrCol == 0)
                        {
                            newPos.Row = 0;
                            newPos.Col = r.Next(this.scene.NumberOfTile - 1);
                        }
                        else if (rowOrCol == 1)
                        {
                            newPos.Row = this.scene.NumberOfTile - 1;
                            newPos.Col = r.Next(this.scene.NumberOfTile - 1);
                        }
                        else if (rowOrCol == 2)
                        {
                            newPos.Col = 0;
                            newPos.Row = r.Next(this.scene.NumberOfTile - 1);
                        }
                        else if (rowOrCol == 3)
                        {
                            newPos.Col = this.scene.NumberOfTile - 1;
                            newPos.Row = r.Next(this.scene.NumberOfTile - 1);
                        }
                    }
                }

                // checking whether in the new position there is any obstacles or not.
                if (!this.scene.IsPositionFree(newPos))
                {
                    this.rover.UpdatePosition(oldPos);
                    return oldPos;
                }

                this.rover.UpdatePosition(newPos);
                v = this.scene.GetVectorFromPosition(this.rover.GetPosition());

                gameObject.transform.position = v;

                break;
            case Command.ROTATE_LEFT:
                gameObject.transform.Rotate(new Vector3(0, 0, 90));
                dir = this.rover.Rotate(com);
                break;
            case Command.ROTATE_RIGHT:
                gameObject.transform.Rotate(new Vector3(0, 0, -90));
                dir = this.rover.Rotate(com);
                break;

            default:
                break;
        }

        return newPos;
    }

    public Position GetRoverPosition()
    {
        return this.rover.GetPosition();
    }

    /// <summary>
    ///  Unity Function. OnTriggerEnter2D is called whenever this object overlaps with a trigger collider.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        //// Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        if (other.gameObject.CompareTag("goal"))
        {
            this.winText.text = "Mission accomplished!";
            other.gameObject.SetActive(false);
        }
    }

    private Command GetCommandByKeyCode(int key)
    {
        switch (key)
        {
            case (int)KeyCode.LeftArrow:
            case (int)KeyCode.L:
                return Command.ROTATE_LEFT;
            case (int)KeyCode.RightArrow:
            case (int)KeyCode.R:
                return Command.ROTATE_RIGHT;

            case (int)KeyCode.UpArrow:
            case (int)KeyCode.G:
                return Command.GO;
        }

        return Command.GO;
    }

    /// <summary>
    /// Unity function. Responsible for key event.
    /// </summary>
    private void OnGUI()
    {
        if (Event.current.isKey && Event.current.type == EventType.KeyDown)
        {
            // while we are typing on the command seq, no command should be executed.
            if (this.inputField.isFocused)
            {
                return;
            }

            int key = (int)Event.current.keyCode;
            this.UpdatePlayer(key);
        }
    }

    /// <summary>
    ///  Unity Function. Trigger when our player collide with a rigid body object.
    /// </summary>
    /// <param name="collision"> The object with which Player collide. if the collider is
    /// a obstacle or background than exception will be thrown</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //// https://gamedev.stackexchange.com/questions/114891/how-do-i-make-unity-halt-on-exceptions
        if (collision.gameObject.tag == "obstacle" || collision.gameObject.tag == "background")
        {
            ////throw new System.Exception();
        }
    }
}