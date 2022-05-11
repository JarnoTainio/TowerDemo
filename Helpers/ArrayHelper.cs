public class ArrayHelper
{
  public static T GetValue<T>(int index, T[] arr){
    if (arr == null || arr.Length == 0){
      return default(T);
    }
    if (arr.Length > index){
      return arr[index];
    }
    return arr[arr.Length -1];
  }

  public static int GetIndex<T>(T[] array, T target){
    for(int i = 0; i < array.Length; i++) {
      if (target.Equals(array[i])){
        return i;
      }
    }
    return -1;
  }

  public static bool Contains<T>(T[] array, T target){
    return GetIndex<T>(array, target) != -1;
  }
}
