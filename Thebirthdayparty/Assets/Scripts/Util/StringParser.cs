using System;
using System.Collections.Generic;
using UnityEngine;

public static class ParseString
{
    private static Dictionary<string, bool> tempVariables = new();
    private static Dictionary<string, Func<bool>> variables = new(){
        {"Napkin",()=>Napkin},
        {"Departure",()=>Departure},
        {"Cabin",()=>Cabin}
    };

    private static bool Napkin
    {
        get
        {
            Debug.LogWarning("Get bool Napkin: " + CollectionManager.Instance.IsCollectionUnlocked("M00"));
            return CollectionManager.Instance.IsCollectionUnlocked("M00");
        }
    }
    private static bool Departure
    {
        get
        {
            return CollectionManager.Instance.IsCollectionUnlocked("G03");
        }
    }
    private static bool Cabin
    {
        get
        {
            return CollectionManager.Instance.IsCollectionUnlocked("G08");
        }
    }
    public static void ClearTempVariables(){
        tempVariables.Clear();
    }
    public static void SetVariableTrue(string variableName)
    {
        if (tempVariables.ContainsKey(variableName))
        {
            tempVariables[variableName] = true;
        }
        else
        {
            tempVariables.Add(variableName, true);
        }
    }
    public static void SetVariableFalse(string variableName)
    {
        if (tempVariables.ContainsKey(variableName))
        {
            tempVariables[variableName] = false;
        }
        else
        {
            tempVariables.Add(variableName, false);
        }
    }

    // 获取变量值，若变量名不存在，则默认返回 false
    public static bool GetVariable(string variableName)
    {
        if (variables.ContainsKey(variableName))
        {
            return variables[variableName]();
        }
        else if (tempVariables.ContainsKey(variableName))
        {
            return tempVariables[variableName];
        }
        else
        {
            return false;
        }
    }

    // 解析字符串的方法
    public static string Parse(string input)
    {
        // 如果输入字符串不是以 $ 开头，直接返回原始字符串
        if (string.IsNullOrEmpty(input) || input[0] != '$')
        {
            return input;
        }

        // 解析 $(...)A;B$ 格式的字符串
        int startIndex = input.IndexOf('$') + 2; // 跳过 $(
        int endIndex = input.IndexOf(')', startIndex); // 找到 )

        if (startIndex > 1 && endIndex > startIndex)
        {
            string variablesList = input.Substring(startIndex, endIndex - startIndex); // 提取变量列表
            int dollarEndIndex = input.IndexOf('$', endIndex + 1); // 找到 $ 结束符

            if (dollarEndIndex > endIndex)
            {
                string trueContent = input.Substring(endIndex + 1, dollarEndIndex - endIndex - 1).Split(';')[0];  // 提取 A 部分
                string falseContent = input.Substring(endIndex + 1, dollarEndIndex - endIndex - 1).Split(';')[1]; // 提取 B 部分

                // 检查所有变量是否都为 true
                string[] variableNames = variablesList.Split(',');

                bool allTrue = true;
                foreach (var variableName in variableNames)
                {
                    if (!GetVariable(variableName.Trim())) // 检查是否所有变量都为 true
                    {
                        allTrue = false;
                        break;
                    }
                }

                return allTrue ? trueContent : falseContent;
            }
        }

        // 如果格式不匹配，返回原始字符串
        return input;
    }
    public static string[] ParseChoice(string input)
    {
        // 如果输入的字符串开头不是 '$'，直接返回原字符串
        if (string.IsNullOrEmpty(input) || input[0] != '$')
        {
            return new string[] { input, "" };
        }

        // 如果字符串以 '$' 开头，去掉 '$' 后进行分割
        string trimmedInput = input.Substring(1);

        // 查找第一个 '#' 的位置
        int hashIndex = trimmedInput.IndexOf('#');

        if (hashIndex == -1)
        {
            // 如果没有找到 '#'，认为输入是一个部分，返回单一的字符串
            return new string[] { trimmedInput, "" };
        }

        // 分割成两部分
        string part1 = trimmedInput.Substring(0, hashIndex);
        string part2 = trimmedInput.Substring(hashIndex); // 保留从 '#' 开始的部分

        return new string[] { part1, part2 };
    }
}
