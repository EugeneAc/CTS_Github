using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSyncService
{

    public class ConfigSection : ConfigurationSection
    {
      [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
      public ConfigInstanceCollection Instances
      {
        get { return (ConfigInstanceCollection)this[""]; }
        set { this[""] = value; }
      }
    }
    public class ConfigInstanceCollection : ConfigurationElementCollection
    {
      protected override ConfigurationElement CreateNewElement()
      {
        return new ConfigInstanceElement();
      }

      protected override object GetElementKey(ConfigurationElement element)
      {
        //set to whatever Element Property you want to use for a key
        return ((ConfigInstanceElement)element).Name;
      }
    }

    public class ConfigInstanceElement : ConfigurationElement
    {
      //Make sure to set IsKey=true for property exposed as the GetElementKey above
      [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
      public string Name
      {
        get { return (string)base["name"]; }
        set { base["name"] = value; }
      }

      [ConfigurationProperty("code", IsRequired = true)]
      public string Code
      {
        get { return (string)base["code"]; }
        set { base["code"] = value; }
      }
    }
  }
