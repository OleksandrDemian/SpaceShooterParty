using System.Collections.Generic;

public class SocketInputManager {

    private List<Command> commands = new List<Command>();

    public void ManageInput(string input)
    {
        commands.Clear();
        for (int i = 0; i < input.Length; i++)
        {
            commands.Add(Converter.toCommand(input[i]));
        }
    }

    public bool GetCommand(Command command)
    {
        return commands.Contains(command);
    }

    public void RemoveCommand(Command command)
    {
        commands.Remove(command);
    }
}
