using System.Collections;
using System.Collections.Generic;
using Investigation;
using UnityEngine;

public class CountItems : MonoBehaviour
{
    [SerializeField] private InvestigationControl investigationControl;

    //[SerializeField] private int itemsNumNeed;
    //private int itemsCount;

    public List<string> needTasks;
    [SerializeField] private List<string> completedTasks;

    public List<GameObject> changeStranges;

    //private void Start()
    //{
    //    itemsCount = 0;
    //}

    //public void CheckItemsNum()
    //{
    //    itemsCount += 1;

    //    if(itemsCount == itemsNumNeed)
    //    {
    //        itemsCount = 0;
    //        investigationControl.isEnd = true;
    //    }
    //}

    public void AddCompletedTask(string taskName)
    {
        if (!completedTasks.Contains(taskName) && needTasks.Contains(taskName))
            completedTasks.Add(taskName);

        if (changeStranges.Count > 0)
        {
            foreach (GameObject changeStrange in changeStranges)
            {
                bool isChanged = false;
                bool result = Random.value > 0.5f;

                if (result)
                    changeStrange.SetActive(false);
                else
                {
                    if (!isChanged)
                    {
                        changeStrange.SetActive(true);
                        isChanged = true;
                    }
                }
            }
        }

        CheckComplete();
    }

    private void CheckComplete()
    {
        if (completedTasks.Count >= needTasks.Count)
            StartCoroutine(investigationControl.EndInvestigation());
    }
}
