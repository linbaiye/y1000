using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using Godot;
using Microsoft.Extensions.Logging;
using NLog;
using y1000.Source.Audio;
using y1000.Source.Storage;
using y1000.Source.Util;
using HttpClient = System.Net.Http.HttpClient;
using ILogger = NLog.ILogger;

namespace y1000.Source.Control;

public partial class Login : NinePatchRect
{
	private Button _signIn;
	
	private Button _signUp;
	
	private AudioStreamPlayer _sound;
	private AudioStreamPlayer _bgm;

	private LineEdit _username;
	
	private LineEdit _password;
	
	private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

	private Button _return;
	
	private Button _joinRealm;
	
	private Button _createChar;

	private string? _token;
	private List<string>? _charNames;
	private CheckBox _maleCheckbox;
	private CheckBox _femaleCheckBox;
	
	public override void _Ready()
	{
		_signIn = GetNode<Button>("Entrance/Signin");
		_signUp = GetNode<Button>("Entrance/Signup");
		_sound = GetNode<AudioStreamPlayer>("Sound");
		_bgm = GetNode<AudioStreamPlayer>("BGM");
		_username = GetNode<LineEdit>("Entrance/Username/Input");
		_password = GetNode<LineEdit>("Entrance/Password/Input");
		var stream = AudioManager.LoadBgmStream("Logon");
		if (stream != null) {
			_bgm.Stream = stream;
			_bgm.Finished += () => _bgm.Play();
			_bgm.Play();
		}
		_signIn.Pressed += LogIn;
		_signUp.Pressed += SwitchToSignupUi;
		GetNode<Button>("Signup/Return").Pressed += ReturnToEntranceUi;
		GetNode<Button>("Signup/Signup").Pressed += Signup;
		GetNode<Button>("CharPanel/JoinRealm").Pressed += JoinRealm;
		GetNode<Button>("CharPanel/CreateChar").Pressed += CreateCharacter;
		_maleCheckbox = GetNode<CheckBox>("CharPanel/Male");
		_femaleCheckBox = GetNode<CheckBox>("CharPanel/Female");
		_maleCheckbox.Pressed += CheckMale;
		_femaleCheckBox.Pressed += CheckFemale;
		CheckMale();
		//AtdChecker.ZipAll();
	}

	
	private void CheckFemale()
	{
		_femaleCheckBox.ButtonPressed = true;
		_maleCheckbox.ButtonPressed = false;
	}

	private void CheckMale()
	{
		_femaleCheckBox.ButtonPressed = false;
		_maleCheckbox.ButtonPressed = true;
	}

	private void ReturnToEntranceUi()
	{
		_sound.Play();
		GetNode<Godot.Control>("Signup").Visible = false;
		GetNode<Godot.Control>("Entrance").Visible = true;
		GetNode<Godot.Control>("CharPanel").Visible = false;
	}
	
	private class CreateCharResponse
	{
		public int code { get; set; }

		public string msg { get; set; }

		public string charName { get; set; }
		
	}

	private void CreateCharacter()
	{
		_sound.Play();
		var tip = GetNode<Label>("CharPanel/Tip");
		if (_token == null)
		{
			tip.Text = "请先登录";
			return;
		}

		var charName = GetNode<LineEdit>("CharPanel/LineEdit");
		if (string.IsNullOrEmpty(charName.Text))
		{
			tip.Text = "请输入角色名, 最长5字符";
			return;
		}
		if (charName.Text.Length > 5)
		{
			tip.Text = "角色名最长5字符";
			return;
		}
		bool male = _maleCheckbox.ButtonPressed;
		var result = SendRequest("CREATE_CHAR",
			new StringContent(JsonSerializer.Serialize(new CreateCharacterRequest(_token, charName.Text, male))));
		if (result == null)
		{
			tip.Text = "服务器没有响应，请稍后再试";
			return;
		}
		var response = JsonSerializer.Deserialize<CreateCharResponse>(result);
		if (response == null)
		{
			tip.Text = "服务器没有响应，请稍后再试";
			return;
		}
		if (response.code != 0)
		{
			tip.Text = response.msg;
		}
		else
		{
			GetNode<ItemList>("CharPanel/Panel/Chars").AddItem(response.charName);
		}
	}

	private void JoinRealm()
	{
		_sound.Play();
		var itemList = GetNode<ItemList>("CharPanel/Panel/Chars");
		var tip = GetNode<Label>("CharPanel/Tip");
		if (itemList.ItemCount == 0)
		{
			tip.Text = "请创建角色";
			return;
		}
		if (!itemList.IsAnythingSelected())
		{
			tip.Text = "请选择角色";
			return;
		}
		var packedScene = ResourceLoader.Load<PackedScene>("res://game.tscn");
		var game = packedScene.Instantiate<Game>();
		var selectedItem = itemList.GetSelectedItems()[0];
		game.SetToken(_token, itemList.GetItemText(selectedItem));
		_bgm.Stop();
		GetParent().AddChild(game);
		QueueFree();
	}

