using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlManager : MonoSingleton<LvlManager>
{
    [SerializeField]
    private List<Lvl> lvls;
    [SerializeField]
    private int lvlDigit;
    public Lvl lvl=> lvls[lvlDigit];
}
