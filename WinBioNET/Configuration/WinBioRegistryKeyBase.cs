using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Win32;

namespace WinBioNET.Configuration
{
    public class WinBioRegistryKeyBase
    {
        private IEnumerable<FieldInfo> GetFields()
        {
            return GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        }

        public void Read(RegistryKey key)
        {
            foreach (var field in GetFields())
            {
                field.SetValue(this, key.GetValue(field.Name));
            }
        }

        public void Write(RegistryKey key)
        {
            foreach (var field in GetFields())
            {
                key.SetValue(field.Name, field.GetValue(this));
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var field in GetFields())
            {
                builder.AppendFormat("{0}: {1}\n", field.Name, field.GetValue(this));
            }
            return builder.ToString();
        }
    }
}