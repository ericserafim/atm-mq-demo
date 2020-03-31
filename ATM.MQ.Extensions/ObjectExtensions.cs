using Newtonsoft.Json;

namespace ATM.MQ.Extensions
{
  public static class ObjectExtensions
  {
    public static string ToJson(this object obj) => JsonConvert.SerializeObject(obj);
  }
}
