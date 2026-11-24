using System.Collections.Generic;
using UnityEngine;

// tracks which/if player is holding key
public class KeyInventory : MonoBehaviour
{
    private readonly HashSet<string> keys = new HashSet<string>();

    public void Add(string keyId) => keys.Add(keyId);

    public bool Has(string keyId) => keys.Contains(keyId);

    public void Remove(string keyId) => keys.Remove(keyId);
}