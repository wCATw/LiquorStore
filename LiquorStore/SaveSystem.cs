using MSCLoader;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace LiquorStore;

public class SaveSystem
{
  private readonly Dictionary<Type, ISerializationSurrogate> _surrogateDictionary = new Dictionary<Type, ISerializationSurrogate>()
  {
    {
      typeof (Vector2),
      (ISerializationSurrogate) new SaveSystem.Vector2SerializationSurrogate()
    },
    {
      typeof (Vector3),
      (ISerializationSurrogate) new SaveSystem.Vector3SerializationSurrogate()
    },
    {
      typeof (Vector4),
      (ISerializationSurrogate) new SaveSystem.Vector4SerializationSurrogate()
    },
    {
      typeof (Quaternion),
      (ISerializationSurrogate) new SaveSystem.QuaternionSerializationSurrogate()
    },
    {
      typeof (Color),
      (ISerializationSurrogate) new SaveSystem.ColorSerializationSurrogate()
    }
  };
  public Dictionary<Type, ISerializationSurrogate> surrogateDictionary = new Dictionary<Type, ISerializationSurrogate>();

  public bool Save(string filepath, object data)
  {
    BinaryFormatter binaryFormatter = this.GetBinaryFormatter();
    FileStream fileStream = File.Create(filepath);
    FileStream serializationStream = fileStream;
    object graph = data;
    binaryFormatter.Serialize((Stream) serializationStream, graph);
    fileStream.Close();
    return true;
  }

  public T Load<T>(string filepath) where T : class, new()
  {
    if (!File.Exists(filepath))
      return new T();
    BinaryFormatter binaryFormatter = this.GetBinaryFormatter();
    FileStream serializationStream = File.Open(filepath, FileMode.Open);
    try
    {
      T obj = (T) binaryFormatter.Deserialize((Stream) serializationStream);
      serializationStream.Close();
      return obj;
    }
    catch
    {
      ModConsole.Error("The save file at path: " + filepath + "couldn't be loaded, (file or saveData class edited), save your game and the error is gone. have a nice day");
      serializationStream.Close();
      return new T();
    }
  }

  public void Delete(string filepath)
  {
    if (!File.Exists(filepath))
      return;
    File.Delete(filepath);
  }

  private BinaryFormatter GetBinaryFormatter()
  {
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    SurrogateSelector surrogateSelector = new SurrogateSelector();
    foreach (KeyValuePair<Type, ISerializationSurrogate> surrogate in this._surrogateDictionary)
      surrogateSelector.AddSurrogate(surrogate.Key, new StreamingContext(StreamingContextStates.All), surrogate.Value);
    foreach (KeyValuePair<Type, ISerializationSurrogate> surrogate in this.surrogateDictionary)
      surrogateSelector.AddSurrogate(surrogate.Key, new StreamingContext(StreamingContextStates.All), surrogate.Value);
    binaryFormatter.SurrogateSelector = (ISurrogateSelector) surrogateSelector;
    return binaryFormatter;
  }

  private class Vector2SerializationSurrogate : ISerializationSurrogate
  {
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
      Vector2 vector2 = (Vector2) obj;
      info.AddValue("x", vector2.x);
      info.AddValue("y", vector2.y);
    }

    public object SetObjectData(
      object obj,
      SerializationInfo info,
      StreamingContext context,
      ISurrogateSelector selector)
    {
      Vector2 vector2 = (Vector2) obj;
      vector2.x = (float) info.GetValue("x", typeof (float));
      vector2.y = (float) info.GetValue("y", typeof (float));
      obj = (object) vector2;
      return obj;
    }
  }

  private class Vector3SerializationSurrogate : ISerializationSurrogate
  {
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
      Vector3 vector3 = (Vector3) obj;
      info.AddValue("x", vector3.x);
      info.AddValue("y", vector3.y);
      info.AddValue("z", vector3.z);
    }

    public object SetObjectData(
      object obj,
      SerializationInfo info,
      StreamingContext context,
      ISurrogateSelector selector)
    {
      Vector3 vector3 = (Vector3) obj;
      vector3.x = (float) info.GetValue("x", typeof (float));
      vector3.y = (float) info.GetValue("y", typeof (float));
      vector3.z = (float) info.GetValue("z", typeof (float));
      obj = (object) vector3;
      return obj;
    }
  }

  private class Vector4SerializationSurrogate : ISerializationSurrogate
  {
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
      Vector4 vector4 = (Vector4) obj;
      info.AddValue("x", vector4.x);
      info.AddValue("y", vector4.y);
      info.AddValue("z", vector4.z);
      info.AddValue("w", vector4.w);
    }

    public object SetObjectData(
      object obj,
      SerializationInfo info,
      StreamingContext context,
      ISurrogateSelector selector)
    {
      Vector4 vector4 = (Vector4) obj;
      vector4.x = (float) info.GetValue("x", typeof (float));
      vector4.y = (float) info.GetValue("y", typeof (float));
      vector4.z = (float) info.GetValue("z", typeof (float));
      vector4.w = (float) info.GetValue("w", typeof (float));
      obj = (object) vector4;
      return obj;
    }
  }

  private class QuaternionSerializationSurrogate : ISerializationSurrogate
  {
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
      Quaternion quaternion = (Quaternion) obj;
      info.AddValue("x", quaternion.x);
      info.AddValue("y", quaternion.y);
      info.AddValue("z", quaternion.z);
      info.AddValue("w", quaternion.w);
    }

    public object SetObjectData(
      object obj,
      SerializationInfo info,
      StreamingContext context,
      ISurrogateSelector selector)
    {
      Quaternion quaternion = (Quaternion) obj;
      quaternion.x = (float) info.GetValue("x", typeof (float));
      quaternion.y = (float) info.GetValue("y", typeof (float));
      quaternion.z = (float) info.GetValue("z", typeof (float));
      quaternion.w = (float) info.GetValue("w", typeof (float));
      obj = (object) quaternion;
      return obj;
    }
  }

  private class ColorSerializationSurrogate : ISerializationSurrogate
  {
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
      Color color = (Color) obj;
      info.AddValue("r", color.r);
      info.AddValue("g", color.g);
      info.AddValue("b", color.b);
      info.AddValue("a", color.a);
    }

    public object SetObjectData(
      object obj,
      SerializationInfo info,
      StreamingContext context,
      ISurrogateSelector selector)
    {
      Color color = (Color) obj;
      color.r = (float) info.GetValue("r", typeof (float));
      color.g = (float) info.GetValue("g", typeof (float));
      color.b = (float) info.GetValue("b", typeof (float));
      color.a = (float) info.GetValue("a", typeof (float));
      obj = (object) color;
      return obj;
    }
  }
}
