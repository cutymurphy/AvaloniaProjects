using System;

namespace AvaloniaApplication4.Models
{
    public class MethodParameter
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string Value { get; set; } = string.Empty;

        public MethodParameter(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
}