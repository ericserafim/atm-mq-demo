using Newtonsoft.Json;

namespace ATM.MQ.Extensions
{
  public static class StringExtensions
  {
    public static T ToObject<T>(this string str) => JsonConvert.DeserializeObject<T>(str);
  }
}
