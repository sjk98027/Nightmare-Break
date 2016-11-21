public enum ClientPacketId
{
    None = 0,
    CreateAccount,
    DeleteAccount,
    Login,
    Logout,
    GameClose,
    CreateCharacter,
    DeleteCharacter,
    SelectCharacter,
    CreateRoom,
    EnterRoom,
    ExitRoom,
}

public enum ServerPacketId
{
    None = 0,
    CreateAccountResult,
    DeleteAccountResult,
    LoginResult,
    CreateCharacterResult,
    DeleteChracterResult,
    SelectCharacterResult,
    Match,
    CreateRoomResult,
    EnterRoomResult,
    ExitRoomResult,
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