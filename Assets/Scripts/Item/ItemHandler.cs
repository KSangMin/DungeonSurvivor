using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    public ItemData ItemData { get { return itemData; } }
}
