using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MinHeap<T> where T : IComparable<T> {
    Dictionary<T,int> positionTable = new Dictionary<T,int>();
    List<T> minHeap;
    private int size = 0;

    // Start is called before the first frame update
    public MinHeap(){
        minHeap = new List<T>();
        this.size = 0;
    }

    //return index position of item
    public int getPosition(T item){
        return positionTable[item];
    }

    //returns true or false if MinHeap empty
    public bool isEmpty(){
        return size <= 0;
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

    //insert item into MinHeap
    public void insert(T item){
        minHeap.Add(item);
        positionTable[item] = size;
        size++;
        decreaseKey(size-1);
    }

    //resorts the MinHeap
    public void decreaseKey(int pos){
        int currPos=pos;
        while (minHeap[currPos].CompareTo(minHeap[parent(currPos)])<0){
            swap(currPos,parent(currPos));
            currPos=parent(currPos);
        }
    }

    public T peak(){
        return minHeap[0];
    }

    private bool moveDown(int pos){
        bool leftSmaller = leftChild(pos) < size && minHeap[leftChild(pos)].CompareTo(minHeap[pos]) < 0;
        bool rightSmaller = rightChild(pos) < size && minHeap[rightChild(pos)].CompareTo(minHeap[pos]) < 0;
        return leftSmaller || rightSmaller;
    }

    public void increaseKey(int pos){
        int currPos = pos;
        while(moveDown(currPos)){
            int rPos = rightChild(currPos);
            int lPos = leftChild(currPos);
            if (rPos < size && minHeap[rPos].CompareTo(minHeap[lPos]) < 0){
                swap(rPos, currPos);
                currPos = rPos;
            }
            else{
                swap(lPos,currPos);
                currPos = lPos;
            }
        }
    }


    //extracts the item with the smallest value in the MinHeap
    public T extractMin(){
        T min = minHeap[0];
        minHeap[0] = minHeap[size-1];
        positionTable[minHeap[0]] = 0;
        size--;
        increaseKey(0);
        return min;
    }
}