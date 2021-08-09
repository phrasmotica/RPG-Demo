using System.Collections.Generic;
using System.Linq;
using BattleSystem.Core.Abilities;
using BattleSystem.Core.Moves;
using BattleSystem.Core.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Characters
{
    public class PlayableCharacter : BattleSystem.Core.Characters.Character
    {
        private readonly Button _attackButton;

        public PlayableCharacter(string name, string team, int maxHealth, StatSet stats, MoveSet moves, Ability ability, Button attackButton) : base(name, team, maxHealth, stats, moves, ability)
        {
            _attackButton = attackButton;
        }

        public override MoveUse ChooseMove(IEnumerable<BattleSystem.Core.Characters.Character> otherCharacters)
        {
            Debug.Log("Playable character choosing move");
            _attackButton.enabled = true;
            Debug.Log("Playable character chose move");

            return new MoveUse
            {
                Move = Moves.Moves.First(),
                User = this,
                OtherCharacters = otherCharacters,
            };
        }
    }
}