	private void SwitchToSignupUi()
	{
		_sound.Play();
		GetNode<Godot.Control>("Signup").Visible = true;
		GetNode<Godot.Control>("Entrance").Visible = false;
		GetNode<Godot.Control>("CharPanel").Visible = false;
	}

	private void SwitchToCharPanel()
	{
		GetNode<Godot.Control>("Signup").Visible = false;
		GetNode<Godot.Control>("Entrance").Visible = false;
		GetNode<Godot.Control>("CharPanel").Visible = true;
		var itemList = GetNode<ItemList>("CharPanel/Panel/Chars");
		if (_charNames != null)
		{
			foreach (var name in _charNames)
			{
				itemList.AddItem(name);
			}
		}
	}

	private class SignupResponse
	{
		public int code { get; set; }
		public string msg { get; set; }
	}
	
	private class CreateCharacterRequest
	{
		public CreateCharacterRequest(string token, string characterName, bool male)
		{
			this.token = token;
			this.characterName = characterName;
			this.male = male;
		}
		public string token { get; set; }
		public string characterName { get; set; }
		public bool male { get; set; }
	}

	private void Signup()
	{
		_sound.Play();
		var user = GetNode<LineEdit>("Signup/Username/Input");
		var pswd = GetNode<LineEdit>("Signup/Password/Input");
		var pswdCfm = GetNode<LineEdit>("Signup/Confirm/Input");
		var tip = GetNode<Label>("Signup/Tip");
		if (string.IsNullOrEmpty(user.Text))
		{
			tip.Text = "请输入账号";
			return;
		}
		if (string.IsNullOrEmpty(pswd.Text))
		{
			tip.Text = "请输入密码";
			return;
		}
		if (string.IsNullOrEmpty(pswdCfm.Text))
		{
			tip.Text = "请确认密码";
			return;
		}
		if (!pswd.Text.Equals(pswdCfm.Text))
		{
			tip.Text = "两次密码不匹配";
			return;
		}
		var result = SendRequest("SIGNUP",
			new StringContent("{\"username\" : \"" + user.Text + "\", \"password\": \"" + pswd.Text + "\"}"));
		if (result == null)
		{
			tip.Text = "服务器没有响应，请稍后再试";
			return;
		}
		var signupResponse = JsonSerializer.Deserialize<SignupResponse>(result);
		if (signupResponse == null)
		{
			tip.Text = "服务器没有响应，请稍后再试";
			return;
		}
		tip.Text = signupResponse.code == 0 ? "注册成功" : signupResponse.msg;
	}
	
	private class LoginResponse
	{
		public int code { get; set; }
		public string msg { get; set; }
		public string token { get; set; }
		public List<string> charNames { get; set; }
	}

	private string? SendRequest(string type, StringContent content)
	{
		try
		{
			var httpRequestMessage = new HttpRequestMessage();
			httpRequestMessage.Headers.Add("X-Type", type);
			httpRequestMessage.RequestUri = new Uri("http://" + Configuration.Instance.ServerAddr + ":9901");
			httpRequestMessage.Method = HttpMethod.Post;
			httpRequestMessage.Content = content;
			using HttpClient httpClient = new();
			var httpResponseMessage = httpClient.Send(httpRequestMessage);
			var readAsStringAsync = httpResponseMessage.Content.ReadAsStringAsync();
			readAsStringAsync.Wait();
			return readAsStringAsync.Result;
		}
		catch (Exception e)
		{
			Logger.Error(e);
			return null;
		}
	}
	

	private void LogIn()
	{
		_sound.Play();
		var tip = GetNode<Label>("Entrance/Tip");
		if (string.IsNullOrEmpty(_username.Text))
		{
			tip.Text = "请输入账号";
			return;
		}
		if (string.IsNullOrEmpty(_password.Text))
		{
			tip.Text = "请输入密码";
			return;
		}
		var result = SendRequest("LOGIN", new StringContent("{\"username\" : \"" + _username.Text+ "\", \"password\": \"" + _password.Text + "\"}"));
		if (result == null)
		{
			tip.Text = "服务器没有响应，请稍后再试";
			return;
		}
		var loginResponse = JsonSerializer.Deserialize<LoginResponse>(result);
		if (loginResponse == null)
		{
			tip.Text = "服务器没有响应，请稍后再试";
			return;
		}
		if (loginResponse.code != 0)
		{
			tip.Text = loginResponse.msg;
		}
		else
		{
			_token = loginResponse.token;
			_charNames = loginResponse.charNames;
			SwitchToCharPanel();
		}
	}
}
