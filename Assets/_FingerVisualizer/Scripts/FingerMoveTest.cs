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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float index =0f, middle =0f, ring=0f, pinky=0f, thumb =0f;

        if (Input.GetKey(KeyCode.Alpha1)) index = 1f;
        if (Input.GetKey(KeyCode.Alpha2)) middle = 1f;
        if (Input.GetKey(KeyCode.Alpha3)) ring = 1f;
        if (Input.GetKey(KeyCode.Alpha4)) pinky = 1f;
        if (Input.GetKey(KeyCode.Alpha5)) thumb = 1f;

        animator.SetFloat(indexParam, index);
        animator.SetFloat(middleParam, middle);
        animator.SetFloat(ringParam, ring);
        animator.SetFloat(pinkyParam, pinky);
        animator.SetFloat(thumbParam, thumb);

    }
}
