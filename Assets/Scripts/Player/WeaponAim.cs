using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Mask[] maskSet;
    
    private readonly float _eyeDistance = 0.13f;

    private SpriteRenderer _eyes;
    private SpriteRenderer _currentMask;

    private void Start()
    {
        _eyes = GetComponent<SpriteRenderer>();
        _currentMask = maskSet[0].GetMask();
    }

    void Update()
    {
        if (movement.rsMove != Vector2.zero)
        {
            _eyes.transform.localPosition = movement.rsMove * _eyeDistance;
            Quaternion eyeRotation;
            if (movement.rsMove.x > 0)
            {
                _currentMask.flipX = false;
                if (movement.rsMove.y > 0)
                    eyeRotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.right, movement.rsMove));
                else
                    eyeRotation = Quaternion.Euler(0, 0, -Vector2.Angle(Vector2.right, movement.rsMove));
            }
            else
            {
                _currentMask.flipX = true;
                if (movement.rsMove.y > 0)
                    eyeRotation = Quaternion.Euler(0, 0, -Vector2.Angle(Vector2.left, movement.rsMove));
                else
                    eyeRotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.left, movement.rsMove));
            }
            
            _eyes.transform.rotation = eyeRotation;
        }
    }
}
