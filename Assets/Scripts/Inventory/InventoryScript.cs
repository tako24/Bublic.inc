using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] public Sprite[] itemBase;//Тестировочное полее, впоследствии изчезнет
    private InventoryItem[,] matrix;
    [SerializeField] public List<GameObject> IconBases;
    private InventoryItem weaponSlot;
    [SerializeField] public List<GameObject> savedIconBases;
    private InventoryItem[] moduleSlots;
    private InventoryItem movingItem;

    private Dictionary<string, int> modulesCounts;

    void Start()
    {
        moduleSlots = new InventoryItem[3];
        matrix = new InventoryItem[4, 4];
    }

    public bool isMoving()
    {
        return movingItem != null;
    }

    public void Use(int number)
    {
        var item = matrix[0, number];
        if (item != null)
        {
            if (item.isWeapon)
            {
                if (weaponSlot == null)
                {
                    weaponSlot = item;
                    matrix[0, number] = null;
                    UpdateInv();
                }
                else
                {
                    var buffer = weaponSlot;
                    weaponSlot = item;
                    matrix[0, number] = buffer;
                    UpdateInv();
                }
            }
            if (item.isHeal)
            {
                if(item.mainObject!=null)
                {
                    var heal = item.mainObject.GetComponent<Heal>();
                    if (heal != null)
                    {
                        heal.HealPlayer();
                        matrix[0, number] = null;
                    }
                }
            }
        }
    }

    public void CollectItem(GameObject gameObject)
    {
        if (gameObject != null)
        {
            var item = gameObject.GetComponent<ItemScript>();
            if (item != null)
            {
                if (item.InventoryItem != null)
                {
                    AddItem(item.InventoryItem);
                }
            }
            item.transform.SetParent(GameController.Player.transform);
            item.transform.position.Set(0, 0, 4);
            item.gameObject.SetActive(false);
        }
    }

    public void StartMoveItem(InventoryItem Item)
    {
        movingItem = Item;
    }

    public void EndMoveItem()
    {
        movingItem = null;
        UpdateInv();
    }

    public void MoveToWeaponSlot()
    {
        RemoveMoved();
        weaponSlot = movingItem;
        movingItem.mainObject.SetActive(true);
        GameController.Player.GetComponent<PlayerController>().Weapon = movingItem.mainObject;
    }

    public void MoveToModuleSlot(int i)
    {
        RemoveMoved();
        moduleSlots[i] = movingItem;
        movingItem.mainObject.GetComponent<ModuleScript>().Activate(true);
    }

    public void MoveToInv(int i,int j)
    {
        RemoveMoved();
        matrix[i, j] = movingItem;
        UpdateInv();
    }

    private void RemoveMoved()
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == movingItem)
                {
                    matrix[i, j] = null;
                    return;
                }
            }
        }

        if (weaponSlot == movingItem)
        {
            weaponSlot = null;
            movingItem.mainObject.SetActive(false);
        }

        for (var i = 0; i < moduleSlots.Length; i++)
        {
            if (moduleSlots[i] == movingItem)
            {
                moduleSlots[i] = null;
                movingItem.mainObject.GetComponent<ModuleScript>().Activate(false);
            }
        }
    }

    public bool AddItem(InventoryItem inventoryItem)
    {
        var isFound = false;

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            if (isFound)
                break;
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == null)
                {
                    isFound = true;
                    matrix[i, j] = inventoryItem;
                    break;
                }
            }
        }

        UpdateInv();
        return isFound;
    }

    public void UpdateInv()
    {
        IconBases = savedIconBases.ToList();
        IconBases.ForEach(x => x.SetActive(false));

        var modulesCountsTemp = new Dictionary<string, int>(); 

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] != null)
                {
                    var item = matrix[i, j];

                    if (item.isModule)
                        if (!modulesCountsTemp.ContainsKey(item.mainObject.name))
                            modulesCountsTemp.Add(item.mainObject.name, 1);
                        else
                            modulesCountsTemp[item.mainObject.name]++;

                    var slotPosition = GameObject.Find($"{i} {j}").GetComponent<RectTransform>();
                    var icon = IconBases.Last();
                    IconBases.Remove(icon);
                    icon.SetActive(true);
                    icon.GetComponent<RectTransform>().anchoredPosition = slotPosition.anchoredPosition;
                    icon.GetComponent<Image>().sprite = item.icon;
                    icon.GetComponent<InventoryItemScript>().inventoryItem = item;
                }
            }
        }

        modulesCounts = modulesCountsTemp;

        foreach (var module in modulesCounts.Keys)
        {
            if (modulesCounts[module] == 3)
            {
                GameObject upgradedModule = null;

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        var item = matrix[i, j];

                        if (item != null && item.isModule && item.mainObject.name == module)
                        {
                            matrix[i, j] = null;

                            if (!upgradedModule)
                                upgradedModule = item.mainObject;
                        }
                    }
                }

                var triplet = upgradedModule.GetComponent<ModuleScript>().TripletModule;
                if (triplet)
                {
                    triplet = Instantiate(triplet);
                    triplet.GetComponent<ModuleScript>().CollectModule();
                }
            }
        }

        if (weaponSlot != null)
        {
            var slotPosition = GameObject.Find($"WeaponSlot").GetComponent<RectTransform>();
            var icon = IconBases.Last();
            IconBases.Remove(icon);
            icon.SetActive(true);
            icon.GetComponent<RectTransform>().anchoredPosition = slotPosition.anchoredPosition;
            icon.GetComponent<Image>().sprite = weaponSlot.icon;
            icon.GetComponent<InventoryItemScript>().inventoryItem = weaponSlot;
        }

        for (var i = 0; i < moduleSlots.Length; i++)
        {
            if (moduleSlots[i] != null)
            {
                var slotPosition = GameObject.Find($"{i}").GetComponent<RectTransform>();
                var icon = IconBases.Last();
                IconBases.Remove(icon);
                icon.SetActive(true);
                icon.GetComponent<RectTransform>().anchoredPosition = slotPosition.anchoredPosition;
                icon.GetComponent<Image>().sprite = moduleSlots[i].icon;
                icon.GetComponent<InventoryItemScript>().inventoryItem = moduleSlots[i];
            }
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    public int id;
    public GameObject mainObject;
    public Sprite icon;
    public bool isModule=false;
    public bool isWeapon = false;
    public bool isHeal = false;
    public InventoryItem(GameObject mainObject,int id,Sprite icon,bool isModule,bool isWeapon,bool isHeal)
    {
        this.mainObject = mainObject;
        this.id = id;
        this.icon = icon;
        this.isModule = isModule;
        this.isWeapon = isWeapon;
        this.isHeal = isHeal;
    }
}

