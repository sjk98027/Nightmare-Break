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
    StartGame,
    RequestUDPConnection,
}

public enum ServerPacketId
{
    None = 0,
    CreateAccountResult,
    DeleteAccountResult,
    LoginResult,
    LogoutResult,
    CreateCharacterResult,
    DeleteChracterResult,
    SelectCharacterResult,
    RoomList,
    CharacterStatus,
    SkillUpResult,
    EquipUpgradeResult,
    CreateRoomResult,
    EnterRoomResult,
    ExitRoomResult,
    UDPConnection,
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