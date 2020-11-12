using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public int count = 3;
    public bool active;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject trackerObject;
    private SpriteRenderer doorSR;
    private BoxCollider2D doorCol;
    private PlayerKillCount trackerKC;

    void Start()
    {
        doorSR = doorPrefab.GetComponent<SpriteRenderer>();
        doorCol = doorPrefab.GetComponent<BoxCollider2D>();
        trackerKC = trackerObject.GetComponent<PlayerKillCount>();       
    }

    void Update()
    {
        if(trackerKC != null)
        {
            count = trackerKC.killcount;
        }
        SpawnDoor();
    }

    public void SpawnDoor()
    {
        if(count <= 0)
        {
            doorSR.enabled = true;
            doorCol.enabled = true;
            active = true;
        }
        else
        {
            doorSR.enabled = false;
            doorCol.enabled = false;
            active = false;
        }
    }

}
