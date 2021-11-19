using System;

namespace ADT
{
    public class LList<T> : IDynamicList<T>
    {
        public LLNode<T> headNode;
        public LLNode<T> tailNode;
        private int count = 0;
        
        public LList()
        {
            headNode = null;
            tailNode = null;
            count = 0;
        }
        
        public int Count
        {
            get { return count; }
        }

        public int IndexOf(T item)
        {
            int index = GetIndexOnItem(item);
            
            return index;
        }

        public bool Contains(T item)
        {
            if(GetNodeOnItem(item) != null)
            {
                return true;
            }
 
            return false; 
        }

        public void Add(T item)
        {
            LLNode<T> newNode = new LLNode<T>(item, null, tailNode);

            if (tailNode == null)
            {
                headNode = newNode;
                tailNode = headNode;
                headNode.nextNode = tailNode;
                count++;
                return;
            }

            tailNode.nextNode = newNode;
            tailNode = newNode;
            count++;
        }

        public void Insert(int index, T item)
        {
            LLNode<T> node = GetNodeOnIndex(index);
            LLNode<T> newNode = new LLNode<T>(item, node, node.previousNode);

            if (node.previousNode != null)
            {
                node.previousNode.nextNode = newNode;
            }
            else
            {
                headNode = newNode;
                headNode.nextNode = node;
            }

            node.previousNode = newNode;
            
            count++;
        }

        public void RemoveAt(int index)
        {
            LLNode<T> node = GetNodeOnIndex(index);

            if (node.nextNode == null)
            {
                node.previousNode.nextNode = null;
                tailNode = node.previousNode;
                count--;
                return;
            }

            if(node.previousNode == null)
            {
                node.nextNode.previousNode = null;
                headNode = node.nextNode;
                count--;
                return;
            }

            node.nextNode.previousNode = node.previousNode;
            node.previousNode.nextNode = node.nextNode;
            
            count--;
        }

        public bool Remove(T item)
        {
            LLNode<T> node = GetNodeOnItem(item);

            if (node != null)
            {
                if (node.nextNode == null)
                {
                    node.previousNode.nextNode = null;
                    tailNode = node.previousNode;
                    count--;
                    return true;
                }

                if(node.previousNode == null)
                {
                    node.nextNode.previousNode = null;
                    headNode = node.nextNode;
                    count--;
                    return true;
                }

                node.nextNode.previousNode = node.previousNode;
                node.previousNode.nextNode = node.nextNode;
            
                count--;
                return true;
            }
            
            return false;
        }

        public void Clear()
        {
            headNode = null;
            tailNode = null;
            count = 0;
        }

        public void CopyTo(T[] target, int index)
        {
            // seems irrelevant to linked list snake
            throw new System.NotImplementedException();
        }

        public T this[int index]
        {
            get => GetNodeOnIndex(index).value;
            set => GetNodeOnIndex(index).value = value;
        }

        private LLNode<T> GetNodeOnIndex(int index)
        {
            int currentIndex = 0;
            LLNode<T> currentNode = headNode;

            while(currentNode != null)
            {
                if (currentIndex == index)
                {
                    break;
                }

                currentNode = currentNode.nextNode;
                currentIndex++;
            }

            return currentNode;
        }

        public LLNode<T> GetNodeOnItem(T item)
        {
            LLNode<T> currentNode = headNode;
 
            while(currentNode != null)
            {
                if (currentNode.value.Equals(item))
                {
                    return currentNode;
                }
                currentNode = currentNode.nextNode;
            }
 
            return null;
        }
        private int GetIndexOnItem(T item)
        {
            int currentIndex = 0;
            LLNode<T> currentNode = headNode;
 
            while(currentNode != null)
            {
                if (currentNode.value.Equals(item))
                {
                    return currentIndex;
                }

                currentIndex++;
                currentNode = currentNode.nextNode;
            }
            
            return -1;
        }
    }

    public class LLNode<T>
    {
        public T value;
        public LLNode<T> nextNode;
        public LLNode<T> previousNode;

        public LLNode(T value, LLNode<T> nextNode, LLNode<T> previousNode)
        {
            this.value = value;
            this.nextNode = nextNode;
            this.previousNode = previousNode;
        }        
    }
}
