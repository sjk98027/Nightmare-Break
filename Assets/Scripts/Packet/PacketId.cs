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
    RoomUserData,
    ExitRoom,
    StartGame,
    RequestUdpConnection,
    UdpConnectComplete,
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
    RoomUserData,
    ExitRoomResult,
    StartGame,
    UdpConnection,
    StartDungeon,
}

public enum P2PPacketId
{
    None = 0,
    RequestConnectionCheck,
    ConnectionCheckAnswer,
    UdpAnswer,
    CreateUnit,
    CharacterPosition,
    CharacterAction,
}