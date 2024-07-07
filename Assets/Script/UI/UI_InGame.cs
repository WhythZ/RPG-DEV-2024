using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_InGame : MonoBehaviour
{
    private PlayerStats pStats;
    private UnityEngine.UI.Slider healthBarSlider;

    [Header("Skill UI")]
    //�ֶ���Hierarchy�ڸ�ֵ�ɣ�Start��ȡ��������Ƚ��ѣ���ΪImage���͵�̫����
    [SerializeField] private UnityEngine.UI.Image dashCooldownImage;
    [SerializeField] private UnityEngine.UI.Image swordCooldownImage;
    [SerializeField] private UnityEngine.UI.Image fireballCooldownImage;
    [SerializeField] private UnityEngine.UI.Image iceballCooldownImage;
    [SerializeField] private UnityEngine.UI.Image blackholeCooldownImage;

    private void Start()
    {
        pStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        healthBarSlider = GetComponentInChildren<UnityEngine.UI.Slider>();

        //Ѫ�������¼��ĵ���
        if (pStats != null)
            pStats.onHealthChanged += UpdateHealthUI;

        //�����Start��������Ҫȷ���ȳ�ʼ��ʵ��Ѫ����Start��������ã�����UI����ʵ��Ѫ��������
        //�����������˳�򣬿���Project Settings��Scripts Execution Order���޸�
        //Debug.Log("UI_InGame Start() Func Called");
        //ʵ�����ɵ�ʱ�򣬵���һ��Ѫ��UI�ĸ���
        UpdateHealthUI();
    }

    private void Update()
    //������Ϸ��UI��Ҫʵʱ���һЩ��Ҫ���ܣ����UI�л���������ȴ��ʾUI�ȣ����Զ���Щ����ʹ��Update����
    {
        //����Ȩ�޼��
        SkillAccessCheck();

        //��ȴ����ˢ��
        UpdateSwordCooldown();
        UpdateDashCooldown();
        UpdateFireBallCooldown();
        UpdateIceBallCooldown();
        UpdateBlackholeCooldown();
    }

    #region HealthUI
    private void UpdateHealthUI()
    //��onHealthChanged�¼����ӵ��ã����ϸ���ʵ��ĵ�ǰѪ�����Ա����뻬������
    {
        //��������ֵ����ʵ���������Ѫ��
        healthBarSlider.maxValue = pStats.GetFinalMaxHealth();
        //����ĵ�ǰֵ����ʵ��ĵ�ǰѪ��
        healthBarSlider.value = pStats.currentHealth;
    }
    #endregion

    #region SkillCooldown
    private void UpdateSwordCooldown()
    {
        //�����ٻ�����ʱʱʹ�ü��ܽ�����ȴ
        if (PlayerSkillManager.instance.assignedSword)
            ResetSkillCooldownUIFor(swordCooldownImage);

        //������ȥ��һֱ������ȴ״̬�������ջؽ�
        if (!PlayerSkillManager.instance.assignedSword)
            swordCooldownImage.fillAmount = 0;
    }
    private void UpdateDashCooldown()
    {
        //��������
        if (PlayerManager.instance.ability_CanDash == false)
            return;

        //dash������ȴ���ĸ���
        UpdateSkillCooldownUIOf(dashCooldownImage, PlayerSkillManager.instance.dashSkill.cooldown);

        //��������shift������ܽ��г�̣�����ҽ����˳�̣�ʱ��Ԥʾ����ҳ�̼��ܽ�����ȴ���ʶ����¼���ͼ�������ȴ
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //��ֹ�ڰ���shift��û�н��г�̵�����£��繥��ʱ��shift���޷���̵ģ�ˢ����ȴ��UI
            if(PlayerManager.instance.player.stateMachine.currentState == PlayerManager.instance.player.dashState)
                ResetSkillCooldownUIFor(dashCooldownImage);
            else
            {
                //�������ֵ���Ч������ʾ���ܴ�����ȴ
                PlayerManager.instance.player.fx.CreatPopUpText("Cooldown", Color.white);
            }
        }
    }
    private void UpdateFireBallCooldown()
    {
        //��������
        if (PlayerManager.instance.ability_CanFireBall == false)
            return;

        //������ȴ���ĸ���
        UpdateSkillCooldownUIOf(fireballCooldownImage, PlayerSkillManager.instance.fireballSkill.cooldown);
        
        //��ֹ�����ڲ����ͷż��ܵ�ʱ���¼��ܰ���������ȴͼ��ˢ��
        if (PlayerManager.instance.player.stateMachine.currentState == PlayerManager.instance.player.idleState || PlayerManager.instance.player.stateMachine.currentState == PlayerManager.instance.player.moveState)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                ResetSkillCooldownUIFor(fireballCooldownImage);
        }
    }
    private void UpdateIceBallCooldown()
    {
        //��������
        if (PlayerManager.instance.ability_CanIceBall == false)
            return;

        //������ȴ���ĸ���
        UpdateSkillCooldownUIOf(iceballCooldownImage, PlayerSkillManager.instance.iceballSkill.cooldown);

        //��ֹ�����ڲ����ͷż��ܵ�ʱ���¼��ܰ���������ȴͼ��ˢ��
        if (PlayerManager.instance.player.stateMachine.currentState == PlayerManager.instance.player.idleState || PlayerManager.instance.player.stateMachine.currentState == PlayerManager.instance.player.moveState)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
                ResetSkillCooldownUIFor(iceballCooldownImage);
        }
    }
    private void UpdateBlackholeCooldown()
    {
        //��������
        if (PlayerManager.instance.ability_CanBlackhole == false)
            return;

        //������ȴ���ĸ���
        UpdateSkillCooldownUIOf(blackholeCooldownImage, PlayerSkillManager.instance.blackholeSkill.cooldown);

        //������ڽ����ڶ����ܵ�ʱ�򣨽�������ʱ�Ǵ���blackholeState�ģ�ˢ����ȴ
        if (PlayerManager.instance.player.stateMachine.currentState == PlayerManager.instance.player.blackholeState)
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
                ResetSkillCooldownUIFor(blackholeCooldownImage);
        }
    }
    #endregion

    #region UpdateOfSkillUI
    private void SkillAccessCheck()
    //Ȩ�޵ļ�⣬����û�н�����Ӧ�������򲻻���ʾ�ڼ�����
    {
        //���
        if (PlayerManager.instance.ability_CanDash == false)
            dashCooldownImage.transform.parent.gameObject.SetActive(false);
        else
            dashCooldownImage.transform.parent.gameObject.SetActive(true);

        //Ͷ����
        if (PlayerManager.instance.ability_CanThrowSword == false)
            swordCooldownImage.transform.parent.gameObject.SetActive(false);
        else
            swordCooldownImage.transform.parent.gameObject.SetActive(true);

        //����
        if (PlayerManager.instance.ability_CanFireBall == false)
            fireballCooldownImage.transform.parent.gameObject.SetActive(false);
        else
            fireballCooldownImage.transform.parent.gameObject.SetActive(true);

        //����
        if (PlayerManager.instance.ability_CanIceBall == false)
            iceballCooldownImage.transform.parent.gameObject.SetActive(false);
        else
            iceballCooldownImage.transform.parent.gameObject.SetActive(true);

        //�ڶ�
        if (PlayerManager.instance.ability_CanBlackhole == false)
            blackholeCooldownImage.transform .parent.gameObject.SetActive(false);
        else
            blackholeCooldownImage.transform.parent .gameObject.SetActive(true);
    }
    private void UpdateSkillCooldownUIOf(UnityEngine.UI.Image _image, float _cooldown)
    //��һ�����ܴ�����ȴʱ��������ȴ�����еݼ���ֱ����ȴ����
    {
        if(_image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }
    private void ResetSkillCooldownUIFor(UnityEngine.UI.Image _image)
    //ʹ����Ϸ��UI�еļ���ͼ����н�����ȴ������
    {
        //�����ô˺���ʱ������ͼ�������ȴ״̬
        //�����Լ������ʹ�ü�ʹ����ȴδ����ǰ�ٴΰ��˴�����ȴ�ļ���Ҳ�������´�ͷ��ʼ��ʾ��ȴʱ�䣬����Update�����ڵ��ж��������Բ���д����ôȫ
        if(_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }
    #endregion
}