using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckAndSetDoorPlayerPref : MonoBehaviour
{
    [SerializeField] private GameControl gameControl;

    [Header("Week1 Variables")]
    [SerializeField] private GameObject hintPanel;
    [SerializeField] private List<GameObject> doorLocksWeek1;

    [Header("Week2 Variables")]
    [SerializeField] private List<GameObject> week2Objs;

    [SerializeField] private int lastOpenedDoor;
    [SerializeField] private List<Door> doorList;

    [SerializeField] private bool needCheck;

    private void OnEnable()
    {

        if (gameControl.items.Contains("HeartRoomKey"))
        {
            foreach(Door door in doorList)
            {
                door.toDoorOnj.GetComponent<Button>().enabled = true;
                door.toDoorOnj.GetComponent<Image>().raycastTarget = true;
            }
        }

        if (lastOpenedDoor < 5)
        {
            foreach (Door doorObj in doorList)
            {
                bool isNextActive = doorObj.nextObj.activeSelf;
                if (isNextActive)
                {
                    //check if lastOpenedDoor = this.indext
                    if ((doorObj.doorIndex + 1) != lastOpenedDoor)
                    {
                        PlayerPrefs.SetInt("LastOpenedDoor", (doorObj.doorIndex + 1));
                        PlayerPrefs.Save();
                        Debug.Log("CheckAndSetDoorPlayerPref: (doorObj.doorIndex + 1) != lastOpenedDoor");
                    }
                }
            }
        }
    }

    private void Start()
    {

        if (gameControl.items.Contains("HeartRoomKey"))
        {
            needCheck = false;

            hintPanel.SetActive(false);
            foreach (GameObject doorLocks in doorLocksWeek1)
                doorLocks.SetActive(false);
            foreach (GameObject obj in week2Objs)
                obj.SetActive(true);

            return;
        }
        else
        {
            needCheck = true;

            hintPanel.SetActive(true);
            foreach (GameObject doorLocks in doorLocksWeek1)
                doorLocks.SetActive(true);
            foreach (GameObject obj in week2Objs)
                obj.SetActive(false);
        }

        if (needCheck)
        {
            if (PlayerPrefs.HasKey("LastOpenedDoor"))
            {
                lastOpenedDoor = PlayerPrefs.GetInt("LastOpenedDoor");

                Debug.Log("CheckAndSetDoorPlayerPref: Get PlayerPref, lastOpenedDoor = " + lastOpenedDoor);

                foreach (Door door in doorList)
                {
                    if (door.doorIndex < lastOpenedDoor)
                    {
                        door.UnlockDoor();
                        if (lastOpenedDoor < 6)
                            door.toDoorOnj.SetActive(false);
                    }
                }
            }
            else
            {
                Debug.Log("CheckAndSetDoorPlayerPref: No data for 'LastOpenedDoor' found. Starting from the beginning.");
            }
        }
    }

    public void UnlocktDoor(int doorIndex)
    {
        if (needCheck && lastOpenedDoor < 5)
        {
            // Logic to visually open the door
            PlayerPrefs.SetInt("LastOpenedDoor", doorIndex);
            PlayerPrefs.Save();

            Debug.Log("CheckAndSetDoorPlayerPref: Saved PlayerPref " + doorIndex);

            foreach (Door door in doorList)
            {
                if (door.doorIndex == doorIndex)
                {
                    door.UnlockDoor();
                }
            }
        }
    }

    public void FirstExitClicked()
    {
        PlayerPrefs.SetInt("LastOpenedDoor", 0);
        PlayerPrefs.Save();
    }
}

[System.Serializable]
public class Door
{
    public int doorIndex; // Unique ID for each door
    public List<GameObject> blocknHintObj;
    public GameObject toDoorOnj; //Including The Hints, Original NormalItem, and Next BlockItem
    public GameObject nextObj;
    public bool isOpen;

    public void UnlockDoor()
    {
        if (!isOpen)
        {
            isOpen = true;

            if (blocknHintObj.Count > 0)
            {
                foreach (GameObject obj in blocknHintObj)
                    obj.SetActive(false);
            }
            nextObj.SetActive(true);
        }
    }
}
