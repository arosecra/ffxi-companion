import { Component, OnInit } from '@angular/core';
import { Character } from '../model/character';
import { Gauge } from '../model/gauge';
import { GameEvent } from '../model/gameevent';
import { SocketService } from './socket.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.html',
  styles: []
})
export class AppComponent implements OnInit {
  title = 'ffxi-companion';
  characters = {};
  ioConnection: any;
  socketService: SocketService;

  constructor() {
    this.socketService = new SocketService();
    let c = new Character();
    c.name = "Specktr";
    c.hp = new Gauge();
    c.hp.status = "warning";
    c.hp.current = 100;
    c.hp.max = 1550;
    this.characters[c.name] = c;

    c = new Character();
    c.name = "Allouette";
    c.hp = new Gauge();
    c.hp.status = "success";
    c.hp.current = 15;
    c.hp.max = 1650;
    this.characters[c.name] = c;

  }

  ngOnInit(): void {
    this.socketService.initSocket(); 
    this.socketService.send('hello');
    this.ioConnection = this.socketService.onCharacter()
      .subscribe((character: Character) => {
        if(character.event == GameEvent.DISCONNECT)
          this.characters[character.name] = undefined;
        else
          this.characters[character.name] = character;
      });

  }

  characterNames(): string[] {
    return Object.keys(this.characters);
  }
  
}
