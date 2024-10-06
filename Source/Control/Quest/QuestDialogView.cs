using Godot;

using y1000.Source.Event;
using y1000.Source.Networking;

using y1000.Source.Networking.Server;



namespace y1000.Source.Control.Quest
{
    public partial class QuestDialogView : NinePatchRect
    {

        private QuestItemView _questItem;

        private RichTextLabel _description;

        private Label _npcName;

        private Button _close;

        private EventMediator? _eventMediator;

        private long? _id;

        public override void _Ready()
        {
            _description = GetNode<RichTextLabel>("QuestDescription");
            _npcName = GetNode<Label>("NpcName");
            _questItem = GetNode<QuestItemView>("QuestItems/QuestItem");
            _questItem.SubmitPressed += HandleSubmit;
            _close = GetNode<Button>("Close");
            _close.Pressed += () => Visible = false;
            Visible = false;
        }

        private void HandleSubmit(QuestItemView which)
        {
            Visible = false;
            if (_id.HasValue)
                _eventMediator?.NotifyServer(new ClientSubmitQuestEvent(_id.Value, which.QuestName));
        }

        public void Initialize(EventMediator eventMediator)
        {
            _eventMediator = eventMediator;
        }

        public void Handle(UpdateQuestWindowMessage message) 
        {
            _questItem.Set(message.QuestName, message.SubmitText);
            _npcName.Text = message.NpcName;
            _description.Text = message.Description;
            Visible = true;
            _id = message.Id;
        }
    }

}