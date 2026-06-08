using UnityEngine;

namespace Gameplay.Enemy
{
    public interface IEnemyState
    {
        void Enter(EnemyAI enemy);
        void Exit();
        void Update();
    }
}
