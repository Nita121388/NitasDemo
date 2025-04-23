using System;
using System.Collections.Generic;


// 客户端代码
class Program
{
    static void Main(string[] args)
    {
        RemoteControl remote = new RemoteControl();

        Light livingRoomLight = new Light("Living Room");
        Light kitchenLight = new Light("Kitchen");

        ICommand livingRoomLightOn = new LightOnCommand(livingRoomLight);
        ICommand livingRoomLightOff = new LightOffCommand(livingRoomLight);
        ICommand kitchenLightOn = new LightOnCommand(kitchenLight);
        ICommand kitchenLightOff = new LightOffCommand(kitchenLight);

        // 设置遥控器的按钮
        remote.SetCommand(0, livingRoomLightOn, livingRoomLightOff);
        remote.SetCommand(1, kitchenLightOn, kitchenLightOff);

        // 执行命令
        Console.WriteLine("Pressing On and Off for living room...");
        remote.OnButtonWasPushed(0);
        remote.OffButtonWasPushed(0);

        Console.WriteLine("Pressing On and Off for kitchen...");
        remote.OnButtonWasPushed(1);
        remote.OffButtonWasPushed(1);

        // 撤销操作
        Console.WriteLine("Undoing...");
        remote.UndoButtonWasPushed();
        remote.UndoButtonWasPushed();

        // 宏命令（组合命令）
        Console.WriteLine("Pressing macro command...");
        ICommand[] partyOnCommands = { livingRoomLightOn, kitchenLightOn };
        ICommand[] partyOffCommands = { livingRoomLightOff, kitchenLightOff };
        ICommand partyOnMacro = new RemoteControl.MacroCommand(partyOnCommands);
        ICommand partyOffMacro = new RemoteControl.MacroCommand(partyOffCommands);

        remote.SetCommand(2, partyOnMacro, partyOffMacro);
        remote.OnButtonWasPushed(2);
        remote.OffButtonWasPushed(2);

        Console.WriteLine("Undoing macro command...");
        remote.UndoButtonWasPushed();
    }
}

// 命令接口
public interface ICommand
{
    void Execute();
    void Undo();
}

// 具体命令类（打开灯光）
public class LightOnCommand : ICommand
{
    private Light _light;

    public LightOnCommand(Light light)
    {
        _light = light;
    }

    public void Execute()
    {
        _light.On();
    }

    public void Undo()
    {
        _light.Off();
    }
}

// 具体命令类（关闭灯光）
public class LightOffCommand : ICommand
{
    private Light _light;

    public LightOffCommand(Light light)
    {
        _light = light;
    }

    public void Execute()
    {
        _light.Off();
    }

    public void Undo()
    {
        _light.On();
    }
}

// 接收者类（灯光）
public class Light
{
    public string Location { get; }

    public Light(string location)
    {
        Location = location;
    }

    public void On()
    {
        Console.WriteLine($"The light in {Location} is on.");
    }

    public void Off()
    {
        Console.WriteLine($"The light in {Location} is off.");
    }
}

// 调用者类（遥控器）
public class RemoteControl
{
    private ICommand[] _onCommands;
    private ICommand[] _offCommands;
    private Stack<ICommand> _commandHistory;

    public RemoteControl()
    {
        _onCommands = new ICommand[7];
        _offCommands = new ICommand[7];
        _commandHistory = new Stack<ICommand>();

        ICommand noCommand = new NoCommand();
        for (int i = 0; i < 7; i++)
        {
            _onCommands[i] = noCommand;
            _offCommands[i] = noCommand;
        }
    }

    public void SetCommand(int slot, ICommand onCommand, ICommand offCommand)
    {
        _onCommands[slot] = onCommand;
        _offCommands[slot] = offCommand;
    }

    public void OnButtonWasPushed(int slot)
    {
        _onCommands[slot].Execute();
        _commandHistory.Push(_onCommands[slot]);
    }

    public void OffButtonWasPushed(int slot)
    {
        _offCommands[slot].Execute();
        _commandHistory.Push(_offCommands[slot]);
    }

    // 撤销操作
    public void UndoButtonWasPushed()
    {
        if (_commandHistory.Count > 0)
        {
            ICommand command = _commandHistory.Pop();
            command.Undo();
        }
    }

    // 宏命令（组合命令）
    public class MacroCommand : ICommand
    {
        private readonly ICommand[] _commands;

        public MacroCommand(ICommand[] commands)
        {
            _commands = commands;
        }

        public void Execute()
        {
            foreach (ICommand command in _commands)
            {
                command.Execute();
            }
        }

        public void Undo()
        {
            foreach (ICommand command in _commands)
            {
                command.Undo();
            }
        }
    }
}

// 无命令类（用于初始化默认情况）
public class NoCommand : ICommand
{
    public void Execute()
    {
    }

    public void Undo()
    {
    }
}
