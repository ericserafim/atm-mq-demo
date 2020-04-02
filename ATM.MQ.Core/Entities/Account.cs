namespace ATM.MQ.Core.Entities
{
  public class Account : EntityBase
  {
    public Owner Owner { get; set; }

    public double Balance { get; set; }
  }
}