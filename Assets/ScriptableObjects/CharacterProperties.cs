using UnityEngine;

[CreateAssetMenu(fileName = "CaracterScriptableObject", menuName = "ScriptableObjects/CharacterProperties")]

public class CharecterProperties : ScriptableObject
{
    public int health = 100;
    public int attackDamage = 0;
    public int speed = 0;
}
