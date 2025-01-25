using Awesome.AI.Common;
using Awesome.AI.Core;

namespace Awesome.AI.Systems
{
    public class Chat
    {
        public string Answer { get; set; }
        public int State { get; set; }

        private TheMind mind;
        private Chat() { }

        public Chat(TheMind mind, string starting)
        {
            this.mind = mind;
            State = 0;
            Answer = starting.Replace("WHAT", "");
        }

        public void Decide(bool _pro)
        {
            if (!mind.curr_unit.IsDECISION())
                return;

            if (mind.epochs < 5)
                return;

            if (!_pro)
                return;

            UNIT current = mind.curr_unit;
            HUB hub = current.HUB;

            List<UNIT> units = mind.mem.UNITS_ALL().Where(x => x.IsDECISION()).ToList();

            MyRandom rand = mind.rand;
            int[] _rand = rand.MyRandomInt(1, 5);
            if (_rand[0] == 1)
                mind.mem.Randomize(units);

            if (hub.subject == "answer_should_decision" && State == 0)
            {
                if (current.data == "ANSWERYES")
                {
                    Answer = ":YES";
                    State = 0;
                }

                if (current.data == "ANSWERNO")
                {
                    Answer = "";
                    State++;
                }
            }

            if (hub.subject == "answer_what_decision" && State == 1)
            {
                Answer = mind.parms._mech.dir.Choise.IsNo() ? current.data : "";
                Answer = Answer.Replace("WHAT", "");
                State = 0;
            }
        }
    }
}
