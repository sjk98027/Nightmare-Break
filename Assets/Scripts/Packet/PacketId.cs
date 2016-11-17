public enum ClientPacketId
{
    None = 0,
    CreateAccount,
    DeleteAccount,
    Login,
    Logout,
    GameClose,
    EnterRoom,
    CreateRoom,
    ExitRoom,
}

public enum ServerPacketId
{
    None = 0,
    CreateResult,
    DeleteResult,
    LoginResult,
    Match,
}

public enum P2PPacketId
{
    None = 0,
    ConnectionCheck,
    ConnectionAnswer,
    CreateUnit,
    CharacterPosition,
    CharacterAction,
}