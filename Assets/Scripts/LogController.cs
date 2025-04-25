using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour
{
    // The maximum number of children this GameObject can have
    private int maxChildren = 10;

    public void OnLogUpdate()
    {
        if (this.gameObject.transform.childCount > maxChildren)
        {
            // calculate how many children need to be removed
            int numChildrenToRemove = this.gameObject.transform.childCount - maxChildren;

            // remove the oldest children game objects until only 3 remain
            for (int i = 0; i < numChildrenToRemove; i++)
            {
                Destroy(this.gameObject.transform.GetChild(i).gameObject);
            }
        }
    }
}
