using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerMoveTest : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private string indexParam = "index";
    private string middleParam = "middle";
    private string ringParam = "ring";
    private string pinkyParam = "pinky";
    private string thumbParam = "thumb";
    // Start is called before the first frame update
    private float index = 0;
    private float middle = 0;
    private float ring = 0;
    private float pinky = 0;
    private float thumb = 0;

    private float decreaseSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1)) thumb = 1f;
        if (Input.GetKey(KeyCode.Alpha2)) index = 1f;
        if (Input.GetKey(KeyCode.Alpha3)) middle = 1f;
        if (Input.GetKey(KeyCode.Alpha4)) ring = 1f;
        if (Input.GetKey(KeyCode.Alpha5)) pinky = 1f;

        animator.SetFloat(indexParam, index);
        animator.SetFloat(middleParam, middle);
        animator.SetFloat(ringParam, ring);
        animator.SetFloat(pinkyParam, pinky);
        animator.SetFloat(thumbParam, thumb);

        DecreaseValues();

    }

	private void DecreaseValues()
	{
		if(index > 0)
		{
            index -= Time.deltaTime * decreaseSpeed;
            index = Mathf.Clamp01(index);
		}

        if(middle > 0)
		{
            middle -= Time.deltaTime * decreaseSpeed;
            middle = Mathf.Clamp01(middle);
		}

        if (ring > 0)
        {
            ring -= Time.deltaTime * decreaseSpeed;
            ring = Mathf.Clamp01(ring);
        }

        if (pinky > 0)
        {
            pinky -= Time.deltaTime * decreaseSpeed;
            pinky = Mathf.Clamp01(pinky);
        }

        if (thumb > 0)
        {
            thumb -= Time.deltaTime * decreaseSpeed;
            thumb = Mathf.Clamp01(thumb);
        }

    }
}
