using System;
using System.IO;
using System.Threading;
using System.Timers;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scene Manager class is the most important class and responsible for all the tasks.
/// </summary>
public class SceneManager : MonoBehaviour, IWorld
{
    /// <summary>
    /// Timer for automatic mode
    /// </summary>
    public bool isGoalTileRandom = true;
    public bool isOnHitBoundaryPlaceRandom = true;
    public bool isObstacleRandom = true;
    public int NumberOfTile = 15;
    public string commandSeq = string.Empty;
    public double timerInterval = 2000; 
    public InputField inputField;
    public Button startRemoteBtn;
    public Text invalidPathTxt;
    public RoverController roverController;
    public GameObject player;
    public GameObject goal;
    public GameObject[] obstacles;
    public Text AutoManualTxt;

    private System.Timers.Timer aTimer;
    private float xSize;
    private float ySize;
    private Tile[][] tileArray = new Tile[15][];
    private string dataPath;
    private int keyTimer = 0;

    /// <summary>
    /// Call this static function when iterested to extit the game.
    /// </summary>
    public static void ExitGame()
    {
#if UNITY_EDITOR

        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // There's no standard way to return an error code to the OS,
        // so just quit regularly.
        Application.Quit();
#endif
    }

    /// <summary>
    /// We can resize the total scene using this function
    /// </summary>
    /// <param name="theGameObject">Typically the background</param>
    /// <param name="newSize">New size of the gameobject</param>
    public void NewScale(GameObject theGameObject, float newSize)
    {
        float sizeY = theGameObject.GetComponent<Renderer>().bounds.size.y;
        float sizeX = theGameObject.GetComponent<Renderer>().bounds.size.x;
        Vector3 rescale = theGameObject.transform.localScale;

        rescale.y = newSize * rescale.y / sizeY;
        rescale.x = newSize * rescale.x / sizeX;
        theGameObject.transform.localScale = rescale;
    }

    /// <summary>
    /// Unity Function. Start is called before the first frame update
    /// </summary>
    public void Start()
    {
        this.dataPath = Path.Combine(Application.persistentDataPath, "App.config");
        this.invalidPathTxt.text = string.Empty;
        this.startRemoteBtn.onClick.AddListener(this.StartRemote);
        this.NewScale(this.gameObject, 150);
        Vector3 v = GetComponent<Renderer>().bounds.size;
        this.xSize = v.x;
        this.ySize = v.y;

        for (int i = 0; i < this.NumberOfTile; i++)
        {
            this.tileArray[i] = new Tile[this.NumberOfTile];
        }

        // Taking the player size.
        float playerSizeX = this.player.GetComponent<Renderer>().bounds.size.x;
        float playerSizeY = this.player.GetComponent<Renderer>().bounds.size.y;

        //// Setting the position of each tile.
        for (int i = 0; i < this.NumberOfTile; i++)
        {
            for (int j = 0; j < this.NumberOfTile; j++)
            {
                this.tileArray[i][j] = new Tile();
                this.tileArray[i][j].Vec.x = (float)(this.xSize / this.NumberOfTile) * i - (float)this.xSize / 2 + (float)(playerSizeX / 2);
                this.tileArray[i][j].Vec.y = (float)(this.ySize / this.NumberOfTile) * j - (float)this.ySize / 2 + (float)(playerSizeY / 2);
            }
        }

        ////placing the goal tile
        if (this.isGoalTileRandom)
        {
            System.Random r = new System.Random();
            int row = r.Next(this.NumberOfTile - 2);
            int col = r.Next(this.NumberOfTile - 2);
            if (row == 0)
            {
                row++;
            }

            if (col == 0)
            {
                col++;
            }

            this.goal.transform.position = this.tileArray[row][col].Vec;
            this.tileArray[row][col].TileTyp = TileType.GOAL;
        }

        /// Placing the obstacles 
        for (int i = 0; i < this.obstacles.Length; i++)
        {
            System.Random r = new System.Random();
            int row = r.Next(this.NumberOfTile - 2);
            int col = r.Next(this.NumberOfTile - 2);
            if (row == 0)
            {
                row++;
            }

            if (col == 0)
            {
                col++;
            }

            this.obstacles[i].transform.position = this.tileArray[row][col].Vec;
            Thread.Sleep(1000);
            this.tileArray[row][col].TileTyp = TileType.OBSTACLE;
        }
    }

