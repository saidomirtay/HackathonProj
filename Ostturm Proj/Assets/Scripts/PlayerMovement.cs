using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    private BattleSystem battleSystem;
    public GameObject[] bottles;

    private void Start()
    {
        battleSystem = GetComponent<BattleSystem>();
    }
    
    private void Update()
    {
        switch (BattleSystem.state)
        {
            case BattleState.FIRSTTURN:
                GetInput(BattleSystem.characters[0], 0);
                break;
            case BattleState.SECONDTURN:
                GetInput(BattleSystem.characters[1], 1);
                break;
            case BattleState.THIRDTURN:
                GetInput(BattleSystem.characters[2], 2);
                break;
        }
    }

    private void GetInput(GameObject MovingObject, int index)
    {
        if (battleSystem.charactersUnits[index].currentActions > 0 && battleSystem.charactersUnits[index].legsFine != 1)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                MovingObject.transform.eulerAngles = new Vector3(0, 90, 0);
                Vector3 pos = MovingObject.transform.position;
                if (pos.x < 5.55f)
                {
                    MovingObject.transform.position = new Vector3(pos.x + 3.7f, pos.y, pos.z);
                    if (!EnemyPositionCheck(pos, MovingObject))
                    {
                        battleSystem.SubtractActions(index);
                    }
                }
                BottleClaim(index);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                MovingObject.transform.eulerAngles = new Vector3(0, -90, 0);
                Vector3 pos = MovingObject.transform.position;
                if (pos.x > -5.55f)
                {
                    MovingObject.transform.position = new Vector3(pos.x - 3.7f, pos.y, pos.z);
                    if (!EnemyPositionCheck(pos, MovingObject))
                    {
                        battleSystem.SubtractActions(index);
                    }
                }
                BottleClaim(index);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                MovingObject.transform.eulerAngles = new Vector3(0, 0, 0);
                Vector3 pos = MovingObject.transform.position;
                if (pos.z < 3.2f)
                {
                    MovingObject.transform.position = new Vector3(pos.x, pos.y, pos.z + 3.2f);
                    if (!EnemyPositionCheck(pos, MovingObject))
                    {
                        battleSystem.SubtractActions(index);
                    }
                }
                BottleClaim(index);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                MovingObject.transform.eulerAngles = new Vector3(0, 180, 0);
                Vector3 pos = MovingObject.transform.position;
                if (pos.z > -3.2f)
                {
                    MovingObject.transform.position = new Vector3(pos.x, pos.y, pos.z - 3.2f);
                    if (!EnemyPositionCheck(pos, MovingObject))
                    {
                        battleSystem.SubtractActions(index);
                    }
                }
                BottleClaim(index);
            }
        }
        else
        {
            battleSystem.charactersUnits[index].legsFine = 0;
            battleSystem.charactersHUD[index].SetStatus("");
            battleSystem.NextTurn(index);
        }
    }

    private bool EnemyPositionCheck(Vector3 pos, GameObject MovingObject)
    {
        foreach (GameObject character in BattleSystem.characters)
        {
            if (MovingObject.transform.position == character.transform.position && MovingObject != character)
            {
                MovingObject.transform.position = pos;
                return true;
            }
        }
        return false;
    }

    private void BottleClaim(int index)
    {
        for (int i = 0; i < bottles.Length; i++)
        {
            if (Vector3.Distance(BattleSystem.characters[index].transform.position, bottles[i].transform.position) < 1)
            {
                bottles[i].transform.position = new Vector3(777, 777, 777);
                battleSystem.RangedWeapon(index);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("bottle claimed");
        switch (BattleSystem.state)
        {
            case BattleState.FIRSTTURN:
                battleSystem.RangedWeapon(0);
                break;
            case BattleState.SECONDTURN:
                battleSystem.RangedWeapon(1);
                break;
            case BattleState.THIRDTURN:
                battleSystem.RangedWeapon(2);
                break;
        }
        Destroy(other.gameObject);
    }
}
