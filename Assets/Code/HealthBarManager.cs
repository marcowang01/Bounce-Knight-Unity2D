using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    public DungeonCharacter character;
    public Transform bar;
    public SpriteRenderer icon;
    public SpriteRenderer barRenderer;

    private float maxHP;
    private float hp;

    private void Start()
    {
        if (character)
        {
            icon.sprite = character.healthBarIcon;
            maxHP = character.maxHitPoints;
        }
        if (!character && FindObjectOfType<EnemyController>())
        {
            character = FindObjectOfType<EnemyController>().gameObject.GetComponent<DungeonCharacter>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!character)
        {
            if (FindObjectOfType<EnemyController>())
            {
                character = FindObjectOfType<EnemyController>().gameObject.GetComponent<DungeonCharacter>();
            }
        } else
        {
            icon.sprite = character.healthBarIcon;
            maxHP = character.maxHitPoints;
            hp = character.hitPoints;
            if (character && bar)
            {
                float ratio = hp / maxHP;
                Vector3 scaleVec = bar.localScale;
                scaleVec.x = ratio * 0.5f;
                bar.localScale = scaleVec;

                if (ratio > 0.5f)
                    barRenderer.color = Color.green;
                else if (ratio < 0.5f && ratio > 0.25f)
                    barRenderer.color = Color.yellow;
                else if (ratio < 0.25f)
                    barRenderer.color = Color.red;
            }
        }
    }
}
