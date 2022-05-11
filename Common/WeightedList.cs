using System.Collections.Generic;
using UnityEngine;

public class WeightedList<T>
{
  private List<WeightedElement<T>> list;

  public WeightedList(List<T> _list, int defaulWeight = 100){
      Create(_list, defaulWeight);
  }

  public void Create(List<T> _list, int defaulWeight){
    this.list = new List<WeightedElement<T>>();
    foreach(T element in _list){
      Add(element, defaulWeight);
    }
  }

  public void Add(T element, int weight = 100){
    WeightedElement<T> weightedElement = new WeightedElement<T>(element, weight);
    list.Add(weightedElement);
  }

  public void Remove(T element){
    WeightedElement<T> weightedElement = Find(element);
    if (element != null){
      list.Remove(weightedElement);
    }
  }

  public T GetRandom(){
    int totalWeight = GetTotalWeight();
    int roll = Random.Range(0, totalWeight);
    foreach(WeightedElement<T> weightedElement in list){
      roll -= weightedElement.weight;
      if (roll <= 0){
        return weightedElement.element;
      }
    }
    throw new System.Exception($"Ran out of options! List size: {list.Count}, total weight: {totalWeight}, roll: {roll}");
  }

  public int Length(){
    return list.Count;
  }

  public List<T> ToList(){
    List<T> result = new List<T>();
    foreach(WeightedElement<T> weightedElement in list){
      result.Add(weightedElement.element);
    }
    return result;
  }

  public T[] ToArray(){
    T[] array = new T[list.Count];
    for(int i = 0; i < list.Count; i++){
      array[i] = list[i].element;
    }
    return array;
  }

  public void ModifyWeight(T element, float weightModify){
    WeightedElement<T> weightedElement = Find(element);
    if (weightedElement != null){
      weightedElement.ModifyWeight(weightModify);
    }
  }

  public WeightedElement<T> Find(T element){
    foreach(WeightedElement<T> weightedElement in list){
      if (weightedElement.element.Equals(element)){
        return weightedElement;
      }
    }
    return null;
  }

  public void Print(){
    foreach(WeightedElement<T> weightedElement in list){
      Debug.Log(weightedElement.ToString());
    }
  }

  private int GetTotalWeight(){
    int weight = 0;
    foreach(WeightedElement<T> element in list){
      weight += element.weight;
    }
    return weight;
  }
}

public class  WeightedElement<T>
{
  public int weight;
  public T element;

  public WeightedElement(T element, int weight){
    this.element = element;
    this.weight = weight;
  }

  public void ModifyWeight(float modifier, int minWeight = 1){
    weight = (int)(weight * modifier);
    if (weight <= 0){
      weight = minWeight;
    }
  }

  public override string ToString()
  {
    return $"{weight}: {element.ToString()}";
  }
}