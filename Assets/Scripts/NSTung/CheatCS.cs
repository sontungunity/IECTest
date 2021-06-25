using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatCS : MonoBehaviour
{
    [SerializeField] private Dropdown dd_type_Item;
    [SerializeField] private Dropdown dd_detail_Item;
    [SerializeField] private InputField ip_amount_Item;
    [SerializeField] private Button btn_AddItem;
    [SerializeField] private Button btn_Close;

    private TypeItem[] lstEnumTypeItem =  System.Enum.GetValues(typeof(TypeItem)) as TypeItem[];
    List<Dropdown.OptionData> lstDataType = new List<Dropdown.OptionData>();
    List<Dropdown.OptionData> lstDataDeatil = new List<Dropdown.OptionData>();
    
    private void Awake() {
        btn_Close.onClick.AddListener(()=> { gameObject.SetActive(false); });
        btn_AddItem.onClick.AddListener(AddItem);
        dd_type_Item.onValueChanged.AddListener(LoadDetail);
    }

    private void Start() {
        //setup defaul Type 
        lstDataType.Clear();
        foreach(var type in lstEnumTypeItem) {
            Dropdown.OptionData optioData = new Dropdown.OptionData(type.ToString());
            lstDataType.Add(optioData);
        }
        dd_type_Item.ClearOptions();
        dd_type_Item.AddOptions(lstDataType);

        //setup defaul Detail
        lstDataDeatil.Clear();
        CoinAndPremium[] lstEnum = System.Enum.GetValues(typeof(CoinAndPremium)) as CoinAndPremium[];
        foreach(var detail in lstEnum) {
            Dropdown.OptionData optioData = new Dropdown.OptionData(detail.ToString());
            lstDataDeatil.Add(optioData);
        }
        dd_detail_Item.ClearOptions();
        dd_detail_Item.AddOptions(lstDataDeatil);
    }

    private void LoadDetail(int index) {
        lstDataDeatil.Clear();
        TypeItem valueItem =(TypeItem)dd_type_Item.value;
        switch(valueItem) {
            case TypeItem.COIN_PREMIUM:
                CoinAndPremium[] lstEnum = System.Enum.GetValues(typeof(CoinAndPremium)) as CoinAndPremium[];
                foreach(var detail in lstEnum) {
                    Dropdown.OptionData optioData = new Dropdown.OptionData(detail.ToString());
                    lstDataDeatil.Add(optioData);
                }
                break;
            case TypeItem.ITEMS:
                foreach(var itemConsum in ShopItemList.s_ConsumablesTypes) {
                    Consumable c = ConsumableDatabase.GetConsumbale(itemConsum);
                    if(c!=null) {
                        Dropdown.OptionData optioData = new Dropdown.OptionData(itemConsum.ToString(),c.icon);
                        lstDataDeatil.Add(optioData);
                    }
                }
                break;
        }
        RenderDetail();
    }


    private void RenderDetail() {
        dd_detail_Item.ClearOptions();
        dd_detail_Item.AddOptions(lstDataDeatil);
    }

    private void AddItem() {
        int indexType = dd_type_Item.value;
        int indexDeatil = dd_detail_Item.value;
        int amount = int.Parse(ip_amount_Item.text);
        if(indexType == 0) {
            if(dd_detail_Item.value == 0) {
                PlayerData.instance.coins += amount;
            } else {
                PlayerData.instance.premium += amount;
            }
            PlayerData.instance.Save();
        }else if(indexType == 1) {
            Consumable c = ConsumableDatabase.GetConsumbale((Consumable.ConsumableType)(indexDeatil+1));
            PlayerData.instance.Add(c.GetConsumableType(), amount);
            PlayerData.instance.Save();
        }
    }
}

[System.Serializable]
public enum TypeItem {
    COIN_PREMIUM,
    ITEMS,
    CHARACTERS,
    ACCESSORIES,
    THEMES
}

[System.Serializable]
public enum CoinAndPremium {
     COIN,
     PREMIUM,
}
