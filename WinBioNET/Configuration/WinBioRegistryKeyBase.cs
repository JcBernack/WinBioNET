using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                var value = key.GetValue(field.Name);
                if (field.FieldType == typeof (Guid))
                {
                    var converter = TypeDescriptor.GetConverter(field.FieldType);
                    value = converter.ConvertFrom(value);
                }
                field.SetValue(this, value);
            }
        }

        public void Write(RegistryKey key)
        {
            foreach (var field in GetFields())
            {
                var value = field.GetValue(this);
                if (field.FieldType.IsEnum)
                {
                    value = Convert.ChangeType(value, field.FieldType.GetEnumUnderlyingType());
                }
                if (field.FieldType == typeof (Guid))
                {
                    value = ((Guid) value).ToString("D").ToUpperInvariant();
                }
                key.SetValue(field.Name, value);
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