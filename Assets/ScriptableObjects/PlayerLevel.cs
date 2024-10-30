using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLevelProperties", menuName = "ScriptableObjects/PlayerLevelProperties")]
public class PlayerLevelProperties : ScriptableObject
{
    
    public int skeletonSuccessHitW1 = 30;
    public int skeletonSuccessHitW2 = 80;
    public int skeletonSuccessHitW3 = 0;

    public int skeletonHitDamageW1 = 25;
    public int skeletonHitDamageW2 = 100;
    public int skeletonHitDamageW3 = 0;

    public int mushroomSuccessHitW1 = 40;
    public int mushroomSuccessHitW2 = 80;
    public int mushroomSuccessHitW3 = 30;

    public int mushroomHitDamageW1 = 20;
    public int mushroomHitDamageW2 = 5;
    public int mushroomHitDamageW3 = 10;

    public int eyeSuccessHitW1 = 5;
    public int eyeSuccessHitW2 = 0;
    public int eyeSuccessHitW3 = 50;

    public int eyeHitDamageW1 = 100;
    public int eyeHitDamageW2 = 0;
    public int eyeHitDamageW3 = 20;




}