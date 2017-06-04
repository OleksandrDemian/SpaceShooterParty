class Converter
{
    public static Command toCommand(char command)
    {
        switch (command) {
            case 'q':
                return Command.NONE;
            case 'w':
                return Command.TURNLEFT;
            case 'e':
                return Command.TURNRIGHT;
            case 'r':
                return Command.ENGINETRIGGER;
            case 't':
                return Command.FIRE;
            case 'y':
                return Command.ABILITYTRIGGER;
            case 'u':
                return Command.ABILITYINFO;
            case 'i':
                return Command.NAME;
            case 'o':
                return Command.SHIPINFO;
            case 's':
                return Command.STARTGAME;
            case 'p':
                return Command.PAUSE;
            case 'a':
                return Command.KILL;
            case 'd':
                return Command.DEAD;
            case 'f':
                return Command.ADDPOINT;
            case 'g':
                return Command.MATCHEND;
            case 'h':
                return Command.COLOR;
            case 'j':
                return Command.SHIELD;
            case 'k':
                return Command.HEALTH;
            case 'l':
                return Command.COMMANDSSTRING;
            case 'z':
                return Command.SERVERCHECKER;

            default:
                return Command.NONE;
        }
    }

    public static char ToChar(Command command)
    {
        switch (command)
        {
            case Command.NONE:
                return 'q';
            case Command.TURNLEFT:
                return 'w';
            case Command.TURNRIGHT:
                return 'e';
            case Command.ENGINETRIGGER:
                return 'r';
            case Command.FIRE:
                return 't';
            case Command.ABILITYTRIGGER:
                return 'y';
            case Command.ABILITYINFO:
                return 'u';
            case Command.NAME:
                return 'i';
            case Command.SHIPINFO:
                return 'o';
            case Command.STARTGAME:
                return 's';
            case Command.PAUSE:
                return 'p';
            case Command.KILL:
                return 'a';
            case Command.DEAD:
                return 'd';
            case Command.ADDPOINT:
                return 'f';
            case Command.MATCHEND:
                return 'g';
            case Command.COLOR:
                return 'h';
            case Command.SHIELD:
                return 'j';
            case Command.HEALTH:
                return 'k';
            case Command.COMMANDSSTRING:
                return 'l';
            case Command.SERVERCHECKER:
                return 'z';

            default:
                return ' ';
        }
    }
    /*
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
            case 'q':
                return Request.PAUSE;
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
            case Request.DEAD:
                return "rq";
            case Request.KILL:
                return "rk";
        }
        return "";
    }
    */
}
