using System.Text.Json;
using CommandLine;
using ExcelToCsvUsingRecords;
using StaticDataLibrary;
using StaticDataLibrary.Tree;

const string command = "-s ClassListTest -e D:\\Workspace\\gitProject\\Playground\\StaticData\\__TestStaticData.xlsx -o D:\\Workspace\\gitProject\\Playground\\StaticData\\Output\\ETC\\";

Parser.Default.ParseArguments<ProgramOptions>(command.Split(' '))
    .WithParsed(RunOptions)
    .WithNotParsed(HandleParseError);

static void RunOptions(ProgramOptions options)
{
    var loader = new ExcelLoader(options.ExcelFileName, options.SheetName);
    foreach (var sheet in loader.SheetList)
    {
        var foo = new RecordReaderFromLibrary(sheet.Name);
    }

    var tree = new Tree<string>("Root");
    var a = tree.Add("A");
    var b = tree.Add("B");
    var c = tree.Add("C");
    a.Add("A-1");
    var a2 = a.Add("A-2");
    c.Add("C-1");

    a2.Add("A-2-1");

    tree.Traverse(tree, value => Console.WriteLine(value));
}

static void HandleParseError(IEnumerable<Error> errors)
{
    Console.WriteLine($"Errors {errors.Count()}");
    foreach (var error in errors)
    {
        Console.WriteLine(error.ToString());
    }
}

static void TargetTest()
{
    Console.WriteLine("TargetTest");
    var targetRoot = new Tree<PropertyNode>(new(PropertyNode.NodeTypes.PrimitiveTypes, "Id", null));
    var target1 = targetRoot.Add(new(PropertyNode.NodeTypes.PrimitiveTypes, "Value1", null));
    target1.Add(new(PropertyNode.NodeTypes.PrimitiveTypes, "Value3", null));
    targetRoot.Traverse(targetRoot, (value) => { Console.WriteLine(value); });
}

static void ArrayTest()
{
    Console.WriteLine();
    Console.WriteLine("ArrayTest");
    var arrayRoot = new Tree<PropertyNode>(new(PropertyNode.NodeTypes.PrimitiveTypes, "Id", null));
    var array1 = arrayRoot.Add(new(PropertyNode.NodeTypes.PrimitiveTypeList, "ValueList", "Value"));
    arrayRoot.Add(new(PropertyNode.NodeTypes.PrimitiveTypes, "Info", null));
    arrayRoot.Traverse(arrayRoot, (value) =>
    {
        if (value.NodeType is PropertyNode.NodeTypes.PrimitiveTypes or PropertyNode.NodeTypes.PrimitiveTypeList)
        {
            Console.WriteLine(value);    
        }
    });
}

static void ClassTest()
{
    Console.WriteLine();
    Console.WriteLine("ClassTest");
    var classRoot = new Tree<PropertyNode>(new(PropertyNode.NodeTypes.PrimitiveTypes, "StudentId", null));
    var class1 = classRoot.Add(new(PropertyNode.NodeTypes.PrimitiveTypes, "Name", null));
    var class2 = class1.Add(new(PropertyNode.NodeTypes.ClassList, "SubjectInfo", null));
    class2.Add(new(PropertyNode.NodeTypes.PrimitiveTypes, "Subject", null));
    class2.Add(new(PropertyNode.NodeTypes.PrimitiveTypes, "Grade", null));
    class1.Add(new(PropertyNode.NodeTypes.PrimitiveTypes, "Note", null));
    classRoot.Traverse(classRoot, (value) =>
    {
        if (value.NodeType is PropertyNode.NodeTypes.PrimitiveTypes or PropertyNode.NodeTypes.PrimitiveTypeList)
        {
            Console.WriteLine(value);    
        }
    });
}

static void ComplexTest()
{
    Console.WriteLine();
    Console.WriteLine("ComplexTest");
    var complexRoot = new Tree<PropertyNode>(new(PropertyNode.NodeTypes.PrimitiveTypes, "Id", null));
    complexRoot.Add(new(PropertyNode.NodeTypes.ClassList, "ClassListTestRecord", null));
}