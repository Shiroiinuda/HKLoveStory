using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    public JumpGameControl jumpGameControl;

    void EndJump()
    {
        jumpGameControl.EndGame();
    }
}