    /// <summary>
    /// Unity Function. Update is called once per frame
    /// </summary>
    public void Update()
    {
        /// Executing the command sequence from timer event handler.
        if (this.keyTimer != 0)
        {
            Position oldPos = this.roverController.GetRoverPosition();
            Position newPos = this.roverController.UpdatePlayer(this.keyTimer);
            /// when the timer is on and the rover hit on obstacles it will show
            /// invalid path message
            if (this.aTimer != null && this.aTimer.Enabled && newPos.IsEqual(oldPos))
            {
                this.commandSeq = string.Empty;
                this.inputField.text = string.Empty;
                this.invalidPathTxt.text = "invalid path";
                Thread.Sleep(500);
                this.aTimer.Stop();
                return;
            }

            this.keyTimer = 0;
            this.inputField.text = this.commandSeq;
        }

        ///Setting Auto or Manual Text based on the command sequences.
        if (String.IsNullOrEmpty(this.commandSeq))
        {
            this.AutoManualTxt.text = "Manual";
        }
        else
        {
            this.AutoManualTxt.text = "Auto";
        }
    }

    public bool IsPositionFree(Position pos)
    {
        return !(this.tileArray[pos.Row][pos.Col].TileTyp == TileType.OBSTACLE);
    }

    public bool IsPositionValid(Position pos)
    {
        return (pos.Col >= 0 && pos.Row >= 0 && pos.Row < this.tileArray.Length && pos.Col < this.tileArray[0].Length);
    }

    public bool IsGoalReached(Position pos)
    {
        return this.tileArray[pos.Row][pos.Col].TileTyp == TileType.GOAL;
    }

    public Vector3 GetVectorFromPosition(Position pos)
    {
        return this.tileArray[pos.Row][pos.Col].Vec;
    }

    /// <summary>
    /// Event Handler for the timer where the sequences are executed.
    /// </summary>
    /// <param name="sender">The Timer</param>
    /// <param name="e">Event Args</param>
    private void OnTimedEvent(object sender, ElapsedEventArgs e)
    {
        //// run command sequence but When there are no sequence to Execute we should stop the timer.
        if (!String.IsNullOrEmpty(this.commandSeq))
        {
            char ch = this.commandSeq[0];
            this.commandSeq = this.commandSeq.Remove(0, 1);

            switch (ch)
            {
                case 'g':
                    this.keyTimer = (int)KeyCode.UpArrow;
                    break;
                case 'l':
                    this.keyTimer = (int)KeyCode.LeftArrow;
                    break;
                case 'r':
                    this.keyTimer = (int)KeyCode.RightArrow;
                    break;
                default:
                    break;
            }
        }
        else
        {
            this.aTimer.Stop();
            this.aTimer.Elapsed -= this.OnTimedEvent;
            this.aTimer.AutoReset = false;
            this.aTimer.Enabled = false;
        }
    }

    /// <summary>
    /// Execute the sequence based on Timer.
    /// </summary>
    private void StartRemote()
    {
        this.commandSeq = this.inputField.text;
        this.aTimer = new System.Timers.Timer(this.timerInterval);
        this.aTimer.Elapsed += this.OnTimedEvent;
        this.aTimer.AutoReset = true;
        this.aTimer.Enabled = true;
        this.aTimer.Start();
    }

    /// <summary>
    /// Unity Function. Event handler function to check which key is pressed from the keyboard.
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
            ///roverController
            switch (key)
            {
                /// We are loading settings from file
                case (int)KeyCode.X:
                    using (StreamReader sr = File.OpenText(this.dataPath))
                    {
                        string jsonString = sr.ReadToEnd();
                        var obj = JObject.Parse(jsonString);
                        this.NumberOfTile = (int)obj["numberOfTile"];
                        this.isGoalTileRandom = (bool)obj["isGoalTileRandom"];
                        this.isObstacleRandom = (bool)obj["isObstacleRandom"];
                        this.isOnHitBoundaryPlaceRandom = (bool)obj["isOnHitBoundaryPlaceRandom"];
                        this.commandSeq = (string)obj["commandSeq"];
                        this.inputField.text = this.commandSeq;
                        this.timerInterval = (int)obj["timerInterval"];
                    }

                    break;
                //// we are saving our settings to file
                case (int)KeyCode.S:
                    String str = JsonUtility.ToJson(this, true);
                    ////str = JsonConverter<SceneManager>
                    using (StreamWriter streamWr = File.CreateText(this.dataPath))
                    {
                        streamWr.Write(str);
                    }

                    break;
                //// Switch to manual mode
                case (int)KeyCode.M:
                    this.aTimer.Stop();
                    this.inputField.text = string.Empty;
                    this.commandSeq = string.Empty;
                    break;
                //// Exiting the game
                case (int)KeyCode.Escape:
                    ExitGame();
                    break;
                //// Transform or rotate the player
                default:
                    break;
            }
        }
    }
}
