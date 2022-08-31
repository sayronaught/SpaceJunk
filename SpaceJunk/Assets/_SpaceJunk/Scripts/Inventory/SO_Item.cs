using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item", order = 1)]
public class SO_Item : ScriptableObject
{
    public string itemName;

    public string itemDescription;

    public Texture itemIcon;

    public float itemMass;

    public float itemBaseValue;

}
