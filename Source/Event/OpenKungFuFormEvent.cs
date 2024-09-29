namespace y1000.Source.Event
{
    public class OpenKungFuFormEvent : IUiEvent
    {
        public static readonly OpenKungFuFormEvent Instance = new OpenKungFuFormEvent();
        private OpenKungFuFormEvent() {}
    }
}