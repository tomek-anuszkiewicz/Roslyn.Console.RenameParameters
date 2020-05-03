using System;
using System.Text;

namespace Roslyn.Console.RenameParameters
{
    public static class ParameterRenamer
    {
        private const string _prefix = "a_";

        public static string Rename(string name)
        {
            if (name is null)
                throw new System.ArgumentNullException(nameof(name));

            static string RemovePrefix(string name)
            {
                if (name.StartsWith(_prefix))
                    return name.Remove(0, 2);
                else
                    return name;
            }

            static string StartWithLower(string name)
            {
                return name.Substring(0, 1).ToLower() + name.Substring(1);
            }

            static string UpperAfterUnderscore(string name)
            {
                StringBuilder sb = new StringBuilder(name);

                for (int i = 0; i < sb.Length - 1; i++)
                {
                    if (sb[i] == '_')
                        sb[i + 1] = char.ToUpper(sb[i + 1]);
                }

                return sb.ToString();
            }

            static string RemoveUnderscores(string name)
            {
                return name.Replace("_", "");
            }

            if (name == "_")
                return name;
            if (name.StartsWith("_"))
                throw new InvalidOperationException(name);
            if (name.EndsWith("_"))
                throw new InvalidOperationException(name);
            if (name.Contains("__"))
                throw new InvalidOperationException(name);

            name = RemovePrefix(name);
            name = StartWithLower(name);
            name = UpperAfterUnderscore(name);
            name = RemoveUnderscores(name);

            return name;
        }
    }
}
