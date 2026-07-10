using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Castillo.Player
{
    public class PlayerShaderPosition : MonoBehaviour
    {
        private static readonly int PLAYER_POSITION_ID =
            Shader.PropertyToID("_PlayerPosition");

        private void LateUpdate()
        {
            Vector3 playerPosition = transform.position;

            Shader.SetGlobalVector(PLAYER_POSITION_ID, playerPosition);
        }
    }
}