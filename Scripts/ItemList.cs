using System.Collections.Generic;
using System.Linq;
using VladB.Utility;

namespace TamrielTradeApp
{
    public class ItemList
    {
        public List<ItemInfo> Items = new();

        public void AddItems(List<ItemInfo> newItems)
        {
            newItems.Act(newItem =>
            {
                ItemInfo existedCopy = Items.FirstOrDefault(item => item.IsEqual(newItem));
                if (existedCopy != null)
                {
                    existedCopy.Time = newItem.Time;
                    if (existedCopy.TimeMinutes > newItem.TimeMinutes)
                    {
                        //Debug.WriteLine($"Item Time Update From {existedCopy.time} To {newItem.time}");
                        existedCopy.IsHidden = false;
                    }

                    //Debug.WriteLine("Item Was Not Added, Because Already Exist In The List");
                    return;
                }

                Items.Add(newItem);
                //Debug.WriteLine("Item Was Added");
            });
        }

        public void SortList()
        {
            Items = Items.OrderBy(it => it.TimeMinutes).ToList();
        }

        public ItemInfo GetItemWithGUID(string guid)
        {
            return Items.FirstOrDefault(it => it.GUID == guid);
        }

        public void SetAllIsHidden(bool isHidden)
        {
            Items.Act(it => it.IsHidden = isHidden);
        }
    }
}