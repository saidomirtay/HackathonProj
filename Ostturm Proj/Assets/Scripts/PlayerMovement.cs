using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    public static GameObject[] enemies;

    private void Start()
    {
        enemies = new GameObject[2];
        enemies[0] = GameObject.Find("Enemy 1");
        enemies[1] = GameObject.Find("Enemy 2");
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Attack.actions > 0 && Attack.isUrTurn)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.eulerAngles = new Vector3(0, 90, 0);
                Vector3 pos = transform.position;
                if (pos.x < 5.55f)
                {
                    transform.position = new Vector3(pos.x + 3.7f, pos.y, pos.z);
                    if (!EnemyPositionCheck(pos))
                    {
                        Attack.actions--;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                transform.eulerAngles = new Vector3(0, -90, 0);
                Vector3 pos = transform.position;
                if (pos.x > -5.55f)
                {
                    transform.position = new Vector3(pos.x - 3.7f, pos.y, pos.z);
                    if (!EnemyPositionCheck(pos))
                    {
                        Attack.actions--;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                Vector3 pos = transform.position;
                if (pos.z < 3.2f)
                {
                    transform.position = new Vector3(pos.x, pos.y, pos.z + 3.2f);
                    if (!EnemyPositionCheck(pos))
                    {
                        Attack.actions--;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                Vector3 pos = transform.position;
                if (pos.z > -3.2f)
                {
                    transform.position = new Vector3(pos.x, pos.y, pos.z - 3.2f);
                    if (!EnemyPositionCheck(pos))
                    {
                        Attack.actions--;
                    }
                }
            }
        }
    }

    private bool EnemyPositionCheck(Vector3 pos)
    {
        foreach (GameObject enemy in enemies)
        {
            if (transform.position == enemy.transform.position)
            {
                transform.position = pos;
                return true;
            }
        }
        return false;
    }
}
