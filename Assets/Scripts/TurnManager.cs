using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    static Dictionary<string, List<TacticsMove>> units = new Dictionary<string, List<TacticsMove>>();
    static Queue<string> keyTurn = new Queue<string>();
    static Queue<TacticsMove> turnTeam = new Queue<TacticsMove>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(turnTeam.Count == 0)
        {
            InitTeamTurnQueue();
        }
    }

    static void InitTeamTurnQueue()
    {
        List<TacticsMove> teamList = units[keyTurn.Peek()];

        foreach(TacticsMove unit in teamList)
        {
            turnTeam.Enqueue(unit);
        }

        StartTurn();
    }

    static void StartTurn()
    {
        if(turnTeam.Count > 0)
        {
            turnTeam.Peek().BeginTurn();
        }
    }

    public static void EndTurn()
    {
        TacticsMove unit = turnTeam.Dequeue();
        unit.EndTurn();

        if(turnTeam.Count > 0)
        {
            StartTurn();
        }
        else
        {
            string team = keyTurn.Dequeue();
            keyTurn.Enqueue(team);

            InitTeamTurnQueue();

        }
    }

    public static void AddUnit(TacticsMove unit)
    {
        List<TacticsMove> list;

        if (!units.ContainsKey(unit.tag))
        {
            list = new List<TacticsMove>();
            units[unit.tag] = list;

            if (!keyTurn.Contains(unit.tag))
            {
                keyTurn.Enqueue(unit.tag);
            }

        }
        else
        {
            list = units[unit.tag];
        }

        list.Add(unit);
    }
}
