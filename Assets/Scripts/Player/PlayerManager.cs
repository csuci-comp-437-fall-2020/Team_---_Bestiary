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
    [SerializeField] private PlayerEffects playerPassives;
    [SerializeField] private Mask[] maskPrefabs;

    private PlayerMovement _movement;

    private Mask[,] _masks = new Mask[4,PerSlot];
    private int[] currentMask = {-1, -1, -1, -1};
    private Mask _equippedMask;

    void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        eyes.movement = _movement;
        _equippedMask = eyes.gameObject.GetComponent<Mask>();

        foreach (Mask mask in maskPrefabs)
        {
            Mask spawnedMask = Instantiate(mask, transform, true);
            WeaponAim maskAim = spawnedMask.GetComponent<WeaponAim>();
            spawnedMask.transform.localPosition = new Vector3(maskAim._eyeDistance, 0, 0);
            maskAim.movement = _movement;
            spawnedMask.movement = _movement;
            int directionIndex = (int) spawnedMask.slot / 4;
            int maskIndex = (int) spawnedMask.slot % PerSlot;
            if (_masks[directionIndex, maskIndex] == null)
            {
                _masks[directionIndex, maskIndex] = spawnedMask;
                currentMask[directionIndex] = maskIndex;
            }
            else
            {
                if (spawnedMask.powerLevel > _masks[directionIndex, maskIndex].powerLevel)
                {
                    _masks[directionIndex, maskIndex] = spawnedMask;
                    currentMask[directionIndex] = maskIndex;
                }
            }
            spawnedMask.gameObject.SetActive(false);
        }

        // TODO decide this better
        if (currentMask[(int) Direction.Down] >= 0)
        {
            playerPassives.slamEquipped = currentMask[(int) Direction.Down];
        }
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
        CycleMask((int) Direction.Down);
    }

    
    private void CycleMask(int direction)
    {
        int current = currentMask[direction];
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

}
