using Assets.Scripts.Characters;
using BattleSystem.Battles.TurnBased.Constants;
using BattleSystem.Core.Abilities;
using BattleSystem.Core.Actions;
using BattleSystem.Core.Actions.Buff;
using BattleSystem.Core.Actions.Damage;
using BattleSystem.Core.Moves;
using BattleSystem.Core.Stats;
using UnityEngine.UI;

namespace Assets.Scripts
{
    /// <summary>
    /// Script for a playable character.
    /// </summary>
    public class Player : Character
    {
        public Button AttackButton;

        protected override BattleSystem.Core.Characters.Character CreateCharacter()
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

            return new PlayableCharacter(Name, Team, MaxHealth, statSet, moveSet, ability, AttackButton);
        }
    }
}
