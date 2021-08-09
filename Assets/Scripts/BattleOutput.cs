using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Extensions;
using BattleSystem.Abstractions.Control;
using BattleSystem.Core.Actions;
using BattleSystem.Core.Actions.Buff;
using BattleSystem.Core.Actions.Damage;
using BattleSystem.Core.Actions.Heal;
using BattleSystem.Core.Actions.Protect;
using BattleSystem.Core.Actions.ProtectLimitChange;
using BattleSystem.Core.Items;
using BattleSystem.Core.Moves;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class BattleOutput : MonoBehaviour, IGameOutput
    {
        public GameObject textContainer;

        private int TextCount => textContainer.GetComponentsInChildren<Text>().Length;

        public void Start()
        {
            foreach (Transform child in textContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void ShowBattleEnd(string winningTeam)
        {
            ShowMessage($"Team {winningTeam} wins!");
        }

        public void ShowCharacterSummary(BattleSystem.Core.Characters.Character character)
        {
            ShowMessage($"{character.Name}: {character.CurrentHealth}/{character.MaxHealth} HP");
        }

        public void ShowItemSummary(Item item)
        {
            ShowMessage($"{item.Name}: {item.Description}");
        }

        public void ShowMessage() => ShowMessage(string.Empty);

        public void ShowMessage(string message)
        {
            var textObj = new GameObject($"text{TextCount}");
            textObj.transform.parent = textContainer.transform;

            var text = textObj.AddComponent<Text>();
            text.text = message;
            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);

            var fitter = textObj.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        public void ShowMoveSetSummary(MoveSet moveSet)
        {
            ShowMessage(moveSet.Summarise(true));
        }

        public void ShowMoveUse(MoveUse moveUse)
        {
            if (moveUse.HasResult && !moveUse.TargetsAllDead)
            {
                switch (moveUse.Result)
                {
                    case MoveUseResult.Success:
                        ShowMessage($"{moveUse.User.Name} used {moveUse.Move.Name}!");
                        break;

                    case MoveUseResult.Miss:
                        ShowMessage($"{moveUse.User.Name} used {moveUse.Move.Name} but missed!");
                        break;

                    case MoveUseResult.Failure:
                        ShowMessage($"{moveUse.User.Name} used {moveUse.Move.Name} but it failed!");
                        break;
                }

                foreach (var actionResult in moveUse.ActionsResults)
                {
                    if (actionResult.Success)
                    {
                        foreach (var result in actionResult.Results)
                        {
                            ShowResult(result);
                        }
                    }
                    else
                    {
                        ShowMessage("But it failed!");
                    }
                }
            }
        }

        public void ShowResult<TSource>(IActionResult<TSource> result)
        {
            if (result.TargetProtected)
            {
                ShowMessage(result.DescribeProtected());
            }
            else switch (result)
            {
                case BuffActionResult<TSource> br:
                    var buffDescription = br.Describe();
                    if (buffDescription != null)
                    {
                        ShowMessage(buffDescription);
                    }
                    break;

                case DamageActionResult<TSource> dr:
                    var damageDescription = dr.Describe();
                    if (damageDescription != null)
                    {
                        ShowMessage(damageDescription);
                    }
                    break;

                case HealActionResult<TSource> hr:
                    var healDescription = hr.Describe();
                    if (healDescription != null)
                    {
                        ShowMessage(healDescription);
                    }
                    break;

                case ProtectLimitChangeActionResult<TSource> plcr:
                    var plcrDescription = plcr.Describe();
                    if (plcrDescription != null)
                    {
                        ShowMessage(plcrDescription);
                    }
                    break;

                case ProtectActionResult<TSource> pr:
                    var protectDescription = pr.Describe();
                    if (protectDescription != null)
                    {
                        ShowMessage(protectDescription);
                    }
                    break;
            }
        }

        public void ShowStartTurn(int turnCounter)
        {
            ShowMessage($"Turn {turnCounter}");
        }

        public void ShowTeamSummary(IEnumerable<BattleSystem.Core.Characters.Character> characters)
        {
            foreach (var c in characters.Where(c => !c.IsDead))
            {
                ShowCharacterSummary(c);
            }
        }
    }
}
