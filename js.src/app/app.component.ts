import { Component, OnInit } from '@angular/core';
import { Character } from '../model/character';
import {Observable} from 'rxjs';
import { interval } from 'rxjs';
import { RestService } from './rest.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.html',
  styles: []
})
export class AppComponent implements OnInit {
  title = 'ffxi-companion';
  characters = {};
  ioConnection: any;
  subscription = undefined;

  constructor(private restService : RestService) {
    let c = new Character();
    c.name = "Specktr";
    c.hpp = 100;
    c.mpp = 60;
    c.tp = 25;
    this.characters[c.name] = c;

    // c = new Character();
    // c.name = "Allouette";
    // c.hp = new Gauge();
    // c.hp.status = "success";
    // c.hp.current = 15;
    // c.hp.max = 1650;
    // this.characters[c.name] = c;

  }

  ngOnInit(): void {
    var _this = this;
    this.subscription = interval(1000)
    .subscribe((val) => { 
      console.log('called'); 
      this.restService.getCharacterData().subscribe((data) => {
        data.forEach(function(character) {
          _this.characters[character.name] = character;
        });
      })
    });

  }

  characterNames(): string[] {
    return Object.keys(this.characters);
  }

  getAccent(num: number): string {
    if(num > 75) {
      return 'success';
    }
    else if (num > 30) {
      return 'warning';
    }
    return 'danger';
  }
  
}
