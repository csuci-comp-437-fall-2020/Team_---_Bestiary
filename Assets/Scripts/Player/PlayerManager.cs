using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum Direction
    {
        Right = 0,
        Left = 1,
        Up = 2,
        Down = 3
    }

    public static readonly int PerSlot = 3;

    public WeaponAim eyes;
    [SerializeField] private PlayerEffects baseEffects;
    [SerializeField] private Mask[] maskPrefabs;

    private PlayerMovement _movement;

    private Mask[,] _masks = new Mask[4,PerSlot];
    private int[] _currentMask = {-1, -1, -1, -1};
    private Mask _equippedMask;
    private PlayerEffects _playerEffects;
    private PlayerHealth _playerHealth;

    void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        eyes.movement = _movement;
        _equippedMask = eyes.gameObject.GetComponent<Mask>();
        _playerEffects = Instantiate(baseEffects);
        _movement.playerEffects = _playerEffects;
        _playerHealth = gameObject.AddComponent<PlayerHealth>();
        _playerHealth.playerEffects = _playerEffects;

        foreach (Mask mask in maskPrefabs)
        {
            Debug.Log("Yes this loaded");
            Mask spawnedMask = Instantiate(mask, transform, true);
            WeaponAim maskAim = spawnedMask.GetComponent<WeaponAim>();
            if (maskAim == null)
                Debug.Log("Failed to get mask aim");
            spawnedMask.transform.localPosition = new Vector3(maskAim._eyeDistance, 0, 0);
            maskAim.movement = _movement;
            spawnedMask.weaponAim = maskAim;
            spawnedMask.playerEffects = _playerEffects;
            
            if (spawnedMask.weaponAim == null)
                Debug.Log("Failed to assign mask aim");

            // TODO function
            if (spawnedMask.passiveEffect != null)
            {
                _playerEffects.extraJumps += spawnedMask.passiveEffect.extraJumps;
                _playerEffects.extraDashes += spawnedMask.passiveEffect.extraDashes;
                _playerEffects.wallClingTime += spawnedMask.passiveEffect.wallClingTime;
                _playerEffects.floatDuration += spawnedMask.passiveEffect.floatDuration;
                _playerEffects.canWallCling = _playerEffects.canWallCling || spawnedMask.passiveEffect.canWallCling;
            }

            int directionIndex = Convert.ToInt32(spawnedMask.slot) / PerSlot;
            int maskIndex = Convert.ToInt32(spawnedMask.slot) % PerSlot;
            if (_masks[directionIndex, maskIndex] == null)
            {
                _masks[directionIndex, maskIndex] = spawnedMask;
                _currentMask[directionIndex] = maskIndex;
            }
            else
            {
                if (spawnedMask.powerLevel > _masks[directionIndex, maskIndex].powerLevel)
                {
                    _masks[directionIndex, maskIndex] = spawnedMask;
                    _currentMask[directionIndex] = maskIndex;
                }
            }
            spawnedMask.gameObject.SetActive(false);
        }

        _playerHealth.RecalculateHitPoints(true);
        RecalculateSlamEquipped();
        Debug.Log("It went all the way");

    }
    
    

    public void CycleRightMask()
    {
        CycleMask((int) Direction.Right);
    }
    
    public void CycleLeftMask()
    {
        CycleMask((int) Direction.Left);
    }
    
    public void CycleUpMask()
    {
        CycleMask((int) Direction.Up);
    }
    
    public void CycleDownMask()
    {
        if (_playerEffects.slamPower > 0)
            _playerEffects.slamEquipped = _playerEffects.slamEquipped % _playerEffects.slamPower + 1;
        CycleMask((int) Direction.Down);
    }

    
    private void CycleMask(int direction)
    {
        int current = _currentMask[direction];
        if (current < 0)
            return;
        
        if (_equippedMask == _masks[direction, current])
        {
            int next = (current + 1) % PerSlot;
            while (next != current)
            {
                if (_masks[direction, next] != null)
                {
                    EquipNewMask((direction, next));
                    return;
                }
                next = (next + 1) % PerSlot;
            }
        }
        else
        {
            EquipNewMask((direction, current));
        }
    }

    private void EquipNewMask((int, int) newMaskIndex)
    {
        _equippedMask.gameObject.SetActive(false);
        _masks[newMaskIndex.Item1, newMaskIndex.Item2].gameObject.SetActive(true);
        _equippedMask = _masks[newMaskIndex.Item1, newMaskIndex.Item2];
    }

    private void RecalculateSlamEquipped()
    {
        
        if (_currentMask[(int) Direction.Down] >= 0)
        {
            int slamEquipped = 0;
            for (int i = 0; i < _currentMask[(int) Direction.Down] ; i++)
            {
                if (_masks[(int) Direction.Down, i] != null)
                    slamEquipped++;
            }
            _playerEffects.slamEquipped = slamEquipped;
        }
    }
}
