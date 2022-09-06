using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Recipe", order = 1)]
public class SO_Recipe : ScriptableObject
{

    [System.Serializable]
    public class Resource
    {
        public SO_Item item;
        public int amount;

        public Resource set(SO_Item newitem, int newamount)
        {
            var newResource = new Resource();
            item = newitem;
            amount = newamount;
            return newResource;
        }
    }

    public string recipeName;

    public string recipeDescription;

    [Tooltip("The Icon to describe this recipe")]
    public Texture recipeIcon;

    public float energyCost;

    public float timeCost;

    public Resource[] required;

    public Resource[] usedup;

    public Resource[] result;

}
