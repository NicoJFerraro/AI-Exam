using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager instance { get; private set; }
    
    public Camera myCamera;

    public Transform redStart, blueStart;

    public List<NPCs> redOnesList = new List<NPCs>();
    public List<NPCs> blueOnesList = new List<NPCs>();

    public FlockingNPCs flocker;               

    public List<Boss> bosses;

    public Room room;

    public int redStartCant;
    public int blueStartCant;

    public float xSize;
    public float ySize;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        redOnesList = new List<NPCs>();
        blueOnesList = new List<NPCs>();
    }

    void Start()
    {
        for (int i = 0; i < redStartCant; i++)
        {
            SpawnFlockers(0);
        }
        for (int i = 0; i < blueStartCant; i++)
        {
            SpawnFlockers(1);
        }
    }

    public void SpawnFlockers(int myTeam)
    {
        var _instatiate = GameObject.Instantiate(flocker);
        Vector3 _startpos;
        if (myTeam == 0)
            _startpos = redStart.position;
        else
            _startpos = blueStart.position;

        Vector3 randomPlus = new Vector3(Random.Range(-ySize, ySize), 0, Random.Range(-xSize, xSize));

        _instatiate.transform.position = _startpos + randomPlus;
        
        _instatiate.team = myTeam;
        _instatiate.myRoom = room;
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(redStart.position, new Vector3(2 * ySize, 1, 2 * xSize));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(blueStart.position, new Vector3(2 * ySize, 1, 2 * xSize));
    }

}
