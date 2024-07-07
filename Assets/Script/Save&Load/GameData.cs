using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    #region Currency
    //��ҳ��еĻ���
    public int currency;
    #endregion

    #region Ability
    public bool canWallSlide;
    public bool canDash;
    public bool canDoubleJump;
    public bool canThrowSword;
    public bool canFireBall;
    public bool canIceBall;
    public bool canBlackhole;
    #endregion

    #region Stats
    //ע�⣬��EntityStat����Щ���Ե�����������"Stat"�����浵�ı����޷�ʹ�ã������û�int��ע������û���õ�Stat���õ�modifiers������²ſ����
    //ʵ�ʵ�����ֵ���ӳɺ󣩻��ڱ���UI������ʾ�������¼�Ķ���δ���ӳ�ǰ��ԭʼֵ��Ҳ����Ҫ�����ֵ

    //��ɫ����ֵ
    public int strength;
    public int agility;
    public int vitality;
    public int intelligence;

    //ԭʼ�������ֵ��δ���ӳɣ�
    public int originalMaxHealth;

    //����������ԣ�δ���ӳɣ�
    public int criticPower;
    public int criticChance;

    //�����ͷ�����������δ���ӳɣ�
    public int primaryPhysicalDamage;
    public int fireAttackDamage;
    public int iceAttackDamage;
    public int lightningAttackDamage;

    //���ܹ�������δ���ӳɣ�
    public int swordDamage;
    public int fireballDamage;
    public int iceballDamage;

    //�����������δ���ӳɣ�
    public int evasionChance;
    public int physicalArmor;
    public int magicalResistance;
    #endregion

    #region Inventory
    //��ҵ���Ʒ����ʹ���Զ���Ŀ����л��ֵ�
    public SerializableDictionary<string, int> inventory;
    #endregion

    #region CheckPoints
    //���漤��Ĵ浵����ֵ�
    public SerializableDictionary<string, bool> checkpointsDict;
    //��������ϴ���Ϣ�Ĵ浵��id
    public string lastRestCPID;
    //��������������Ĵ浵��id
    //public string closestCheckPointID;
    #endregion

    #region Settings
    //�洢���֣�bgm��cd������Ч��sfx��������С���õ��ֵ�
    public SerializableDictionary<string, float> volumeSettings; 
    #endregion

    public GameData()
    //���캯��
    {
        #region Currency
        //������Ϸ�浵ʱ��Ĭ�ϻ���Ϊ��0
        this.currency = 0;
        #endregion

        #region Ability
        //Ĭ�ϳ�ʼ��������
        this.canWallSlide = false;
        this.canDash = false;
        this.canDoubleJump = true;
        this.canThrowSword = false;
        this.canFireBall = false;
        this.canIceBall = false;
        this.canBlackhole = false;
        #endregion

        #region Stats
        //�趨��Щ���Եĳ�ʼĬ��ֵ
        this.strength = 0;
        this.agility = 0;
        this.vitality = 0;
        this.intelligence = 0;

        this.originalMaxHealth = 200;

        this.criticPower = 150;
        this.criticChance = 10;

        this.primaryPhysicalDamage = 20;
        this.fireAttackDamage = 0;
        this.iceAttackDamage = 0;
        this.lightningAttackDamage = 5;

        this.swordDamage = 5;
        this.fireballDamage = 25;
        this.iceballDamage = 25;

        this.evasionChance = 5;
        this.physicalArmor = 10;
        this.magicalResistance = 10;
        #endregion

        #region Inventory
        this.inventory = new SerializableDictionary<string, int>();
        #endregion

        #region CheckPoints
        this.checkpointsDict = new SerializableDictionary<string, bool>();
        this.lastRestCPID = string.Empty;
        //this.closestCheckPointID = string.Empty;
        #endregion

        #region Settings
        this.volumeSettings = new SerializableDictionary<string, float>();
        #endregion
    }
}