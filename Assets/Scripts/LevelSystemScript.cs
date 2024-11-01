using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSystemScript : MonoBehaviour
{
    public delegate void UpdateLevelEvent();
    public static event UpdateLevelEvent updateLevelEvent;

    [SerializeField] int playerLevel = 1;
    [SerializeField] int level2Required = 100;
    [SerializeField] int level3Required = 200;
    int currectPointAmount = 0;
    [SerializeField] int skeletonPoints = 10;
    [SerializeField] int mushroomPoints = 20;
    [SerializeField] int eyePoints = 20;

    [Header("HP Text In Game UI (Level)")]
    [SerializeField] TextMeshProUGUI textMeshProUGUI;

    public static int skeletonSuccessHitW1;
    public static int skeletonSuccessHitW2;
    public static int SkeletonSuccessHitW3;

    public static int skeletonHitDamageW1;
    public static int skeletonHitDamageW2;
    public static int skeletonHitDamageW3;

    public static int mushroomSuccessHitW1;
    public static int mushroomSuccessHitW2;
    public static int mushroomSuccessHitW3;

    public static int mushroomHitDamageW1;
    public static int mushroomHitDamageW2;
    public static int mushroomHitDamageW3;

    public static int eyeSuccessHitW1;
    public static int eyeSuccessHitW2;
    public static int eyeSuccessHitW3;

    public static int eyeHitDamageW1;
    public static int eyeHitDamageW2;
    public static int eyeHitDamageW3;

    [SerializeField] PlayerLevelProperties level1;
    [SerializeField] PlayerLevelProperties level2;
    [SerializeField] PlayerLevelProperties level3;
    void Start()
    {
        EnemyScript.skeletonPoints += AddPointsSkeleton;
        Enemy2Script.mushroomPoints += AddPointsMushroom;
        Enemy3Script.eyePoints += AddPontsEye;
        UpdateLevel(level1);
        UpdateLevelUI();
        
    }

    private void UpdateLevelUI()
    {
        if(playerLevel == 1)
            textMeshProUGUI.text = "Level: " + playerLevel + " Next:" + currectPointAmount + "/" + level2Required;
        if (playerLevel == 2)
            textMeshProUGUI.text = "Level: " + playerLevel + " Next:" + currectPointAmount + "/" + level3Required;
        if (playerLevel == 3)
            textMeshProUGUI.text = "Level: " + playerLevel + " Next:" + currectPointAmount + "/---";


    }

    private void UpdateLevel(PlayerLevelProperties currentLevel)
    {
         skeletonSuccessHitW1 = currentLevel.skeletonSuccessHitW1;
         skeletonSuccessHitW2 = currentLevel.skeletonSuccessHitW2;
         SkeletonSuccessHitW3 = currentLevel.skeletonSuccessHitW3;

         skeletonHitDamageW1 = currentLevel.skeletonHitDamageW1;
         skeletonHitDamageW2 = currentLevel.skeletonHitDamageW2;
         skeletonHitDamageW3 = currentLevel.skeletonHitDamageW3;

         mushroomSuccessHitW1 = currentLevel.mushroomSuccessHitW1;
         mushroomSuccessHitW2 = currentLevel.mushroomSuccessHitW2;
         mushroomSuccessHitW3 = currentLevel.mushroomSuccessHitW3;

         mushroomHitDamageW1 = currentLevel.mushroomHitDamageW1;
         mushroomHitDamageW2 = currentLevel.mushroomHitDamageW2;
         mushroomHitDamageW3 = currentLevel.mushroomHitDamageW3;

         eyeSuccessHitW1 = currentLevel.eyeSuccessHitW1;
         eyeSuccessHitW2 = currentLevel.eyeSuccessHitW2;
         eyeSuccessHitW3 = currentLevel.eyeSuccessHitW3;

         eyeHitDamageW1 = currentLevel.eyeHitDamageW1;
         eyeHitDamageW2 = currentLevel.eyeHitDamageW2;
         eyeHitDamageW3 = currentLevel.eyeHitDamageW3;

        if (updateLevelEvent != null)
            updateLevelEvent.Invoke();

    }

    private void AddPointsSkeleton()
    {
        currectPointAmount += skeletonPoints;
        Debug.Log("Get points");
        if (currectPointAmount >= level2Required)
        {
            playerLevel = 2;
            UpdateLevel(level2);
        }
        else if (currectPointAmount >= level3Required)
        {
            playerLevel = 3;
            UpdateLevel(level3);
        }
        UpdateLevelUI();

    }
    private void AddPointsMushroom()
    {
        currectPointAmount += mushroomPoints;
        Debug.Log("Get points");
        if (currectPointAmount >= level2Required)
        {
            playerLevel = 2;
            UpdateLevel(level2);
        }
        else if (currectPointAmount >= level3Required)
        {
            playerLevel = 3;
            UpdateLevel(level3);
        }
        UpdateLevelUI();
    }
    private void AddPontsEye()
    {
        currectPointAmount += eyePoints;
        Debug.Log("Get points");
        if (currectPointAmount >= level2Required)
        {
            playerLevel = 2;
            UpdateLevel(level2);
        }
        else if (currectPointAmount >= level3Required)
        {
            playerLevel = 3;
            UpdateLevel(level3);
        }
        UpdateLevelUI();
    }

    // Update is called once per frame
  
}
