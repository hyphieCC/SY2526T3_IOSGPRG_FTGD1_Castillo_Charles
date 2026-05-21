using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Enums in separate .cs files
    [SerializeField] private SwipeDirection _swipeDirection;
    [SerializeField] private ArrowType _arrowType;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer _arrowRenderer;
    [SerializeField] private List<Sprite> _arrowSprites;

    private void Start()
    {
        RandomizeArrow(); //For Testing
        SetupArrow();
    }

    public bool CheckPlayerSwipe(SwipeDirection direction)
    {
        if (_arrowType == ArrowType.Red)
        {
            return direction == GetOppositeDirection(_swipeDirection); //Red arrows require the opposite swipe direction
        }

        return direction == _swipeDirection;
    }

    private void SetupArrow()
    {
        int directionIndex = (int)_swipeDirection; //Check SwipeDirection.cs for order
        _arrowRenderer.sprite = _arrowSprites[directionIndex];

        if (_arrowType == ArrowType.Green)
        {
            _arrowRenderer.color = Color.green;
        }
        else
        {
            _arrowRenderer.color = Color.red;
        }
    }

    private SwipeDirection GetOppositeDirection(SwipeDirection direction)
    {
        switch (direction)
        {
            case SwipeDirection.Left:
            {
                return SwipeDirection.Right;
            }
            case SwipeDirection.Right:
            {
                return SwipeDirection.Left;
            }
            case SwipeDirection.Up:
            { 
                return SwipeDirection.Down;
            }
            case SwipeDirection.Down:
            {
                return SwipeDirection.Up;
            }
            default:
            {
                return direction; //Should not reach here
            }
        }
    }

    //Testing Area
    private void RandomizeArrow()
    {
        _swipeDirection = (SwipeDirection)Random.Range(0, 4);
        _arrowType = (ArrowType)Random.Range(0, 2);
    }
}
