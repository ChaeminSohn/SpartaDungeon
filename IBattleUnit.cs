using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public enum Stat  //스탯 종류
    {
        Health,     //체력
        Attack,     //공격력
        Defense,    //방어력
    }

    internal interface IBattleUnit
    {
        string Name { get; }
        int Level { get; }
        int BaseFullHP { get; }
        int CurrentHP { get; }
        int BaseAttack { get; }
        int BaseDefense { get; }

        void OnDamage(int damage);
        void OnDie();

        void RecoverHP(int hp);
    }
}
