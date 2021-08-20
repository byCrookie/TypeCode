using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TypeCode.Business.Bootstrapping;

namespace TypeCode.Business.Configuration
{
    public class AssemblyGroup
    {
        public AssemblyGroup()
        {
            AssemblyPathSelector = new List<AssemblyPathSelector>();
            AssemblyPath = new List<AssemblyPath>();
        }
        
        public List<AssemblyPathSelector> AssemblyPathSelector { get; set; }
        public List<AssemblyPath> AssemblyPath { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public string Text { get; set; }
    }
    
    public class AssemblyRoot
    {
        public AssemblyRoot()
        {
            IncludeAssemblyPattern = new List<string>();
            AssemblyGroup = new List<AssemblyGroup>();
        }
        
        public int Priority { get; set; }
        public List<string> IncludeAssemblyPattern { get; set; }
        public List<AssemblyGroup> AssemblyGroup { get; set; }
        public string Path { get; set; }
        public string Text { get; set; }
    }
    
    public class TypeCodeConfiguration
    {
        public TypeCodeConfiguration()
        {
            AssemblyRoot = new List<AssemblyRoot>();
        }
        
        public bool CloseCmd { get; set; }
        public string SpaceKey { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string VersionPageName { get; set; }
        public string BaseUrl { get; set; }
        public List<AssemblyRoot> AssemblyRoot { get; set; }
    }

    public class AssemblyPath : IAssemblyHolder, IDictionaryHolder
    {
        public AssemblyPath()
        {
            AssemblyDirectories = new ConcurrentBag<AssemblyDirectory>();
            TypesByNameDictionary = new Dictionary<string, List<Type>>();
            TypesByFullNameDictionary = new Dictionary<string, List<Type>>();   
        }
        
        public string Path { get; set; }
        public int Priority { get; set; }
        public ConcurrentBag<AssemblyDirectory> AssemblyDirectories { get; set; }
        public IDictionary<string, List<Type>> TypesByNameDictionary { get; set; }
        public IDictionary<string, List<Type>> TypesByFullNameDictionary { get; set; }
    }

    internal interface IDictionaryHolder
    {
        IDictionary<string, List<Type>> TypesByNameDictionary { get; set; }
        IDictionary<string, List<Type>> TypesByFullNameDictionary { get; set; }
    }

    public class AssemblyPathSelector : IAssemblyHolder, IDictionaryHolder
    {
        public AssemblyPathSelector()
        {
            AssemblyDirectories = new ConcurrentBag<AssemblyDirectory>();
            TypesByNameDictionary = new Dictionary<string, List<Type>>();
            TypesByFullNameDictionary = new Dictionary<string, List<Type>>();
        }
        
        public string Path { get; set; }
        public int Priority { get; set; }
        public string Selector { get; set; }
        public ConcurrentBag<AssemblyDirectory> AssemblyDirectories { get; set; }
        public IDictionary<string, List<Type>> TypesByNameDictionary { get; set; }
        public IDictionary<string, List<Type>> TypesByFullNameDictionary { get; set; }
    }

    internal interface IAssemblyHolder
    {
        string Path { get; set; }
        ConcurrentBag<AssemblyDirectory> AssemblyDirectories { get; set; }
    }
}