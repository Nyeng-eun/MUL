using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSelector : MonoBehaviour
{
    public enum Anims
    {
        idle, // 0
        walk_front, walk_back, // 1, 2
        walk_front_left, walk_front_right, // 3, 4
        walk_back_left, walk_back_right, // 5, 6
        run, // 7
        jump, // 8
        pose1, pose2, // 9, 10
        cast1_1, cast1_2, cast2_1, cast2_2, // 11, 12, 13, 14
        attacked, // 15
        die// 16
    }

    public Anims anims;

    void Start()
    {
        GetComponent<Animator>().SetInteger("anim", (int) anims);
    }
}
