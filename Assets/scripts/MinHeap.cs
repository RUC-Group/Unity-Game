using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap<T> {
    Dictionary<T,int> positionTable = new Dictionary<T,int>();
    private readonly IComparer<T> comparer;
    List<T> minHeap;
    private int size = 0;
    // Start is called before the first frame update
    public MinHeap(){
        minHeap = new List<T>();
        this.size = 0;
    }
    public int getPosition(T item){
        return positionTable[item];
    }

    public bool isEmpty(){
        return size >=0;
    }

    private int parent(int pos){
        return (pos-1)/2;
    }

    private int leftChild(int pos){
        return pos*2 + 1;
    }

    private int rightChild(int pos){
        return pos * 2 + 2; 
    }

    private void swap(int pos1, int pos2){
        T temp = minHeap[pos1];

        minHeap[pos1] = minHeap[pos2];
        minHeap[pos2] = temp;

        positionTable[minHeap[pos1]] = pos1;
        positionTable[minHeap[pos2]] = pos2;
        
    }

    public void insert(T item){
        minHeap.Add(item);
        positionTable[item] = size;
        size++;
        decreaseKey(size-1);
    }

    private void decreaseKey(int i) {
        for (int left = leftChild(i); left < minHeap.Count; left = leftChild(i)) {
            int smallest = comparer.Compare(minHeap[left], minHeap[i]) <= 0 ? left : i;
            int right = rightChild(i);
            if (right < minHeap.Count && comparer.Compare(minHeap[right], minHeap[smallest]) <= 0) smallest = right;
            if (smallest == i) return;
            (minHeap[i], minHeap[smallest]) = (minHeap[smallest], minHeap[i]);
            i = smallest;
        }
    }
    /*
    public void decreaseKey(int pos){
        int currPos = pos;
        while (comparer.Compare(minHeap[currPos], minHeap[parent(currPos)]) < 0){
            swap(currPos, parent(currPos));
            currPos = parent(currPos);
        }
    }
    */
    public T peak(){
        return minHeap[0];
    }

    private bool moveDown(int pos){
        bool leftSmaller = leftChild(pos) < size && comparer.Compare(minHeap[leftChild(pos)],minHeap[pos]) < 0;
        bool rightSmaller = rightChild(pos) < size && comparer.Compare(minHeap[rightChild(pos)],minHeap[pos]) < 0;
        return leftSmaller || rightSmaller;
    }

    public void increaseKey(int pos){
        int currPos = pos;
        while(moveDown(currPos)){
            int rPos = rightChild(currPos);
            int lPos = leftChild(currPos);
            if (rPos < size && comparer.Compare(minHeap[rPos],minHeap[lPos]) < 0){
                swap(rPos, currPos);
                currPos = rPos;
            }
            else{
                swap(lPos,currPos);
                currPos = lPos;
            }
        }
    }

    
    public T extractMin(){
        T min = minHeap[0];
        minHeap[0] = minHeap[size-1];
        positionTable[minHeap[0]] = 0;
        size--;
        increaseKey(0);
        return min;
    }
}