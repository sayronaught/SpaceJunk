using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Recipe", order = 1)]
public class SO_Recipe : ScriptableObject
{

    public string recipeName;

    public string recipeDescription;

    [Tooltip("The Icon to describe this recipe")]
    public Texture recipeIcon;

    [Tooltip("How much Energy will it Cost to Create this?")]
    public float energyCost;

    public float timeCost;

    public SO_Item_Inventory.Resource[] required;

    public SO_Item_Inventory.Resource[] usedup;

    public SO_Item_Inventory.Resource[] result;

}
