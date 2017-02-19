class Converter
{
    public static Command toCommand(char command)
    {
        switch (command) {
            case 'l':
                return Command.TURNLEFT;
            case 'r':
                return Command.TURNRIGHT;
            case 's':
                return Command.FIRE;
            case 'e':
                return Command.ENGINETRIGGER;
        }
        return Command.NONE;
    }

    public static Request toRequest(char request) {
        switch (request) {
            case 'n':
                return Request.NAME;
            case 's':
                return Request.SHIPINFO;
            case 'a':
                return Request.ABILITYINFO;
        }
        return Request.NONE;
    }

    public static string toString(Request request) {
        switch (request) {
            case Request.NAME:
                return "rn";
            case Request.SHIPINFO:
                return "rs";
            case Request.ABILITYINFO:
                return "ra";
        }
        return "";
    }
}
