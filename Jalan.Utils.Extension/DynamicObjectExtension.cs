using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jalan.Utils.Extension
{
    public class DynamicObjectExtension : DynamicObject
    {
        public Dictionary<string, object> PropertiesExt = new Dictionary<string, object>();
        public DynamicObjectExtension()
        {
            //var t = this.GetDynamicMemberNames();
            //var props = this.GetType().GetProperties();
            //foreach (System.Reflection.PropertyInfo p in props)
            //{
            //    if (p.PropertyType.IsPublic)
            //        PropertiesExt.Add(p.Name, p.GetValue(this, null));
            //}
        }
        public DynamicObjectExtension(Dictionary<string, object> properties) : this()
        {
            foreach (var item in properties)
            {
                PropertiesExt.Add(item.Key, item.Value);
            }
        }
        public void AddPropertyItem(string propertyName, object propertyValue)
        {
            if (!PropertiesExt.ContainsKey(propertyName))
                PropertiesExt.Add(propertyName, propertyValue);
            else
                throw new Exception("存在重复的自定义属性：" + propertyName);
        }
        public void RemovePropertyItem(string propertyName)
        {
            if (PropertiesExt.ContainsKey(propertyName))
                PropertiesExt.Remove(propertyName);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return PropertiesExt.Keys;
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!PropertiesExt.Keys.Contains(binder.Name))
            {
                PropertiesExt.Add(binder.Name, value.ToString());
            }
            return true;
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return PropertiesExt.TryGetValue(binder.Name, out result);
        }
    }
}
