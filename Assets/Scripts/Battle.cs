using System.Collections.Generic;
using System.Linq;
using BattleSystem.Abstractions.Control;
using BattleSystem.Battles.TurnBased;
using UnityEngine;

namespace Assets.Scripts
{
    public class Battle : MonoBehaviour
    {
        public List<Character> characters;

        private TurnBasedBattle battle;

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                StartBattle();
            }
        }

        private void StartBattle()
        {
            var gameOutput = GetComponent<IGameOutput>();
            var actualCharacters = characters.Select(c => c.GetCharacter()).ToList();

            battle = new TurnBasedBattle(
                new MoveProcessor(),
                new ActionHistory(),
                gameOutput,
                actualCharacters
            );

            battle.Start();
        }
    }
}
