using UnityEngine;

public static class Utils
{
    public static T LoadObject<T>(string name, string folder = "") where T : UnityEngine.Object {
        T obj = Resources.Load<T>(folder + name);
        if (obj == null) {
            string message = "Utils.LoadObject: unable to load object {0} of type {1} from folder {2}";
            throw new System.Exception(string.Format(message, name, typeof(T).Name, folder));
        }
        //Debug.Log(name + " loaded successfully");
        return obj;
    }
}