using FastHotKeyForWPF;
using System.Windows;
using System.Windows.Input;

namespace HotKeyGlobalActiveWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> keysPressedList = new List<string>();
        private ModelKeys currentModifier = ModelKeys.ALT;
        private NormalKeys currentKey = NormalKeys.O;
        public MainWindow()
        {
            InitializeComponent();
            this.StateChanged += Window_StateChanged;
            this.textBox.Text = currentModifier.ToString() + " + " + currentKey.ToString();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            }
            else if (this.WindowState == WindowState.Normal)
            {
                OpenWindow();
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            GlobalHotKey.Awake();//激活

            GlobalHotKey.Add(ModelKeys.ALT, NormalKeys.O, OpenWindow);//注册Alt+O 打开窗口

        }
        private void OpenWindow()
        {
            this.ShowInTaskbar = true;
            this.GlobalActivate();
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox.Text))
            { 
                textBox.Text = "";
                keysPressedList.Clear();
            }
            var keyStr = e.Key == Key.System ? e.SystemKey.ToString() : e.Key.ToString();
            if (e.KeyboardDevice.Modifiers != ModifierKeys.None)
            {
                if (!keysPressedList.Contains(e.KeyboardDevice.Modifiers.ToString()))
                {
                    keysPressedList.Add(e.KeyboardDevice.Modifiers.ToString());
                }
            }
            if (Enum.IsDefined(typeof(NormalKeys), keyStr))
            {
                if (!keysPressedList.Contains(keyStr))
                {
                    keysPressedList.Add(keyStr);
                }
            }
        }
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(keysPressedList.Count == 0) return;
            textBox.Text = string.Join(" + ", keysPressedList);
            keysPressedList.Clear();
            SaveHotKey(textBox.Text);
            textBlock.Text = $"2. 可按下{textBox.Text}再次打开窗口";
            var taskbarIcon = ((App)Application.Current).TaskbarIcon;
            taskbarIcon.ToolTipText = "你好！可按下" + textBox.Text + "打开窗口";
            MessageBox.Show("快捷键已保存!");
        }
        private void SaveHotKey(string keyText)
        {

            string[] keys = keyText.Split('+');
            ModelKeys modifier = currentModifier;
            NormalKeys key = currentKey;

            for (int i = 0; i < keys.Length; i++)
            {
                string keyStr = keys[i].Trim();
                if (keyStr.Length == 0)
                {
                    continue;
                }

                if (Enum.IsDefined(typeof(NormalKeys), keyStr))
                {
                    key = (NormalKeys)Enum.Parse(typeof(NormalKeys), keyStr, true);
                }
                else if (Enum.IsDefined(typeof(ModelKeys), keyStr))
                {
                    modifier = (ModelKeys)Enum.Parse(typeof(ModelKeys), keyStr, true);
                }
            }

            RegisterHotKey(modifier, key);
        }
        private void RegisterHotKey(ModelKeys modifier, NormalKeys key)
        {
            if (GlobalHotKey.IsHotKeyProtected(modifier, key))
            {
                MessageBox.Show("快捷键已被占用，请重新选择！");
                return;
            }
            GlobalHotKey.DeleteByKeys(currentModifier, currentKey); // 移除旧的快捷键
            GlobalHotKey.Add(modifier, key, OpenWindow); // 注册新的快捷键
            currentModifier = modifier;
            currentKey = key;
        }
    }
}