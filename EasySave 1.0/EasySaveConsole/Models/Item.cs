using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace item.model;

public abstract class Item
{
    protected string name;
    protected int size;
    protected DateTime lastModified;

    public Item(string name, int size, DateTime lastModified)
    {
        this.name = name;
        this.size = size;
        this.lastModified = lastModified;
    }

    public virtual void Get()
    {
    }
}
