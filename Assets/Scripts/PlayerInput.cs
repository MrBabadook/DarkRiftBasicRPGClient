using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRiftRPG
{
    public class PlayerInput : MonoBehaviour
    {
        Camera cam;
        private void Start()
        {
            cam = Camera.main;
        }
        private void Update()
        {
            CheckForClick();
        }

        private void CheckForClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.CompareTag("Terrain"))
                {
                    Debug.DrawRay(ray.origin, Input.mousePosition, Color.red, 1f);
                    Debug.Log($"Click detected at: {hit.point}");
                    GameManager.Instance.SendClickPosToServer(hit.point);
                }
            }
        }
    }
}

