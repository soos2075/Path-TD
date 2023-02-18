using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Base : MonoBehaviour
{
    //? 모든 타워가 가질 수 있는 속성과 타입 등등의 인터페이스를 만들어놓아야함
    //? 다만 기능적인 구현을 요구할 뿐이지, 실제 타워의 스탯은 여기서 정하는건 아님 (사거리, 공격속도, 타입 등등)
    //? 근데 여기서는 어차피 인터페이스만 제공하고 조립은 다른곳에서 하는데 기능적인 구현을 어떻게해야 가능할지가 문제

    public enum TowerCode
    {
        FireTower = 0,
        LazerTower,
        SplashTower,
        MultiTower,
        PoisonTower,
        SpiderTower,
        SlowTower,
        BuffTower,
        SpearTower,
        AirTower,

        ThunderTower = 10,
        ArcaneTower,
        WaterTower,
        LightningTower,
        CoinTower,

    }


    public enum Attackable
    {
        Ground,
        Air,
        Allrounder,
        Buff,
        OnlyAir,
        OnlySP,
        Non,
    }
    public enum Attack_Type //? 탄에서 구현해야 할 것도 있는데 일단은 다 해놓는게 나을듯
    {
        Normal ,
        Multiple, // 
        Splash,
        Piercing, //(관통)
        Lazer,
        Poison, //(독, 화상, 특수 이것저것 도트 전부 포함)
        Slow,
        Debuff, //(방깍, 피격 데미지 증가 등등. 둔화쪽 말고 스탯에 관한 부분만.
    }

    public enum Attack_Property
    {
        Normal,
        Physics,
        Magic,
    }

    public enum Bullet_Type //? 탄이름 ("Resources/Bullet/Bullet_name") 그리고 이거 테이블에서 int형으로 가져오는데 굳이 0~9안해도되고 훨씬늘려도됨
    {//? 대신 사용시 주의사항이 항상 숫자는 순차적으로 증가하기때문에 맨앞이 101이면 그다음은 자동으로 102가됨. 
        Bullet_Basic = 0,

        Bullet_Fire = 101,
        Bullet_Air = 102,

        

        Bullet_Splash = 201,

        Bullet_Lazer = 301,

        Bullet_Poison = 401,

        //Bullet_DOT,
        Bullet_Web = 501,

        Bullet_Slow = 601,

        Bullet_Spear = 701,

        Bullet_Buff = 901,

        Bullet_Thunder = 1001,

        Bullet_Arcane = 1101,
        Bullet_Lightning = 1102,

        Bullet_Water = 1201,

        Bullet_Non = 9999,
    }



    protected interface I_AddCoin
    {
        void AddTowerCoin(int value);
        int Coin { get; set; }
    }
    protected interface I_MultiShot
    {
        int Multi_Numbers { get; set; }
        //void MultiShot();
    }

    protected interface I_DOT //? 도트가 Damage Over Time의 약자라고함
    {
        float CycleTime { get; set; }
        int Count { get; set; }
        //void DOT(float cycleTime, int count);
    }

    //public interface I_Spin //? 타워 대가리 회전
    //{
    //    void Spin(float spin_Speed);
    //}

    protected interface I_SelfDisappear
    {
        void Disappear(Collider2D range);
    }

    public interface I_Buff
    {
        void Buff();
    }

    public interface I_Detail //? 테이블에서 받아온 정보에서 추가정보가 있는애들은 다 상속받아야함. 일단은 4개까지인데 추가해도상관무
    {
        void Detail_Data(float value1 = 0, float value2 = 0, float value3 = 0, float value4 = 0);

        void RestoreData();
    }
}
