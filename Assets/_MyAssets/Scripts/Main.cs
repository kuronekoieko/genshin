using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Main : MonoBehaviour
{
    [SerializeField] BaseCharacter character;
    [SerializeField] bool isSub;

    async void Start()
    {
        await Calculator.Calc(character, isSub);
    }



}
