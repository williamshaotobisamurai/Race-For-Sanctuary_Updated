using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{

    public class BossMoveRandomly : ActionTask<Transform>
    {
        public BBParameter<GameObject> target;
        public BBParameter<float> speed = 50;
        public BBParameter<float> distanceToSkully = 180;

        public BBParameter<float> stopDistance = 0.1f;
        public bool waitActionFinish;

        public BBParameter<float> height;
        public BBParameter<float> width;
        public BBParameter<Vector3> center;


        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            //	EndAction(true);
        }

        private bool isInit = false;
        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {
            if (!isInit)
            {
                GetRandomPositionAroundSkully(distanceToSkully.value);
                isInit = true;
            }

            if (isInit)
            {
                if ((agent.position - target.value.transform.position).magnitude <= stopDistance.value)
                {
                    isInit = false;
                    EndAction();
                    return;
                }

                agent.position = Vector3.MoveTowards(agent.position, target.value.transform.position, speed.value * Time.deltaTime);

                if (!waitActionFinish)
                {
                    isInit = false;
                    EndAction();
                }
            }
        }

        //Called when the task is disabled.
        protected override void OnStop()
        {

        }

        //Called when the task is paused.
        protected override void OnPause()
        {

        }


        private Vector3 GetRandomPositionAroundSkully(float targetDistance)
        {
            int tries = 0;
            float maxDistance = 0;
            Vector3 pendingPos = agent.transform.position;
            while (tries < 10)
            {
                tries++;
                float randomX = center.value.x + Random.Range(-width.value, width.value);
                float randomY = center.value.y + Random.Range(-height.value, height.value);

                Vector3 targetPos = new Vector3(randomX, randomY, center.value.z);
                target.value.transform.position = targetPos;

                float distance = Vector3.Distance(targetPos, agent.transform.position);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    pendingPos = targetPos;
                }
            }

            return pendingPos;
        }
    }
}