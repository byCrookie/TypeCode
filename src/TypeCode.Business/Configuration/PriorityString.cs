﻿namespace TypeCode.Business.Configuration;

public class PriorityString
{
    public PriorityString(string priority, string message)
    {
        Priority = priority;
        Message = message;
    }
    
    public string Priority { get; }
    public string Message { get; }
}