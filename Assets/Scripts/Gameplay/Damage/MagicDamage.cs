using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class MagicDamage : IDamage
{
    public int GetCalculate(CreatureStats creature, int damage)
    {
        return damage;
    }
}
