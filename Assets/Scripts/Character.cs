using BattleSystem.Battles.TurnBased.Constants;
using BattleSystem.Core.Abilities;
using BattleSystem.Core.Actions;
using BattleSystem.Core.Actions.Buff;
using BattleSystem.Core.Actions.Damage;
using BattleSystem.Core.Characters;
using BattleSystem.Core.Moves;
using BattleSystem.Core.Stats;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Script for a character.
    /// </summary>
    public class Character : MonoBehaviour
    {
        public string Name;
        public string Team;

        [Range(10, 50)]
        public int MaxHealth;

        [Range(1, 5)]
        public int AttackStat;

        [Range(1, 5)]
        public int DefenceStat;

        [Range(1, 5)]
        public int SpeedStat;

        private BattleSystem.Core.Characters.Character character;

        public delegate void HealthChangeEvent(int? changeAmount, int newHealth, float newFraction);
        public event HealthChangeEvent OnHealthChange = (_, __, ___) => { };

        public BattleSystem.Core.Characters.Character GetCharacter() => character;
        public float HealthFraction => (float) character.CurrentHealth / character.MaxHealth;

        public void Start()
        {
            character = CreateCharacter();
        }

        protected virtual BattleSystem.Core.Characters.Character CreateCharacter()
        {
            var statSet = new StatSet
            {
                Attack = new Stat(AttackStat),
                Defence = new Stat(DefenceStat),
                Speed = new Stat(SpeedStat),
            };

            var moveSet = new MoveSetBuilder()
                .WithMove(
                    new MoveBuilder()
                        .Name("Strike")
                        .Describe("Strikes the enemy.")
                        .WithMaxUses(100)
                        .AlwaysSucceeds()
                        .WithAction(
                            new DamageActionBuilder()
                                .TargetsFirstEnemy()
                                .PercentageDamage(10)
                                .Build()
                        )
                        .Build()
                )
                .Build();

            var ability = new AbilityBuilder()
                .Name("Ability")
                .Describe("Raises the user's attack stat by 10% at the end of each turn.")
                .WithActionContainer(
                    new ActionContainerBuilder()
                        .WithTaggedAction(
                            new BuffActionBuilder()
                                .TargetsUser()
                                .WithRaiseAttack(0.1)
                                .Build(),
                            ActionTags.EndTurn
                        )
                        .Build()
                )
                .Build();

            var random = new BattleSystem.Core.Random.Random();

            return new BasicCharacter(Name, Team, MaxHealth, statSet, moveSet, ability, random);
        }
    }
}
