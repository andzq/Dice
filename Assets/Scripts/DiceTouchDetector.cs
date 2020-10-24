using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DiceTouchDetector : MonoBehaviour
{
    public Die redDie;
    public Die blueDie;
    [Space]
    public TextMeshProUGUI debugLabel;
    [Space]
    public Visualizer visualizer;

    // question: which two faces are touching and what is the difference in dregree

    private void Start()
    {
        Reset();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Reset();
        }
    }

    void Reset()
    {
        RandomizeDicePosition();
        RandomizeDiceRotation();
        DebugTouchingSides();
    }

    void ResetWithMatchingFaces()
    {
        RandomizeDicePosition();
        RandomizeDiceRotation();

        if (Die.TouchingWithEqualFaceCount(redDie, blueDie))
            DebugTouchingSides();
        else
            ResetWithMatchingFaces();
    }

    void RandomizeDicePosition()
    {
        redDie.transform.position = Vector3.zero;
        blueDie.transform.position = Side.DirectionAsVector(Side.RandomDirection());
    }

    void RandomizeDiceRotation()
    {
        redDie.transform.localEulerAngles = RandomIncrementedRotation(90);
        blueDie.transform.localEulerAngles = RandomIncrementedRotation(90);
    }

    Vector3 RandomIncrementedRotation(int incrementStepSize)
    {
        int maxIncrements = 360 / incrementStepSize;
        int x = UnityEngine.Random.Range(0, maxIncrements) * incrementStepSize;
        int y = UnityEngine.Random.Range(0, maxIncrements) * incrementStepSize;
        int z = UnityEngine.Random.Range(0, maxIncrements) * incrementStepSize;
        return new Vector3(x, y, z);
    }

    void DebugTouchingSides()
    {

        Side.Direction redDirection = Die.TouchDirection(redDie, blueDie);
        Side.Direction blueDirection = Die.TouchDirection(blueDie, redDie);

        string sidesTouchingText = string.Format("Red {0} touching Blue {1} \n", redDie.DirectionToEyeCount(redDirection), blueDie.DirectionToEyeCount(blueDirection));
        string matchingFacesText = string.Format("Touching sides eye count is equal: {0} \n", Die.TouchingWithEqualFaceCount(redDie, blueDie) ? "YES" : "NO!");
        string anglesText = "- no mirror possible -";
        if (Die.TouchingWithEqualFaceCount(redDie, blueDie))
        {
            anglesText = string.Format("{0}", Die.DiceEyesMirrored(redDie, blueDie) ? "Eyes are mirrored" : "Eyes not mirrored");
        }


        visualizer.Visualize(redDie, redDirection, blueDie, blueDirection);

        debugLabel.text = sidesTouchingText + matchingFacesText + anglesText;
    }
}
