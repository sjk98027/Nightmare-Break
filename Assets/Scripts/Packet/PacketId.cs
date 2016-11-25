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
    RequestCharacterStatus,
    RequestRoomList,
    SkillUp,
    EquipUpgrade,
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
    RoomList,
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