using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FingerVisualizer
{
	public class FingerVisualizer : Visualizer
    {
        [Header(" [ Finger Visualizer ]")]
        [SerializeField] private Finger[] fingers;

        [SerializeField] private Animator anim;

		protected override void Start()
		{
			base.Start();

			foreach(var finger in fingers)
			{
				finger.Init(anim, this);
			}
			
		}
		void Update()
        {
            for(int i=0; i < fingers.Length; i++)
			{
				fingers[i].ProcessAnimation();
			}
        }
    }

}

