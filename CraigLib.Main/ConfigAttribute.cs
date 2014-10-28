using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraigLib
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigAttribute : Attribute
    {
        private readonly bool _visible = true;
        private readonly string _name;
        private readonly string _descr;
        private readonly string _default;

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Description
        {
            get
            {
                return _descr;
            }
        }

        public string Default
        {
            get
            {
                return _default;
            }
        }

        public bool Visible
        {
            get
            {
                return _visible;
            }
        }

        public ConfigAttribute(string name, string descr, string def)
        {
            _name = name;
            _descr = descr;
            _default = def;
        }

        public ConfigAttribute(string name, string descr, string def, bool visible)
        {
            _name = name;
            _descr = descr;
            _default = def;
            _visible = visible;
        }
    }
}
