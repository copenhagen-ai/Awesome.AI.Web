﻿using Awesome.AI.Common;
using Awesome.AI.Core;
using Awesome.AI.Helpers;

namespace Awesome.AI.Systems
{
    public class ChatAnswer
    {
        public string Answer { get; set; }
        public int ChatState { get; set; }

        private TheMind mind;
        private ChatAnswer() { }

        public ChatAnswer(TheMind mind, string starting)
        {
            this.mind = mind;
            ChatState = 0;
            Answer = starting.Replace("WHAT", "");
        }

        public void Decide(bool _pro)
        {
            if (!mind.chat_answer)
                return;

            if (!mind.curr_unit.IsDECISION())
                return;

            if (mind.epochs < 5)
                return;

            if (!_pro)
                return;

            UNIT current = mind.curr_unit;
            HUB hub = current.HUB;

            HUB _1 = mind.mem.HUBS_SUB(Constants.subject_decision[2]);
            HUB _2 = mind.mem.HUBS_SUB(Constants.subject_decision[3]);

            MyRandom rand = mind.rand;
            int[] _rand = rand.MyRandomInt(1, 5);
            if (_rand[0] == 1)
            {
                mind.mem.Randomize(_1);
                mind.mem.Randomize(_2);
            }

            if (hub.subject == "answer_should_decision" && ChatState == 0)
            {
                if (current.data == "ANSWERYES")
                {
                    Answer = ":YES";
                    ChatState = 0;
                }

                if (current.data == "ANSWERNO")
                {
                    Answer = ". . . . .";
                    ChatState++;
                }
            }

            if (hub.subject == "answer_what_decision" && ChatState == 1)
            {
                //Answer = mind.parms._mech.dir.Choise.IsNo() ? current.data : ". . . . .";
                Answer = current.data;
                Answer = Answer.Replace("WHAT", "");
                ChatState = 0;
            }
        }
    }
}
