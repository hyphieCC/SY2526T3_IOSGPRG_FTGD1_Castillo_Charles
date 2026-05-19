using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /*
    [SerializeField] private int _health;
    [SerializeField] private int _speed;

    public void Initialize()
    {
        _health = Random.Range(1, 100);
        _speed = Random.Range(1, 10);
    }
    */

    [SerializeField] private SwipeDirection _swipeDirection;
    [SerializeField] private ArrowType _arrowType;

    public bool CheckPlayerSwipe(SwipeDirection direction)
    {
        if (_arrowType == ArrowType.Red)
        {
            return direction == GetOppositeDirection(_swipeDirection); //Red arrows require the opposite swipe direction
        }

        return direction == _swipeDirection;
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
}
