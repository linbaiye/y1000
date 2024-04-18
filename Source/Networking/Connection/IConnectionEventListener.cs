namespace y1000.Source.Networking.Connection
{
    public interface IConnectionEventListener
    {
        void OnMessageArrived(object message);

        void OnConnectionClosed();

    }
}