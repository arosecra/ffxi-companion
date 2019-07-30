import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Observer } from 'rxjs';
import { Character } from '../model/character';
import { Gauge } from '../model/gauge';
import { GameEvent } from '../model/gameevent';

import * as socketIo from 'socket.io-client';

const SERVER_URL = 'http://localhost:5000/angular-client';

@Injectable()
export class SocketService {
    private socket;

    public initSocket(): void {
        this.socket = socketIo(SERVER_URL);
    }

    public send(message: string): void {
        this.socket.emit('message', message);
    }

    public onCharacter(): Observable<Character> {
        return new Observable<Character>(observer => {
            this.socket.on('character', (data: Character) => observer.next(data));
        });
    }

    // public onEvent(echodMessage: string): Observable<any> {
    //     return new Observable<string>(observer => {
    //         this.socket.on(event, () => observer.next());
    //     });
    // }
}