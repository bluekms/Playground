namespace StaticDataLibrary.Tree;

public sealed class Tree<T>
{
    public delegate void TreeVisitor(T value);
    
    public T Value { get; }
    
    private LinkedList<Tree<T>> children;

    public Tree(T value)
    {
        Value = value;
        children = new();
    }

    public Tree<T> Add(T data)
    {
        var item = new Tree<T>(data);
        children.AddFirst(item);
        return item;
    }

    public void Traverse(Tree<T> node, TreeVisitor visitor)
    {
        visitor(node.Value);
        foreach (var kid in node.children)
        {
            Traverse(kid, visitor);
        }
    }
}