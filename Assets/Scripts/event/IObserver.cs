namespace EventBusEx.@event
{
    public interface IObserver
    {
        void OnNotify(IEvent @event);
    }
}