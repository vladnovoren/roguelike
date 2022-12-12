using AI.Interaction;
using System;
using UnityEngine;

namespace AI.Common.Watch
{
    public class ToWatchDecision : ToWatchDecisionCore
    {
        public ToWatchDecision(GameObject owner, GameObject enemy,
                               SpottingManager spottingManager) :
            base(owner, enemy)
        {
            spottingManager.SubscribeToWatcher(this);
        }

        public override bool Decide()
        {
            if (base.Decide())
            {
                OnEnemySpotted();
                return true;
            }
            return false;
        }

        public event EventHandler EnemySpotted;

        private void OnEnemySpotted()
        {
            Debug.Log("spotted");
            EnemySpotted?.Invoke(this, EventArgs.Empty);
        }
    }
}