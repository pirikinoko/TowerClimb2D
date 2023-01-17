using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    GameObject playerGO, spearGO;
    Rigidbody2D spearrbody2D;
    float attackDuration, defaultAD = 0.5f;
    float angle; //�}�E�X�|�C���^�[�̊p�x
    float spearRot = 0f; //���̊p�x
    public static bool clicked, attacked;
    Vector3 playerPos; Vector2 spearGoal, pos, spearColVector;


    void Start()
    {
        playerGO = GameObject.Find("Player");
        spearGO = this.gameObject;
        attacked = false;
        attackDuration = defaultAD;
        spearrbody2D = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {

        playerPos = playerGO.transform.position;
        if (GameSystem.playable)
        {
            //���p�x�擾
            angle = GetAngle(playerGO.transform.position, MousePointer.pointer);
            // �p�x���f         
            spearrbody2D.MoveRotation(spearRot);

            SpearAttack();
        }

        if (attacked == false)
        {
            this.transform.position = playerPos;
        }
    }

    void SpearAttack()
    {
        //�N���b�N�������ǂ���
        if (Input.GetMouseButtonDown(0))
        {
            clicked = true;
        }
        else
        {
            clicked = false;
        }

        if (clicked && attackDuration == defaultAD)�@//�N���b�N���ꂽ�Ƃ��U���������ł��Ă���Ȃ�
        {
            spearColVector = AngleToVector2(angle);
            attacked = true;
        }

        if (attacked)              //�U�����̑��̓���
        {
            CalcSpearCollision();
            attackDuration -= Time.deltaTime;
        }

        else if (attacked == false)�@�@�@�@�@�@�@�@�@�@//��U�����̑��̊p�x
        {
            if (Player.characterDirection.x == 0)
            {

                spearRot = angle + 270;
            }
            else
            {
                spearRot = angle + 270;
            }
        }

        if (attackDuration < 0)�@�@�@�@�@�@�@//�U�����I�������̏����@
        {
            attackDuration = defaultAD;
            attacked = false;
        }
    }
    void SpearRotation()
    {
        spearrbody2D.MoveRotation(spearRot);  // �p�x���f 
    }

    float GetAngle(Vector2 start, Vector2 target)
    {
        Vector2 dt = target - start;
        float rad = Mathf.Atan2(dt.y, dt.x);
        float degree = rad * Mathf.Rad2Deg;

        return degree;
    }
    public static Vector2 AngleToVector2(float angle)
    {
        var radian = angle * (Mathf.PI / 180);
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;
    }

    void CalcSpearCollision()//���̓����蔻��̈ʒu���v�Z
    {
        Vector2 spearPos = this.transform.position;
        Vector2 playerPos = playerGO.transform.position;
        spearGoal.x = playerPos.x + spearColVector.x * 0.35f;
        spearGoal.y = playerPos.y + spearColVector.y * 0.35f;
        if (spearPos.x < spearGoal.x)
        {
            spearPos.x += 3.5f * Time.deltaTime;
        }
        if (spearPos.x > spearGoal.x)
        {
            spearPos.x -= 3.5f * Time.deltaTime; ;
        }
        if (spearPos.y < spearGoal.y)
        {
            spearPos.y += 3.5f * Time.deltaTime; ;
        }
        if (spearPos.y > spearGoal.y)
        {
            spearPos.y -= 3.5f * Time.deltaTime;
        }
        this.transform.position = spearPos;
    }


}
