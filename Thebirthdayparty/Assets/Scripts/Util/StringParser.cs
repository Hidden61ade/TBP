using System;
using System.Collections.Generic;

public static class ParseString
{
    // 字典用于存储变量名和对应的布尔值
    private static Dictionary<string, bool> variables = new(){
        {"TestVariable",TestVariable}
    };
    private static bool TestVariable
    {
        get
        {
            return false;
        }
    }
    public static void SetVariableTrue(string variableName)
    {
        if (variables.ContainsKey(variableName))
        {
            variables[variableName] = true;
        }
        else
        {
            variables.Add(variableName, true);
        }
    }
    public static void SetVariableFalse(string variableName)
    {
        if (variables.ContainsKey(variableName))
        {
            variables[variableName] = false;
        }
        else
        {
            variables.Add(variableName, false);
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
            string variableName = input.Substring(startIndex, endIndex - startIndex);
            int dollarEndIndex = input.IndexOf('$', endIndex + 1); // 找到 $ 结束符

            if (dollarEndIndex > endIndex)
            {
                string trueContent = input.Substring(endIndex + 1, dollarEndIndex - endIndex - 1).Split(';')[0];  // 提取 A 部分
                string falseContent = input.Substring(endIndex + 1, dollarEndIndex - endIndex - 1).Split(';')[1]; // 提取 B 部分

                // 检查变量值，返回相应内容
                if (variables.ContainsKey(variableName))
                {
                    return variables[variableName] ? trueContent : falseContent;
                }
                else
                {
                    return falseContent;
                }
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
