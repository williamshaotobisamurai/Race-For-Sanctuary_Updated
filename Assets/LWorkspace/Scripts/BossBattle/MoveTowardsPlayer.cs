using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [Category("BossMovement/MoveTowrdsPlayer")]
    [Description("Moves the agent towards to target per frame without pathfinding")]
    public class MoveTowardsPlayer : ActionTask<Transform>
    {
        [RequiredField]
        public BBParameter<GameObject> target;
        public BBParameter<Vector3> offset = new Vector3(0, 0, 100);

        public BBParameter<float> speed = 2;
        public BBParameter<float> stopDistance = 0.1f;
        public bool waitActionFinish;

        protected override void OnUpdate()
        {
            float remainingDistance = Vector3.Distance(agent.position, target.value.transform.position + offset.value);
            Debug.Log("remaining Distance" + remainingDistance);
            if (remainingDistance <= stopDistance.value)
            {
                EndAction();
                return;
            }

            agent.position = Vector3.MoveTowards(agent.position, target.value.transform.position + offset.value, speed.value * Time.deltaTime);
            if (!waitActionFinish)
            {
                EndAction();
            }

        }
    }
}