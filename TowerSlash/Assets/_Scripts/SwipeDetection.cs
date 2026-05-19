using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Player _player;
    [SerializeField] private float _minSwipeDistance; //Minimum distance for a swipe to count

    private InputAction _touchPressedAction;
    private InputAction _touchPositionAction;

    private Vector2 _touchStart;
    private Vector2 _touchEnd;

    private void Awake()
    {
        _touchPressedAction = _playerInput.actions["TouchPress"];
        _touchPositionAction = _playerInput.actions["TouchPosition"];
    }

    private void OnEnable()
    {
        _touchPressedAction.started += OnTouchStarted;
        _touchPressedAction.canceled += OnTouchReleased;
    }

    private void OnDisable()
    {
        _touchPressedAction.started -= OnTouchStarted;
        _touchPressedAction.canceled -= OnTouchReleased;
    }

    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        _touchStart = _touchPositionAction.ReadValue<Vector2>();
    }

    private void OnTouchReleased(InputAction.CallbackContext context)
    {
        _touchEnd = _touchPositionAction.ReadValue<Vector2>();
        Vector2 swipe = _touchEnd - _touchStart;

        if (swipe.magnitude < _minSwipeDistance)
        {
            Debug.Log("Swipe too short");
            return;
        }

        SwipeDirection swipeDirection = GetSwipeDirection(swipe);
        _player.CheckSwipe(swipeDirection);
        Debug.Log($"Swipe detected: {swipeDirection}");
    }

    private SwipeDirection GetSwipeDirection(Vector2 swipe)
    {
        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y)) //Check if the swipe is more horizontal than vertical
        {
            if (swipe.x > 0)
            {
                return SwipeDirection.Right;
            }

            return SwipeDirection.Left;
        }

        if (swipe.y > 0)
        {
            return SwipeDirection.Up;
        }
        
        return SwipeDirection.Down;
    }
}
