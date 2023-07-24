import RealtimeClient from "./RealtimeClient";
import {PinochleApi} from "../GeneratedClients";

export default class MatchmakingHub extends RealtimeClient {

    constructor(url: string, tokenFactory: () => Promise<string>) {
        super(url, 'matchmaking', tokenFactory);
    }

    public Start() : Promise<MatchmakingHub>
    {
        return this.Connect().then(() => this)
    }

    public WatchMatchmaking()
    {
        this.Subscribe("WatchMatchmaking")
    }

    public WatchGames()
    {
        this.Subscribe("WatchGames")
    }

    public OnRoomAdded(callback: (room: PinochleApi.IGameRoomDto) => any)
    {
        this.On("RoomAdded", callback)
    }

    public OnPlayerJoined(callback: (roomId: string, player: PinochleApi.IPlayerDto) => any)
    {
        this.On("PlayerJoined", callback)
    }

    public OnLobbyClosed(callback: (roomId: string, reason: string) => any)
    {
        this.On("LobbyClosed", callback)
    }

    public OnGameStarted(callback: (game: PinochleApi.IGameRoomDto) => any)
    {
        this.On("GameStarted", callback)
    }
}