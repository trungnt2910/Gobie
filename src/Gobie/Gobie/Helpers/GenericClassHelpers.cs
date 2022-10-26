namespace Gobie.Helpers;

public static class GenericClassHelpers
{
    public static string AppendAttributeToClassName(string identifier)
    {
        var identifierAndGenericParts = identifier.Split('<');

        identifier = identifierAndGenericParts[0];
        if (!identifier.EndsWith("Attribute"))
        {
            identifier += "Attribute";
        }
        if (identifierAndGenericParts.Length > 1)
        {
            identifier += "<" + string.Join("<", identifierAndGenericParts.Skip(1));
        }

        return identifier;
    }

    public static bool IsSameClass(string classA, string classB)
    {
        if (classA == classB)
        {
            return true;
        }

        var indexOfTypeParams = classA.IndexOf('<');
        if (indexOfTypeParams == -1 || indexOfTypeParams >= classB.Length)
        {
            return false;
        }

        if (!classB.StartsWith(classA.Substring(0, indexOfTypeParams)))
        {
            return false;
        }

        if (GetGenericParameters(classA).Length != GetGenericParameters(classB).Length)
        {
            return false;
        }

        return true;
    }

    public static string[] GetGenericParameters(string className)
    {
        var listOpening = className.IndexOf('<');
        var listClosing = className.LastIndexOf('>');

        if (listOpening == -1)
        {
            return Array.Empty<string>();
        }

        var paramString = className.Substring(listOpening + 1, listClosing - (listOpening + 1));

        var result = new List<string>();
        var currentString = new StringBuilder();
        var currentLevel = 0;

        foreach (var ch in paramString)
        {
            switch (ch)
            {
                case '<':
                    ++currentLevel;
                break;
                case '>':
                    --currentLevel;
                break;
                case ',':
                    if (currentLevel == 0)
                    {
                        result.Add(currentString.ToString().Trim());
                        currentString.Clear();
                        continue;
                    }
                break;
            }

            currentString.Append(ch);
        }

        if (currentLevel == 0)
        {
            result.Add(currentString.ToString().Trim());
        }
        else
        {
            throw new ArgumentException($"\"{className}\" is not a valid class name.");
        }

        return result.ToArray();
    }

    public static string EscapeFileName(string className)
    {
        return className.Replace('<', '[').Replace('>', ']').Replace('?', '-');
    }
}
