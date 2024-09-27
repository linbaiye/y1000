using System;
using Godot;
using y1000.Source.KungFu;
using y1000.Source.KungFu.Attack;
using y1000.Source.Util;

namespace y1000.Source.Control.Guild
{
    public partial class GuildKungFuFormView : NinePatchRect
    {
        private LineEdit _name;
        private CheckBox _fist;
        private CheckBox _sword;
        private CheckBox _blade;
        private CheckBox _axe;
        private CheckBox _spear;
        private TextureButton _confirmButton;
        private TextureButton _cancelButton;
        private LineEdit _attackSpeed;
        private LineEdit _headDamage;
        private LineEdit _headArmor;
        private LineEdit _swingInnerPower;
        private LineEdit _swingOuterPower;
        private LineEdit _swingPower;
        private LineEdit _swingLife;
        private LineEdit _bodyDamage;
        private LineEdit _armDamage;
        private LineEdit _armArmor;
        private LineEdit _recovery;
        private LineEdit _legDamage;
        private LineEdit _legArmor;
        private LineEdit _avoid;
        private LineEdit _bodyArmor;
        private Label _label;
        public override void _Ready()
        {
            _name = GetNode<LineEdit>("Name");
            _fist = GetNode<CheckBox>("Fist");
            _sword = GetNode<CheckBox>("Sword");
            _blade = GetNode<CheckBox>("Blade");
            _axe = GetNode<CheckBox>("Axe");
            _spear = GetNode<CheckBox>("Spear");
            _fist.Pressed += () => OnChecked(_fist);
            _sword.Pressed += () => OnChecked(_sword);
            _blade.Pressed += () => OnChecked(_blade);
            _spear.Pressed += () => OnChecked(_spear);
            _axe.Pressed += () => OnChecked(_axe);
            _confirmButton = GetNode<TextureButton>("ConfirmButton");
            _cancelButton = GetNode<TextureButton>("CancelButton");
            _attackSpeed = GetNode<LineEdit>("AttackSpeed");
            _headDamage = GetNode<LineEdit>("HeadDamage");
            _headArmor = GetNode<LineEdit>("HeadArmor");
            _swingInnerPower = GetNode<LineEdit>("SwingInnerPower");
            _swingOuterPower = GetNode<LineEdit>("SwingOuterPower");
            _swingPower = GetNode<LineEdit>("SwingPower");
            _swingLife = GetNode<LineEdit>("SwingLife");
            _bodyDamage = GetNode<LineEdit>("BodyDamage");
            _armDamage = GetNode<LineEdit>("ArmDamage");
            _armArmor = GetNode<LineEdit>("ArmArmor");
            _recovery = GetNode<LineEdit>("Recovery");
            _legDamage = GetNode<LineEdit>("LegDamage");
            _legArmor = GetNode<LineEdit>("LegArmor");
            _avoid = GetNode<LineEdit>("Avoid");
            _bodyArmor = GetNode<LineEdit>("BodyArmor");
            _label = GetNode<Label>("Label");
            //Visible = false;
            _confirmButton.Pressed += OnConfirm;
            _cancelButton.Pressed += () => Visible = false;
            _fist.SetPressedNoSignal(true);
        }

        private void OnChecked(CheckBox checkBox)
        {
            _fist.SetPressedNoSignal(false);
            _blade.SetPressedNoSignal(false);
            _sword.SetPressedNoSignal(false);
            _axe.SetPressedNoSignal(false);
            _spear.SetPressedNoSignal(false);
            checkBox.SetPressedNoSignal(true);
        }

        private bool CheckDigitInput(LineEdit inputEdit, string field)
        {
            if (string.IsNullOrEmpty(inputEdit.Text) || !inputEdit.Text.DigitOnly())
            {
                _label.Text = field + "只能是数字";
                return false;
            }
            return true;
        }
        
        private bool CheckInputs()
        {
            if (string.IsNullOrEmpty(_name.Text))
            {
                _label.Text = "请输入武功名";
                return false;
            }
            if (!CheckDigitInput(_attackSpeed, "速度"))
            {
                return false;
            }
            if (!CheckDigitInput(_headDamage, "头部攻击"))
            {
                return false;
            }
            if (!CheckDigitInput(_headArmor, "头部防御"))
            {
                return false;
            }
            if (!CheckDigitInput(_swingInnerPower, "内功消耗"))
            {
                return false;
            }
            if (!CheckDigitInput(_swingOuterPower, "外功消耗"))
            {
                return false;
            }
            if (!CheckDigitInput(_swingPower, "武功消耗"))
            {
                return false;
            }
            if (!CheckDigitInput(_swingLife, "活力消耗"))
            {
                return false;
            }
            if (!CheckDigitInput(_bodyDamage, "身体攻击"))
            {
                return false;
            }
            if (!CheckDigitInput(_armDamage, "手臂攻击"))
            {
                return false;
            }
            if (!CheckDigitInput(_armArmor, "手臂防御"))
            {
                return false;
            }
            if (!CheckDigitInput(_recovery, "恢复"))
            {
                return false;
            }
            if (!CheckDigitInput(_legDamage, "腿部攻击"))
            {
                return false;
            }
            if (!CheckDigitInput(_legArmor, "腿部防御"))
            {
                return false;
            }
            if (!CheckDigitInput(_avoid, "闪躲"))
            {
                return false;
            }
            if (!CheckDigitInput(_bodyArmor, "身体防御"))
            {
                return false;
            }
            return true;
        }

        private AttackKungFuType GetAttackKungFuType()
        {
            if (_fist.ButtonPressed)
                return AttackKungFuType.QUANFA;
            if (_axe.ButtonPressed)
                return AttackKungFuType.AXE;
            if (_sword.ButtonPressed)
                return AttackKungFuType.SWORD;
            if (_blade.ButtonPressed)
                return AttackKungFuType.BLADE;
            if (_spear.ButtonPressed)
                return AttackKungFuType.SPEAR;
            throw new ArgumentException();
        }

        public void OnConfirm()
        {
            if (!CheckInputs())
            {
                return;
            }

            var guildKungFuForm = new GuildKungFuForm()
            {
                Speed = _attackSpeed.Text.ToInt(),
                Recovery = _recovery.Text.ToInt(),
                Avoid = _avoid.Text.ToInt(),
                BodyDamage = _bodyDamage.Text.ToInt(),
                HeadDamage = _headDamage.Text.ToInt(),
                ArmDamage = _armDamage.Text.ToInt(),
                LegDamage = _legDamage.Text.ToInt(),
                BodyArmor = _bodyArmor.Text.ToInt(),
                HeadArmor = _headArmor.Text.ToInt(),
                ArmArmor = _armArmor.Text.ToInt(),
                LegArmor = _legArmor.Text.ToInt(),
                LifeToSwing = _swingLife.Text.ToInt(),
                PowerToSwing = _swingPower.Text.ToInt(),
                OuterPowerToSwing = _swingOuterPower.Text.ToInt(),
                InnerPowerToSwing = _swingInnerPower.Text.ToInt(),
                Type = GetAttackKungFuType(),
                Name = _name.Text,
            };
            string ret = guildKungFuForm.Validate();
            if (!string.IsNullOrEmpty(ret))
            {
                _label.Text = ret;
                return;
            }
        }

    }
}