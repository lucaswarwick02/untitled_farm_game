using System;
using System.Collections;
using Data;
using Databases;
using Enumerations;
using Player;
using ScriptableObjects;
using ScriptableObjects.Items;
using UnityEngine;

public class VegetableSpot : MonoBehaviour
{
    [SerializeField] private Interactable interactable;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private OutlineSpriteRenderer outlineSpriteRenderer;
    [SerializeField] private Sprite emptySprite;
    
    private VegetableState _state = VegetableState.Empty;
    private Seed _seed;
    
    private Coroutine _growthCoroutine;
    private Coroutine _deathCoroutine;

    private void Update()
    {
        switch (_state)
        {
            case VegetableState.Dead:
                interactable.enabled = true;
                break;
            case VegetableState.Empty:
                if (string.IsNullOrEmpty(PlayerHandler.Instance.interact.CurrentItem.itemID))
                {
                    interactable.enabled = false;
                }
                else
                {
                    var heldItem = ItemDatabase.Query(PlayerHandler.Instance.interact.CurrentItem.itemID);
                    interactable.enabled = heldItem is Seed;
                }
                break;
            case VegetableState.Growing:
                interactable.enabled = false;
                break;
            case VegetableState.Ready:
                interactable.enabled = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Interact()
    {
        switch (_state)
        {
            case VegetableState.Dead:
                ClearSpot();
                break;
            case VegetableState.Empty:
                if (string.IsNullOrEmpty(PlayerHandler.Instance.interact.CurrentItem.itemID)) break;
                var heldItem = ItemDatabase.Query(PlayerHandler.Instance.interact.CurrentItem.itemID);
                if (heldItem is Seed seed)
                {
                    ClearSpot();
                    _seed = seed;
                    _state = VegetableState.Growing;
                    _growthCoroutine = StartCoroutine(GrowthTimer());
                    _deathCoroutine = StartCoroutine(DeathTimer());
                    SaveManager.CurrentSave.inventory.UseItem(PlayerHandler.Instance.interact.CurrentIndex);
                }
                break;
            case VegetableState.Ready:
                SaveManager.CurrentSave.inventory.AddToInventory(new ItemInstance{ itemID = _seed.Vegetable.ItemID, amount = UnityEngine.Random.Range(_seed.MinAmount, _seed.MaxAmount + 1)});
                ClearSpot();
                break;
        }
    }

    private void ClearSpot()
    {
        if (_growthCoroutine != null) StopCoroutine(_growthCoroutine);
        if (_deathCoroutine != null) StopCoroutine(_deathCoroutine);
        _state = VegetableState.Empty;
        _seed = null;
        spriteRenderer.sprite = emptySprite;
        outlineSpriteRenderer.UpdateSprites(emptySprite);
    }

    private IEnumerator GrowthTimer()
    {
        var timer = 0f;
        _state = VegetableState.Growing;
        
        while (timer < _seed.GrowthTime)
        {
            spriteRenderer.sprite = _seed.GrowthImages[Mathf.FloorToInt(timer / _seed.GrowthTime * _seed.GrowthImages.Length)];
            
            timer += Time.deltaTime;
            yield return null;
        }

        _state = VegetableState.Ready;
    }

    private IEnumerator DeathTimer()
    {
        var timer = 0f;
        
        while (timer < _seed.DeathTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.sprite = _seed.DeadImage;
        _state = VegetableState.Dead;
    }
}