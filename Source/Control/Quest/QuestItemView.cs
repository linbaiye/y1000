using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Godot;



namespace y1000.Source.Control.Quest

{
    public partial class QuestItemView : HFlowContainer
    {

        private Label _questName;

        private Button _submit;

        public event Action<QuestItemView>? SubmitPressed;

        public override void _Ready()
        {
            _questName = GetNode<Label>("QuestName");
            _submit = GetNode<Button>("Submit");
        }

        public string QuestName => _questName.Text;

        public void Set(string name, string submit)
        {
            _questName.Text = name;
            _submit.Text = submit;
            _submit.Pressed += () => SubmitPressed?.Invoke(this);
        }
    }

}