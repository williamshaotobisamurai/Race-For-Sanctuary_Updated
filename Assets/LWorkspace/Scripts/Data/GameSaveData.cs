using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaveData 
{
    public int levelIndex = 0;
    public int checkPointID ;
    public int weaponType;
    public int coinsCollected;
    public int sirsActivated = 0;
}
