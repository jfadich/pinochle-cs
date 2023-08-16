import RealtimeClient from "./RealtimeClient";
import {PinochleApi} from "../GeneratedClients";

export default class GameHub extends RealtimeClient {

    constructor(url: string, tokenFactory: () => Promise<string>) {
        super(url, 'game', tokenFactory);
    }

    public Start() : Promise<GameHub>
    {
        return this.Connect().then((connection) => {
            this.Subscribe("JoinTable")

            return this
        })
    }

    public OnPlayerJoined(callback: (player: PinochleApi.IPlayerDto) => any)
    {
        this.On("PlayerJoined", callback)
    }

    public OnTurnTaken(callback: (roomId: string, turn: object) => any)
    {
        this.On("TurnTaken", callback)
    }

    public OnCardsReceived(callback: (roomId: string, cards: PinochleApi.PinochleCard[]) => any)
    {
        this.On("CardsReceived", callback)
    }

    public OnCardsTaken(callback: (roomId: string, cards: PinochleApi.PinochleCard[]) => any)
    {
        this.On("CardsTaken", callback)
    }
}