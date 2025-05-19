using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using item.model;

namespace File.model;

public class File : Item
{
    private string type;

    public File(string name, int size, DateTime lastModified, string type)
        : base(name, size, lastModified)
    {
        this.type = type;
    }

    public override void Get()
    {
        base.Get();
    }

    public string GetFullPath(string directory)
    {
        return System.IO.Path.Combine(directory, name);
    }

}
