using UnityEngine;
using System.Collections.Generic;
public class Node<T>
{
  public Node<T> Next { get; set; }
  public Node<T> Prev { get; set; }
  public T Data { get; set; }

  public Node(T data)
  {
    Next = Prev = null;
    Data = data;
  }
}

public class DoubleLinkedList<T>
{
  public Node<T> Head { get; private set; }
  public Node<T> Tail { get; private set; }

  public void AddLast(T data)
  {
    var node = new Node<T>(data);

    if (Tail == null)
    {
      Head = Tail = node;
    }
    else
    {
      Tail.Next = node;
      node.Prev = Tail;
      Tail = node;
    }
  }

  public void ShiftLeft()
  {
    if (Head == null || Tail == null || Head == Tail)
    {
      return;
    }

    var node = Head;

    Head = Head.Next;
    Head.Prev = null;

    Tail.Next = node;
    node.Prev = Tail;

    node.Next = null;
    Tail = node;
  }
}