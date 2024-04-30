using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
//����ű���ֵ��Player�µ�Inventory���������ǹ���CharacterUI�ڵ�Inventory��Slots����ʹ������ͬ�ķ���������CharacterUI�ڵ����ݵĸ���
{
    //ȷ�����еط����ܵ��ôˣ�Ψһ�ģ���Ʒ���ű���ֱ��ʹ��instance����
    //��ͬ��PlayerManager��ͨ��instance.player�����ã���Ϊ������instance��������PlayerManager����Player���Ͷ���
    public static Inventory instance;

    [Header("Character UI")]
    //������Ʒ����λ��
    [SerializeField] private Transform inventorySlotParent;
    //������Ʒ��UI���б�
    private UI_ItemSlot[] itemSlotsUIList;
    
    //��¼Stat��������
    [SerializeField] private Transform statSlotParent;
    //��¼��slot���б�
    private UI_StatSlot[] statSlotsUIList;

    [Header("Inventory Items")]
    //��¼����Ʒ����Ʒ����һ������Stat���Զ����������ͣ�������ֱ��ʹ�ò��ù�����ItemData����Ϣ���б�
    public List<InventoryStoragedItem> inventoryItemsList;
    //ʹ���ֵ����洢ItemData��InventoryItemһһ��Ӧ�Ĺ�ϵ
    public Dictionary<ItemData,InventoryStoragedItem> inventoryItemsDictionary;

    private void Awake()
    {
        //������Ʒ���Լ�ȷ��ֻ��һ���˽ű���instance
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        //��ʼ����Ʒ����Ʒ�б��Լ���Ʒ���ֵ�
        inventoryItemsList = new List<InventoryStoragedItem>();
        inventoryItemsDictionary = new Dictionary<ItemData,InventoryStoragedItem>();
    
        //ע��������Components����s����Ϊ��ֵ���б�����¼�������
        itemSlotsUIList = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        statSlotsUIList = statSlotParent.GetComponentsInChildren<UI_StatSlot>();
    }

    #region UpdateSlotUIs
    private void UpdateItemSlotUI()
    //�˺���������Ʒ���ĸ��ӣ�����Ʒ�仯��Add��Remove������ʱ������һ��
    {
        //����Ʒ���б��ڵ���Ʒ���б���
        for (int i = 0; i < inventoryItemsList.Count; i++)
        {
            //����Ʒ���б��ڵ���Ʒһһ������Ʒ��UI�б�
            itemSlotsUIList[i].UpdateItemSlotUI(inventoryItemsList[i]);
        }
    }
    private void UpdateStatSlotUI()
    //�˺�������CharacterUI�ڵ�������ֵ���Ӹ��£�����ֵ�仯��ʱ�򱻵��ø���
    {
        for (int i = 0; i < statSlotsUIList.Length; i++)
        {
            statSlotsUIList[i].UpdateStatValueSlotUI();
        }
    }
    #endregion

    #region ChangeInventoryItems
    public bool CanAddNewItem()
    //�����Ʒ���Ƿ��������ռ�
    {
        if (inventoryItemsList.Count >= itemSlotsUIList.Length)
        {
            Debug.Log("Inventory No More Space");
            return false;
        }
        else
        {
            //Debug.Log("Inventory Has Space");
            return true;
        }
    }
    public void AddItem(ItemData _newItemData)
    {
        //��ԭ������Ʒ���ֵ����������Ʒ�ˣ���ô���ڴ˻���������һ���ѵ��������ɣ�ע���Ƿ��жѵ����ޣ�
        if(inventoryItemsDictionary.TryGetValue(_newItemData, out InventoryStoragedItem _value))
        {
            //һ�μ���һ��
            _value.AddStackSizeBy(1);
        }
        //��ԭ��û�������Ʒ�������б�����������Ʒ��ǰ���Ǳ���û��
        else
        {
            if(CanAddNewItem())
            {
                //C#�д����Զ������¶���ķ�ʽ
                InventoryStoragedItem _newInventoryStoragedItem = new InventoryStoragedItem(_newItemData);

                //��Ʒ���´���һ����Ʒ������װ���������Ʒ���ѵ����ڹ��캯���б�Ĭ�ϳ�ʼ��Ϊ1
                inventoryItemsList.Add(_newInventoryStoragedItem);

                //�����µ�ӳ���ϵ���Ա��´α���⵽��ֱ�ӽ��ѵ�����������
                inventoryItemsDictionary.Add(_newItemData, _newInventoryStoragedItem);
            }
        }

        //ˢ����Ʒ��UI
        UpdateItemSlotUI();
    }
    public void RemoveItemTotally(ItemData _itemData)
    {
        //�����������Ҫ��ɾ������Ʒ����ɾ�������Ʒ��ȫ�����������
        if(inventoryItemsDictionary.TryGetValue(_itemData, out InventoryStoragedItem _InvItem))
        {
            //���˸��Ӵ���ƷΪ0����1�������������Ʒ����
            if(_InvItem.stackSize <= 1)
            {
                //�Ƴ���Ʒ�������б��е����ItemData
                inventoryItemsList.Remove(_InvItem);
                //�Ƴ��ֵ��е������Ϊkey��ItemData
                inventoryItemsDictionary.Remove(_itemData);
            }
            else
            {
                //��֮����������ѵ�������
                _InvItem.DecreaseStackSizeBy(1);
            }
        }

        //ˢ����Ʒ��UI
        UpdateItemSlotUI();
    }
    #endregion

}