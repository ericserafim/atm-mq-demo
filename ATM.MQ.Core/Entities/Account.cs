namespace ATM.MQ.Core.Entities
{
	public class Account
	{
		public long Id { get; set; }

		public Owner Owner { get; set; }

		public double Balance { get; set; }
	}
}