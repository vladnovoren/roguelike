﻿using AI.Base;
using UnityEngine;
using Utils.Time;

namespace AI.Roaming
{
    class StayToFollowDecision : IDecision
    {
        public StayToFollowDecision(CountdownTimer timer)
        {
            _timer = timer;
        }

        public bool Decide()
        {
            return _timer.IsDown();
        }

        private CountdownTimer _timer;
    }
}
