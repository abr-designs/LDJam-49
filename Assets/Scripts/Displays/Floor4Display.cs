using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Floor4Display : MonoBehaviour, IShowWarning
{
    private static GameManager GameManager => GameManager.Instance;

    [SerializeField]
    private SpriteRenderer[] targets;

    [SerializeField]
    private SpriteRenderer starterSpriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var values = GameManager.breakerBoxValues;
        var sum = values.Sum();
        for (int i = 0; i < targets.Length; i++)
        {
            Color color;

            switch (values[i])
            {
                case 0:
                    color = Color.red;
                    break;
                case 1:
                    color = Color.yellow;
                    break;
                case 2:
                    color = Color.green;
                    break;
                default:
                    throw new Exception();
            }

            targets[i].color = color;

            if (Starter.Active)
            {
                starterSpriteRenderer.color = Color.green;

            }
            else
                starterSpriteRenderer.color = sum == 8  ? Color.yellow : Color.gray;

        }
        
        
    }

    public bool ShouldDisplayWarning() => !Starter.Active;
}
