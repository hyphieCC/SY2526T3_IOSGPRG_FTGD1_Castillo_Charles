using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private Renderer _bgRenderer;

    void Update()
    {
        _bgRenderer.material.mainTextureOffset += new Vector2(0, _speed * Time.deltaTime);
    }
}
