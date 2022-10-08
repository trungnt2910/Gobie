using System.Security.Cryptography.X509Certificates;

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
        if (!string.IsNullOrEmpty(identifierAndGenericParts.ElementAtOrDefault(1)))
        {
            identifier += "<" + identifierAndGenericParts[1];
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

        if (classA.Count(ch => ch == ',') != classB.Count(ch => ch == ','))
        {
            return false;
        }

        return true;
    }

    public static string[] GetGenericParameters(string className)
    {
        var listOpening = className.IndexOf('<');
        var listClosing = className.IndexOf('>');

        if (listOpening == -1)
        {
            return Array.Empty<string>();
        }

        var list = className.Substring(listOpening + 1, listClosing - (listOpening + 1)).Split(',');

        return list.Select(x => x.Trim()).ToArray();
    }

    public static string EscapeFileName(string className)
    {
        return className.Replace('<', '[').Replace('>', ']');
    }
}
