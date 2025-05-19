using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using item.model; 

namespace Folder.model;

public class Folder : Item
{
    private int NbofItems;
    public List<Item> ListItems { get; set; }

    public Folder(string name, int size, DateTime lastModified)
        : base(name, size, lastModified)
    {
        ListItems = new List<Item>();
        NbofItems = 0;
    }

    public void OrderItemsByName()
    {
        ListItems = ListItems.OrderBy(item => item.ToString()).ToList();
    }

    public override void Get()
    {
        base.Get();
    }

    public void AddItem(Item item)
    {
        ListItems.Add(item);
        NbofItems++;
    }

    public void RemoveItem(Item item)
    {
        ListItems.Remove(item);
        NbofItems--;
    }

    public void ClearItems()
    {
        ListItems.Clear();
        NbofItems = 0;
    }

    public int GetNbofItems()
    {
        return NbofItems;
    }
}
