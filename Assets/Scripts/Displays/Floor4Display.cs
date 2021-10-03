using System;
using System.Linq;
using UnityEngine;

public class Floor4Display : MonoBehaviour, IShowWarning, ICanBeGarbled
{
    private static GameManager GameManager => GameManager.Instance;

    [SerializeField]
    private BreakerWindow breakerWindow;

    [SerializeField] private SpriteRenderer[] targets;

    [SerializeField] private SpriteRenderer starterSpriteRenderer;

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
                starterSpriteRenderer.color = sum == 8 ? Color.yellow : Color.gray;

        }


    }

    public bool ShouldDisplayWarning() => !Starter.Active;

    public void Garble()
    {
        var count = GameManager.breakerBoxValues.Length;

        for (int i = 0; i < count; i++)
        {
            GameManager.StoreStationValue(Station.TYPE.BREAKER, i, breakerWindow.GetRandomValue());
        }
    }
}
