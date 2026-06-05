using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Enums in separate .cs files
    [SerializeField] private SwipeDirection _swipeDirection;
    [SerializeField] private ArrowType _arrowType;

    [SerializeField] private float _moveSpeed;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer _arrowRenderer;
    [SerializeField] private List<Sprite> _arrowSprites;
    [SerializeField] private GameObject _arrowBox;

    private bool _isPlayerInRange = false;
    private bool _hasInteractedWithPlayer = false;

    private void Start()
    {
        _arrowBox.SetActive(false);
        RandomizeArrowType();
        RandomizeArrowDirection();
        SetupArrow();

        if (_arrowType == ArrowType.Yellow)
        {
            StartCoroutine(CO_RotateArrow());
        }
    }

    private void Update()
    {
        FallDown();
    }

    private void OnDestroy()
    {
        if (Spawner.Instance != null)
        {
            Spawner.Instance.RemoveEnemyFromList(this);
        }
    }

    public bool CheckPlayerSwipe(SwipeDirection direction)
    {
        if (_arrowType == ArrowType.Red)
        {
            return direction == GetOppositeDirection(_swipeDirection); //Red arrows require the opposite swipe direction
        }

        return direction == _swipeDirection;
    }

    public void MarkAsInteractedWithPlayer() //Ensures that the player can only get damaged once from the same enemy
    {
        _hasInteractedWithPlayer = true;
    }

    public bool HasInteractedWithPlayer
    {
        get => _hasInteractedWithPlayer;
    }

    public void SetPlayerInRange(bool value) //Helper for player collision detection
    {
        _isPlayerInRange = value;
        _arrowBox.SetActive(value);

        if (_isPlayerInRange && _arrowType == ArrowType.Yellow)
        {
            RandomizeArrowType();
            RandomizeArrowDirection();
            SetupArrow();
        }
    }

    private void RandomizeArrowType()
    {
        if (_isPlayerInRange)
        {
            _arrowType = (ArrowType)Random.Range(0, 2);
        }
        else
        {
            _arrowType = (ArrowType)Random.Range(0, 3);
        }
    }

    private void RandomizeArrowDirection()
    {
        _swipeDirection = (SwipeDirection)Random.Range(0, 4);
    }

    private void FallDown()
    {
        transform.position += Vector3.down * _moveSpeed * Time.deltaTime;
    }

    private void SetupArrow()
    {
        int directionIndex = (int)_swipeDirection; //Check SwipeDirection.cs for order
        _arrowRenderer.sprite = _arrowSprites[directionIndex];

        switch (_arrowType)
        {
            case ArrowType.Green:
                {
                    _arrowRenderer.color = Color.green;
                    break;
                }

            case ArrowType.Red:
                {
                    _arrowRenderer.color = Color.red;
                    break;
                }

            case ArrowType.Yellow:
                {
                    _arrowRenderer.color = Color.yellow;
                    break;
                }
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

    private IEnumerator CO_RotateArrow()
    {
        int index = 0;

        while (!_isPlayerInRange)
        {
            _swipeDirection = (SwipeDirection)(index % 4);
            SetupArrow();
            index++;

            yield return new WaitForSeconds(0.15f);
        }
    }
}
