using System.Threading.Tasks;
using ATM.MQ.Core.Entities;

namespace ATM.MQ.Core.Interfaces.MQ
{
	public interface IMQProvider
	{
		void Connect();

		void PublishMessage(string senderId, MessageData<Transaction> message);

		void SubscribeQueue(string queueName);

		void Close();
	}
}
