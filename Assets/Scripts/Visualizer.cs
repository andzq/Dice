using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Visualizer : MonoBehaviour
{
    public LineRenderer lineRend;
    public Transform redEyeLabelContainer;
    public Transform blueEyeLabelContainer;
    public TextMeshProUGUI lbl_redDieEyes;
    public TextMeshProUGUI lbl_blueDieEyes;

    public void Visualize(Die redDie, Side.Direction redDirection, Die blueDie, Side.Direction blueDirection)
    {
        Vector3 offset = Vector3.right * 2f;

        Vector3 lineStart = redDie.transform.position + offset;
        Vector3 lineEnd = blueDie.transform.position + offset;

        redEyeLabelContainer.position = lineStart;
        blueEyeLabelContainer.position = lineEnd;

        lineRend.SetPosition(0, Vector3.Lerp(lineStart, lineEnd, 0.2f));
        lineRend.SetPosition(1, Vector3.Lerp(lineStart, lineEnd, 0.8f));

        lbl_redDieEyes.text = redDie.DirectionToEyeCount(redDirection).ToString();
        lbl_blueDieEyes.text = blueDie.DirectionToEyeCount(blueDirection).ToString();
    }
}
