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
            case 'a':
                return Command.ABILITYTRIGGER;
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
            case 'd':
                return Request.DISABLECONTROLLER;
            case 'p':
                return Request.ADDPOINT;
            case 'm':
                return Request.MATCHEND;
            case 'g':
                return Request.STARTGAME;
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
            case Request.DISABLECONTROLLER:
                return "rd";
            case Request.ADDPOINT:
                return "rp";
            case Request.MATCHEND:
                return "rm";
        }
        return "";
    }
}
