using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace treeAttributes
{
    public struct delegates
    {
        public delegate bool decision(AI bot);
        public delegate bool gameObjectDecision(AI bot, GameObject obj);
        public delegate bool vector3Decision(AI bot, Vector3 Pos);

        public delegate bool action(AI bot);
        public delegate bool gameObjectAction(AI bot, GameObject obj);
        public delegate bool vector3Action(AI bot, Vector3 Pos);

    }

    class Actions
    {
        public static bool MoveToGameObj(AI bot, GameObject target)
        {
            //Debug.Log(target);
            return bot.getActions().MoveTo(target);
        }

        public static bool MoveToVec3(AI bot, Vector3 targetPos)
        {
            return bot.getActions().MoveTo(targetPos);
        }

        public static bool MoveToEnemy(AI bot)
        {
            return bot.getActions().MoveTo(bot.getTargetObj());
        }

        public static bool Attack(AI bot)
        {
            bot.getActions().AttackEnemy(bot.getTargetObj());
            return true;
        }

        public static bool dropItem(AI bot, GameObject item)
        {
            if (bot.GetInventory().HasItem(item.name))
            {
                bot.getActions().DropItem(item);
                if (item == bot.getEnemyFlagObj())
                {
                    bot.setFlagCaptured();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool pickUpItem(AI bot, GameObject item)
        {
            bot.getActions().CollectItem(item);
            return true;
        }

        public static bool useItem(AI bot, GameObject item)
        {
            bot.getActions().UseItem(item);
            return true;
        }
    }

    class Decisions
    {

        public static bool friendlyFlagTaken(AI bot)
        {
            return bot.returnFlagCaptured();
        }
        public static bool isPoweredUp(AI bot)
        {
            return bot.getData().IsPoweredUp;
        }
        public static bool isEnemyFlagCaptured(AI bot)
        {
            return bot.enemyFlagAtBase();
        }
        public static bool ItemInRange(AI bot, GameObject item)
        {
            if (Vector3.Distance(bot.transform.position, item.transform.position) <= bot.getData().PickUpRange)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool checkHealth(AI bot)
        {
            if (bot.getData().CurrentHitPoints > bot.getData().CurrentHitPoints / 2)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool haveItem(AI bot, GameObject item)
        {
            if (item != null && bot.GetInventory().HasItem(item.name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool inAttackRange(AI bot)
        {
            if (Vector3.Distance(bot.transform.position, bot.nearestEnemy.transform.position) <= bot.getData().AttackRange)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool enemySeen(AI bot)
        {
            if (bot.getSenses().GetEnemiesInView() != null)
            {
                bot.setTarget(bot.nearestEnemy);

                if (bot.getTargetObj() != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool itemSeen(AI bot, GameObject item)
        {
            if (bot.getSenses().GetCollectablesInView() != null)
            {
                foreach(GameObject obj in bot.getSenses().GetCollectablesInView())
                {
                    if(obj == item)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public static bool atBase(AI bot)
        {
            if (Vector3.Distance(bot.transform.position, bot.getBase().transform.position) < 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
