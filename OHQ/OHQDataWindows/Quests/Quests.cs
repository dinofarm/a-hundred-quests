using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using OHQData;
using OHQData.Items;
using OHQData.Actors;
using OHQData.NPC;



namespace OHQData.Quests
{
    public class Objective
    {
        public string description;
        public int days, Amount;
        public Monster monster;
        public Item item;
        public string hint;
        public NPCs.NPC npc;
        public bool finished = false;
        public Objective(string Type, string description, Monster monster, int amount, int days)
        {
            //    Monster Objective
            this.description = description;
            this.Amount = amount;
            this.monster = monster;
            this.days = days;
        }
        public Objective(string Type, string description, Item item, int amount, int days)
        {
            //    Fetch Item Objective
            this.description = description;
            this.item = item;
            this.Amount = amount;
            this.days = days;
        }
        public Objective(string Type, string description, Item item, int amount, int days, string hint)
        {
            // Fetch Item with Hint
            this.description = description;
            this.item = item;
            this.Amount = amount;
            this.days = days;
            this.hint = hint;
        }
        public Objective(string Type, string description, NPCs.NPC npc, int days)
        {
            // Go talk to an NPC
            this.description = description;
            this.npc = npc;
            this.days = days;
        }
        public Objective(string Type, string description, NPCs.NPC npc, int days, string hint)
        {
            // Go talk to an NPC with hint
            this.description = description;
            this.npc = npc;
            this.days = days;
            this.hint = hint;
        }
        public Objective(string Type, string description, NPCs.NPC npc, Item item, int amount, int days)
        {
            // Bring an item(s) to an NPC with a hint
            this.description = description;
            this.npc = npc;
            this.item = item;
            this.Amount = amount;
            this.days = days;

        }
        public Objective(string Type, string description, NPCs.NPC npc, Item item, int amount, int days, string hint)
        {
            // Bring an item(s) to an NPC with a hint
            this.description = description;
            this.npc = npc;
            this.item = item;
            this.Amount = amount;
            this.days = days;
            this.hint = hint;

        }

    }

    public class Quest
    {
        public quest thisQuest;

        public struct quest
        {
            public string QuestName;
            public List<Quest> qReq;
            public int lReq;
            public int race;  //0=human 1=pixie 2=dinoman 3=skeleman
            public int expReward;
            public List<Item> itemReward;
            public List<Objective> objectives;
            public OHQData.Skills.Skill skillReward;
            public bool available;
            public bool completed;
            public quest(string qName, List<Quest> qReq, int lReq,
                          int race, int expReward,
                            List<Item> itemReward, OHQData.Skills.Skill skillReward, List<Objective> objectives)
            {
                this.QuestName = qName;
                this.qReq = qReq;
                this.lReq = lReq;
                this.race = race;
                this.expReward = expReward;
                this.itemReward = itemReward;
                this.objectives = objectives;
                this.skillReward = skillReward;


                int count = qReq.Count;
                for (int i = 0; i < qReq.Count; i++)

                    for (int a = 0; a < Player.instance.CompQuests.Count; a++)
                    {
                        for (int b = 0; b < Player.instance.CurrentQuests.Count; b++)
                        {
                            if (qReq[i] == Player.instance.CompQuests[a])
                            {
                                count--;
                            }
                            if (qReq[i] == Player.instance.CurrentQuests[b])
                            {
                                count++;
                                break;
                            }
                        }
                    }
                if (count == 0)
                {
                    this.available = true;
                }
                else
                {
                    this.available = false;
                }
                this.completed = false;
            }
        }
        public Quest(string qName, List<Quest> qReq, int lReq, int race, int expReward, List<Item> ItemReward, OHQData.Skills.Skill skillReward, List<Objective> objectives)
        {

            thisQuest = new quest(qName, qReq, lReq,
                          race, expReward, ItemReward, skillReward, objectives);
        }
        public void compObjective(Objective objective)
        {
            for (int i = 0; i < thisQuest.objectives.Count; i++)
            {
                if (objective == thisQuest.objectives[i])
                {
                    thisQuest.objectives[i].finished = true;

                }

            }
            for (int i = 0; i < thisQuest.objectives.Count + 1; i++)
            {
                if (!thisQuest.objectives[i].finished)
                {
                    break;

                }
                else
                {
                    if (i == thisQuest.objectives.Count + 1)
                    {
                        thisQuest.completed = true; //good job
                        OHQData.Player.instance.CompQuests.Add(this);


                    }
                }
            }

        }


    }
}

