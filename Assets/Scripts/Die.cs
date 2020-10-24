using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Die : MonoBehaviour
{
    public Side one;
    public Side two;
    public Side three;
    public Side four;
    public Side five;
    public Side six;

    public Side[] sides;

    private void Awake()
    {
        sides = new Side[] { one, two, three, four, five, six };
    }

    public int DirectionToEyeCount(Side.Direction direction)
    {
        return (int)sides.Where(side => side.direction.Equals(direction)).Select(side => side.eyes).First();
    }

    public Side DirectionToSide(Side.Direction direction)
    {
        return sides.Where(side => side.direction.Equals(direction)).First();
    }

    public static Side.Direction TouchDirection(Die die, Die other)
    {
        Vector3 worldDirection = other.transform.position - die.transform.position;
        Vector3 localDirection = die.transform.InverseTransformDirection(worldDirection);
        Side.Direction direction = Side.DirectionToSide(localDirection);
        return direction;
    }

    public static bool TouchingWithEqualFaceCount(Die dOne, Die dTwo)
    {
        Side.Direction directionOne = TouchDirection(dOne, dTwo);
        Side.Direction directionTwo = TouchDirection(dTwo, dOne);
        return directionOne == directionTwo;
    }

    public void DebugFacesInWorldSpace()
    {
        foreach (Side side in sides)
        {
            Side.Direction direction = Side.DirectionToSide(SideToVectorInWorldSpace(side));
            Debug.LogFormat("{0}: {1}/{2}", name, side.eyes, direction);
        }
    }

    public static int ParallelFaceCount(Die dOne, Die dTwo)
    {
        // wenn zwei seiten matchen -> dann nochmal 2 gleiche richtung haben, sind die matching faces auseinander
        int parallelFaceCount = 0;
        for (int i = 0; i < dOne.sides.Length; i++)
        {
            Side dOneSide = dOne.sides[i];
            Side dTwoSide = dTwo.sides[i];
            Side.Direction dOneDirection = Side.DirectionToSide(dOne.SideToVectorInWorldSpace(dOneSide));
            Side.Direction dTwoDirection = Side.DirectionToSide(dTwo.SideToVectorInWorldSpace(dTwoSide));
            //Debug.LogFormat("{0} : {1} : {2} - {3} : {4} : {5}", dOne.name, dOneSide.eyes, dOneDirection, dTwo.name, dTwoSide.eyes, dTwoDirection);
            if (dOneDirection == dTwoDirection)
                parallelFaceCount++;
        }

        return parallelFaceCount;
    }

    public static bool DiceEyesMirrored(Die dOne, Die dTwo)
    {
        if (TouchingWithEqualFaceCount(dOne, dTwo))
        {
            int parallelFaceCount = ParallelFaceCount(dOne, dTwo);
            int eyes = dOne.DirectionToEyeCount(TouchDirection(dOne, dTwo));
            if (eyes == 6)
            {
                return parallelFaceCount == 2;
            }
            else if (eyes == 2 || eyes == 3)
            {
                return parallelFaceCount != 2;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public float DirectionToAngle(Side.Direction direction)
    {
        switch (direction)
        {
            case Side.Direction.up:
                return transform.localEulerAngles.y;
            case Side.Direction.down:
                return transform.localEulerAngles.y;

            case Side.Direction.left:
                return transform.localEulerAngles.x;
            case Side.Direction.right:
                return transform.localEulerAngles.x;

            case Side.Direction.front:
                return transform.localEulerAngles.z;
            case Side.Direction.back:
                return transform.localEulerAngles.z;

            default:
                return 0;
        }
    }

    public Vector3 SideToVectorInWorldSpace(Side side)
    {
        switch (side.direction)
        {
            case Side.Direction.up:
                return transform.up;
            case Side.Direction.down:
                return -transform.up;
            case Side.Direction.left:
                return -transform.right;
            case Side.Direction.right:
                return transform.right;
            case Side.Direction.front:
                return -transform.forward;
            case Side.Direction.back:
                return transform.forward;
            default:
                return Vector3.zero;
        }
    }
}

[System.Serializable]
public class Side
{
    public enum Eyes { _1 = 1, _2 = 2, _3 = 3, _4 = 4, _5 = 5, _6 = 6 }
    public Eyes eyes;
    public enum Direction { up, down, left, right, front, back }
    public Direction direction;

    public Vector3 directionAsVector => DirectionAsVector(direction);

    public static Vector3 DirectionAsVector(Direction side)
    {
        switch (side)
        {
            case Direction.up:
                return Vector3.up;
            case Direction.down:
                return Vector3.down;
            case Direction.left:
                return Vector3.left;
            case Direction.right:
                return Vector3.right;
            case Direction.front:
                return Vector3.back;
            case Direction.back:
                return Vector3.forward;
            default:
                return Vector3.zero;
        }
    }

    public static Direction DirectionToSide(Vector3 direction)
    {
        if (Vector3.Dot(Vector3.up, direction) > 0.9f)
            return Direction.up;
        else if (Vector3.Dot(Vector3.down, direction) > 0.9f)
            return Direction.down;
        else if (Vector3.Dot(Vector3.left, direction) > 0.9f)
            return Direction.left;
        else if (Vector3.Dot(Vector3.right, direction) > 0.9f)
            return Direction.right;
        else if (Vector3.Dot(Vector3.back, direction) > 0.9f)
            return Direction.front;
        else if (Vector3.Dot(Vector3.forward, direction) > 0.9f)
            return Direction.back;
        else
            return default;
    }

    public static Direction RandomDirection()
    {
        Array sides = Enum.GetValues(typeof(Side.Direction));
        System.Random random = new System.Random();
        return (Side.Direction)sides.GetValue(random.Next(sides.Length));
    }
}
