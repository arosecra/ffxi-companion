import { Gauge } from './gauge';
import { GameEvent } from './gameevent';

export class Character {
    event: GameEvent;
    name: string;
    hp: Gauge;
    mp: Gauge;
    tp: Gauge;
}