using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BossLife : MonoBehaviour
{
    private Slider slider;
    private BEnemy boss;
    private PlayerControl player;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        boss = GameObject.Find("Boss").GetComponent<BEnemy>();
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
    }

    private void Start()
    {
        slider.maxValue = boss.health;
    }

    private void Update()
    {
        slider.value = boss.health;
    }
}

