using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SoundType
{
    public Sounds soundType;
    public AudioClip soundClip;
}

public enum Sounds
{
   FoodConsume,
   PoisonConsume,
   ShieldPickup,
   ScoreBoostPickup,
   SpeedBoostPickup,
   PlayerKilled
}

